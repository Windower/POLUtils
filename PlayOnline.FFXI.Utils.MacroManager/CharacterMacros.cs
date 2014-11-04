// Copyright © 2004-2014 Tim Van Holder, Windower Team
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS"
// BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using System.IO;

namespace PlayOnline.FFXI
{
    public class CharacterMacros : SpecialMacroFolder
    {
        public CharacterMacros(Character C)
            : base(C.Name)
        {
            MacroBook[] Books = MacroBook.Load(C);
            if (Books != null)
            {
                this.Folders.AddRange(Books);
            }
            else
            {
                for (int j = 0; j < 10; ++j)
                {
                    this.Folders.Add(MacroSet.Load(C.GetUserFileName(string.Format("mcr{0:#####}.dat", j)),
                        string.Format("Macro Set {0}", j + 1)));
                }
            }
        }

        public override bool CanSave
        {
            get { return true; }
        }

        public override bool Save()
        {
            bool OK = true;
            foreach (MacroFolder MF in this.Folders)
            {
                if (MF.CanSave)
                {
                    OK = OK && MF.Save();
                }
            }
            return OK;
        }
    }
}
