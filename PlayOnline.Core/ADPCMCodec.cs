// Copyright © 2004-2014 Tim Van Holder, Windower Team
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS"
// BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using System;
using System.Collections;
using System.Text;

namespace PlayOnline.Core.Audio
{
    internal class ADPCMCodec
    {
        private int Channels;
        private int BlockSize;
        private int[] DecoderState;

        public ADPCMCodec(int Channels, int BlockSize) { this.Reset(Channels, BlockSize); }

        public void Reset(int Channels, int BlockSize)
        {
            if (Channels <= 0 || Channels > 6)
            {
                throw new ArgumentException(I18N.GetText("ADPCMBadChannelCount"), "Channels");
            }
            this.Channels = Channels;
            this.BlockSize = BlockSize;
            this.DecoderState = new int[2 * Channels];
        }

        private static readonly int[] Filter0 = new int[] { 0x0000, 0x00F0, 0x01CC, 0x0188, 0x01E8 };
        private static readonly int[] Filter1 = new int[] { 0x0000, 0x0000, -0x00D0, -0x00DC, -0x00F0 };

        private int Round(int Value)
        {
            if (Value > 0x7FFF)
            {
                return 0x7FFF;
            }
            if (Value < -0x8000)
            {
                return -0x8000;
            }
            return Value;
        }

        public void DecodeSampleBlock(byte[] In, byte[] Out)
        {
            if (In.Length < ((1 + this.BlockSize / 2) * this.Channels))
            {
                throw new ArgumentException(String.Format(I18N.GetText("ADPCMInputTooSmall"), 1 + this.BlockSize / 2), "In");
            }
            if (Out.Length < (this.BlockSize * this.Channels * 2))
            {
                throw new ArgumentException(String.Format(I18N.GetText("ADPCMOutputTooSmall"), 2 * this.BlockSize), "Out");
            }
            for (int Channel = 0; Channel < this.Channels; ++Channel)
            {
                int BaseIndex = Channel * (1 + this.BlockSize / 2);
                int Scale = (0x0C - (In[BaseIndex + 0] & 0x0F));
                int Index = (In[BaseIndex + 0] >> 4);
                if (Index < 5)
                {
                    for (byte Sample = 0; Sample < (this.BlockSize / 2); ++Sample)
                    {
                        byte SampleByte = In[BaseIndex + Sample + 1];
                        for (byte Nibble = 0; Nibble < 2; ++Nibble)
                        {
                            int Value = ((SampleByte >> (4 * Nibble)) & 0x0F);
                            if (Value >= 8)
                            {
                                Value -= 16;
                            }
                            int TempValue = (Value << Scale);
                            TempValue += ((this.DecoderState[Channel * 2 + 0] * ADPCMCodec.Filter0[Index] +
                                           this.DecoderState[Channel * 2 + 1] * ADPCMCodec.Filter1[Index]) / 256);
                            this.DecoderState[Channel * 2 + 1] = this.DecoderState[Channel * 2 + 0];
                            this.DecoderState[Channel * 2 + 0] = this.Round(TempValue);
                            Out[((2 * Sample + Nibble) * this.Channels + Channel) * 2 + 0] =
                                (byte)((this.DecoderState[Channel * 2 + 0] >> 0) & 0xff);
                            Out[((2 * Sample + Nibble) * this.Channels + Channel) * 2 + 1] =
                                (byte)((this.DecoderState[Channel * 2 + 0] >> 8) & 0xff);
                        }
                    }
                }
            }
        }

        public void EncodeSampleBlock(byte[] In, byte[] Out)
        {
            throw new NotImplementedException(I18N.GetText("ADPCMEncodingNotSupported"));
        }
    }
}
