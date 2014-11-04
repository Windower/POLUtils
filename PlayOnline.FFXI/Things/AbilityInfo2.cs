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
using PlayOnline.Core;

namespace PlayOnline.FFXI.Things
{
    public class AbilityInfo2 : Thing
    {
        public AbilityInfo2()
        {
            // Clear fields
            this.Clear();
        }

        public override string ToString() { return String.Format("Ability #{0}", this.ID_); }

        public override List<PropertyPages.IThing> GetPropertyPages() { return base.GetPropertyPages(); }

        #region Fields

        public static List<string> AllFields
        {
            get
            {
                return
                    new List<string>(new string[]
                    {
                        "id", "type", "list-icon-id", "mp-cost", "valid-targets", "shared-timer-id", "tp-cost", "category-id",
                        "unknown-1",
                    });
            }
        }

        public override List<string> GetAllFields() { return AbilityInfo2.AllFields; }

        #region Data Fields

        private ushort? ID_;
        private AbilityType? Type_;
        private byte? ListIconID_;
        private ushort? MPCost_;
        private ushort? SharedTimerID_;
        private ValidTarget? ValidTargets_;
        private byte? TPCost_;
        private byte? CategoryID_;
        private ushort? Unknown1_;

        #endregion

        public override void Clear()
        {
            this.ID_ = null;
            this.Type_ = null;
            this.ListIconID_ = null;
            this.MPCost_ = null;
            this.SharedTimerID_ = null;
            this.ValidTargets_ = null;
            this.TPCost_ = null;
            this.CategoryID_ = null;
            this.Unknown1_ = null;
        }

        #endregion

        #region Field Access

        public override bool HasField(string Field)
        {
            switch (Field)
            {
                // Nullables
            case "category-id":
                return this.CategoryID_.HasValue;
            case "id":
                return this.ID_.HasValue;
            case "list-icon-id":
                return this.ListIconID_.HasValue;
            case "mp-cost":
                return this.MPCost_.HasValue;
            case "shared-timer-id":
                return this.SharedTimerID_.HasValue;
            case "tp-cost":
                return this.TPCost_.HasValue;
            case "type":
                return this.Type_.HasValue;
            case "valid-targets":
                return this.ValidTargets_.HasValue;
            case "unknown-1":
                return this.Unknown1_.HasValue;
            default:
                return false;
            }
        }

        public override string GetFieldText(string Field)
        {
            switch (Field)
            {
                // Nullables - Simple Values
            case "id":
                return (!this.ID_.HasValue ? String.Empty : String.Format("{0}", this.ID_.Value));
            case "list-icon-id":
                return (!this.ListIconID_.HasValue ? String.Empty : String.Format("{0}", this.ListIconID_.Value));
            case "mp-cost":
                return (!this.MPCost_.HasValue ? String.Empty : String.Format("{0}", this.MPCost_.Value));
            case "shared-timer-id":
                return (!this.SharedTimerID_.HasValue ? String.Empty : String.Format("{0}", this.SharedTimerID_.Value));
            case "type":
                return (!this.Type_.HasValue ? String.Empty : String.Format("{0}", this.Type_.Value));
            case "valid-targets":
                return (!this.ValidTargets_.HasValue ? String.Empty : String.Format("{0}", this.ValidTargets_.Value));
                // Nullables - Hex Values
            case "unknown-1":
                return (!this.Unknown1_.HasValue ? String.Empty : String.Format("{0:X2} ({0})", this.Unknown1_.Value));
                // Category ID: Blank when zero
            case "category-id":
                if (!this.CategoryID_.HasValue || this.CategoryID_.Value == 0)
                {
                    return String.Empty;
                }
                else
                {
                    return String.Format("{0}", this.CategoryID_.Value);
                }
                // TP Cost: show as "nn%", or blank if not applicable
            case "tp-cost":
                if (!this.TPCost_.HasValue)
                {
                    return String.Empty;
                }
                else
                {
                    return String.Format("{0}%", this.TPCost_.Value);
                }
            default:
                return null;
            }
        }

        public override object GetFieldValue(string Field)
        {
            switch (Field)
            {
                // Nullables
            case "category-id":
                return (!this.CategoryID_.HasValue ? null : (object)this.CategoryID_.Value);
            case "id":
                return (!this.ID_.HasValue ? null : (object)this.ID_.Value);
            case "list-icon-id":
                return (!this.ListIconID_.HasValue ? null : (object)this.ListIconID_.Value);
            case "mp-cost":
                return (!this.MPCost_.HasValue ? null : (object)this.MPCost_.Value);
            case "shared-timer-id":
                return (!this.SharedTimerID_.HasValue ? null : (object)this.SharedTimerID_.Value);
            case "tp-cost":
                return (!this.TPCost_.HasValue ? null : (object)this.TPCost_.Value);
            case "type":
                return (!this.Type_.HasValue ? null : (object)this.Type_.Value);
            case "valid-targets":
                return (!this.ValidTargets_.HasValue ? null : (object)this.ValidTargets_.Value);
            case "unknown-1":
                return (!this.Unknown1_.HasValue ? null : (object)this.Unknown1_.Value);
            default:
                return null;
            }
        }

        protected override void LoadField(string Field, System.Xml.XmlElement Node)
        {
            switch (Field)
            {
                // "Simple" Fields
            case "category-id":
                this.CategoryID_ = (byte)this.LoadUnsignedIntegerField(Node);
                break;
            case "id":
                this.ID_ = (ushort)this.LoadUnsignedIntegerField(Node);
                break;
            case "list-icon-id":
                this.ListIconID_ = (byte)this.LoadUnsignedIntegerField(Node);
                break;
            case "mp-cost":
                this.MPCost_ = (ushort)this.LoadUnsignedIntegerField(Node);
                break;
            case "shared-timer-id":
                this.SharedTimerID_ = (ushort)this.LoadUnsignedIntegerField(Node);
                break;
            case "tp-cost":
                this.TPCost_ = (byte)this.LoadUnsignedIntegerField(Node);
                break;
            case "type":
                this.Type_ = (AbilityType)this.LoadHexField(Node);
                break;
            case "valid-targets":
                this.ValidTargets_ = (ValidTarget)this.LoadHexField(Node);
                break;
            case "unknown-1":
                this.Unknown1_ = (byte)this.LoadUnsignedIntegerField(Node);
                break;
            }
        }

        #endregion

        #region ROM File Reading

        // Block Layout:
        // 000-001 U16 Index
        // 002-002 U8  Type
        // 003-003 U8  List Icon ID (e.g. 40-47 for the elemental-colored dots)
        // 004-005 U16 Unknown #1
        // 006-007 U16 MP Cost
        // 008-009 U16 Shared Timer ID
        // 00a-00b U16 Valid Targets
        // 00c-00c I8  TP Cost (percentage, or -1 if not applicable)
        // 00d-00d U8  Category ID (for entries that are categories instead of real abilities)
        // 010-02e U8  Padding (NULs)
        // 02f-02f U8  End marker (0xff)
        public bool Read(BinaryReader BR)
        {
            this.Clear();
            try
            {
                byte[] Bytes = BR.ReadBytes(0x30);
                if (Bytes[0x9] > 0xc0 || Bytes[0x2f] != 0xff)
                {
                    return false;
                }
                if (!FFXIEncryption.DecodeDataBlockMask(Bytes))
                {
                    return false;
                }
                FFXIEncoding E = new FFXIEncoding();
                BR = new BinaryReader(new MemoryStream(Bytes, false));
            }
            catch
            {
                return false;
            }
            this.ID_ = BR.ReadUInt16();
            this.Type_ = (AbilityType)BR.ReadByte();
            this.ListIconID_ = BR.ReadByte();
            this.Unknown1_ = BR.ReadUInt16();
            this.MPCost_ = BR.ReadUInt16();
            this.SharedTimerID_ = BR.ReadUInt16();
            this.ValidTargets_ = (ValidTarget)BR.ReadUInt16();
            this.TPCost_ = BR.ReadByte();
            this.CategoryID_ = BR.ReadByte();
            this.Unknown1_ = BR.ReadUInt16();
#if DEBUG // Check the padding bytes for unexpected data
            for (byte i = 0; i < 31; ++i)
            {
                byte PaddingByte = BR.ReadByte();
                if (PaddingByte != 0)
                {
                    Console.WriteLine("AbilityInfo2: Entry #{0}: Padding Byte #{1} is non-zero: {2:X2} ({2})", this.ID_, i + 1,
                        PaddingByte);
                }
            }
#endif
            BR.Close();
            return true;
        }

        #endregion
    }
}
