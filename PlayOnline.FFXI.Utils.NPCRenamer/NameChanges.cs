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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using PlayOnline.Core;

namespace PlayOnline.FFXI.Utils.NPCRenamer
{
    public partial class NameChanges : Form
    {
        public NameChanges()
        {
            this.InitializeComponent();
            this.RefreshList();
        }

        private void RefreshList()
        {
            this.lstNameChanges.Items.Clear();
            this.lstNameChanges.Groups.Clear();
            ListViewGroup lvgPending = new ListViewGroup("Pending", I18N.GetText("Group:PendingChanges"));
            this.lstNameChanges.Groups.Add(lvgPending);
            foreach (NameChange NC in NameChange.Pending)
            {
                ListViewItem LVI = this.lstNameChanges.Items.Add(NC.Area);
                LVI.Tag = NC;
                LVI.Group = lvgPending;
                LVI.SubItems.Add(NC.Old);
                LVI.SubItems.Add(NC.New);
            }
            ListViewGroup lvgApplied = new ListViewGroup("Applied", I18N.GetText("Group:AppliedChanges"));
            this.lstNameChanges.Groups.Add(lvgApplied);
            foreach (NameChange NC in NameChange.Applied)
            {
                ListViewItem LVI = this.lstNameChanges.Items.Add(NC.Area);
                LVI.Tag = NC;
                LVI.Group = lvgApplied;
                LVI.SubItems.Add(NC.Old);
                LVI.SubItems.Add(NC.New);
            }
            this.btnForgetAll.Enabled = this.btnRevertAll.Enabled = (this.lstNameChanges.Items.Count > 0);
            this.btnDiscardPending.Enabled = this.btnWritePending.Enabled = (lvgPending.Items.Count > 0);
            NameChange.SaveHistory();
        }

        private void lstNameChanges_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                // Ctrl-A = Select All
                this.lstNameChanges.SelectedIndices.Clear();
                for (int i = 0; i < this.lstNameChanges.Items.Count; ++i)
                {
                    this.lstNameChanges.SelectedIndices.Add(i);
                }
                e.Handled = true;
            }
        }

        private void mnuNameChangeContext_Opening(object sender, CancelEventArgs e)
        {
            if (this.lstNameChanges.SelectedItems.Count == 0)
            {
                e.Cancel = true;
            }
            else
            {
                this.mnuWriteSelected.Enabled = false;
                foreach (ListViewItem LVI in this.lstNameChanges.SelectedItems)
                {
                    if (LVI.Group == this.lstNameChanges.Groups["Pending"])
                    {
                        this.mnuWriteSelected.Enabled = true;
                        break;
                    }
                }
            }
        }

        private void mnuWriteSelected_Click(object sender, EventArgs e)
        {
            if (this.lstNameChanges.SelectedItems.Count > 0)
            {
                this.prbWriteChanges.Value = 0;
                this.prbWriteChanges.Maximum = this.lstNameChanges.SelectedItems.Count;
                this.prbWriteChanges.Visible = true;
                foreach (ListViewItem LVI in this.lstNameChanges.SelectedItems)
                {
                    NameChange.Apply(LVI.Tag as NameChange);
                    ++this.prbWriteChanges.Value;
                }
                this.RefreshList();
                this.prbWriteChanges.Visible = false;
            }
        }

        private void mnuRevertSelected_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem LVI in this.lstNameChanges.SelectedItems)
            {
                NameChange.Revert(LVI.Tag as NameChange);
            }
            this.RefreshList();
        }

        private void mnuSaveSelected_Click(object sender, EventArgs e)
        {
            if (this.dlgSaveChangeset.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    XmlDocument Changeset = NameChange.CreateChangeset();
                    foreach (ListViewItem LVI in this.lstNameChanges.SelectedItems)
                    {
                        NameChange NC = LVI.Tag as NameChange;
                        if (NC != null)
                        {
                            NC.AddToChangeset(Changeset);
                        }
                    }
                    Changeset.Save(this.dlgSaveChangeset.FileName);
                }
                catch (Exception E)
                {
                    MessageBox.Show(String.Format(I18N.GetText("Message:ChangesetSaveError"), E.Message),
                        I18N.GetText("Title:ChangesetSaveError"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnApplySet_Click(object sender, EventArgs e)
        {
            if (this.dlgLoadChangeset.ShowDialog(this) == DialogResult.OK)
            {
                NameChange.LoadChangeset(this.dlgLoadChangeset.FileName);
                this.RefreshList();
            }
        }

        private void btnUnapplySet_Click(object sender, EventArgs e)
        {
            if (this.dlgLoadChangeset.ShowDialog(this) == DialogResult.OK)
            {
                NameChange.LoadChangeset(this.dlgLoadChangeset.FileName, true);
                this.RefreshList();
            }
        }

        private void btnForgetAll_Click(object sender, EventArgs e)
        {
            if (
                MessageBox.Show(this, I18N.GetText("Message:ForgetAll"), I18N.GetText("Title:ForgetAll"), MessageBoxButtons.YesNo,
                    MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                NameChange.Applied.Clear();
                NameChange.Pending.Clear();
                this.RefreshList();
            }
        }

        private void btnRevertAll_Click(object sender, EventArgs e)
        {
            if (
                MessageBox.Show(this, I18N.GetText("Message:RevertAll"), I18N.GetText("Title:RevertAll"), MessageBoxButtons.YesNo,
                    MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                NameChange.Pending.Clear();
                foreach (NameChange NC in NameChange.Applied)
                {
                    NameChange.Revert(NC);
                }
                while (NameChange.Pending.Count > 0)
                {
                    NameChange.Apply(NameChange.Pending[0]);
                }
                this.RefreshList();
            }
        }

        private void btnDiscardPending_Click(object sender, EventArgs e)
        {
            NameChange.Pending.Clear();
            this.RefreshList();
        }

        private void btnWritePending_Click(object sender, EventArgs e)
        {
            if (NameChange.Pending.Count > 0)
            {
                this.prbWriteChanges.Value = 0;
                this.prbWriteChanges.Maximum = NameChange.Pending.Count;
                this.prbWriteChanges.Visible = true;
                while (NameChange.Pending.Count > 0)
                {
                    NameChange.Apply(NameChange.Pending[0]);
                    ++this.prbWriteChanges.Value;
                }
                this.RefreshList();
                this.prbWriteChanges.Visible = false;
            }
        }

        private void btnClose_Click(object sender, EventArgs e) { this.Close(); }
    }
}
