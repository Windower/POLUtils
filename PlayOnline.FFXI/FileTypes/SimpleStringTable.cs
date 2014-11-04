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
    public class SimpleStringTable : FileType
    {
        public override ThingList Load(BinaryReader BR, ProgressCallback ProgressCallback)
        {
            ThingList TL = new ThingList();
            if (ProgressCallback != null)
            {
                ProgressCallback(I18N.GetText("FTM:CheckingFile"), 0);
            }
            if ((BR.BaseStream.Length % 0x40) != 0 || BR.BaseStream.Position != 0)
            {
                return TL;
            }
            long EntryCount = BR.BaseStream.Length / 0x40;
            if (ProgressCallback != null)
            {
                ProgressCallback(I18N.GetText("FTM:LoadingData"), 0);
            }
            for (int i = 0; i < EntryCount; ++i)
            {
                Things.SimpleStringTableEntry SSTE = new Things.SimpleStringTableEntry();
                if (!SSTE.Read(BR))
                {
                    TL.Clear();
                    break;
                }
                if (ProgressCallback != null)
                {
                    ProgressCallback(null, (double)(i + 1) / EntryCount);
                }
                TL.Add(SSTE);
            }
            return TL;
        }
    }
}
