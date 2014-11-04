// Copyright © 2004-2014 Tim Van Holder, Windower Team
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS"
// BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using System;
using System.IO;
using System.Text;

namespace PlayOnline.Core.Audio
{
    public class AudioFileStream : Stream
    {
        private string Path_;
        private AudioFileHeader Header_;
        private ADPCMCodec Codec_;
        private FileStream File_;
        private bool AddWAVHeader_;

        private long Position_;
        private byte[] WAVHeader_;
        private readonly int BufferBlocks_ = 32;
        private byte[] Buffer_;
        private int BufferPos_;
        private int BufferSize_;

        internal AudioFileStream(string Path, AudioFileHeader Header)
            : this(Path, Header, false) {}

        internal AudioFileStream(string Path, AudioFileHeader Header, bool AddWAVHeader)
        {
            this.Path_ = Path;
            this.Header_ = Header;
            this.AddWAVHeader_ = AddWAVHeader;
            this.Codec_ = null;
            this.Position_ = 0;
            if (this.Header_.SampleFormat == SampleFormat.ADPCM)
            {
                this.Codec_ = new ADPCMCodec(this.Header_.Channels, this.Header_.BlockSize);
            }
            this.File_ = new FileStream(Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            this.File_.Seek(0x30, SeekOrigin.Begin);
            if (AddWAVHeader)
            {
                // Prepare WAV file header
                this.WAVHeader_ = new byte[0x2C];
                BinaryWriter BW = new BinaryWriter(new MemoryStream(this.WAVHeader_, true), Encoding.ASCII);
                // File Header
                BW.Write("RIFF".ToCharArray());
                BW.Write((int)this.Length);
                // Wave Format Header
                BW.Write("WAVEfmt ".ToCharArray());
                BW.Write((int)0x10);
                // Wave Format Data
                BW.Write((short)1); // PCM
                BW.Write((short)this.Header_.Channels);
                BW.Write((int)this.Header_.SampleRate);
                BW.Write((int)(2 * this.Header_.Channels * this.Header_.SampleRate)); // bytes per second
                BW.Write((short)(2 * this.Header_.Channels)); // bytes per sample
                BW.Write((short)16); // bits
                // Wave Data Header
                BW.Write("data".ToCharArray());
                BW.Write((int)(this.Length - 0x2C));
                BW.Close();
            }
            this.Buffer_ = null;
            this.BufferPos_ = 0;
            this.BufferSize_ = 0;
        }

        public static bool IsFormatSupported(SampleFormat SF)
        {
            switch (SF)
            {
            case SampleFormat.ADPCM:
            case SampleFormat.PCM:
                return true;
            default:
                return false;
            }
        }

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get
            {
                return false; // Disallow this for now
            }
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override long Length
        {
            get
            {
                long Bytes = this.Header_.SampleBlocks * this.Header_.BlockSize * this.Header_.Channels * 2;
                if (this.Header_.SampleFormat == SampleFormat.PCM)
                {
                    Bytes = this.Header_.Size - 0x30;
                }
                if (this.AddWAVHeader_)
                {
                    Bytes += 0x2C;
                }
                return Bytes;
            }
        }

        public override long Position
        {
            get
            {
                if (this.AddWAVHeader_ && this.Position_ <= 0x2C)
                {
                    return this.Position_;
                }
                long RawPos = this.Position_;
                if (this.AddWAVHeader_)
                {
                    RawPos -= 0x2C;
                }
                long CookedPos = RawPos;
                if (this.Header_.SampleFormat == SampleFormat.ADPCM)
                {
                    double BlockPos = (double)RawPos / ((1 + this.Header_.BlockSize / 2));
                    CookedPos = (long)Math.Floor(BlockPos * this.Header_.BlockSize * 2);
                }
                if (this.AddWAVHeader_)
                {
                    CookedPos += 0x2C;
                }
                return CookedPos;
            }
            set { throw new NotSupportedException(I18N.GetText("SeekNotAllowed")); }
        }

        public override void Close()
        {
            this.File_.Close();
            base.Close();
        }

        public override void Flush() { this.File_.Flush(); }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int BytesRead = 0;
            if (this.AddWAVHeader_ && this.Position_ < 0x2C)
            {
                int HeaderBytesToRead = (int)Math.Min(0x2C, count - this.Position_);
                Array.Copy(this.WAVHeader_, this.Position_, buffer, offset, HeaderBytesToRead);
                this.Position_ += HeaderBytesToRead;
                BytesRead += HeaderBytesToRead;
                count -= HeaderBytesToRead;
            }
            if (this.Header_.SampleFormat == SampleFormat.PCM)
            {
                int RawBytesRead = this.File_.Read(buffer, offset + BytesRead, count);
                BytesRead += RawBytesRead;
                this.Position_ += RawBytesRead;
            }
            else
            {
                ReadSomeMore:
                int BytesAvail = this.BufferSize_ - this.BufferPos_;
                if (BytesAvail >= count)
                {
                    Array.Copy(this.Buffer_, this.BufferPos_, buffer, offset + BytesRead, count);
                    this.BufferPos_ += count;
                    BytesRead += count;
                }
                else
                {
                    if (BytesAvail > 0)
                    {
                        Array.Copy(this.Buffer_, this.BufferPos_, buffer, offset + BytesRead, BytesAvail);
                        count -= BytesAvail;
                        BytesRead += BytesAvail;
                    }
                    if (this.Buffer_ == null || this.BufferSize_ == this.Buffer_.Length)
                    {
                        // There's more data to be read from the file
                        this.FillBuffer();
                        goto ReadSomeMore;
                    }
                }
            }
            return BytesRead;
        }

        private void FillBuffer()
        {
            if (this.Buffer_ == null)
            {
                this.Buffer_ = new byte[this.Header_.BlockSize * this.Header_.Channels * 2 * this.BufferBlocks_];
            }
            this.BufferPos_ = 0;
            this.BufferSize_ = 0;
            byte[] ADPCMBlock = new byte[(1 + this.Header_.BlockSize / 2) * this.Header_.Channels];
            byte[] PCMBlock = new byte[this.Header_.BlockSize * this.Header_.Channels * 2];
            for (int i = 0; i < this.BufferBlocks_; ++i)
            {
                int BytesRead = this.File_.Read(ADPCMBlock, 0, ADPCMBlock.Length);
                this.Position_ += BytesRead;
                if (BytesRead == ADPCMBlock.Length)
                {
                    this.Codec_.DecodeSampleBlock(ADPCMBlock, PCMBlock);
                    Array.Copy(PCMBlock, 0, this.Buffer_, this.BufferSize_, PCMBlock.Length);
                    this.BufferSize_ += PCMBlock.Length;
                }
            }
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            if (origin == SeekOrigin.Current)
            {
                offset += this.Position;
            }
            else if (origin == SeekOrigin.Current)
            {
                offset += this.Length;
            }
            this.Position = offset;
            return this.Position;
        }

        public override void SetLength(long value) { throw new NotSupportedException(I18N.GetText("SetLengthNotAllowed")); }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException(I18N.GetText("WriteNotAllowed"));
        }
    }
}
