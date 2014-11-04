// Copyright © 2004-2014 Tim Van Holder, Windower Team
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS"
// BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

#if DEBUG
# define IncludeUnknownFields
#endif

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using PlayOnline.Core;

namespace PlayOnline.FFXI.Things
{
    public class DMSGStringTableEntry : Thing
    {
        public DMSGStringTableEntry()
        {
            // Clear fields
            this.Clear();
        }

        public override string ToString() { return this.Text_; }

        public override List<PropertyPages.IThing> GetPropertyPages() { return base.GetPropertyPages(); }

        #region Fields

        public static List<string> AllFields
        {
            get
            {
                return new List<string>(new string[]
                {
                    "index", "text",
#if IncludeUnknownFields
                    "unknown-1", "unknown-2", "unknown-3", "unknown-4", "unknown-5", "unknown-6", "unknown-7", "unknown-8",
                    "unknown-9",
#endif
                });
            }
        }

        public override List<string> GetAllFields() { return DMSGStringTableEntry.AllFields; }

        #region Data Fields

        private uint? Index_;
        private string Text_;
#if IncludeUnknownFields
        private uint? Unknown1_;
        private ushort? Unknown2_;
        private uint? Unknown3_;
        private uint? Unknown4_;
        private uint? Unknown5_;
        private uint? Unknown6_;
        private uint? Unknown7_;
        private ushort? Unknown8_;
        private ushort? Unknown9_;
#endif

        #endregion

        public override void Clear()
        {
            this.Index_ = null;
            this.Text_ = null;
#if IncludeUnknownFields
            this.Unknown1_ = null;
#endif
        }

        #endregion

        #region Field Access

        public override bool HasField(string Field)
        {
            switch (Field)
            {
            case "index":
                return this.Index_.HasValue;
            case "text":
                return (this.Text_ != null);
#if IncludeUnknownFields
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
            case "unknown-6":
                return this.Unknown6_.HasValue;
            case "unknown-7":
                return this.Unknown7_.HasValue;
            case "unknown-8":
                return this.Unknown8_.HasValue;
            case "unknown-9":
                return this.Unknown9_.HasValue;
#endif
            default:
                return false;
            }
        }

        public override string GetFieldText(string Field)
        {
            switch (Field)
            {
            case "index":
                return (!this.Index_.HasValue ? String.Empty : String.Format("{0:00000}", this.Index_.Value));
            case "text":
                return this.Text_;
#if IncludeUnknownFields
            case "unknown-1":
                return (!this.Unknown1_.HasValue ? String.Empty : String.Format("{0:X8} ({0})", this.Unknown1_.Value));
            case "unknown-2":
                return (!this.Unknown2_.HasValue ? String.Empty : String.Format("{0:X4} ({0})", this.Unknown2_.Value));
            case "unknown-3":
                return (!this.Unknown3_.HasValue ? String.Empty : String.Format("{0:X8} ({0})", this.Unknown3_.Value));
            case "unknown-4":
                return (!this.Unknown4_.HasValue ? String.Empty : String.Format("{0:X8} ({0})", this.Unknown4_.Value));
            case "unknown-5":
                return (!this.Unknown5_.HasValue ? String.Empty : String.Format("{0:X8} ({0})", this.Unknown5_.Value));
            case "unknown-6":
                return (!this.Unknown6_.HasValue ? String.Empty : String.Format("{0:X8} ({0})", this.Unknown6_.Value));
            case "unknown-7":
                return (!this.Unknown7_.HasValue ? String.Empty : String.Format("{0:X8} ({0})", this.Unknown7_.Value));
            case "unknown-8":
                return (!this.Unknown8_.HasValue ? String.Empty : String.Format("{0:X4} ({0})", this.Unknown8_.Value));
            case "unknown-9":
                return (!this.Unknown9_.HasValue ? String.Empty : String.Format("{0:X4} ({0})", this.Unknown9_.Value));
#endif
            default:
                return null;
            }
        }

        public override object GetFieldValue(string Field)
        {
            switch (Field)
            {
            case "index":
                return (!this.Index_.HasValue ? null : (object)this.Index_.Value);
            case "text":
                return this.Text_;
#if IncludeUnknownFields
            case "unknown-1":
                return (!this.Unknown1_.HasValue ? null : (object)this.Unknown1_.Value);
            case "unknown-2":
                return (!this.Unknown2_.HasValue ? null : (object)this.Unknown2_.Value);
            case "unknown-3":
                return (!this.Unknown3_.HasValue ? null : (object)this.Unknown3_.Value);
            case "unknown-4":
                return (!this.Unknown4_.HasValue ? null : (object)this.Unknown4_.Value);
            case "unknown-5":
                return (!this.Unknown5_.HasValue ? null : (object)this.Unknown5_.Value);
            case "unknown-6":
                return (!this.Unknown6_.HasValue ? null : (object)this.Unknown6_.Value);
            case "unknown-7":
                return (!this.Unknown7_.HasValue ? null : (object)this.Unknown7_.Value);
            case "unknown-8":
                return (!this.Unknown8_.HasValue ? null : (object)this.Unknown8_.Value);
            case "unknown-9":
                return (!this.Unknown9_.HasValue ? null : (object)this.Unknown9_.Value);
#endif
            default:
                return null;
            }
        }

        protected override void LoadField(string Field, System.Xml.XmlElement Node)
        {
            switch (Field)
            {
            case "index":
                this.Index_ = (uint)this.LoadUnsignedIntegerField(Node);
                break;
            case "text":
                this.Text_ = this.LoadTextField(Node);
                break;
#if IncludeUnknownFields
            case "unknown-1":
                this.Unknown1_ = (uint)this.LoadUnsignedIntegerField(Node);
                break;
            case "unknown-2":
                this.Unknown2_ = (ushort)this.LoadUnsignedIntegerField(Node);
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
            case "unknown-6":
                this.Unknown6_ = (uint)this.LoadUnsignedIntegerField(Node);
                break;
            case "unknown-7":
                this.Unknown7_ = (uint)this.LoadUnsignedIntegerField(Node);
                break;
            case "unknown-8":
                this.Unknown8_ = (ushort)this.LoadUnsignedIntegerField(Node);
                break;
            case "unknown-9":
                this.Unknown9_ = (ushort)this.LoadUnsignedIntegerField(Node);
                break;
#endif
            }
        }

        #endregion

        #region ROM File Reading

        public bool Read(BinaryReader BR, Encoding E, uint EntryBytes, uint DataBytes)
        {
            return this.Read(BR, E, null, EntryBytes, DataBytes);
        }

        public bool Read(BinaryReader BR, Encoding E, uint? Index, uint EntryBytes, uint DataBytes)
        {
            this.Clear();
            this.Index_ = Index;
            BR.BaseStream.Seek(0x38 + 0x24 * Index.Value, SeekOrigin.Begin);
            long IndexPos = -1;
            try
            {
                uint Offset = BR.ReadUInt32();
#if IncludeUnknownFields
                this.Unknown1_ = BR.ReadUInt32();
#else
	BR.BaseStream.Position += 4;
#endif
                short Size = BR.ReadInt16();
#if IncludeUnknownFields
                this.Unknown2_ = BR.ReadUInt16();
                this.Unknown3_ = BR.ReadUInt32();
                this.Unknown4_ = BR.ReadUInt32();
                this.Unknown5_ = BR.ReadUInt32();
                this.Unknown6_ = BR.ReadUInt32();
                this.Unknown7_ = BR.ReadUInt32();
                this.Unknown8_ = BR.ReadUInt16();
                this.Unknown9_ = BR.ReadUInt16();
#else
	BR.BaseStream.Position += 26;
#endif
                if (Size < 0 || Offset + Size > DataBytes)
                {
                    return false;
                }
                IndexPos = BR.BaseStream.Position;
                BR.BaseStream.Seek(0x38 + EntryBytes + Offset, SeekOrigin.Begin);
                this.Text_ = E.GetString(BR.ReadBytes(Size)).TrimEnd('\0');
                return true;
            }
            catch {}
            return false;
        }

        #endregion
    }
}
