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
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using PlayOnline.Core;

namespace PlayOnline.FFXI
{
    public class FFXIEncoding : Encoding
    {
        // http://www.microsoft.com/globaldev/reference/dbcs/932.htm

        // The main table, and the 60 lead-byte tables
        private static SortedList ConversionTables = new SortedList(61);

        public FFXIEncoding() { }

        public override string EncodingName
        {
            get { return "Japanese (Shift-JIS, with FFXI extensions)"; }
        }

        public override string BodyName
        {
            get { return "iso-2022-jp-ffxi"; }
        }

        public override string HeaderName
        {
            get { return "iso-2022-jp-ffxi"; }
        }

        public override string WebName
        {
            get { return "iso-2022-jp-ffxi"; }
        }

        public static readonly char SpecialMarkerStart = '\u227A'; // ≺
        public static readonly char SpecialMarkerEnd = '\u227B'; // ≻

        #region Utility Functions

        public override int GetByteCount(char[] chars, int index, int count) { return this.GetBytes(chars, index, count).Length; }

        public override int GetCharCount(byte[] bytes, int index, int count) { return this.GetString(bytes, index, count).Length; }

        public override int GetMaxByteCount(int charCount) { return charCount * 2; }

        public override int GetMaxCharCount(int byteCount)
        {
            // Assume all autotrans stuff -> every 6 bytes can become, say, 60 characters -> bytes * 10
            // Assume all elements -> every 2 bytes can become <Element: ElementName> (say, 30 characters,
            //  given that ElementName (and possibly Element:) should be localizable) -> bytes * 15.
            return byteCount * 15;
        }

        internal BinaryReader GetConversionTable(byte Table)
        {
            if (FFXIEncoding.ConversionTables[Table] == null)
            {
                Stream ResourceStream =
                    Assembly.GetExecutingAssembly().GetManifestResourceStream(String.Format("ConversionTables.{0:X2}xx.dat", Table));
                if (ResourceStream != null)
                {
                    FFXIEncoding.ConversionTables[Table] = new BinaryReader(ResourceStream);
                }
            }
            return FFXIEncoding.ConversionTables[Table] as BinaryReader;
        }

        private ushort GetTableEntry(byte Table, byte Entry)
        {
            BinaryReader BR = this.GetConversionTable(Table);
            if (BR != null)
            {
                BR.BaseStream.Seek(2 * Entry, SeekOrigin.Begin);
                return BR.ReadUInt16();
            }
            return 0xFFFF;
        }

        #endregion

        #region Encoding

        private byte[] EncodeSpecialMarker(string Marker)
        {
            if (Marker.StartsWith("BAD CHAR:"))
            {
                string HexBytes = Marker.Substring(9).Trim();
                if (HexBytes.Length > 0 && (HexBytes.Length % 2) == 0)
                {
                    try
                    {
                        byte[] EncodedBadChar = new byte[HexBytes.Length / 2];
                        for (int i = 0; i < EncodedBadChar.Length; ++i)
                        {
                            EncodedBadChar[i] = byte.Parse(HexBytes.Substring(2 * i, 2), NumberStyles.HexNumber);
                        }
                        return EncodedBadChar;
                    }
                    catch {}
                }
            }
            else if (Marker.StartsWith("AutoTrans:"))
            {
                byte[] Result = new byte[] { 0xEF, 0x00 };
                switch (Marker.Substring(8).Trim())
                {
                case "Start":
                    Result[1] = 0x27;
                    break;
                case "End":
                    Result[1] = 0x28;
                    break;
                }
                if (Result[1] != 0x00)
                {
                    return Result;
                }
            }
            else if (Marker.StartsWith("Element:"))
            {
                byte[] Result = new byte[] { 0xEF, 0x00 };
                try
                {
                    Result[1] = (byte)Enum.Parse(typeof (Element), Marker.Substring(8).Trim());
                    Result[1] += 0x1f;
                }
                catch {}
                if (Result[1] >= 0x1f && Result[1] <= 0x26)
                {
                    return Result;
                }
            }
            else if (Marker.StartsWith("["))
            {
                int CloseBracket = Marker.IndexOf(']', 1);
                if (CloseBracket > 0)
                {
                    string HexID = Marker.Substring(1, CloseBracket - 1);
                    try
                    {
                        uint ResourceID = uint.Parse(HexID, NumberStyles.HexNumber);
                        byte[] EncodedResourceString = new byte[6];
                        EncodedResourceString[5] = 0xFD;
                        EncodedResourceString[4] = (byte)(ResourceID & 0xff);
                        ResourceID >>= 8;
                        EncodedResourceString[3] = (byte)(ResourceID & 0xff);
                        ResourceID >>= 8;
                        EncodedResourceString[2] = (byte)(ResourceID & 0xff);
                        ResourceID >>= 8;
                        EncodedResourceString[1] = (byte)(ResourceID & 0xff);
                        ResourceID >>= 8;
                        EncodedResourceString[0] = 0xFD;
                        return EncodedResourceString;
                    }
                    catch {}
                }
            }
            // No match with one of our special marker formats => let GetBytes() do regular processing
            return null;
        }

        private ushort FindTableEntry(char C)
        {
            // Check main table, branching off to other tables if main table indicates a valid lead byte
            BinaryReader MainBR = this.GetConversionTable(0x00);
            if (MainBR != null)
            {
                MainBR.BaseStream.Seek(0, SeekOrigin.Begin);
                for (ushort i = 0; i <= 0xff; ++i)
                {
                    ushort MainEntry = MainBR.ReadUInt16();
                    if (MainEntry == (ushort)C) // match found
                    {
                        return i;
                    }
                    else if (MainEntry == 0xFFFE)
                    {
                        // valid lead byte
                        BinaryReader SubBR = this.GetConversionTable((byte)i);
                        if (SubBR != null)
                        {
                            SubBR.BaseStream.Seek(0, SeekOrigin.Begin);
                            for (ushort j = 0x00; j <= 0xff; ++j)
                            {
                                if (SubBR.ReadUInt16() == (ushort)C) // match found
                                {
                                    return (ushort)((i << 8) + j);
                                }
                            }
                        }
                    }
                }
            }
            return 0xFFFF; // no such entry in conversion tables => cannot be encoded
        }

        public override byte[] GetBytes(char[] chars, int index, int count)
        {
            ArrayList EncodedBytes = new ArrayList();
            for (int pos = index; pos < index + count; ++pos)
            {
                if (chars[pos] == FFXIEncoding.SpecialMarkerStart)
                {
                    // Potential special string
                    int endpos = pos + 1;
                    while (endpos < index + count && chars[endpos] != FFXIEncoding.SpecialMarkerEnd)
                    {
                        ++endpos;
                    }
                    if (endpos < index + count)
                    {
                        // valid end marker found => parse
                        byte[] EncodedMarker = this.EncodeSpecialMarker(new string(chars, pos + 1, endpos - pos - 1));
                        if (EncodedMarker != null)
                        {
                            EncodedBytes.AddRange(EncodedMarker);
                            pos = endpos;
                            continue;
                        }
                    }
                }
                ushort TableEntry = this.FindTableEntry(chars[pos]);
                if (TableEntry != 0xFFFF)
                {
                    if (TableEntry > 0xff)
                    {
                        EncodedBytes.Add((byte)((TableEntry & 0xFF00) >> 8));
                    }
                    EncodedBytes.Add((byte)(TableEntry & 0xFF));
                }
            }
            return (byte[])EncodedBytes.ToArray(typeof (byte));
        }

        public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
        {
            byte[] EncodedBytes = this.GetBytes(chars, charIndex, charCount);
            Array.Copy(EncodedBytes, 0, bytes, byteIndex, EncodedBytes.Length);
            return EncodedBytes.Length;
        }

        #endregion

        #region Decoding

        public override string GetString(byte[] bytes)
        {
            return this.GetString(bytes, bytes.GetLowerBound(0), 1 + (bytes.GetUpperBound(0) - bytes.GetLowerBound(0)));
        }

        public override string GetString(byte[] bytes, int index, int count)
        {
            string DecodedString = String.Empty;
            for (int pos = index; pos < index + count; ++pos)
            {
                // FFXI Extension: Elemental symbols
                if (bytes[pos] == 0xEF && (pos + 1) < (index + count) && bytes[pos + 1] >= 0x1F && bytes[pos + 1] <= 0x26)
                {
                    DecodedString += String.Format("{0}Element: {1}{2}", FFXIEncoding.SpecialMarkerStart,
                        (Element)(bytes[++pos] - 0x1f), FFXIEncoding.SpecialMarkerEnd);
                    continue;
                }
                // FFXI Extension: Open/Close AutoTranslator Text
                if (bytes[pos] == 0xEF && (pos + 1) < (index + count) && bytes[pos + 1] >= 0x27 && bytes[pos + 1] <= 0x28)
                {
                    DecodedString += FFXIEncoding.SpecialMarkerStart;
                    DecodedString += "AutoTrans: ";
                    switch (bytes[++pos])
                    {
                    case 0x27:
                        DecodedString += "Start";
                        break;
                    case 0x28:
                        DecodedString += "End";
                        break;
                    }
                    DecodedString += FFXIEncoding.SpecialMarkerEnd;
                    continue;
                }
                // FFXI Extension: Resource Text (Auto-Translator/Item/Key Item)
                if (bytes[pos] == 0xFD && pos + 5 < index + count && bytes[pos + 5] == 0xFD)
                {
                    uint ResourceID = 0;
                    ResourceID <<= 8;
                    ResourceID += bytes[pos + 1];
                    ResourceID <<= 8;
                    ResourceID += bytes[pos + 2];
                    ResourceID <<= 8;
                    ResourceID += bytes[pos + 3];
                    ResourceID <<= 8;
                    ResourceID += bytes[pos + 4];
                    DecodedString += String.Format("{0}[{1:X8}] {2}{3}", FFXIEncoding.SpecialMarkerStart, ResourceID,
                        FFXIResourceManager.GetResourceString(ResourceID), FFXIEncoding.SpecialMarkerEnd);
                    pos += 5;
                    continue;
                }
                // Default behaviour - use table
                ushort DecodedChar = this.GetTableEntry(0, bytes[pos]);
                if (DecodedChar == 0xFFFE)
                {
                    // Possible Lead Byte
                    if (pos + 1 < index + count)
                    {
                        byte Table = bytes[pos++];
                        DecodedChar = this.GetTableEntry(Table, bytes[pos]);
                        if (DecodedChar == 0xFFFF)
                        {
                            DecodedString += String.Format("{0}BAD CHAR: {1:X2}{2:X2}{3}", FFXIEncoding.SpecialMarkerStart, Table,
                                bytes[pos], FFXIEncoding.SpecialMarkerEnd);
                        }
                        else
                        {
                            DecodedString += (char)DecodedChar;
                        }
                    }
                    else
                    {
                        DecodedString += String.Format("{0}BAD CHAR: {1:X2}{2}", FFXIEncoding.SpecialMarkerStart, bytes[pos],
                            FFXIEncoding.SpecialMarkerEnd);
                    }
                }
                else if (DecodedChar == 0xFFFF)
                {
                    DecodedString += String.Format("{0}BAD CHAR: {1:X2}{2}", FFXIEncoding.SpecialMarkerStart, bytes[pos],
                        FFXIEncoding.SpecialMarkerEnd);
                }
                else
                {
                    DecodedString += (char)DecodedChar;
                }
            }
            return DecodedString;
        }

        public override char[] GetChars(byte[] bytes, int index, int count)
        {
            return this.GetString(bytes, index, count).ToCharArray();
        }

        public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
        {
            char[] DecodedChars = this.GetChars(bytes, byteIndex, byteCount);
            Array.Copy(DecodedChars, 0, chars, charIndex, DecodedChars.Length);
            return DecodedChars.Length;
        }

        #endregion
    }
}
