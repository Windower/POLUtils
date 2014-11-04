// Copyright © 2004-2014 Tim Van Holder, Nevin Stepan, Windower Team
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
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using PlayOnline.Core;

namespace POLUtils
{
    public partial class POLUtilsUI : Form
    {
        public POLUtilsUI()
        {
            this.InitializeComponent();
            this.Icon = Icons.POLViewer;
            {
                Version V = Assembly.GetExecutingAssembly().GetName().Version;
                this.Text += String.Format(" {0}.{1}", V.Major, V.Minor);
                if (V.Build != 0)
                {
                    this.Text += String.Format(".{0}", V.Build);
                }
                if (V.Revision != 0)
                {
                    this.Text += String.Format(".{0}", V.Revision);
                }
            }
            this.cmbCultures.Items.AddRange(POLUtils.AvailableCultures.ToArray());
            this.cmbCultures.SelectedItem = CultureChoice.Current;
            if (this.cmbCultures.Items.Count < 2)
            {
                this.cmbCultures.Enabled = false;
            }
            this.Show();
            this.UpdateSelectedRegion();
            // "Engrish Onry" is only of use if
            //  1) a JP POL client is installed, and
            //  2) the JP version of FFXI is installed.
            if ((POL.AvailableRegions & POL.Region.Japan) == 0 || !POL.IsAppInstalled(AppID.FFXI, POL.Region.Japan))
            {
                this.btnFFXIEngrishOnry.Enabled = false;
            }
            // FFXITC is only of use if
            // FFXI Test Client is installed
            if (!POL.IsAppInstalled(AppID.FFXITC))
            {
                this.btnFFXITCDataBrowser.Enabled = false;
            }
        }

        private void UpdateSelectedRegion()
        {
            this.txtSelectedRegion.Text = new NamedEnum(POL.SelectedRegion).Name;
            this.btnChooseRegion.Enabled = POL.MultipleRegionsAvailable;
            this.btnFFXIConfigEditor.Enabled = POL.IsAppInstalled(AppID.FFXI);
            this.btnFFXIDataBrowser.Enabled = POL.IsAppInstalled(AppID.FFXI);
            this.btnFFXIMacroManager.Enabled = POL.IsAppInstalled(AppID.FFXI);
            this.btnFFXIStrangeApparatus.Enabled = POL.IsAppInstalled(AppID.FFXI);
            this.btnFFXIEngrishOnry.Enabled = POL.IsAppInstalled(AppID.FFXI) && (POL.AvailableRegions & POL.Region.Japan) != 0 &&
                                              false;
            this.btnTetraViewer.Enabled = POL.IsAppInstalled(AppID.TetraMaster);
            this.btnFFXINPCRenamer.Enabled = POL.IsAppInstalled(AppID.FFXI);
        }

        #region Language Selection Events

        private void cmbCultures_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (!this.Visible || this.cmbCultures.SelectedItem == null)
            {
                return;
            }
            CultureChoice CC = this.cmbCultures.SelectedItem as CultureChoice;
            if (CC != null && CC.Name != CultureChoice.Current.Name)
            {
                CultureChoice.Current = CC;
                // Need to close and reopen the form to activate the changes
                POLUtils.KeepGoing = true;
                this.Close();
            }
        }

        #endregion

        #region TOS Warnings

        private bool AskMaybeTOSViolation()
        {
            return
                (MessageBox.Show(this, I18N.GetText("Text:AskMaybeTOSViolation"), I18N.GetText("Caption:AskMaybeTOSViolation"),
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes);
        }

        private bool AskTOSViolation()
        {
            return
                (MessageBox.Show(this, I18N.GetText("Text:AskTOSViolation"), I18N.GetText("Caption:AskTOSViolation"),
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes);
        }

        #endregion

        #region Button Events

        private void btnChooseRegion_Click(object sender, System.EventArgs e)
        {
            POL.ChooseRegion(this);
            this.UpdateSelectedRegion();
        }

        private void btnAudioManager_Click(object sender, System.EventArgs e)
        {
            this.Hide();
            using (Form Utility = new PlayOnline.Utils.AudioManager.MainWindow())
            {
                Utility.ShowDialog();
            }
            this.Show();
            this.Activate();
        }

        private void btnFFXIConfigEditor_Click(object sender, System.EventArgs e)
        {
            this.Hide();
            using (Form Utility = new PlayOnline.FFXI.Utils.ConfigEditor.MainWindow())
            {
                Utility.ShowDialog();
            }
            this.Show();
            this.Activate();
        }

        private void btnFFXIDataBrowser_Click(object sender, System.EventArgs e)
        {
            this.Hide();
            using (Form Utility = new PlayOnline.FFXI.Utils.DataBrowser.MainWindow(AppID.FFXI))
            {
                Utility.ShowDialog();
            }
            this.Show();
            this.Activate();
        }

        private void btnFFXITCDataBrowser_Click(object sender, System.EventArgs e)
        {
            this.Hide();
            using (Form Utility = new PlayOnline.FFXI.Utils.DataBrowser.MainWindow(AppID.FFXITC))
            {
                Utility.ShowDialog();
            }
            this.Show();
            this.Activate();
        }

        private void btnFFXIEngrishOnry_Click(object sender, EventArgs e)
        {
            if (this.AskMaybeTOSViolation())
            {
                this.Hide();
                using (Form Utility = new PlayOnline.FFXI.Utils.EngrishOnry.MainWindow())
                {
                    Utility.ShowDialog();
                }
                this.Show();
                this.Activate();
            }
        }

        private void btnFFXIItemComparison_Click(object sender, System.EventArgs e)
        {
            this.Hide();
            using (Form Utility = new PlayOnline.FFXI.Utils.ItemComparison.MainWindow())
            {
                Utility.ShowDialog();
            }
            this.Show();
            this.Activate();
        }

        private void btnFFXIMacroManager_Click(object sender, System.EventArgs e)
        {
            if (this.AskMaybeTOSViolation())
            {
                this.Hide();
                using (Form Utility = new PlayOnline.FFXI.Utils.MacroManager.MainWindow())
                {
                    Utility.ShowDialog();
                }
                this.Show();
                this.Activate();
            }
        }

        private void btnFFXIStrangeApparatus_Click(object sender, System.EventArgs e)
        {
            this.Hide();
            using (Form Utility = new PlayOnline.FFXI.Utils.StrangeApparatus.MainWindow())
            {
                Utility.ShowDialog();
            }
            this.Show();
            this.Activate();
        }

        private void btnTetraViewer_Click(object sender, System.EventArgs e)
        {
            this.Hide();
            using (Form Utility = new PlayOnline.Utils.TetraViewer.MainWindow())
            {
                Utility.ShowDialog();
            }
            this.Show();
            this.Activate();
        }

        private void btnFFXINPCRenamer_Click(object sender, EventArgs e)
        {
            if (this.AskTOSViolation())
            {
                this.Hide();
                using (Form Utility = new PlayOnline.FFXI.Utils.NPCRenamer.MainWindow())
                {
                    Utility.ShowDialog();
                }
                this.Show();
                this.Activate();
            }
        }

        #endregion
    }
}
