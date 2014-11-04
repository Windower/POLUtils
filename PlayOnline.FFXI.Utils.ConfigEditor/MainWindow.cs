// Copyright © 2004-2014 Tim Van Holder, Windower Team
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS"
// BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;
using PlayOnline.Core;

namespace PlayOnline.FFXI.Utils.ConfigEditor
{
    public partial class MainWindow : Form
    {
        private Label[] ColorLabels;

        public MainWindow()
        {
            this.InitializeComponent();
            this.Icon = Icons.POLConfig;
            this.picWarning.Image = Icons.POLConfigWarn.ToBitmap();
            this.lblWarning.Text = I18N.GetText("SettingsWarning");
            this.cmbCharacters.Items.Clear();
            foreach (Character C in Game.Characters)
            {
                CharacterConfig CC = new CharacterConfig(C);
                if (CC.Colors != null)
                {
                    this.cmbCharacters.Items.Add(CC);
                }
            }
            if (this.cmbCharacters.Items.Count == 0)
            {
                this.grpCharConfig.Visible = false;
                this.Height -= this.grpCharConfig.Height + this.grpCharConfig.Top -
                               (this.grpGlobalConfig.Top + this.grpGlobalConfig.Height);
            }
            else
            {
                this.ColorLabels = new Label[]
                {
                    this.lblColor1, this.lblColor2, this.lblColor3, this.lblColor4, this.lblColor5, this.lblColor6, this.lblColor7,
                    this.lblColor8, this.lblColor9, this.lblColor10, this.lblColor11, this.lblColor12, this.lblColor13,
                    this.lblColor14, this.lblColor15, this.lblColor16, this.lblColor17, this.lblColor18, this.lblColor19,
                    this.lblColor20, this.lblColor21, this.lblColor22, this.lblColor23
                };
                // TODO: Take these from the relevant string table (entries 32-54 (and/or 55-77) of E0-97-39/J0-97-21)
                this.lblColor1.Tag = "Say";
                this.lblColor2.Tag = "Shout";
                this.lblColor3.Tag = "Tell";
                this.lblColor4.Tag = "Party";
                this.lblColor5.Tag = "Linkshell";
                this.lblColor6.Tag = "Message";
                this.lblColor7.Tag = "Emote";
                this.lblColor8.Tag = "NPC";
                this.lblColor9.Tag = "(Self) HP/MP Recovered";
                this.lblColor10.Tag = "(Self) HP/MP Lost";
                this.lblColor11.Tag = "(Self) Beneficial Effects Received";
                this.lblColor12.Tag = "(Self) Detrimental Effects Received";
                this.lblColor13.Tag = "(Self) Resisted Effects";
                this.lblColor14.Tag = "(Self) Evaded Effects";
                this.lblColor15.Tag = "(Other) HP/MP Recovered";
                this.lblColor16.Tag = "(Other) HP/MP Lost";
                this.lblColor17.Tag = "(Other) Beneficial Effects Received";
                this.lblColor18.Tag = "(Other) Detrimental Effects Received";
                this.lblColor19.Tag = "(Other) Resisted Effects";
                this.lblColor20.Tag = "(Self) Evaded Effects";
                this.lblColor21.Tag = "Battle Message";
                this.lblColor22.Tag = "Call For Help";
                this.lblColor23.Tag = "System Message";
            }
            this.LoadSettings();
        }

        private void LoadSettings()
        {
            // Load Global settings
            RegistryKey ConfigKey = POL.OpenAppConfigKey(AppID.FFXI);
            if (ConfigKey == null)
            {
                this.grpGlobalConfig.Enabled = false;
            }
            else
            {
                this.txtGUIWidth.Text = String.Format("{0}", ConfigKey.GetValue("0001"));
                this.txtGUIHeight.Text = String.Format("{0}", ConfigKey.GetValue("0002"));
                this.txt3DWidth.Text = String.Format("{0}", ConfigKey.GetValue("0003"));
                this.txt3DHeight.Text = String.Format("{0}", ConfigKey.GetValue("0004"));
                this.txtSoundEffects.Text = String.Format("{0}", ConfigKey.GetValue("0029"));
            }
            // Select the first character to trigger loading its config
            if (this.cmbCharacters.Items.Count > 0)
            {
                this.cmbCharacters.SelectedIndex = 0;
            }
            this.btnApply.Enabled = false;
        }

        private void SaveSettings()
        {
            if (this.btnApply.Enabled)
            {
                if (this.grpGlobalConfig.Enabled)
                {
                    RegistryKey ConfigKey = POL.OpenAppConfigKey(AppID.FFXI);
                    try
                    {
                        ConfigKey.SetValue("0001", int.Parse(this.txtGUIWidth.Text));
                    }
                    catch {}
                    try
                    {
                        ConfigKey.SetValue("0002", int.Parse(this.txtGUIHeight.Text));
                    }
                    catch {}
                    try
                    {
                        ConfigKey.SetValue("0003", int.Parse(this.txt3DWidth.Text));
                    }
                    catch {}
                    try
                    {
                        ConfigKey.SetValue("0004", int.Parse(this.txt3DHeight.Text));
                    }
                    catch {}
                    try
                    {
                        ConfigKey.SetValue("0029", int.Parse(this.txtSoundEffects.Text));
                    }
                    catch {}
                }
                foreach (CharacterConfig CC in this.cmbCharacters.Items)
                {
                    CC.Save();
                }
                this.btnApply.Enabled = false;
            }
        }

        #region Button Events

        private void btnApply_Click(object sender, System.EventArgs e) { this.SaveSettings(); }

        private void btnCancel_Click(object sender, System.EventArgs e) { this.Close(); }

        private void btnClose_Click(object sender, System.EventArgs e)
        {
            this.SaveSettings();
            this.Close();
        }

        #endregion

        private void cmbCharacters_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            CharacterConfig CC = this.cmbCharacters.SelectedItem as CharacterConfig;
            if (CC != null && CC.Colors != null)
            {
                for (int i = 0; i < 23; ++i)
                {
                    this.ColorLabels[i].BackColor = CC.Colors[i];
                }
            }
        }

        private void ColorLabel_DoubleClick(object sender, System.EventArgs e)
        {
            if (this.cmbCharacters.SelectedIndex < 0)
            {
                return;
            }
            Label L = sender as Label;
            this.dlgChooseColor.Color = L.BackColor;
            if (this.dlgChooseColor.ShowDialog(this) == DialogResult.OK)
            {
                L.BackColor = this.dlgChooseColor.Color;
                {
                    // Also update the character config
                    CharacterConfig CC = this.cmbCharacters.SelectedItem as CharacterConfig;
                    CC.Colors[(sender as Label).TabIndex - 1] = this.dlgChooseColor.Color;
                }
                this.btnApply.Enabled = true;
            }
        }

        private void ColorLabel_MouseEnter(object sender, EventArgs e)
        {
            this.txtSample.ForeColor = (sender as Label).BackColor;
            this.txtSample.Text = (sender as Label).Tag as String;
        }

        private void ColorLabel_MouseLeave(object sender, EventArgs e)
        {
            this.txtSample.ForeColor = Color.White;
            this.txtSample.Text = "";
        }

        private void Something_Changed(object sender, System.EventArgs e) { this.btnApply.Enabled = true; }
    }
}
