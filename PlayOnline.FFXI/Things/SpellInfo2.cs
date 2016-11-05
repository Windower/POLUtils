// Copyright © 2010-2012 Chris Baggett, Tim Van Holder, Nevin Stepan
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS"
// BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using System;
using System.Collections.Generic;
using System.IO;

namespace PlayOnline.FFXI.Things
{
    public class SpellInfo2 : Thing
    {
        public SpellInfo2()
        {
            // Clear fields
            Clear();
        }

        public override string ToString() { return $"Spell #{Index_}"; }

        #region Fields

        public static List<string> AllFields => new List<string>(new []
        {
            "id", "index", "magic-type", "element", "valid-targets", "skill", "level-required", "mp-cost", "casting-time",
            "recast-delay", "list-icon-id", "unknown-1", "unknown-2", "unknown-3", "unknown-4", "unknown-5",
        });

        public override List<string> GetAllFields() { return AllFields; }

        #region Data Fields

        private ushort? Index_;
        private MagicType? MagicType_;
        private Element? Element_;
        private ValidTarget? ValidTargets_;
        private Skill? Skill_;
        private ushort? MPCost_;
        private byte? CastingTime_;
        private ushort? RecastDelay_;
        private int[] LevelRequired_;
        private ushort? ID_;
        private byte? ListIconID_;
        private byte? Unknown1_;
        private byte? Unknown2_;
        private byte? Unknown3_;
        private byte? Unknown4_;
        private uint? Unknown5_;

        #endregion

        public sealed override void Clear()
        {
            Index_ = null;
            MagicType_ = null;
            Element_ = null;
            ValidTargets_ = null;
            Skill_ = null;
            MPCost_ = null;
            CastingTime_ = null;
            RecastDelay_ = null;
            LevelRequired_ = null;
            ID_ = null;
            ListIconID_ = null;
            Unknown1_ = null;
            Unknown2_ = null;
            Unknown3_ = null;
            Unknown4_ = null;
        }

        #endregion

        #region Field Access

        public override bool HasField(string Field)
        {
            switch (Field)
            {
                // Objects
            case "level-required":
                return LevelRequired_ != null;
                // Nullables
            case "casting-time":
                return CastingTime_.HasValue;
            case "element":
                return Element_.HasValue;
            case "list-icon-id":
                return ListIconID_.HasValue;
            case "id":
                return ID_.HasValue;
            case "index":
                return Index_.HasValue;
            case "magic-type":
                return MagicType_.HasValue;
            case "mp-cost":
                return MPCost_.HasValue;
            case "recast-delay":
                return RecastDelay_.HasValue;
            case "skill":
                return Skill_.HasValue;
            case "unknown-1":
                return Unknown1_.HasValue;
            case "unknown-2":
                return Unknown2_.HasValue;
            case "unknown-3":
                return Unknown3_.HasValue;
            case "unknown-4":
                return Unknown4_.HasValue;
            case "unknown-5":
                return Unknown5_.HasValue;
            case "valid-targets":
                return ValidTargets_.HasValue;
            default:
                return false;
            }
        }

        public override string GetFieldText(string Field)
        {
            switch (Field)
            {
                // Special Values
            case "level-required":
            {
                string LevelInfo = string.Empty;
                if (LevelRequired_ == null || LevelRequired_.Length > sizeof(Job) * 8)
                {
                    return LevelInfo;
                }
                for (int i = 0; i < LevelRequired_.Length; ++i)
                {
                    if (LevelRequired_[i] != 0xff)
                    {
                        if (LevelInfo != string.Empty)
                        {
                            LevelInfo += '/';
                        }
                        LevelInfo += $"{LevelRequired_[i]:00}{(Job)(1 << i)}";
                    }
                }
                return LevelInfo;
            }
                // Nullables - Simple Values
            case "element":
                return !Element_.HasValue ? string.Empty : $"{Element_.Value}";
            case "list-icon-id":
                return !ListIconID_.HasValue ? string.Empty : $"{ListIconID_.Value}";
            case "id":
                return !ID_.HasValue ? string.Empty : $"{ID_.Value:000}";
            case "index":
                return !Index_.HasValue ? string.Empty : $"{Index_.Value:000}";
            case "magic-type":
                return !MagicType_.HasValue ? string.Empty : $"{MagicType_.Value}";
            case "mp-cost":
                return !MPCost_.HasValue ? string.Empty : $"{MPCost_.Value}";
            case "skill":
                return !Skill_.HasValue ? string.Empty : $"{Skill_.Value}";
            case "valid-targets":
                return !ValidTargets_.HasValue ? string.Empty : $"{ValidTargets_.Value}";
                // Nullables - Hex Values
            case "unknown-1":
                return !Unknown1_.HasValue ? string.Empty : string.Format("{0:X2} ({0})", Unknown1_.Value);
            case "unknown-2":
                return !Unknown2_.HasValue ? string.Empty : string.Format("{0:X2} ({0})", Unknown2_.Value);
            case "unknown-3":
                return !Unknown3_.HasValue ? string.Empty : string.Format("{0:X2} ({0})", Unknown3_.Value);
            case "unknown-4":
                return !Unknown4_.HasValue ? string.Empty : string.Format("{0:X2} ({0})", Unknown4_.Value);
                // Nullables - Time Values
            case "casting-time":
                return !CastingTime_.HasValue ? string.Empty : FormatTime(CastingTime_.Value / 4.0);
            case "recast-delay":
                return !RecastDelay_.HasValue ? string.Empty : FormatTime(RecastDelay_.Value / 4.0);
            default:
                return null;
            }
        }

        public override object GetFieldValue(string Field)
        {
            switch (Field)
            {
                // Objects
            case "level-required":
                return LevelRequired_;
                // Nullables
            case "casting-time":
                return !CastingTime_.HasValue ? null : (object)CastingTime_.Value;
            case "element":
                return !Element_.HasValue ? null : (object)Element_.Value;
            case "list-icon-id":
                return !ListIconID_.HasValue ? null : (object)ListIconID_.Value;
            case "id":
                return !ID_.HasValue ? null : (object)ID_.Value;
            case "index":
                return !Index_.HasValue ? null : (object)Index_.Value;
            case "magic-type":
                return !MagicType_.HasValue ? null : (object)MagicType_.Value;
            case "mp-cost":
                return !MPCost_.HasValue ? null : (object)MPCost_.Value;
            case "recast-delay":
                return !RecastDelay_.HasValue ? null : (object)RecastDelay_.Value;
            case "skill":
                return !Skill_.HasValue ? null : (object)Skill_.Value;
            case "unknown-1":
                return !Unknown1_.HasValue ? null : (object)Unknown1_.Value;
            case "unknown-2":
                return !Unknown2_.HasValue ? null : (object)Unknown2_.Value;
            case "unknown-3":
                return !Unknown3_.HasValue ? null : (object)Unknown3_.Value;
            case "unknown-4":
                return !Unknown4_.HasValue ? null : (object)Unknown4_.Value;
            case "unknown-5":
                return !Unknown5_.HasValue ? null : (object)Unknown5_.Value;
            case "valid-targets":
                return !ValidTargets_.HasValue ? null : (object)ValidTargets_.Value;
            default:
                return null;
            }
        }

        protected override void LoadField(string Field, System.Xml.XmlElement Node)
        {
            switch (Field)
            {
                // "Simple" Fields
            case "casting-time":
                CastingTime_ = (byte)LoadUnsignedIntegerField(Node);
                break;
            case "element":
                Element_ = (Element)LoadHexField(Node);
                break;
            case "list-icon-id":
                ListIconID_ = (byte)LoadUnsignedIntegerField(Node);
                break;
            case "id":
                ID_ = (ushort)LoadUnsignedIntegerField(Node);
                break;
            case "index":
                Index_ = (ushort)LoadUnsignedIntegerField(Node);
                break;
            case "level-required":
                LevelRequired_ = LoadIntegerArray<int>(Node);
                break;
            case "magic-type":
                MagicType_ = (MagicType)LoadHexField(Node);
                break;
            case "mp-cost":
                MPCost_ = (ushort)LoadUnsignedIntegerField(Node);
                break;
            case "recast-delay":
                RecastDelay_ = (ushort)LoadUnsignedIntegerField(Node);
                break;
            case "skill":
                Skill_ = (Skill)LoadHexField(Node);
                break;
            case "unknown-1":
                Unknown1_ = (byte)LoadUnsignedIntegerField(Node);
                break;
            case "unknown-2":
                Unknown2_ = (byte)LoadUnsignedIntegerField(Node);
                break;
            case "unknown-3":
                Unknown3_ = (byte)LoadUnsignedIntegerField(Node);
                break;
            case "unknown-4":
                Unknown4_ = (byte)LoadUnsignedIntegerField(Node);
                break;
            case "unknown-5":
                Unknown5_ = (byte)LoadUnsignedIntegerField(Node);
                break;
            case "valid-targets":
                ValidTargets_ = (ValidTarget)LoadHexField(Node);
                break;
            }
        }

        #endregion

        #region ROM File Reading

        // Block Layout:
        // 000-001 U16 Index
        // 002-003 U16 Magic Type (1/2/3/4/5/6 - White/Black/Summon/Ninja/Bard/Blue)
        // 004-005 U16 Element
        // 006-007 U16 Valid Targets
        // 008-009 U16 Skill
        // 00a-00b U16 MP Cost
        // 00c-00c U8  Cast Time (1/4 second)
        // 00d-00d U8  Recast Delay (1/4 second)
        // 00e-025 U8  Level required (1 byte per job, 0xff if not learnable; first is for the NUL job, so always 0xff; only 24 slots despite 32 possible job flags)
        // 026-027 U16 ID (0 for "unused" spells; starts out equal to the index, but doesn't stay that way)
        // 028-028 U8  List Icon ID (not sure what this is an index of, but it seems to match differences in item icon)
        // 029-029 U8  Unknown #1
        // 02a-02b U8  Unknown #2
        // 02c-02d U8  Unknown #3
        // 02e-02f U8  Unknown #4
        // 030-03e U8  Padding (NULs)
        // 03f-03f U8  End marker (0xff)
        public bool Read(BinaryReader BR)
        {
            Clear();
            try
            {
                var Bytes = BR.ReadBytes(0x64);
                if (Bytes[0x3] != 0x00 || Bytes[0x5] != 0x00 || Bytes[0x7] != 0x00 || Bytes[0x9] != 0x00 || Bytes[0xE] != 0xFF || Bytes[0x63] != 0xFF)
                {
                    return false;
                }
                if (!FFXIEncryption.DecodeDataBlockMask(Bytes))
                {
                    return false;
                }
                BR = new BinaryReader(new MemoryStream(Bytes, false));
            }
            catch
            {
                return false;
            }
            Index_ = BR.ReadUInt16();
            MagicType_ = (MagicType)BR.ReadUInt16();
            Element_ = (Element)BR.ReadUInt16();
            ValidTargets_ = (ValidTarget)BR.ReadUInt16();
            Skill_ = (Skill)BR.ReadUInt16();
            MPCost_ = BR.ReadUInt16();
            CastingTime_ = BR.ReadByte();
            RecastDelay_ = BR.ReadByte();
            LevelRequired_ = new int[0x18];
            for (var i = 0; i < 0x18; ++i)
            {
                LevelRequired_[i] = BR.ReadInt16();
            }
            ID_ = BR.ReadUInt16();
            ListIconID_ = BR.ReadByte();
            Unknown1_ = BR.ReadByte();
            Unknown2_ = BR.ReadByte();
            Unknown3_ = BR.ReadByte();
            Unknown4_ = BR.ReadByte();
            Unknown5_ = BR.ReadUInt32();

#if DEBUG // Check the padding bytes for unexpected data
            for (byte i = 0; i < 26; ++i)
            {
                var PaddingByte = BR.ReadByte();
                if (PaddingByte != 0)
                {
                    Console.WriteLine("SpellInfo2: Entry #{0}: Padding Byte #{1} is non-zero: {2:X2} ({2})", Index_, i + 1, PaddingByte);
                }
            }
#endif
            BR.Close();
            return true;
        }

        #endregion
    }
}
