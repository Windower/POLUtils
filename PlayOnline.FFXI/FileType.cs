// Copyright © 2004-2014 Tim Van Holder, Windower Team
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS"
// BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using PlayOnline.Core;
using PlayOnline.FFXI.Things;

namespace PlayOnline.FFXI
{
    public abstract class FileType
    {
        public delegate void ProgressCallback(string Message, double PercentCompleted);

        public static List<FileType> AllTypes
        {
            get
            {
                List<FileType> Results = new List<FileType>();
                Results.Add(new FileTypes.DialogTable());
                Results.Add(new FileTypes.XIStringTable());
                Results.Add(new FileTypes.DMSGStringTable());
                Results.Add(new FileTypes.DMSGStringTable2());
                Results.Add(new FileTypes.DMSGStringTable3());
                Results.Add(new FileTypes.SimpleStringTable());
                Results.Add(new FileTypes.SpellAndAbilityInfo());
                Results.Add(new FileTypes.SpellInfo());
                Results.Add(new FileTypes.AbilityInfo());
                Results.Add(new FileTypes.StatusInfo());
                Results.Add(new FileTypes.QuestInfo());
                Results.Add(new FileTypes.ItemData());
                Results.Add(new FileTypes.MobList());
                Results.Add(new FileTypes.Images());
                return Results;
            }
        }

        public virtual string Name
        {
            get
            {
                string MessageID = String.Format("FT:{0}", this.GetType().Name);
                string Result = MessageID;
                try
                {
                    Result = I18N.GetText(MessageID, this.GetType().Assembly);
                }
                catch {}
                if (Result == MessageID)
                {
                    Result = this.GetType().Name;
                }
                return Result;
            }
        }

        public abstract ThingList Load(BinaryReader BR, ProgressCallback ProgressCallback);

        public virtual ThingList Load(BinaryReader BR) { return this.Load(BR, null); }
        public virtual ThingList Load(string FileName) { return this.Load(FileName, null); }

        public virtual ThingList Load(string FileName, ProgressCallback ProgressCallback)
        {
            if (ProgressCallback != null)
            {
                ProgressCallback(I18N.GetText("FTM:OpeningFile"), 0);
            }
            BinaryReader BR = null;
            try
            {
                BR = new BinaryReader(new FileStream(FileName, FileMode.Open, FileAccess.Read), Encoding.ASCII);
            }
            catch {}
            if (BR == null || BR.BaseStream == null)
            {
                return null;
            }
            ThingList Results = this.Load(BR, ProgressCallback);
            BR.Close();
            return Results;
        }

        public static ThingList LoadAll(string FileName, ProgressCallback ProgressCallback, bool FirstMatchOnly)
        {
            ThingList Results = new ThingList();
            if (ProgressCallback != null)
            {
                ProgressCallback(I18N.GetText("FTM:OpeningFile"), 0);
            }
            BinaryReader BR = null;
            try
            {
                BR = new BinaryReader(new FileStream(FileName, FileMode.Open, FileAccess.Read), Encoding.ASCII);
            }
            catch {}
            if (BR == null || BR.BaseStream == null)
            {
                return Results;
            }
            foreach (FileType FT in FileType.AllTypes)
            {
                ProgressCallback SubCallback = null;
                if (ProgressCallback != null)
                {
                    SubCallback = new ProgressCallback(delegate(string Message, double PercentCompleted)
                        {
                            string SubMessage = null;
                            if (Message != null)
                            {
                                SubMessage = String.Format("[{0}] {1}", FT.Name, Message);
                            }
                            ProgressCallback(SubMessage, PercentCompleted);
                        });
                }
                ThingList SubResults = FT.Load(BR, SubCallback);
                if (SubResults != null)
                {
                    Results.AddRange(SubResults);
                    if (FirstMatchOnly && Results.Count > 0)
                    {
                        break;
                    }
                }
                BR.BaseStream.Seek(0, SeekOrigin.Begin);
            }
            return Results;
        }

        public static ThingList LoadAll(string FileName, ProgressCallback ProgressCallback)
        {
            return FileType.LoadAll(FileName, ProgressCallback, true);
        }

        public static ThingList LoadAll(string FileName, bool FirstMatchOnly)
        {
            return FileType.LoadAll(FileName, null, FirstMatchOnly);
        }

        public static ThingList LoadAll(string FileName) { return FileType.LoadAll(FileName, null, true); }
    }
}
