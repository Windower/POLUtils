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
using System.Text;
using System.Collections.Generic;
using PlayOnline.Core;

namespace PlayOnline.FFXI.FileTypes
{
    public class QuestInfo : FileType
    {
        public override ThingList Load(BinaryReader BR, ProgressCallback ProgressCallback)
        {
            ThingList TL = new ThingList();
            if (ProgressCallback != null)
            {
                ProgressCallback(I18N.GetText("FTM:CheckingFile"), 0);
            }
            if (Encoding.ASCII.GetString(BR.ReadBytes(4)) != "menu")
            {
                return TL;
            }
            if (BR.ReadInt32() != 0x101)
            {
                return TL;
            }
            if (BR.ReadInt64() != 0x000)
            {
                return TL;
            }
            if (BR.ReadInt64() != 0)
            {
                return TL;
            }
            if (BR.ReadInt64() != 0)
            {
                return TL;
            }
            if (ProgressCallback != null)
            {
                ProgressCallback(I18N.GetText("FTM:LoadingData"), 0);
            }
            while (BR.BaseStream.Position + 0x10 <= BR.BaseStream.Length)
            {
                if (ProgressCallback != null)
                {
                    ProgressCallback(null, ((double)(BR.BaseStream.Position + 1) / BR.BaseStream.Length));
                }
                long Offset = BR.BaseStream.Position;
                string ShortName = Encoding.ASCII.GetString(BR.ReadBytes(4));
                uint SizeInfo = BR.ReadUInt32();
                if (BR.ReadUInt64() != 0)
                {
                    TL.Clear();
                    return TL;
                }
                if (BR.BaseStream.Position < BR.BaseStream.Length)
                {
                    if (Encoding.ASCII.GetString(BR.ReadBytes(8)) != "menu    ")
                    {
                        TL.Clear();
                        return TL;
                    }
                    string MenuName = Encoding.ASCII.GetString(BR.ReadBytes(8));
                    // Used to be a full match but the JP data of 20061218 had pr_sc vs pr_1
                    if (BR.ReadUInt32() != 0 || MenuName.Substring(0, 3) != ShortName.Substring(0, 3))
                    {
                        TL.Clear();
                        return TL;
                    }
                    int EntryCount = BR.ReadInt32();
                    for (int i = 0; i < EntryCount; ++i)
                    {
                        Things.QuestInfo QI = new Things.QuestInfo();
                        if (!QI.Read(BR, MenuName, Offset + 0x10))
                        {
                            TL.Clear();
                            return TL;
                        }
                        TL.Add(QI);
                    }
                }
                BR.BaseStream.Position = Offset + ((SizeInfo & 0xFFFFFF80) >> 3);
            }
            return TL;
        }
    }
}
