// Copyright © 2004-2014 Tim Van Holder, Windower Team
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS"
// BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

#define VerifyChecksum

using System;
using System.Collections;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using PlayOnline.Core;

namespace PlayOnline.FFXI
{
    public class MacroSet : SpecialMacroFolder
    {
        private string FileName_;

        public override bool CanSave
        {
            get { return (this.FileName_ != null); }
        }

        private MacroSet(string Name)
            : base(Name) { }

        public static MacroFolder Load(string PathName, string FolderName)
        {
            FileStream DATFile = null;
            if (File.Exists(PathName))
            {
                try
                {
                    DATFile = new FileStream(PathName, FileMode.Open, FileAccess.Read, FileShare.Read);
                }
                catch {}
            }
            BinaryReader BR = null;
            if (DATFile != null)
            {
                try
                {
                    BR = new BinaryReader(DATFile, Encoding.ASCII);
                    if (BR.BaseStream.Length != 7624 || BR.ReadUInt32() != 1)
                    {
                        BR.Close();
                        BR = null;
                    }
                    if (BR != null)
                    {
                        BR.ReadUInt32(); // Unknown - sometimes zero, sometimes 0x80000000
                        byte[] StoredMD5 = BR.ReadBytes(16); // MD5 Checksum of the rest of the data
#if VerifyChecksum
                        {
                            byte[] Data = BR.ReadBytes(7600);
                            BR.BaseStream.Seek(-7600, SeekOrigin.Current);
                            MD5 Hash = new MD5CryptoServiceProvider();
                            byte[] ComputedMD5 = Hash.ComputeHash(Data);
                            for (int i = 0; i < 16; ++i)
                            {
                                if (StoredMD5[i] != ComputedMD5[i])
                                {
                                    string Message = String.Format("MD5 Checksum failure for {0}:\n- Stored Hash  :", PathName);
                                    for (int j = 0; j < 16; ++j)
                                    {
                                        Message += String.Format(" {0:X2}", StoredMD5[j]);
                                    }
                                    Message += "\n- Computed Hash:";
                                    for (int j = 0; j < 16; ++j)
                                    {
                                        Message += String.Format(" {0:X2}", ComputedMD5[j]);
                                    }
                                    Message += '\n';
                                    MessageBox.Show(null, Message, "Warning");
                                    break;
                                }
                            }
                        }
#endif
                    }
                }
                catch {}
            }
            MacroSet MS = new MacroSet(FolderName);
            MS.FileName_ = PathName;
            MS.Folders.Add(new MacroFolder("Top Bar (Control)"));
            MS.Folders.Add(new MacroFolder("Bottom Bar (Alt)"));
            Encoding E = new FFXIEncoding();
            for (int i = 0; i < 2; ++i)
            {
                for (int j = 0; j < 10; ++j)
                {
                    MS.Folders[i].Macros.Add((BR != null) ? Macro.ReadFromDATFile(BR, E) : new Macro());
                }
            }
            MS.LockTree();
            if (BR != null)
            {
                BR.Close();
            }
            return MS;
        }

        private bool IsEmpty(MacroFolder folder)
        {
            foreach (Macro M in folder.Macros)
            {
                if (!M.Empty)
                {
                    return false;
                }
            }
            foreach (MacroFolder MF in folder.Folders)
            {
                if (!this.IsEmpty(MF))
                {
                    return false;
                }
            }
            return true;
        }

        public override bool Save()
        {
            if (this.FileName_ == null)
            {
                return false;
            }
            try
            {
                if (this.IsEmpty(this))
                {
                    // If there is no content in the set, delete the file.
                    if (File.Exists(this.FileName_))
                    {
                        File.Delete(this.FileName_);
                    }
                }
                else
                {
                    FileStream FS = new FileStream(this.FileName_, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    BinaryWriter BW = new BinaryWriter(FS, Encoding.ASCII);
                    BW.Write((uint)1);
                    BW.Write((uint)0); // TODO: Figure out when to write 0x80000000
                    BW.Write(new byte[16]); // dummy MD5 hash; real one is computed below
                    this.WriteToDATFile(this, BW, new FFXIEncoding());
                    FS.Flush();
                    {
                        BinaryReader BR = new BinaryReader(FS, Encoding.ASCII);
                        BR.BaseStream.Seek(0x18, SeekOrigin.Begin);
                        byte[] Data = BR.ReadBytes((int)BR.BaseStream.Length - 0x18);
                        BW.BaseStream.Seek(0x8, SeekOrigin.Begin);
                        BW.Write(new MD5CryptoServiceProvider().ComputeHash(Data));
                    }
                    FS.Close();
                }
                return true;
            }
            catch (Exception E)
            {
                Console.WriteLine(E.ToString());
            }
            return false;
        }

        private void WriteToDATFile(MacroFolder MF, BinaryWriter BW, Encoding E)
        {
            foreach (MacroFolder SubFolder in MF.Folders)
            {
                this.WriteToDATFile(SubFolder, BW, E);
            }
            foreach (Macro M in MF.Macros)
            {
                M.WriteToDATFile(BW, E);
            }
        }
    }
}
