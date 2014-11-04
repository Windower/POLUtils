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
    public class Images : FileType
    {
        public override ThingList Load(BinaryReader BR, ProgressCallback ProgressCallback)
        {
            ThingList TL = new ThingList();
            if (ProgressCallback != null)
            {
                ProgressCallback(I18N.GetText("FTM:ScanningFile"), 0);
            }
            Graphic G = new Graphic();
            while (BR.BaseStream.Position < BR.BaseStream.Length)
            {
                long Pos = BR.BaseStream.Position; // Save Position (G.Read() will advance it an unknown amount)
                if (G.Read(BR))
                {
                    TL.Add(G);
                    G = new Graphic();
                    if (ProgressCallback != null)
                    {
                        ProgressCallback(null, (double)(BR.BaseStream.Position + 1) / BR.BaseStream.Length);
                    }
                }
                else
                {
                    BR.BaseStream.Seek(Pos + 1, SeekOrigin.Begin);
                    if (ProgressCallback != null &&
                        (BR.BaseStream.Position == BR.BaseStream.Length || (BR.BaseStream.Position % 1024) == 0))
                    {
                        ProgressCallback(null, (double)(BR.BaseStream.Position + 1) / BR.BaseStream.Length);
                    }
                }
            }
            G = null;
            return TL;
        }
    }
}
