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
    public enum AudioFileType
    {
        Unknown,
        BGMStream,
        SoundEffect,
    }

    public enum SampleFormat : uint
    {
        ADPCM = 0,
        PCM = 1,
        ATRAC3 = 3,
    }

    internal class AudioFileHeader
    {
        // Direct Members

        public int Size;
        public SampleFormat SampleFormat;
        public int ID;
        public int SampleBlocks;
        public int LoopStart;
        public int SampleRateLow;
        public int SampleRateHigh;
        public int Unknown1;
        public byte Unknown2;
        public byte Unknown3;
        public byte Channels;
        public byte BlockSize;
        public int Unknown4;

        // Indirect Members

        public double Length
        {
            get { return this.SamplesToSeconds(this.SampleBlocks); }
        }

        public double LoopStartTime
        {
            get { return this.SamplesToSeconds(this.LoopStart); }
        }

        public bool Looped
        {
            get { return (this.LoopStart >= 0); }
        }

        public int SampleRate
        {
            get { return this.SampleRateHigh + this.SampleRateLow; }
        }

        // Utility Functions

        public double SamplesToSeconds(long Samples)
        {
            double ByteCount = Samples;
            if (this.SampleFormat == SampleFormat.ADPCM)
            {
                ByteCount *= this.BlockSize;
            }
            return ByteCount / this.SampleRate;
        }

        public long SecondsToSamples(double Seconds)
        {
            double ByteCount = Seconds * this.SampleRate;
            if (this.SampleFormat == SampleFormat.ADPCM)
            {
                ByteCount /= this.BlockSize;
            }
            return (long)Math.Floor(ByteCount);
        }
    }

    public class AudioFile
    {
        // === Data === //

        private string Path_;
        private AudioFileType Type_;
        private AudioFileHeader Header_;

        // === Public Member Functions === //

        public AudioFile(string Path)
        {
            this.Path_ = Path;
            try
            {
                BinaryReader BR =
                    new BinaryReader(new FileStream(this.Path_, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 0x30),
                        Encoding.ASCII);
                this.DetermineType(BR);
                if (this.Type_ != AudioFileType.Unknown)
                {
                    this.Header_ = new AudioFileHeader();
                    switch (this.Type_)
                    {
                    case AudioFileType.BGMStream:
                        this.Header_.SampleFormat = (SampleFormat)BR.ReadInt32();
                        this.Header_.Size = BR.ReadInt32();
                        break;
                    case AudioFileType.SoundEffect:
                        this.Header_.Size = BR.ReadInt32();
                        this.Header_.SampleFormat = (SampleFormat)BR.ReadInt32();
                        break;
                    }
                    this.Header_.ID = BR.ReadInt32();
                    this.Header_.SampleBlocks = BR.ReadInt32();
                    this.Header_.LoopStart = BR.ReadInt32();
                    this.Header_.SampleRateHigh = BR.ReadInt32();
                    this.Header_.SampleRateLow = BR.ReadInt32();
                    this.Header_.Unknown1 = BR.ReadInt32();
                    this.Header_.Unknown2 = BR.ReadByte();
                    this.Header_.Unknown3 = BR.ReadByte();
                    this.Header_.Channels = BR.ReadByte();
                    this.Header_.BlockSize = BR.ReadByte();
                    switch (this.Type_)
                    {
                    case AudioFileType.BGMStream:
                        this.Header_.Unknown4 = 0;
                        break;
                    case AudioFileType.SoundEffect:
                        this.Header_.Unknown4 = BR.ReadInt32();
                        break;
                    }
                }
                BR.Close();
            }
            catch
            {
                this.Type_ = AudioFileType.Unknown;
                this.Header_ = new AudioFileHeader();
            }
        }

        public AudioFileStream OpenStream() { return this.OpenStream(false); }

        public AudioFileStream OpenStream(bool AddWAVHeader)
        {
            if (this.Type_ == AudioFileType.Unknown || this.Header_ == null)
            {
                return null;
            }
            if (!AudioFileStream.IsFormatSupported(this.Header_.SampleFormat))
            {
                return null;
            }
            return new AudioFileStream(this.Path_, this.Header_, AddWAVHeader);
        }

        public bool Playable
        {
            get { return (this.Header_ != null && AudioFileStream.IsFormatSupported(this.Header_.SampleFormat)); }
        }

        // === Properties === //

        public string Path
        {
            get { return this.Path_; }
        }

        public AudioFileType Type
        {
            get { return this.Type_; }
        }

        public int Size
        {
            get { return this.Header_.Size; }
        }

        public int ID
        {
            get { return this.Header_.ID; }
        }

        public int SampleRate
        {
            get { return this.Header_.SampleRate; }
        }

        public byte Channels
        {
            get { return this.Header_.Channels; }
        }

        public byte BitsPerSample
        {
            get { return 16; }
        }

        public SampleFormat SampleFormat
        {
            get { return this.Header_.SampleFormat; }
        }

        public int LoopStart
        {
            get { return this.Header_.LoopStart; }
        }

        public bool Looped
        {
            get { return this.Header_.Looped; }
        }

        public double Length
        {
            get { return this.Header_.Length; }
        }

        // === Private Member Functions === //

        private void DetermineType(BinaryReader BR)
        {
            string marker = new string(BR.ReadChars(8));
            if (marker == "SeWave\0\0")
            {
                this.Type_ = AudioFileType.SoundEffect;
            }
            else
            {
                marker += new string(BR.ReadChars(4));
                if (marker == "BGMStream\0\0\0")
                {
                    this.Type_ = AudioFileType.BGMStream;
                }
            }
        }
    }
}
