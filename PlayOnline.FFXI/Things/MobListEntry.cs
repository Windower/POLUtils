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
using PlayOnline.Core;

namespace PlayOnline.FFXI.Things
{
    public class MobListEntry : Thing
    {
        public MobListEntry()
        {
            // Clear fields
            this.Clear();
        }

        public override string ToString() { return this.Name_; }

        public override List<PropertyPages.IThing> GetPropertyPages() { return base.GetPropertyPages(); }

        #region Fields

        public static List<string> AllFields
        {
            get { return new List<string>(new string[] { "id", "name", }); }
        }

        public override List<string> GetAllFields() { return MobListEntry.AllFields; }

        #region Data Fields

        private uint? ID_;
        private string Name_;

        #endregion

        public override void Clear()
        {
            this.ID_ = null;
            this.Name_ = null;
        }

        #endregion

        #region Field Access

        public override bool HasField(string Field)
        {
            switch (Field)
            {
            case "id":
                return this.ID_.HasValue;
            case "name":
                return (this.Name_ != null);
            default:
                return false;
            }
        }

        public override string GetFieldText(string Field)
        {
            switch (Field)
            {
            case "id":
                return (!this.ID_.HasValue ? String.Empty : String.Format("{0:X8}", this.ID_.Value));
            case "name":
                return this.Name_;
            default:
                return null;
            }
        }

        public override object GetFieldValue(string Field)
        {
            switch (Field)
            {
            case "id":
                return (!this.ID_.HasValue ? null : (object)this.ID_.Value);
            case "name":
                return this.Name_;
            default:
                return null;
            }
        }

        protected override void LoadField(string Field, System.Xml.XmlElement Node)
        {
            switch (Field)
            {
            case "id":
                this.ID_ = (uint)this.LoadUnsignedIntegerField(Node);
                break;
            case "name":
                this.Name_ = this.LoadTextField(Node);
                break;
            }
        }

        #endregion

        #region ROM File Reading

        public bool Read(BinaryReader BR)
        {
            this.Clear();
            try
            {
                FFXIEncoding E = new FFXIEncoding();
                this.Name_ = E.GetString(BR.ReadBytes(0x1C)).TrimEnd('\0');
                this.ID_ = BR.ReadUInt32();
                // ID seems to be 010 + zone id + mob id (=> there's a hard max of 0xFFF (= 4095) mobs per zone, which seems plenty :))
                // Special 'instanced' zones like MMM or Meebles use 013 + 'zone id' + mob id.
                if ((this.ID_ != 0 && (this.ID_ & 0xFFF00000) != 0x01000000) &&
                    (this.ID_ != 0 && (this.ID_ & 0xFFF00000) != 0x01300000) &&
                    (this.ID_ != 0 && (this.ID_ & 0xFFF00000) != 0x01100000))
                {
                    return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }
}
