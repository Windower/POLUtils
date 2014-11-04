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
using PlayOnline.FFXI.Things;

namespace PlayOnline.FFXI.FileTypes
{
    public class ItemData : FileType
    {
        public override ThingList Load(BinaryReader BR, ProgressCallback ProgressCallback)
        {
            ThingList TL = new ThingList();
            if (ProgressCallback != null)
            {
                ProgressCallback(I18N.GetText("FTM:CheckingFile"), 0);
            }
            if ((BR.BaseStream.Length % 0xC00) != 0 || BR.BaseStream.Length < 0xc000 || BR.BaseStream.Position != 0)
            {
                return TL;
            }
            // First deduce the type of item data is in the file.
            Item.Type T;
            Item.DeduceType(BR, out T);
            // Now read the items
            if (ProgressCallback != null)
            {
                ProgressCallback(I18N.GetText("FTM:LoadingData"), 0);
            }
            long ItemCount = BR.BaseStream.Length / 0xC00;
            long CurrentItem = 0;
            while (BR.BaseStream.Position < BR.BaseStream.Length)
            {
                Item I = new Item();
                if (!I.Read(BR, T))
                {
                    TL.Clear();
                    break;
                }
                if (ProgressCallback != null)
                {
                    ProgressCallback(null, (double)++CurrentItem / ItemCount);
                }
                TL.Add(I);
                // A currency DAT currently has 1 "real" item and 15 dummy entries (all NULs); a better thing to do would be to break if such a dummy entry
                // is seen, but since we currently detect currency from its 0xFFFF ID, this is safe enough for now.
                if (BR.BaseStream.Length == 0xc000 && T == Item.Type.Currency)
                {
                    break;
                }
            }
            return TL;
        }
    }
}
