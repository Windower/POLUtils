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
using System.Text;

namespace PlayOnline.Utils.TetraViewer
{
    internal class DataFile
    {
        public enum EntryType
        {
            None = 0x0000,
            PNG = 0x4000,
            SubTOC = 0x8000,
        }

        public class TOC
        {
            public string Name;
            public TOC[] SubTOCs;
            public TOCEntry[] Entries;
        }

        public class TOCEntry
        {
            public TOCEntry(Stream S, long BaseOffset)
            {
                BinaryReader BR = new BinaryReader(S, Encoding.ASCII);
                this.Type_ = (EntryType)BR.ReadInt32();
                Console.WriteLine("TOC Entry Type: 0x{0:x}", (int)this.Type_);
                this.Name_ = new string(BR.ReadChars(12));
                {
                    int end = this.Name_.IndexOf('\0');
                    if (end >= 0)
                    {
                        this.Name_ = this.Name_.Substring(0, end);
                    }
                }
                //foreach (char c in Path.InvalidPathChars)
                //  this.Name = this.Name.Replace(c, '_');
                this.Offset_ = BaseOffset + BR.ReadInt32();
                this.Size_ = BR.ReadInt32();
#if DEBUG
                Console.WriteLine("Name     : {0}", this.Name_);
                Console.WriteLine("Offset   : 0x{0:X}", this.Offset_);
                Console.WriteLine("Size     : {0}", this.Size_);
                Console.WriteLine("Unknown 1: {0}", BR.ReadInt32());
                Console.WriteLine("Unknown 2: {0}", BR.ReadInt32());
#else
                BR.ReadBytes(4);
#endif
            }

            internal EntryType Type_;
            internal string Name_;
            internal long Offset_;
            internal int Size_;

            public EntryType Type
            {
                get { return this.Type_; }
            }

            public string Name
            {
                get { return this.Name_; }
            }

            public int Size
            {
                get { return this.Size_; }
            }
        }

        public DataFile(string FilePath)
        {
            this.Location_ = FilePath;
            this.MainTOC_ = new TOC();
            this.MainTOC_.Name = Path.GetFileName(FilePath);
            FileStream F = new FileStream(FilePath, FileMode.Open, FileAccess.Read);
            this.ReadTOC(F, this.MainTOC_);
            F.Close();
        }

        private string Location_;
        private TOC MainTOC_;

        public string Location
        {
            get { return this.Location_; }
        }

        private void ReadTOC(Stream S, TOC T)
        {
            ArrayList Entries = new ArrayList();
            ArrayList SubTOCs = new ArrayList();
            long BaseOffset = S.Position;
            TOCEntry TE = new TOCEntry(S, BaseOffset);
            bool ValidEntry = true;
            while (ValidEntry)
            {
                switch (TE.Type)
                {
                case EntryType.SubTOC:
                    long Pos = S.Position;
                    S.Seek(BaseOffset + TE.Offset_, SeekOrigin.Begin);
                    TOC SubTOC = new TOC();
                    SubTOC.Name = TE.Name;
                    this.ReadTOC(S, SubTOC);
                    SubTOCs.Add(SubTOC);
                    S.Seek(Pos, SeekOrigin.Begin);
                    break;
                case EntryType.PNG:
                    Entries.Add(TE);
                    break;
                default:
                    ValidEntry = false;
                    break;
                }
                TE = new TOCEntry(S, BaseOffset);
            }
            T.SubTOCs = (TOC[])SubTOCs.ToArray(typeof (TOC));
            T.Entries = (TOCEntry[])Entries.ToArray(typeof (TOCEntry));
        }
    }
}
