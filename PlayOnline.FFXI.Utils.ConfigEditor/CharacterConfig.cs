// Copyright © 2004-2014 Tim Van Holder, Windower Team
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS"
// BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Text;
using System.Xml;
using PlayOnline.Core;
using PlayOnline.FFXI.Things;

namespace PlayOnline.FFXI.Utils.ConfigEditor
{
    public class CharacterConfig
    {
        public CharacterConfig(Character C)
        {
            this.Character_ = C;
            this.LoadConfig();
        }

        #region Properties

        public Character Character
        {
            get { return this.Character_; }
        }

        public Color[] Colors
        {
            get { return this.Colors_; }
        }

        public string CharacterName
        {
            // For ComboBox purposes
            get { return ((this.Character_ == null) ? "<No Character>" : this.Character_.Name); }
        }

        #region Private Data

        private Character Character_;
        private Color[] Colors_;

        #endregion

        #endregion

        private void LoadConfig()
        {
            if (this.Character_ == null)
            {
                return;
            }
            try
            {
                BinaryReader BR = new BinaryReader(this.Character_.OpenUserFile("cnf.dat", FileMode.Open, FileAccess.Read));
                if (BR != null)
                {
                    BR.BaseStream.Seek(0x50, SeekOrigin.Begin);
                    this.Colors_ = new Color[23];
                    for (int i = 0; i < 23; ++i)
                    {
                        this.Colors_[i] = Graphic.ReadColor(BR, 32);
                    }
                    BR.Close();
                }
            }
            catch
            {
                this.Colors_ = null;
            }
        }

        public void Save()
        {
            if (this.Character_ == null || this.Colors_ == null)
            {
                return;
            }
            BinaryWriter BW = new BinaryWriter(this.Character_.OpenUserFile("cnf.dat", FileMode.Open, FileAccess.ReadWrite));
            if (BW != null)
            {
                BW.BaseStream.Seek(0x50, SeekOrigin.Begin);
                for (int i = 0; i < 23; ++i)
                {
                    Graphic.WriteColor(BW, this.Colors_[i], 32);
                }
                BW.Close();
            }
        }
    }
}
