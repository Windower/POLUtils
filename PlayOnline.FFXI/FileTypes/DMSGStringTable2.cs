// Copyright © 2004-2014 Tim Van Holder, Windower Team
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS"
// BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using System;
using System.IO;
using System.Collections.Generic;
using PlayOnline.Core;

namespace PlayOnline.FFXI.FileTypes
{
    public class DMSGStringTable2 : FileType
    {
        public override ThingList Load(BinaryReader BR, ProgressCallback ProgressCallback)
        {
            ThingList TL = new ThingList();
            if (ProgressCallback != null)
            {
                ProgressCallback(I18N.GetText("FTM:CheckingFile"), 0);
            }
            if (BR.BaseStream.Length < 0x40 || BR.BaseStream.Position != 0)
            {
                return TL;
            }
            FFXIEncoding E = new FFXIEncoding();
            // Skip (presumably) fixed portion of the header
            if ((E.GetString(BR.ReadBytes(8)) != "d_msg".PadRight(8, '\0')) || BR.ReadUInt16() != 1 || BR.ReadUInt16() != 1 ||
                BR.ReadUInt32() != 3 || BR.ReadUInt32() != 3)
            {
                return TL;
            }
            // Read the useful header fields
            uint FileSize = BR.ReadUInt32();
            if (FileSize != BR.BaseStream.Length)
            {
                return TL;
            }
            uint HeaderBytes = BR.ReadUInt32();
            if (HeaderBytes != 0x40)
            {
                return TL;
            }
            uint EntryBytes = BR.ReadUInt32();
            if (BR.ReadUInt32() != 0)
            {
                return TL;
            }
            uint DataBytes = BR.ReadUInt32();
            if (FileSize != HeaderBytes + EntryBytes + DataBytes)
            {
                return TL;
            }
            uint EntryCount = BR.ReadUInt32();
            if (EntryBytes != EntryCount * 8)
            {
                return TL;
            }
            if (BR.ReadUInt32() != 1 || BR.ReadUInt64() != 0 || BR.ReadUInt64() != 0)
            {
                return TL;
            }
            if (ProgressCallback != null)
            {
                ProgressCallback(I18N.GetText("FTM:LoadingData"), 0);
            }
            for (uint i = 0; i < EntryCount; ++i)
            {
                BR.BaseStream.Position = HeaderBytes + i * 8;
                int Offset = ~BR.ReadInt32();
                int Length = ~BR.ReadInt32();
                if (Length < 0 || Offset < 0 || Offset + Length > DataBytes)
                {
                    TL.Clear();
                    break;
                }
                BR.BaseStream.Position = HeaderBytes + EntryBytes + Offset;
                BinaryReader EntryBR = new BinaryReader(new MemoryStream(BR.ReadBytes(Length)));
                bool ItemAdded = false;
                {
                    Things.DMSGStringBlock SB = new Things.DMSGStringBlock();
                    if (SB.Read(EntryBR, E, i))
                    {
                        TL.Add(SB);
                        ItemAdded = true;
                    }
                }
                EntryBR.Close();
                if (!ItemAdded)
                {
                    TL.Clear();
                    break;
                }
                if (ProgressCallback != null)
                {
                    ProgressCallback(null, (double)(i + 1) / EntryCount);
                }
            }
            return TL;
        }
    }
}
