// Copyright © 2004-2014 Tim Van Holder, Windower Team
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS"
// BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using PlayOnline.Core;

namespace PlayOnline.FFXI.Things
{
    public class StatusInfo : Thing
    {
        public StatusInfo()
        {
            // Fill Thing helpers
            this.IconField_ = "icon";
            // Clear fields
            this.Clear();
        }

        public override string ToString() { return this.Description_; }

        public override List<PropertyPages.IThing> GetPropertyPages() { return base.GetPropertyPages(); }

        #region Fields

        public static List<string> AllFields
        {
            get
            {
                return
                    new List<string>(new string[]
                    { "id", "description", "icon", "unknown-1", "unknown-2", "unknown-3", "unknown-4", "unknown-5", });
            }
        }

        public override List<string> GetAllFields() { return StatusInfo.AllFields; }

        #region Data Fields

        // General
        private ushort? ID_;
        private string Description_;
        private ushort? Unknown1_;
        private uint? Unknown2_;
        private uint? Unknown3_;
        private uint? Unknown4_;
        private uint? Unknown5_;
        private Graphic Icon_;

        #endregion

        public override void Clear()
        {
            if (this.Icon_ != null)
            {
                this.Icon_.Clear();
            }
            this.ID_ = null;
            this.Description_ = null;
            this.Unknown1_ = null;
            this.Unknown2_ = null;
            this.Unknown3_ = null;
            this.Unknown4_ = null;
            this.Unknown5_ = null;
        }

        #endregion

        #region Field Access

        public override bool HasField(string Field)
        {
            switch (Field)
            {
                // Objects
            case "description":
                return (this.Description_ != null);
            case "icon":
                return (this.Icon_ != null);
                // Nullables
            case "id":
                return this.ID_.HasValue;
            case "unknown-1":
                return this.Unknown1_.HasValue;
            case "unknown-2":
                return this.Unknown2_.HasValue;
            case "unknown-3":
                return this.Unknown3_.HasValue;
            case "unknown-4":
                return this.Unknown4_.HasValue;
            case "unknown-5":
                return this.Unknown5_.HasValue;
            default:
                return false;
            }
        }

        public override string GetFieldText(string Field)
        {
            switch (Field)
            {
                // Strings
            case "description":
                return this.Description_;
                // Objects
            case "icon":
                return this.Icon_.ToString();
                // Nullables
            case "id":
                return (!this.ID_.HasValue ? String.Empty : String.Format("{0}", this.ID_.Value));
                // Nullables - Hex form
            case "unknown-1":
                return (!this.Unknown1_.HasValue ? String.Empty : String.Format("{0:X4}", this.Unknown1_.Value));
            case "unknown-2":
                return (!this.Unknown2_.HasValue ? String.Empty : String.Format("{0:X8}", this.Unknown2_.Value));
            case "unknown-3":
                return (!this.Unknown3_.HasValue ? String.Empty : String.Format("{0:X8}", this.Unknown3_.Value));
            case "unknown-4":
                return (!this.Unknown4_.HasValue ? String.Empty : String.Format("{0:X8}", this.Unknown4_.Value));
            case "unknown-5":
                return (!this.Unknown5_.HasValue ? String.Empty : String.Format("{0:X8}", this.Unknown5_.Value));
            default:
                return null;
            }
        }

        public override object GetFieldValue(string Field)
        {
            switch (Field)
            {
                // Objects
            case "description":
                return this.Description_;
            case "icon":
                return this.Icon_;
                // Nullables
            case "id":
                return (this.ID_.HasValue ? (object)this.ID_.Value : null);
            case "unknown-1":
                return (this.Unknown1_.HasValue ? (object)this.Unknown1_.Value : null);
            case "unknown-2":
                return (this.Unknown2_.HasValue ? (object)this.Unknown2_.Value : null);
            case "unknown-3":
                return (this.Unknown3_.HasValue ? (object)this.Unknown3_.Value : null);
            case "unknown-4":
                return (this.Unknown4_.HasValue ? (object)this.Unknown4_.Value : null);
            case "unknown-5":
                return (this.Unknown5_.HasValue ? (object)this.Unknown5_.Value : null);
            default:
                return null;
            }
        }

        protected override void LoadField(string Field, System.Xml.XmlElement Node)
        {
            switch (Field)
            {
            case "description":
                this.Description_ = this.LoadTextField(Node);
                break;
            case "id":
                this.ID_ = (ushort)this.LoadUnsignedIntegerField(Node);
                break;
            case "unknown-1":
                this.Unknown1_ = (ushort)this.LoadUnsignedIntegerField(Node);
                break;
            case "unknown-2":
                this.Unknown2_ = (uint)this.LoadUnsignedIntegerField(Node);
                break;
            case "unknown-3":
                this.Unknown3_ = (uint)this.LoadUnsignedIntegerField(Node);
                break;
            case "unknown-4":
                this.Unknown4_ = (uint)this.LoadUnsignedIntegerField(Node);
                break;
            case "unknown-5":
                this.Unknown5_ = (uint)this.LoadUnsignedIntegerField(Node);
                break;
            case "icon":
                if (this.Icon_ == null)
                {
                    this.Icon_ = new Graphic();
                }
                this.LoadThingField(Node, this.Icon_);
                break;
            }
        }

        #endregion

        #region ROM File Reading

        // Block Layout:
        // 000-003 U32 Index
        // 004-007 U32 Unknown
        // 008-00b U32 Unknown
        // 00c-00f U32 Unknown
        // 010-013 U32 Unknown
        // 014-02b     Unknown
        // 02c-0ab TXT Description
        // 0ac-27f     Unknown
        // 280-281 U16 Icon Size
        // 282-bff IMG Icon (+ padding)
        public bool Read(BinaryReader BR)
        {
            this.Clear();
            try
            {
                byte[] Bytes = BR.ReadBytes(0x280);
                byte[] IconBytes = BR.ReadBytes(0x980);
                if (!FFXIEncryption.DecodeDataBlock(Bytes))
                {
                    return false;
                }
                if (IconBytes[0x97f] != 0xff)
                {
                    return false;
                }
                {
                    // Verify that the icon info is valid
                    Graphic StatusIcon = new Graphic();
                    BinaryReader IconBR = new BinaryReader(new MemoryStream(IconBytes, false));
                    int IconSize = IconBR.ReadInt32();
                    if (IconSize > 0 && IconSize <= 0x97b)
                    {
                        if (!StatusIcon.Read(IconBR) || IconBR.BaseStream.Position != 4 + IconSize)
                        {
                            IconBR.Close();
                            return false;
                        }
                    }
                    IconBR.Close();
                    if (StatusIcon == null)
                    {
                        return false;
                    }
                    this.Icon_ = StatusIcon;
                }
                BR = new BinaryReader(new MemoryStream(Bytes, false));
            }
            catch
            {
                return false;
            }
            FFXIEncoding E = new FFXIEncoding();
            this.ID_ = BR.ReadUInt16();
            this.Unknown1_ = BR.ReadUInt16();
            this.Unknown2_ = BR.ReadUInt32();
            this.Unknown3_ = BR.ReadUInt32();
            this.Unknown4_ = BR.ReadUInt32();
            this.Unknown4_ = BR.ReadUInt32();
            BR.ReadBytes(0x18);
            this.Description_ = E.GetString(BR.ReadBytes(128)).TrimEnd('\0');
            BR.Close();
            return true;
        }

        #endregion
    }
}
