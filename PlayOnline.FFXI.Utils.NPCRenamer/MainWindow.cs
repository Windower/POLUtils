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
using System.Windows.Forms;
using PlayOnline.Core;

namespace PlayOnline.FFXI.Utils.NPCRenamer
{
    public partial class MainWindow : Form
    {
        private class Area
        {
            public ushort ID;

            public Area(ushort ID) { this.ID = ID; }

            public override string ToString() { return FFXIResourceManager.GetAreaName(this.ID); }

            public List<NPCInfo> Contents
            {
                get
                {
                    List<NPCInfo> Result = new List<NPCInfo>();
                    string DATFileName = FFXI.GetFilePath(6720 + this.ID);
                    if (DATFileName != null)
                    {
                        try
                        {
                            BinaryReader BR = new BinaryReader(new FileStream(DATFileName, FileMode.Open, FileAccess.Read),
                                Encoding.ASCII);
                            while (BR.BaseStream.Position != BR.BaseStream.Length)
                            {
                                string Name = new string(BR.ReadChars(0x1C)).TrimEnd('\0');
                                Result.Add(new NPCInfo(BR.ReadUInt32(), Name));
                            }
                            BR.Close();
                        }
                        catch
                        {
                            Result.Clear();
                        }
                    }
                    return Result;
                }
            }
        }

        private class NPCInfo
        {
            public uint ID;
            public string Name;

            public NPCInfo(uint ID, string Name)
            {
                this.ID = ID;
                this.Name = Name;
            }
        }

        public MainWindow()
        {
            this.InitializeComponent();
            this.Icon = Icons.TextFile;
            NameChange.LoadHistory();
            for (ushort AreaID = 0; AreaID < 256; ++AreaID)
            {
                string AreaName = FFXIResourceManager.GetAreaName(AreaID);
                if (AreaName == null || AreaName == String.Empty)
                {
                    continue;
                }
                this.cmbArea.Items.Add(new Area(AreaID));
            }
        }

        private void MainWindow_Shown(object sender, EventArgs e)
        {
            this.cmbArea.Select();
            this.Update();
            if (this.cmbArea.Items.Count > 0)
            {
                this.cmbArea.SelectedIndex = 0;
            }
        }

        private void cmbArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.lstNPCNames.Items.Clear();
            Area A = this.cmbArea.SelectedItem as Area;
            if (A != null)
            {
                foreach (NPCInfo NI in A.Contents)
                {
                    ListViewItem LVI = this.lstNPCNames.Items.Add(NI.Name);
                    LVI.Tag = NI;
                }
            }
        }

        private void lstNPCNames_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            if (e.Label == null) // User made no changes to the label text
            {
                return;
            }
            NPCInfo NI = this.lstNPCNames.Items[e.Item].Tag as NPCInfo;
            if (NI != null)
            {
                string NewName = e.Label;
                if (NewName.Length > 0x1C)
                {
                    NewName = NewName.Substring(0, 0x1C);
                    this.lstNPCNames.Items[e.Item].Text = NewName;
                }
                NameChange.Add(NI.ID, NI.Name, NewName);
                NI.Name = NewName;
            }
        }

        private void lstNPCNames_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2 && this.lstNPCNames.SelectedItems.Count > 0)
            {
                this.lstNPCNames.SelectedItems[0].BeginEdit();
                e.Handled = true;
            }
        }

        private void btnShowChanges_Click(object sender, EventArgs e)
        {
            using (NameChanges NC = new NameChanges())
            {
                NC.ShowDialog(this);
            }
        }

        private void btnClose_Click(object sender, EventArgs e) { this.Close(); }
    }
}
