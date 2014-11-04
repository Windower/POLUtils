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
using System.IO;
using Microsoft.Win32;
using PlayOnline.Core;

namespace PlayOnline.FFXI
{
    public class Character
    {
        internal Character(string ContentID)
        {
            this.ID_ = ContentID;
            this.DataDir_ = Path.Combine(POL.GetApplicationPath(AppID.FFXI), Path.Combine("User", ContentID));
        }

        #region Data Members

        public string ID
        {
            get { return this.ID_; }
        }

        public string Name
        {
            get
            {
                string DefaultName = String.Format("Unknown Character ({0})", this.ID_);
                string value = null;
                using (RegistryKey NameMappings = POL.OpenPOLUtilsConfigKey("Character Names", true))
                {
                    if (NameMappings != null)
                    {
                        value = NameMappings.GetValue(this.ID_, null) as string;
                    }
                }
                return ((value == null) ? DefaultName : value);
            }
            set
            {
                using (RegistryKey NameMappings = POL.OpenPOLUtilsConfigKey("Character Names", true))
                {
                    if (NameMappings != null)
                    {
                        if (value == null)
                        {
                            NameMappings.DeleteValue(this.ID_, false);
                        }
                        else
                        {
                            NameMappings.SetValue(this.ID_, value);
                        }
                    }
                }
            }
        }

        #region Private Fields

        private string ID_;
        private string DataDir_;

        #endregion

        #endregion

        public override string ToString() { return this.Name; }

        public string GetUserFileName(string FileName) { return Path.Combine(this.DataDir_, FileName); }

        public FileStream OpenUserFile(string FileName, FileMode Mode, FileAccess Access)
        {
            return new FileStream(this.GetUserFileName(FileName), Mode, Access, FileShare.Read);
        }
    }

    public class CharacterCollection : ReadOnlyCollectionBase
    {
        public void Add(Character C) { this.InnerList.Add(C); }
        public bool Contains(Character C) { return this.InnerList.Contains(C); }
        public int IndexOf(Character C) { return this.InnerList.IndexOf(C); }
        public void Remove(Character C) { this.InnerList.Remove(C); }

        public Character this[int Index]
        {
            get { return this.InnerList[Index] as Character; }
            set { this.InnerList[Index] = value; }
        }

        public Character this[string ID]
        {
            get
            {
                foreach (Character C in this.InnerList)
                {
                    if (C.ID == ID)
                    {
                        return C;
                    }
                }
                return null;
            }
        }
    }
}
