// Copyright © 2004-2014 Tim Van Holder, Windower Team
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS"
// BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using System;
using System.Globalization;
using System.IO;
using System.Text;
using PlayOnline.Core;
using PlayOnline.FFXI.Things;

namespace PlayOnline.FFXI
{
    // TODO: Make a real design decision on how to decide what language a string resource should be returned as.
    //       Should it be the UI language, the selected POL region, ...?
    // For now, this will always return the English text, except for non-expando autotrans messages, which will be in the language specified by their ID.
    public class FFXIResourceManager
    {
        private static FFXIEncoding E = new FFXIEncoding();

        public static string GetResourceString(uint ResourceID)
        {
            string ResourceString = FFXIResourceManager.GetResourceStringInternal(ResourceID);
            return ((ResourceString == null) ? I18N.GetText("BadResID") : ResourceString);
        }

        public static bool IsValidResourceID(uint ResourceID)
        {
            return (FFXIResourceManager.GetResourceStringInternal(ResourceID) != null);
        }

        public static string GetAreaName(ushort ID) { return FFXIResourceManager.GetStringTableEntry(55465, ID); }

        public static string GetJobName(ushort ID) { return FFXIResourceManager.GetStringTableEntry(55467, ID); }

        public static string GetRegionName(ushort ID) { return FFXIResourceManager.GetStringTableEntry(55654, ID); }

        public static string GetAbilityName(ushort ID)
        {
#if false // FIXME: Not updated to new DAT file format
            BinaryReader BR = FFXIResourceManager.OpenDATFile(85); // JP = 10
            if (BR != null)
            {
                if ((ID + 1) * 0x400 <= BR.BaseStream.Length)
                {
                    BR.BaseStream.Position = ID * 0x400;
                    byte[] AbilityData = BR.ReadBytes(0x400);
                    BR.Close();
                    if (FFXIEncryption.DecodeDataBlock(AbilityData))
                    {
                        return FFXIResourceManager.E.GetString(AbilityData, 0x0a, 32).TrimEnd('\0');
                    }
                }
                BR.Close();
            }
            return null;
#else
            return String.Format("Ability #{0}", ID);
#endif
        }

        public static string GetSpellName(ushort ID)
        {
#if false // FIXME: Not updated to new DAT file format
            BinaryReader BR = FFXIResourceManager.OpenDATFile(86);
            if (BR != null)
            {
                if ((ID + 1) * 0x400 <= BR.BaseStream.Length)
                {
                    BR.BaseStream.Position = ID * 0x400;
                    byte[] SpellData = BR.ReadBytes(0x400);
                    BR.Close();
                    if (FFXIEncryption.DecodeDataBlock(SpellData))
                    {
                        return FFXIResourceManager.E.GetString(SpellData, 0x3d, 20).TrimEnd('\0');
                    }
                }
                BR.Close();
            }
            return null;
#else
            return String.Format("Spell #{0}", ID);
#endif
        }

        // JP: 4, 5, 6, 7, 8
        private static ushort[] ItemDATs = new ushort[] { 73, 74, 75, 76, 77 };

        public static string GetItemName(byte Language, ushort ID)
        {
            foreach (ushort ItemDAT in FFXIResourceManager.ItemDATs)
            {
                BinaryReader BR = FFXIResourceManager.OpenDATFile(ItemDAT);
                if (BR != null)
                {
                    Item.Type T;
                    Item.DeduceType(BR, out T);
                    long Offset = (ID & 0xfff) * 0xc00;
                    if (BR.BaseStream.Length >= Offset + 0xc00)
                    {
                        Item I = new Item();
                        BR.BaseStream.Position = Offset;
                        if (I.Read(BR, T) && (uint)I.GetFieldValue("id") == ID)
                        {
                            BR.Close();
                            return I.GetFieldText("name");
                        }
                    }
                    BR.Close();
                }
            }
            return null;
        }

        public static string GetKeyItemName(byte Language, ushort ID)
        {
#if false // FIXME: Not updated to new DAT file format
            BinaryReader BR = FFXIResourceManager.OpenDATFile(82); // JP = 80
            if (BR != null)
            {
                if (Encoding.ASCII.GetString(BR.ReadBytes(4)) == "menu" && BR.ReadUInt32() == 0x101)
                {
                    BR.BaseStream.Position = 0x20;
                    while (BR.BaseStream.Position < BR.BaseStream.Length)
                    {
                        long Offset = BR.BaseStream.Position;
                        string ShortName = Encoding.ASCII.GetString(BR.ReadBytes(4));
                        uint SizeInfo = BR.ReadUInt32();
                        if (BR.ReadUInt64() != 0)
                        {
                            break;
                        }
                        if (ShortName == "sc_i")
                        {
                            BR.BaseStream.Position += 0x14;
                            uint EntryCount = BR.ReadUInt32();
                            for (uint i = 0; i < EntryCount; ++i)
                            {
                                if (BR.ReadUInt32() == ID)
                                {
                                    BR.BaseStream.Position += 4;
                                    BR.BaseStream.Position = Offset + 0x10 + BR.ReadUInt32();
                                    return FFXIEncryption.ReadEncodedString(BR, FFXIResourceManager.E);
                                }
                                BR.BaseStream.Position += 16;
                            }
                        }
                        // Skip to next one
                        BR.BaseStream.Position = Offset + ((SizeInfo & 0xFFFFFF80) >> 3);
                    }
                }
                BR.Close();
            }
            return null;
#else
            return String.Format("Key Item #{0}", ID);
#endif
        }

        public static string GetAutoTranslatorMessage(byte Category, byte Language, ushort ID)
        {
            // FIXME: This is probably a stale file
            BinaryReader BR = FFXIResourceManager.OpenDATFile(55665); // JP = 55545
            if (BR != null)
            {
                while (BR.BaseStream.Position + 76 <= BR.BaseStream.Length)
                {
                    byte GroupCat = BR.ReadByte();
                    byte GroupLang = BR.ReadByte();
                    ushort GroupID = (ushort)(BR.ReadByte() * 256 + BR.ReadByte());
                    BR.BaseStream.Position += 64;
                    uint Messages = BR.ReadUInt32();
                    uint DataBytes = BR.ReadUInt32();
                    if (GroupID == (ID & 0xff00))
                    {
                        // We found the right group (ignoring category & language for now)
                        for (uint i = 0; i < Messages && BR.BaseStream.Position + 5 < BR.BaseStream.Length; ++i)
                        {
                            byte MessageCat = BR.ReadByte();
                            byte MessageLang = BR.ReadByte();
                            ushort MessageID = (ushort)(BR.ReadByte() * 256 + BR.ReadByte());
                            byte TextLength = BR.ReadByte();
                            if (MessageID == ID)
                            {
                                // We found the right message (ignoring category & language for now)
                                byte[] MessageBytes = BR.ReadBytes(TextLength);
                                BR.Close();
                                string MessageText = FFXIResourceManager.E.GetString(MessageBytes).TrimEnd('\0');
                                return FFXIResourceManager.MaybeExpandAutoTranslatorMessage(MessageText);
                            }
                            else
                            {
                                BR.BaseStream.Position += TextLength;
                                if (MessageLang == 0x04)
                                {
                                    // There is an extra string to skip for Japanese entries
                                    TextLength = BR.ReadByte();
                                    BR.BaseStream.Position += TextLength;
                                }
                            }
                        }
                    }
                    else
                    {
                        BR.BaseStream.Position += DataBytes;
                    }
                }
                BR.Close();
            }
            return null;
        }

        private static string GetResourceStringInternal(uint ResourceID)
        {
            byte Category = (byte)((ResourceID >> 24) & 0xff);
            byte Language = (byte)((ResourceID >> 16) & 0xff);
            ushort ID = (ushort)(ResourceID & 0xffff);
            switch (Category)
            {
            case 0x02:
                return FFXIResourceManager.GetAutoTranslatorMessage(Category, Language, ID);
            case 0x04:
                return FFXIResourceManager.GetAutoTranslatorMessage(Category, Language, ID);
            case 0x06:
                return FFXIResourceManager.GetItemName(Language, ID);
            case 0x07:
                return FFXIResourceManager.GetItemName(Language, ID);
            case 0x08:
                return FFXIResourceManager.GetItemName(Language, ID);
            case 0x09:
                return FFXIResourceManager.GetItemName(Language, ID);
            case 0x13:
                return FFXIResourceManager.GetKeyItemName(Language, ID);
            }
            return null;
        }

        private static string GetStringTableEntry(ushort FileNumber, ushort ID)
        {
            BinaryReader BR = FFXIResourceManager.OpenDATFile(FileNumber);
            try
            {
                if (BR != null)
                {
                    BR.BaseStream.Position = 0x18;
                    // FIXME: Assumes single-string table; code should be made more generic.
                    uint HeaderBytes = BR.ReadUInt32();
                    uint EntryBytes = BR.ReadUInt32();
                    BR.ReadUInt32();
                    uint DataBytes = BR.ReadUInt32();
                    if (HeaderBytes == 0x40 && ID * 8 < EntryBytes && HeaderBytes + EntryBytes + DataBytes == BR.BaseStream.Length)
                    {
                        BR.BaseStream.Position = 0x40 + ID * 8;
                        uint Offset = (BR.ReadUInt32() ^ 0xFFFFFFFF);
                        uint Length = (BR.ReadUInt32() ^ 0xFFFFFFFF) - 40;
                        if (Length >= 0 && 40 + Offset + Length <= DataBytes)
                        {
                            BR.BaseStream.Position = HeaderBytes + EntryBytes + 40 + Offset;
                            byte[] TextBytes = BR.ReadBytes((int)Length);
                            for (uint i = 0; i < TextBytes.Length; ++i)
                            {
                                TextBytes[i] ^= 0xff;
                            }
                            return E.GetString(TextBytes).TrimEnd('\0');
                        }
                    }
                }
            }
            catch
            {
                // ignore
            }
            finally
            {
                BR.Close();
            }
            return null;
        }

        private static string MaybeExpandAutoTranslatorMessage(string Text)
        {
            // Reference to a string table entry? => return referenced string
            if (Text != null && Text.Length > 2 && Text.Length <= 6 && Text[0] == '@')
            {
                char ReferenceType = Text[1];
                try
                {
                    ushort ID = ushort.Parse(Text.Substring(2), NumberStyles.AllowHexSpecifier);
                    switch (ReferenceType)
                    {
                    case 'A':
                        return FFXIResourceManager.GetAreaName(ID);
                    case 'C':
                        return FFXIResourceManager.GetSpellName(ID);
                    case 'J':
                        return FFXIResourceManager.GetJobName(ID);
                    case 'Y':
                        return FFXIResourceManager.GetAbilityName(ID);
                    }
                }
                catch {}
            }
            return Text;
        }

        private static BinaryReader OpenDATFile(ushort FileNumber)
        {
            try
            {
                string FullDATFileName = FFXI.GetFilePath(FileNumber);
                if (File.Exists(FullDATFileName))
                {
                    return new BinaryReader(new FileStream(FullDATFileName, FileMode.Open, FileAccess.Read));
                }
            }
            catch {}
            return null;
        }
    }
}
