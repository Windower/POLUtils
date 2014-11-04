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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using PlayOnline.Core;
using PlayOnline.FFXI.Things;

namespace PlayOnline.FFXI.Utils.DataBrowser
{
    internal partial class ItemFindDialog : Form
    {
        private ThingList<Item> Items_;
        private Item SelectedItem_;

        public Item SelectedItem
        {
            get { return this.SelectedItem_; }
        }

        public ItemFindDialog(ThingList<Item> Items)
        {
            InitializeComponent();
            this.Icon = Icons.Search;
            this.Items_ = Items;
            this.SelectedItem_ = null;
            this.lstItems.Columns.Add(I18N.GetText("ColumnHeader:Index"), 40, HorizontalAlignment.Left);
            {
                // Add all item fields as columns for the result, and as entries on the "Copy" context menu
                Item I = new Item();
                foreach (string Field in I.GetAllFields())
                {
                    this.lstItems.Columns.Add(I.GetFieldName(Field), 100, HorizontalAlignment.Left);
                    this.mnuILCCopy.MenuItems.Add(new MenuItem(I.GetFieldName(Field), new EventHandler(this.CopyContextMenu_Click)));
                }
            }
            this.lstItems.ColumnClick += new ColumnClickEventHandler(ListViewColumnSorter.ListView_ColumnClick);
            this.AddPredicate();
        }

        #region Predicate Handling

        private List<ItemPredicate> Predicates_ = new List<ItemPredicate>(1);

        private void AddPredicate()
        {
            ItemPredicate IP = new ItemPredicate();
            IP.Width = this.pnlSearchOptions.Width;
            IP.Left = 0;
            if (this.Predicates_.Count > 0)
            {
                ItemPredicate LastIP = this.Predicates_[this.Predicates_.Count - 1];
                IP.Top = LastIP.Top + LastIP.Height;
            }
            else
            {
                IP.Top = this.btnClose.Top;
            }
            IP.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            this.pnlSearchOptions.Height += IP.Height;
            this.pnlSearchOptions.Controls.Add(IP);
            Button B = new Button(); // Add or Remove button
            B.Tag = IP;
            IP.Tag = B;
            B.FlatStyle = FlatStyle.System;
            B.Width = 20;
            B.Height = 20;
            IP.Width -= 3 + B.Width;
            B.Top = IP.Top;
            B.Left = IP.Left + IP.Width;
            B.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            if (this.Predicates_.Count == 0)
            {
                B.Text = "+";
                B.Click += new EventHandler(this.AddButton_Click);
            }
            else
            {
                B.Text = "-";
                B.Click += new EventHandler(this.RemoveButton_Click);
            }
            this.pnlSearchOptions.Controls.Add(B);
            this.Predicates_.Add(IP);
        }

        private void AddButton_Click(object sender, System.EventArgs e) { this.AddPredicate(); }

        private void RemoveButton_Click(object sender, System.EventArgs e)
        {
            Button B = sender as Button;
            if (B == null)
            {
                return;
            }
            ItemPredicate IP = B.Tag as ItemPredicate;
            if (IP != null)
            {
                int idx = this.Predicates_.IndexOf(IP);
                this.pnlSearchOptions.Controls.Remove(IP);
                this.pnlSearchOptions.Controls.Remove(B);
                this.Predicates_.Remove(IP);
                for (int i = idx; i < this.Predicates_.Count; ++i)
                {
                    ItemPredicate LowerIP = this.Predicates_[i];
                    LowerIP.Top -= 4 + IP.Height;
                    Button LowerButton = LowerIP.Tag as Button;
                    if (LowerButton != null)
                    {
                        LowerButton.Top -= 4 + IP.Height;
                    }
                }
                this.pnlSearchOptions.Height -= 4 + IP.Height;
            }
        }

        #endregion

        private ThingList<Item> SearchResults_ = new ThingList<Item>();

        private void InitializeResultsPane()
        {
            this.lstItems.Items.Clear();
            this.lstItems.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            this.ilItemIcons.Images.Clear();
            this.SearchResults_.Clear();
            this.stbStatus.Visible = true;
            this.stbStatus.Text = String.Format(I18N.GetText("Status:ItemSearch"), this.lstItems.Items.Count);
            Application.DoEvents();
        }

        private void FinalizeResultsPane()
        {
            foreach (ColumnHeader CH in this.lstItems.Columns)
            {
                CH.Width = -1;
                CH.Width += 2;
            }
            this.lstItems.HeaderStyle = ColumnHeaderStyle.Clickable;
            this.mnuILCEResults.Enabled = (this.lstItems.Items.Count > 0);
            this.stbStatus.Text = String.Format(I18N.GetText("Status:ItemSearchDone"), this.lstItems.Items.Count, this.Items_.Count);
        }

        private bool CheckQuery(Item I)
        {
            // Assume AND between predicates for now
            foreach (ItemPredicate IP in this.Predicates_)
            {
                if (!IP.IsMatch(I))
                {
                    return false;
                }
            }
            return true;
        }

        private PleaseWaitDialog PWD = null;

        private void DoExport(ThingList<Item> Items)
        {
            if (this.dlgExportFile.ShowDialog() == DialogResult.OK)
            {
                this.PWD = new PleaseWaitDialog(I18N.GetText("Dialog:ExportItems"));
                Thread T = new Thread(new ThreadStart(delegate()
                    {
                        Application.DoEvents();
                        Items.Save(this.dlgExportFile.FileName);
                        Application.DoEvents();
                        this.PWD.Invoke(new AnonymousMethod(delegate() { this.PWD.Close(); }));
                    }));
                T.CurrentUICulture = Thread.CurrentThread.CurrentUICulture;
                T.Start();
                this.PWD.ShowDialog(this);
                this.Activate();
                this.PWD.Dispose();
                this.PWD = null;
            }
        }

        #region Events

        private void btnRunQuery_Click(object sender, System.EventArgs e)
        {
            // Ensure the query is valid
            for (int i = 0; i < this.Predicates_.Count; ++i)
            {
                ItemPredicate IP = this.Predicates_[i];
                if (IP == null)
                {
                    continue;
                }
                string ValidationError = IP.ValidateQuery();
                if (ValidationError != null)
                {
                    MessageBox.Show(this, String.Format(I18N.GetText("Message:InvalidQuery"), i + 1, ValidationError),
                        I18N.GetText("Title:InvalidQuery"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            // Clear the results
            this.InitializeResultsPane();
            // And add all items that match
            foreach (Item I in this.Items_)
            {
                if (this.CheckQuery(I))
                {
                    this.SearchResults_.Add(I);
                    this.stbStatus.Text = String.Format(I18N.GetText("Status:ItemSearch"), this.SearchResults_.Count);
                    Application.DoEvents();
                }
            }
            foreach (Item I in this.SearchResults_)
            {
                this.ilItemIcons.Images.Add(I.GetIcon());
                ListViewItem LVI = this.lstItems.Items.Add("", this.ilItemIcons.Images.Count - 1);
                LVI.Tag = I;
                LVI.Text = this.lstItems.Items.Count.ToString();
                foreach (string Field in I.GetAllFields())
                {
                    LVI.SubItems.Add(I.GetFieldText(Field));
                }
            }
            this.FinalizeResultsPane();
        }

        private void btnClose_Click(object sender, EventArgs e) { this.Close(); }

        private void chkShowIcons_CheckedChanged(object sender, System.EventArgs e)
        {
            this.lstItems.SmallImageList = (this.chkShowIcons.Checked ? this.ilItemIcons : null);
        }

        private void lstItems_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            this.mnuILCESelected.Enabled = (this.lstItems.SelectedItems.Count > 0);
        }

        private void lstItems_DoubleClick(object sender, System.EventArgs e)
        {
            this.SelectedItem_ = this.lstItems.SelectedItems[0].Tag as Item;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void mnuILCProperties_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem LVI in this.lstItems.SelectedItems)
            {
                ThingPropertyPages TPP = new ThingPropertyPages(LVI.Tag as Item);
                TPP.Show(this);
            }
        }

        private void CopyContextMenu_Click(object sender, System.EventArgs e)
        {
            MenuItem MI = sender as MenuItem;
            if (MI != null && this.lstItems.SelectedItems.Count > 0)
            {
                string CopyText = String.Empty;
                foreach (ListViewItem LVI in this.lstItems.SelectedItems)
                {
                    if (CopyText != "")
                    {
                        CopyText += '\n';
                    }
                    CopyText += LVI.SubItems[MI.Index + 1].Text;
                }
                Clipboard.SetDataObject(CopyText, true);
            }
        }

        private void mnuILCECAll_Click(object sender, System.EventArgs e) { this.DoExport(this.Items_); }

        private void mnuILCEResults_Click(object sender, EventArgs e) { this.DoExport(this.SearchResults_); }

        private void mnuILCESelected_Click(object sender, EventArgs e)
        {
            ThingList<Item> Items = new ThingList<Item>();
            foreach (ListViewItem LVI in this.lstItems.SelectedItems)
            {
                Items.Add(LVI.Tag as Item);
            }
            this.DoExport(Items);
            Items.Clear();
        }

        #endregion
    }
}
