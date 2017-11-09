// Copyright © 2010-2012 Chris Baggett, Tim Van Holder, Nevin Stepan
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
    public class SpellAndAbilityInfo : FileType
    {
        public override ThingList Load(BinaryReader BR, ProgressCallback ProgressCallback)
        {
            ThingList TL = new ThingList();
            if (ProgressCallback != null)
            {
                ProgressCallback(I18N.GetText("FTM:CheckingFile"), 0);
            }
            if (BR.BaseStream.Position != 0)
            {
                goto Failed;
            }
            if (Encoding.ASCII.GetString(BR.ReadBytes(4)) != "menu")
            {
                goto Failed;
            }
            if (BR.ReadInt32() != 0x101)
            {
                goto Failed;
            }
            if (BR.ReadInt64() != 0)
            {
                goto Failed;
            }
            if (BR.ReadInt64() != 0)
            {
                goto Failed;
            }
            if (BR.ReadInt64() != 0)
            {
                goto Failed;
            }
            if (ProgressCallback != null)
            {
                ProgressCallback(I18N.GetText("FTM:LoadingData"), (double)BR.BaseStream.Position / BR.BaseStream.Length);
            }
            string firstFourBytes = Encoding.ASCII.GetString(BR.ReadBytes(4));
            {
                // Part 0: Monster?
                if (firstFourBytes != "mon_")
                {
                    goto Part1;
                }
                uint SizeInfo = BR.ReadUInt32();
                if (BR.ReadInt64() != 0)
                {
                    goto Failed;
                }
                uint BlockSize = (SizeInfo & 0xFFFFFF80) >> 3;
                if ((BlockSize - 0x10) % 0x58 != 0)
                {
                    goto Failed;
                }
                uint EntryCount = (BlockSize - 0x10) / 0x58;
                while (EntryCount-- > 0)
                {
                    Things.MonsterSpellInfo2 MSI2 = new Things.MonsterSpellInfo2();
                    if (!MSI2.Read(BR))
                    {
                        goto Failed;
                    }
                    if (ProgressCallback != null)
                    {
                        ProgressCallback(null, (double)BR.BaseStream.Position / BR.BaseStream.Length);
                    }
                    TL.Add(MSI2);
                }
            }
            firstFourBytes = Encoding.ASCII.GetString(BR.ReadBytes(4));
            Part1:
            {
                // Part 1: Spell Info
                if (firstFourBytes != "mgc_")
                {
                    goto Failed;
                }
                uint SizeInfo = BR.ReadUInt32();
                if (BR.ReadInt64() != 0)
                {
                    goto Failed;
                }
                uint BlockSize = (SizeInfo & 0xFFFFFF80) >> 3;
                if ((BlockSize - 0x10) % 0x58 != 0)
                {
                    goto Failed;
                }
                uint EntryCount = (BlockSize - 0x10) / 0x58;
                while (EntryCount-- > 0)
                {
                    Things.SpellInfo2 SI2 = new Things.SpellInfo2();
                    if (!SI2.Read(BR))
                    {
                        goto Failed;
                    }
                    if (ProgressCallback != null)
                    {
                        ProgressCallback(null, (double)BR.BaseStream.Position / BR.BaseStream.Length);
                    }
                    TL.Add(SI2);
                }
            }
            {
                // Part 2: Ability Info
                if (Encoding.ASCII.GetString(BR.ReadBytes(4)) != "comm")
                {
                    goto Failed;
                }
                uint SizeInfo = BR.ReadUInt32();
                if (BR.ReadInt64() != 0)
                {
                    goto Failed;
                }
                uint BlockSize = (SizeInfo & 0xFFFFFF80) >> 3;
                if ((BlockSize - 0x10) % 0x30 != 0)
                {
                    goto Failed;
                }
                uint EntryCount = (BlockSize - 0x10) / 0x30;
                while (EntryCount-- > 0)
                {
                    Things.AbilityInfo2 AI2 = new Things.AbilityInfo2();
                    if (!AI2.Read(BR))
                    {
                        goto Failed;
                    }
                    if (ProgressCallback != null)
                    {
                        ProgressCallback(null, (double)BR.BaseStream.Position / BR.BaseStream.Length);
                    }
                    TL.Add(AI2);
                }
            }
            {
                // Part 3: End Marker
                if (Encoding.ASCII.GetString(BR.ReadBytes(4)) != "end\0")
                {
                    goto Failed;
                }
                uint SizeInfo = BR.ReadUInt32();
                if (BR.ReadInt64() != 0)
                {
                    goto Failed;
                }
                uint BlockSize = (SizeInfo & 0xFFFFFF80) >> 3;
                if (BlockSize != 0x10) // Header only
                {
                    goto Failed;
                }
                if (ProgressCallback != null)
                {
                    ProgressCallback(null, (double)BR.BaseStream.Position / BR.BaseStream.Length);
                }
            }
            goto Done;
            Failed:
            TL.Clear();
            Done:
            return TL;
        }
    }
}
