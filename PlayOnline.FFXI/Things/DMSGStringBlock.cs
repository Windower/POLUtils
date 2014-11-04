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
using System.IO;
using System.Text;
using PlayOnline.Core;

namespace PlayOnline.FFXI.Things
{
    public class DMSGStringBlock : Thing
    {
        public DMSGStringBlock()
        {
            // Clear fields
            this.Clear();
        }

        public override string ToString()
        {
            foreach (string S in this.Strings_)
            {
                if (S != null)
                {
                    return S;
                }
            }
            return String.Empty;
        }

        public override List<PropertyPages.IThing> GetPropertyPages() { return base.GetPropertyPages(); }

        #region Fields

        public static List<string> AllFields
        {
            get
            {
                return
                    new List<string>(new string[]
                    {
                        "index", "string-1", "string-2", "string-3", "string-4", "string-5", "string-6", "string-7", "string-8",
                        "string-9", "string-10", "string-11", "string-12", "string-13", "string-14", "string-15",
                        // 15 is an arbitrary limit; the most used so far is 11
                        "string-count"
                    });
            }
        }

        public override List<string> GetAllFields() { return DMSGStringBlock.AllFields; }

        #region Data Fields

        private uint? Index_;
        private List<string> Strings_;
        private int? StringCount_;

        #endregion

        public override void Clear()
        {
            this.Index_ = null;
            this.Strings_ = new List<string>();
            this.StringCount_ = null;
        }

        #endregion

        #region Field Access

        public override bool HasField(string Field)
        {
            switch (Field)
            {
            case "index":
                return this.Index_.HasValue;
            case "string-1":
                return (this.StringCount_.HasValue && this.StringCount_.Value >= 1);
            case "string-2":
                return (this.StringCount_.HasValue && this.StringCount_.Value >= 2);
            case "string-3":
                return (this.StringCount_.HasValue && this.StringCount_.Value >= 3);
            case "string-4":
                return (this.StringCount_.HasValue && this.StringCount_.Value >= 4);
            case "string-5":
                return (this.StringCount_.HasValue && this.StringCount_.Value >= 5);
            case "string-6":
                return (this.StringCount_.HasValue && this.StringCount_.Value >= 6);
            case "string-7":
                return (this.StringCount_.HasValue && this.StringCount_.Value >= 7);
            case "string-8":
                return (this.StringCount_.HasValue && this.StringCount_.Value >= 8);
            case "string-9":
                return (this.StringCount_.HasValue && this.StringCount_.Value >= 9);
            case "string-10":
                return (this.StringCount_.HasValue && this.StringCount_.Value >= 10);
            case "string-11":
                return (this.StringCount_.HasValue && this.StringCount_.Value >= 11);
            case "string-12":
                return (this.StringCount_.HasValue && this.StringCount_.Value >= 12);
            case "string-13":
                return (this.StringCount_.HasValue && this.StringCount_.Value >= 13);
            case "string-14":
                return (this.StringCount_.HasValue && this.StringCount_.Value >= 14);
            case "string-15":
                return (this.StringCount_.HasValue && this.StringCount_.Value >= 15);
            case "string-count":
                return this.StringCount_.HasValue;
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
            case "string-1":
                return ((this.Strings_ == null || this.Strings_.Count < 1 || this.Strings_[0] == null)
                    ? String.Empty
                    : this.Strings_[0]);
            case "string-2":
                return ((this.Strings_ == null || this.Strings_.Count < 2 || this.Strings_[1] == null)
                    ? String.Empty
                    : this.Strings_[1]);
            case "string-3":
                return ((this.Strings_ == null || this.Strings_.Count < 3 || this.Strings_[2] == null)
                    ? String.Empty
                    : this.Strings_[2]);
            case "string-4":
                return ((this.Strings_ == null || this.Strings_.Count < 4 || this.Strings_[3] == null)
                    ? String.Empty
                    : this.Strings_[3]);
            case "string-5":
                return ((this.Strings_ == null || this.Strings_.Count < 5 || this.Strings_[4] == null)
                    ? String.Empty
                    : this.Strings_[4]);
            case "string-6":
                return ((this.Strings_ == null || this.Strings_.Count < 6 || this.Strings_[5] == null)
                    ? String.Empty
                    : this.Strings_[5]);
            case "string-7":
                return ((this.Strings_ == null || this.Strings_.Count < 7 || this.Strings_[6] == null)
                    ? String.Empty
                    : this.Strings_[6]);
            case "string-8":
                return ((this.Strings_ == null || this.Strings_.Count < 8 || this.Strings_[7] == null)
                    ? String.Empty
                    : this.Strings_[7]);
            case "string-9":
                return ((this.Strings_ == null || this.Strings_.Count < 9 || this.Strings_[8] == null)
                    ? String.Empty
                    : this.Strings_[8]);
            case "string-10":
                return ((this.Strings_ == null || this.Strings_.Count < 10 || this.Strings_[9] == null)
                    ? String.Empty
                    : this.Strings_[9]);
            case "string-11":
                return ((this.Strings_ == null || this.Strings_.Count < 11 || this.Strings_[10] == null)
                    ? String.Empty
                    : this.Strings_[10]);
            case "string-12":
                return ((this.Strings_ == null || this.Strings_.Count < 12 || this.Strings_[11] == null)
                    ? String.Empty
                    : this.Strings_[11]);
            case "string-13":
                return ((this.Strings_ == null || this.Strings_.Count < 13 || this.Strings_[12] == null)
                    ? String.Empty
                    : this.Strings_[12]);
            case "string-14":
                return ((this.Strings_ == null || this.Strings_.Count < 14 || this.Strings_[13] == null)
                    ? String.Empty
                    : this.Strings_[13]);
            case "string-15":
                return ((this.Strings_ == null || this.Strings_.Count < 15 || this.Strings_[14] == null)
                    ? String.Empty
                    : this.Strings_[14]);
            case "string-count":
                return (!this.StringCount_.HasValue ? String.Empty : this.StringCount_.ToString());
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
            case "string-1":
                return ((this.Strings_ == null || this.Strings_.Count < 1) ? null : this.Strings_[0]);
            case "string-2":
                return ((this.Strings_ == null || this.Strings_.Count < 2) ? null : this.Strings_[1]);
            case "string-3":
                return ((this.Strings_ == null || this.Strings_.Count < 3) ? null : this.Strings_[2]);
            case "string-4":
                return ((this.Strings_ == null || this.Strings_.Count < 4) ? null : this.Strings_[3]);
            case "string-5":
                return ((this.Strings_ == null || this.Strings_.Count < 5) ? null : this.Strings_[4]);
            case "string-6":
                return ((this.Strings_ == null || this.Strings_.Count < 6) ? null : this.Strings_[5]);
            case "string-7":
                return ((this.Strings_ == null || this.Strings_.Count < 7) ? null : this.Strings_[6]);
            case "string-8":
                return ((this.Strings_ == null || this.Strings_.Count < 8) ? null : this.Strings_[7]);
            case "string-9":
                return ((this.Strings_ == null || this.Strings_.Count < 9) ? null : this.Strings_[8]);
            case "string-10":
                return ((this.Strings_ == null || this.Strings_.Count < 10) ? null : this.Strings_[9]);
            case "string-11":
                return ((this.Strings_ == null || this.Strings_.Count < 11) ? null : this.Strings_[10]);
            case "string-12":
                return ((this.Strings_ == null || this.Strings_.Count < 12) ? null : this.Strings_[11]);
            case "string-13":
                return ((this.Strings_ == null || this.Strings_.Count < 13) ? null : this.Strings_[12]);
            case "string-14":
                return ((this.Strings_ == null || this.Strings_.Count < 14) ? null : this.Strings_[13]);
            case "string-15":
                return ((this.Strings_ == null || this.Strings_.Count < 15) ? null : this.Strings_[14]);
            case "string-count":
                return (!this.StringCount_.HasValue ? null : (object)this.StringCount_.Value);
            default:
                return null;
            }
        }

        private void LoadString(System.Xml.XmlElement Node, int Index)
        {
            while (this.Strings_.Count <= Index)
            {
                this.Strings_.Add(null);
            }
            this.Strings_[Index] = this.LoadTextField(Node);
        }

        protected override void LoadField(string Field, System.Xml.XmlElement Node)
        {
            switch (Field)
            {
            case "index":
                this.Index_ = (uint)this.LoadUnsignedIntegerField(Node);
                break;
            case "string-1":
                this.LoadString(Node, 0);
                break;
            case "string-2":
                this.LoadString(Node, 1);
                break;
            case "string-3":
                this.LoadString(Node, 2);
                break;
            case "string-4":
                this.LoadString(Node, 3);
                break;
            case "string-5":
                this.LoadString(Node, 4);
                break;
            case "string-6":
                this.LoadString(Node, 5);
                break;
            case "string-7":
                this.LoadString(Node, 6);
                break;
            case "string-8":
                this.LoadString(Node, 7);
                break;
            case "string-9":
                this.LoadString(Node, 8);
                break;
            case "string-10":
                this.LoadString(Node, 9);
                break;
            case "string-11":
                this.LoadString(Node, 10);
                break;
            case "string-12":
                this.LoadString(Node, 11);
                break;
            case "string-13":
                this.LoadString(Node, 12);
                break;
            case "string-14":
                this.LoadString(Node, 13);
                break;
            case "string-15":
                this.LoadString(Node, 14);
                break;
            case "string-count":
                this.StringCount_ = (int)this.LoadSignedIntegerField(Node);
                break;
            }
        }

        #endregion

        #region ROM File Reading

        public bool Read(BinaryReader BR, Encoding E, uint Index)
        {
            this.Clear();
            this.Index_ = Index;
            try
            {
                bool need_bitflip = false;
                this.StringCount_ = BR.ReadInt32();
                if (this.StringCount_ < 0 || this.StringCount_ > 100)
                {
                    // 100 is an arbitrary cut-off point
                    this.StringCount_ = ~this.StringCount_;
                    if (this.StringCount_ < 0 || this.StringCount_ > 100)
                    {
                        return false;
                    }
                    else
                    {
                        need_bitflip = true;
                    }
                }
                uint[] StringOffsets = new uint[this.StringCount_.Value];
                uint[] StringFlags = new uint[this.StringCount_.Value];
                for (uint i = 0; i < this.StringCount_.Value; ++i)
                {
                    StringOffsets[i] = BR.ReadUInt32() ^ (need_bitflip ? 0xffffffff : 0x00000000);
                    StringFlags[i] = BR.ReadUInt32() ^ (need_bitflip ? 0xffffffff : 0x00000000);
                    if (StringOffsets[i] < 0 || StringOffsets[i] + 28 + 4 > BR.BaseStream.Length ||
                        (StringFlags[i] != 0 && StringFlags[i] != 1))
                    {
                        return false;
                    }
                }
                // Meaning of flags is unclear so far.
                for (uint i = 0; i < this.StringCount_.Value; ++i)
                {
                    // not sure why the offsets are based 28 bytes past the start of the entry, but they are
                    BR.BaseStream.Position = 28 + StringOffsets[i];
                    List<byte> TextBytes = new List<byte>();
                    while (true)
                    {
                        byte[] FourBytes = BR.ReadBytes(4);
                        if (need_bitflip)
                        {
                            FourBytes[0] ^= 0xff;
                            FourBytes[1] ^= 0xff;
                            FourBytes[2] ^= 0xff;
                            FourBytes[3] ^= 0xff;
                        }
                        TextBytes.AddRange(FourBytes);
                        if (FourBytes[3] == 0)
                        {
                            break;
                        }
                    }
                    this.Strings_.Add(E.GetString(TextBytes.ToArray()).TrimEnd('\0'));
                }
                return true;
            }
            catch {}
            this.Clear();
            return false;
        }

        #endregion
    }
}
