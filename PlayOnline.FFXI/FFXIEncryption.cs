// Copyright © 2004-2014 Tim Van Holder, Nevin Stepan, Windower Team
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS"
// BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PlayOnline.FFXI
{
    public sealed class FFXIEncryption
    {
        private static byte Rotate(byte B, int ShiftSize) { return (byte)((B >> ShiftSize) | (B << (8 - ShiftSize))); }

        public static bool Rotate(IList<byte> Data, byte ShiftSize)
        {
            return FFXIEncryption.Rotate(Data, 0, Data.Count, ShiftSize);
        }

        public static bool Rotate(IList<byte> Data, int Offset, int Size, byte ShiftSize)
        {
            if (ShiftSize < 1 || ShiftSize > 8)
            {
                return false;
            }
            for (int i = 0; i < Size; ++i)
            {
                Data[Offset + i] = FFXIEncryption.Rotate(Data[Offset + i], ShiftSize);
            }
            return true;
        }

        private static int CountBits(byte B)
        {
            int Count = 0;
            while (B != 0)
            {
                if ((B & 0x01) != 0)
                {
                    ++Count;
                }
                B >>= 1;
            }
            return Count;
        }

        private static byte GetTextShiftSize(IList<byte> Data, int Offset, int Size)
        {
            if (Size < 2)
            {
                return 0;
            }
            if (Data[Offset + 0] == 0 && Data[Offset + 1] == 0)
            {
                return 0;
            }
            // This is the heuristic that ffxitool uses to determine the shift size - it makes absolutely no
            // sense to me, but it works; I suppose the author of ffxitool reverse engineered what FFXI does.
            int BitCount = FFXIEncryption.CountBits(Data[Offset + 1]) - FFXIEncryption.CountBits(Data[Offset + 0]);
            switch (Math.Abs(BitCount) % 5)
            {
            case 0:
                return 1;
            case 1:
                return 7;
            case 2:
                return 2;
            case 3:
                return 6;
            case 4:
                return 3;
            }
            return 0;
        }

        private static byte GetDataShiftSize(IList<byte> Data, int Offset, int Size)
        {
            if (Size < 13)
            {
                return 0;
            }
            // This is the heuristic that ffxitool uses to determine the shift size - it makes absolutely no
            // sense to me, but it works; I suppose the author of ffxitool reverse engineered what FFXI does.
            int BitCount = FFXIEncryption.CountBits(Data[Offset + 2]) - FFXIEncryption.CountBits(Data[Offset + 11]) +
                           FFXIEncryption.CountBits(Data[Offset + 12]);
            switch (Math.Abs(BitCount) % 5)
            {
            case 0:
                return 7;
            case 1:
                return 1;
            case 2:
                return 6;
            case 3:
                return 2;
            case 4:
                return 5;
            }
            return 0;
        }

        public static bool DecodeTextBlock(IList<byte> Data) { return FFXIEncryption.DecodeTextBlock(Data, 0, Data.Count); }

        public static bool DecodeTextBlock(IList<byte> Data, int Offset, int Size)
        {
            return FFXIEncryption.Rotate(Data, Offset, Size, FFXIEncryption.GetTextShiftSize(Data, Offset, Size));
        }

        public static bool DecodeDataBlock(IList<byte> Data) { return FFXIEncryption.DecodeDataBlock(Data, 0, Data.Count); }

        public static bool DecodeDataBlockMask(IList<byte> Data)
        {
            byte save3 = Data[2];
            byte save12 = Data[11];
            byte save13 = Data[12];
            bool returnvalue = FFXIEncryption.DecodeDataBlock(Data, 0, Data.Count);
            Data[2] = save3;
            Data[11] = save12;
            Data[12] = save13;

            return returnvalue;
        }

        public static bool DecodeDataBlock(IList<byte> Data, int Offset, int Size)
        {
            return FFXIEncryption.Rotate(Data, Offset, Size, FFXIEncryption.GetDataShiftSize(Data, Offset, Size));
        }

        public static string ReadEncodedString(BinaryReader BR, Encoding E)
        {
            List<byte> LineBytes = new List<byte>();
            // It's NUL-terminated, BUT we need at least two bytes to determine the shift size - for example, a single space becomes "10 00".
            do
            {
                LineBytes.Add(BR.ReadByte());
            }
            while (LineBytes.Count < 2 || (byte)LineBytes[LineBytes.Count - 1] != 0);
            FFXIEncryption.DecodeTextBlock(LineBytes);
            return E.GetString(LineBytes.ToArray()).TrimEnd('\0');
        }
    }
}
