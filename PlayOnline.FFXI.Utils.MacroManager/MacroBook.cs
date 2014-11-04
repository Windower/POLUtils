// Copyright © 2004-2014 Tim Van Holder, Windower Team
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS"
// BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace PlayOnline.FFXI
{
    public class MacroBook : SpecialMacroFolder
    {
        private string FileName_;
        private int Index_;

        public MacroBook(string FileName, int Index, string Name)
            : base(Name)
        {
            this.FileName_ = FileName;
            this.Index_ = Index;
        }

        public override bool CanSave
        {
            get { return true; }
        }

        public override bool Save()
        {
            bool OK = true;
            try
            {
                FileStream FS = new FileStream(this.FileName_, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                BinaryWriter BW = new BinaryWriter(FS, Encoding.ASCII);
                BW.BaseStream.Seek(0x18 + 16 * this.Index_, SeekOrigin.Begin);
                string Name = this.Name;
                if (Name.Length > 16)
                {
                    Name = Name.Substring(0, 16);
                }
                BW.Write(Name.PadRight(16, '\0').ToCharArray());
                FS.Flush();
                {
                    BinaryReader BR = new BinaryReader(FS, Encoding.ASCII);
                    MD5 Hash = new MD5CryptoServiceProvider();
                    BR.BaseStream.Seek(0x18, SeekOrigin.Begin);
                    byte[] Data = BR.ReadBytes((int)BR.BaseStream.Length - 0x18);
                    BW.BaseStream.Seek(0x8, SeekOrigin.Begin);
                    BW.Write(Hash.ComputeHash(Data));
                }
                FS.Close();
            }
            catch
            {
                OK = false;
            }
            foreach (MacroFolder MF in this.Folders)
            {
                if (MF.CanSave)
                {
                    OK = OK && MF.Save();
                }
            }
            return OK;
        }

        public static MacroBook[] Load(Character C)
        {
            string BookNameFile = C.GetUserFileName("mcr.ttl");
            if (!File.Exists(BookNameFile))
            {
                return null;
            }
            BinaryReader BR = new BinaryReader(new FileStream(BookNameFile, FileMode.Open, FileAccess.Read), Encoding.ASCII);
            int BookCount = 1;
            if ((BR.BaseStream.Length - 0x18) % 16 != 0 || BR.ReadUInt32() != 1)
            {
                BR.Close();
                return null;
            }
            else
            {
                BookCount = (int)((BR.BaseStream.Length - 0x18) / 16);
            }
            BR.BaseStream.Seek(0x18, SeekOrigin.Begin);
            List<MacroBook> Books = new List<MacroBook>();
            for (int i = 0; i < BookCount; ++i)
            {
                MacroBook MB = new MacroBook(BookNameFile, i, new string(BR.ReadChars(16)).TrimEnd('\0'));
                for (int j = 0; j < 10; ++j)
                {
                    MB.Folders.Add(MacroSet.Load(C.GetUserFileName(string.Format("mcr{0:#####}.dat", 10 * i + j)),
                        string.Format("Macro Set {0}", j + 1)));
                }
                Books.Add(MB);
            }
            BR.Close();
            return Books.ToArray();
        }
    }
}
