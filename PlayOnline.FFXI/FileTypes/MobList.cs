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
    public class MobList : FileType
    {
        public override ThingList Load(BinaryReader BR, ProgressCallback ProgressCallback)
        {
            ThingList TL = new ThingList();
            if (ProgressCallback != null)
            {
                ProgressCallback(I18N.GetText("FTM:CheckingFile"), 0);
            }
            if ((BR.BaseStream.Length % 0x20) != 0 || BR.BaseStream.Position != 0)
            {
                return TL;
            }
            long EntryCount = BR.BaseStream.Length / 0x20;
            if (ProgressCallback != null)
            {
                ProgressCallback(I18N.GetText("FTM:LoadingData"), 0);
            }
            try
            {
                int ZoneID = -1;
                for (int i = 0; i < EntryCount; ++i)
                {
                    Things.MobListEntry MLE = new Things.MobListEntry();
                    if (!MLE.Read(BR))
                    {
                        TL.Clear();
                        break;
                    }
                    uint ThisID = (uint)MLE.GetFieldValue("id");
                    if (i == 0 && (ThisID != 0 || MLE.GetFieldText("name") != "none"))
                    {
                        TL.Clear();
                        break;
                    }
                    else if (i > 0)
                    {
                        // Entire file should be for 1 specific zone
                        int ThisZone = (int)(ThisID & 0x000FF000);
                        if (ZoneID < 0)
                        {
                            ZoneID = ThisZone;
                        }
                        else if (ThisZone != ZoneID)
                        {
                            TL.Clear();
                            break;
                        }
                    }
                    if (ProgressCallback != null)
                    {
                        ProgressCallback(null, (double)(i + 1) / EntryCount);
                    }
                    TL.Add(MLE);
                }
            }
            catch
            {
                TL.Clear();
            }
            return TL;
        }
    }
}
