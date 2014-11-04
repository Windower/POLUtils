// Copyright © 2004-2014 Tim Van Holder, Nevin Stepan, Windower Team
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS"
// BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using System;
using System.IO;
using System.Text;
using PlayOnline.Core;

namespace PlayOnline.FFXI
{
    public class FFXI
    {
        private FFXI()
        {
            /* static use only */
        }

        public static bool GetFilePath(int FileNumber, out byte App, out short Dir, out byte File)
        {
            string DataRoot = POL.GetApplicationPath(AppID.FFXI);
            for (byte i = 1; i < 20; ++i)
            {
                string Suffix = "";
                string DataDir = DataRoot;
                if (i > 1)
                {
                    Suffix = i.ToString();
                    DataDir = Path.Combine(DataRoot, "Rom" + Suffix);
                }
                string VTableFile = Path.Combine(DataDir, String.Format("VTABLE{0}.DAT", Suffix));
                string FTableFile = Path.Combine(DataDir, String.Format("FTABLE{0}.DAT", Suffix));
                if (i == 1) // add the Rom now (not needed for the *TABLE.DAT, but needed for the other DAT paths)
                {
                    DataDir = Path.Combine(DataRoot, "Rom");
                }
                if (System.IO.File.Exists(VTableFile) && System.IO.File.Exists(FTableFile))
                {
                    try
                    {
                        BinaryReader VBR =
                            new BinaryReader(new FileStream(VTableFile, FileMode.Open, FileAccess.Read, FileShare.Read));
                        if (FileNumber < VBR.BaseStream.Length)
                        {
                            VBR.BaseStream.Seek(FileNumber, SeekOrigin.Begin);
                            if (VBR.ReadByte() == i)
                            {
                                BinaryReader FBR =
                                    new BinaryReader(new FileStream(FTableFile, FileMode.Open, FileAccess.Read, FileShare.Read));
                                FBR.BaseStream.Seek(2 * FileNumber, SeekOrigin.Begin);
                                ushort FileDir = FBR.ReadUInt16();
                                App = (byte)(i - 1);
                                Dir = (short)(FileDir / 0x80);
                                File = (byte)(FileDir % 0x80);
                                FBR.Close();
                                return true;
                            }
                        }
                        VBR.Close();
                    }
                    catch {}
                }
            }
            App = File = 0;
            Dir = 0;
            return false;
        }

        public static string GetFilePath(byte App, short Dir, byte File)
        {
            string ROMDir = "Rom";
            if (App > 0)
            {
                ++App;
                ROMDir += App.ToString();
            }
            return Path.Combine(POL.GetApplicationPath(AppID.FFXI),
                Path.Combine(ROMDir, Path.Combine(Dir.ToString(), Path.ChangeExtension(File.ToString(), ".dat"))));
        }

        public static string GetFilePath(int FileNumber)
        {
            byte App = 0;
            short Dir = 0;
            byte File = 0;
            if (!FFXI.GetFilePath(FileNumber, out App, out Dir, out File))
            {
                return null;
            }
            return FFXI.GetFilePath(App, Dir, File);
        }

        private static bool GetFilePath(int FileNumber, out byte App, out short Dir, out byte File, string APPID)
        {
            string DataRoot = POL.GetApplicationPath(APPID);
            for (byte i = 1; i < 20; ++i)
            {
                string Suffix = "";
                string DataDir = DataRoot;
                if (i > 1)
                {
                    Suffix = i.ToString();
                    DataDir = Path.Combine(DataRoot, "Rom" + Suffix);
                }
                string VTableFile = Path.Combine(DataDir, String.Format("VTABLE{0}.DAT", Suffix));
                string FTableFile = Path.Combine(DataDir, String.Format("FTABLE{0}.DAT", Suffix));
                if (i == 1) // add the Rom now (not needed for the *TABLE.DAT, but needed for the other DAT paths)
                {
                    DataDir = Path.Combine(DataRoot, "Rom");
                }
                if (System.IO.File.Exists(VTableFile) && System.IO.File.Exists(FTableFile))
                {
                    try
                    {
                        BinaryReader VBR =
                            new BinaryReader(new FileStream(VTableFile, FileMode.Open, FileAccess.Read, FileShare.Read));
                        if (FileNumber < VBR.BaseStream.Length)
                        {
                            VBR.BaseStream.Seek(FileNumber, SeekOrigin.Begin);
                            if (VBR.ReadByte() == i)
                            {
                                BinaryReader FBR =
                                    new BinaryReader(new FileStream(FTableFile, FileMode.Open, FileAccess.Read, FileShare.Read));
                                FBR.BaseStream.Seek(2 * FileNumber, SeekOrigin.Begin);
                                ushort FileDir = FBR.ReadUInt16();
                                App = (byte)(i - 1);
                                Dir = (short)(FileDir / 0x80);
                                File = (byte)(FileDir % 0x80);
                                FBR.Close();
                                return true;
                            }
                        }
                        VBR.Close();
                    }
                    catch {}
                }
            }
            App = File = 0;
            Dir = 0;
            return false;
        }

        private static string GetFilePath(byte App, short Dir, byte File, string APPID)
        {
            string ROMDir = "Rom";
            if (App > 0)
            {
                ++App;
                ROMDir += App.ToString();
            }
            return Path.Combine(POL.GetApplicationPath(APPID),
                Path.Combine(ROMDir, Path.Combine(Dir.ToString(), Path.ChangeExtension(File.ToString(), ".dat"))));
        }

        public static string GetFilePath(int FileNumber, string APPID)
        {
            byte App = 0;
            short Dir = 0;
            byte File = 0;
            if (!FFXI.GetFilePath(FileNumber, out App, out Dir, out File, APPID))
            {
                return null;
            }
            return FFXI.GetFilePath(App, Dir, File, APPID);
        }
    }
}
