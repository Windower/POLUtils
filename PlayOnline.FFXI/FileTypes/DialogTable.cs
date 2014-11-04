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
    public class DialogTable : FileType
    {
        public override ThingList Load(BinaryReader BR, ProgressCallback ProgressCallback)
        {
            ThingList TL = new ThingList();
            if (ProgressCallback != null)
            {
                ProgressCallback(I18N.GetText("FTM:CheckingFile"), 0);
            }
            if (BR.BaseStream.Length < 4)
            {
                return TL;
            }
            uint FileSizeMaybe = BR.ReadUInt32();
            if (FileSizeMaybe != (0x10000000 + BR.BaseStream.Length - 4))
            {
                return TL;
            }
            uint FirstTextPos = (BR.ReadUInt32() ^ 0x80808080);
            if ((FirstTextPos % 4) != 0 || FirstTextPos > BR.BaseStream.Length || FirstTextPos < 8)
            {
                return TL;
            }
            if (ProgressCallback != null)
            {
                ProgressCallback(I18N.GetText("FTM:LoadingData"), 0);
            }
            uint EntryCount = FirstTextPos / 4;
            // The entries are usually, but not always, sequential in the file.
            // Because we need to know how long one entry is (no clear end-of-message marker), we need them in
            // sequential order.
            List<uint> Entries = new List<uint>((int)EntryCount + 1);
            Entries.Add(FirstTextPos);
            for (int i = 1; i < EntryCount; ++i)
            {
                Entries.Add(BR.ReadUInt32() ^ 0x80808080);
            }
            Entries.Add((uint)BR.BaseStream.Length - 4);
            Entries.Sort();
            for (uint i = 0; i < EntryCount; ++i)
            {
                if (Entries[(int)i] < 4 * EntryCount || 4 + Entries[(int)i] >= BR.BaseStream.Length)
                {
                    TL.Clear();
                    break;
                }
                Things.DialogTableEntry DTE = new Things.DialogTableEntry();
                if (!DTE.Read(BR, i, Entries[(int)i], Entries[(int)i + 1]))
                {
                    TL.Clear();
                    break;
                }
                if (ProgressCallback != null)
                {
                    ProgressCallback(null, (double)(i + 1) / EntryCount);
                }
                TL.Add(DTE);
            }
            return TL;
        }
    }
}
