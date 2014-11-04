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
    public class XIStringTable : FileType
    {
        public override ThingList Load(BinaryReader BR, ProgressCallback ProgressCallback)
        {
            ThingList TL = new ThingList();
            if (ProgressCallback != null)
            {
                ProgressCallback(I18N.GetText("FTM:CheckingFile"), 0);
            }
            if (BR.BaseStream.Length < 0x38 || BR.BaseStream.Position != 0)
            {
                return TL;
            }
            FFXIEncoding E = new FFXIEncoding();
            // Read past the marker (32 bytes)
            if ((E.GetString(BR.ReadBytes(10)) != "XISTRING".PadRight(10, '\0')) || BR.ReadUInt16() != 2)
            {
                return TL;
            }
            foreach (byte B in BR.ReadBytes(20))
            {
                if (B != 0)
                {
                    return TL;
                }
            }
            // Read The Header
            uint FileSize = BR.ReadUInt32();
            if (FileSize != BR.BaseStream.Length)
            {
                return TL;
            }
            uint EntryCount = BR.ReadUInt32();
            uint EntryBytes = BR.ReadUInt32();
            uint DataBytes = BR.ReadUInt32();
            BR.ReadUInt32(); // Unknown
            BR.ReadUInt32(); // Unknown
            if (EntryBytes != EntryCount * 12 || FileSize != 0x38 + EntryBytes + DataBytes)
            {
                return TL;
            }
            if (ProgressCallback != null)
            {
                ProgressCallback(I18N.GetText("FTM:LoadingData"), 0);
            }
            for (uint i = 0; i < EntryCount; ++i)
            {
                Things.XIStringTableEntry XSTE = new Things.XIStringTableEntry();
                if (!XSTE.Read(BR, E, i, EntryBytes, DataBytes))
                {
                    TL.Clear();
                    break;
                }
                if (ProgressCallback != null)
                {
                    ProgressCallback(null, (double)(i + 1) / EntryCount);
                }
                TL.Add(XSTE);
            }
            return TL;
        }
    }
}
