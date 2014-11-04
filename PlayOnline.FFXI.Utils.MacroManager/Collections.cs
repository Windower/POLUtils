// Copyright © 2004-2014 Tim Van Holder, Windower Team
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS"
// BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using System.Collections;

namespace PlayOnline.FFXI
{
    public class MacroCollection : CollectionBase
    {
        public void Add(Macro M) { this.InnerList.Add(M); }
        public void AddRange(Macro[] M) { this.InnerList.AddRange(M); }
        public bool Contains(Macro M) { return this.InnerList.Contains(M); }
        public int IndexOf(Macro M) { return this.InnerList.IndexOf(M); }
        public void Remove(Macro M) { this.InnerList.Remove(M); }

        public Macro this[int Index]
        {
            get { return this.InnerList[Index] as Macro; }
            set { this.InnerList[Index] = value; }
        }
    }

    public class MacroFolderCollection : CollectionBase
    {
        public void Add(MacroFolder MF) { this.InnerList.Add(MF); }
        public void AddRange(MacroFolder[] MF) { this.InnerList.AddRange(MF); }
        public bool Contains(MacroFolder MF) { return this.InnerList.Contains(MF); }
        public int IndexOf(MacroFolder MF) { return this.InnerList.IndexOf(MF); }
        public void Remove(MacroFolder MF) { this.InnerList.Remove(MF); }

        public MacroFolder this[int Index]
        {
            get { return this.InnerList[Index] as MacroFolder; }
            set { this.InnerList[Index] = value; }
        }
    }
}
