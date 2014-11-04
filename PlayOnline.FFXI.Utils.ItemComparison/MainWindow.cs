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
using System.Drawing.Imaging;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Xml;
using PlayOnline.Core;
using PlayOnline.FFXI;
using PlayOnline.FFXI.Things;

namespace PlayOnline.FFXI.Utils.ItemComparison
{
    public partial class MainWindow : Form
    {
        private ThingList<Item> LeftItems;
        private ThingList<Item> LeftItemsShown;
        private ThingList<Item> RightItems;
        private ThingList<Item> RightItemsShown;

        private int CurrentItem = -1;
        private int StartupHeight = -1;

        public MainWindow()
        {
            this.InitializeComponent();
            this.StartupHeight = this.Height;
            this.Icon = Icons.FileSearch;
            this.EnableNavigation();
        }

        // If possible, give the window that nice gradient look
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            if (VisualStyleRenderer.IsSupported)
            {
                VisualStyleRenderer VSR = new VisualStyleRenderer(VisualStyleElement.Tab.Body.Normal);
                VSR.DrawBackground(e.Graphics, this.ClientRectangle, e.ClipRectangle);
            }
            else
            {
                base.OnPaintBackground(e);
            }
        }

        private PleaseWaitDialog PWD = null;

        private OpenFileDialog dlgLoadItems1 = null;
        private OpenFileDialog dlgLoadItems2 = null;

        #region Item Loading & Duplicate Removal

        private void LoadItemsWorker(string FileName, ItemEditor IE)
        {
            ThingList<Item> TL = new ThingList<Item>();
            if (TL.Load(FileName))
            {
                if (IE == this.ieLeft)
                {
                    this.LeftItems = TL;
                }
                else
                {
                    this.RightItems = TL;
                }
            }
            this.LeftItemsShown = null;
            this.RightItemsShown = null;
            if (this.RightItems == null && this.LeftItems == null)
            {
                this.CurrentItem = -1;
            }
            else
            {
                this.CurrentItem = 0;
            }
            // In general, this tool supports comparing heterogenic item sets (as useless as that may be).
            // However, the 2010-09-09 patch prepended a range of 1024 armor pieces to the previous range (so 0x2800-0x4000 instead of 0x2C00-0x4000).
            // So we detect that specific case and cope with it padding the shorter set at the front (with nulls); this also means we should drop leading null entries whenever
            // a new set is loaded.
            while (this.LeftItems != null && this.LeftItems.Count > 0 && this.LeftItems[0] == null)
            {
                this.LeftItems.RemoveAt(0);
            }
            while (this.RightItems != null && this.RightItems.Count > 0 && this.RightItems[0] == null)
            {
                this.RightItems.RemoveAt(0);
            }
            if (this.RightItems != null && this.LeftItems != null)
            {
                if (this.LeftItems.Count != this.RightItems.Count)
                {
                    uint LID = (uint)this.LeftItems[0].GetFieldValue("id");
                    uint RID = (uint)this.RightItems[0].GetFieldValue("id");
                    if (LID == 0x2800 && RID == 0x2c00)
                    {
                        this.RightItems.InsertRange(0, new Item[0x400]);
                    }
                    if (LID == 0x2c00 && RID == 0x2800)
                    {
                        this.LeftItems.InsertRange(0, new Item[0x400]);
                    }
                }
                this.btnRemoveUnchanged.Invoke(new AnonymousMethod(delegate() { this.btnRemoveUnchanged.Enabled = true; }));
            }
            this.PWD.Invoke(new AnonymousMethod(delegate() { this.PWD.Close(); }));
        }

        private void LoadItems(string FileName, ItemEditor IE)
        {
            this.PWD = new PleaseWaitDialog(I18N.GetText("Dialog:LoadItems"));
            Thread T = new Thread(new ThreadStart(delegate() { this.LoadItemsWorker(FileName, IE); }));
            T.CurrentUICulture = Thread.CurrentThread.CurrentUICulture;
            T.Start();
            PWD.ShowDialog(this);
            this.Activate();
            this.PWD.Dispose();
            this.PWD = null;
            this.EnableNavigation();
            this.MarkItemChanges();
        }

        private void RemoveUnchangedItemsWorker()
        {
            Application.DoEvents();
            this.LeftItemsShown = new ThingList<Item>();
            this.RightItemsShown = new ThingList<Item>();
            for (int i = 0; i < this.LeftItems.Count && i < this.RightItems.Count; ++i)
            {
                Item LI = this.LeftItems[i];
                Item RI = this.RightItems[i];
                bool DifferenceSeen = false;
                if (LI == null)
                {
                    DifferenceSeen = !(RI == null || RI.GetFieldText("name") == String.Empty || RI.GetFieldText("name") == ".");
                }
                else if (RI == null)
                {
                    DifferenceSeen = !(LI == null || LI.GetFieldText("name") == String.Empty || LI.GetFieldText("name") == ".");
                }
                else if (this.GetIconString(LI) != this.GetIconString(RI))
                {
                    DifferenceSeen = true;
                }
                else
                {
                    foreach (string Field in Item.AllFields)
                    {
                        if (!this.ieLeft.IsFieldShown(Field)) // If we can't see the difference, there's no point
                        {
                            continue;
                        }
                        if (LI.GetFieldText(Field) != RI.GetFieldText(Field))
                        {
                            DifferenceSeen = true;
                            break;
                        }
                    }
                }
                if (DifferenceSeen)
                {
                    this.LeftItemsShown.Add(LI);
                    this.RightItemsShown.Add(RI);
                }
                Application.DoEvents();
            }
            // All non-dummy overflow items are "changed"
            if (this.LeftItems.Count < this.RightItems.Count)
            {
                int OverflowPos = this.LeftItems.Count;
                while (OverflowPos < this.RightItems.Count)
                {
                    Item I = this.RightItems[OverflowPos++];
                    if (I == null || I.GetFieldText("name") == String.Empty || I.GetFieldText("name") == ".")
                    {
                        continue;
                    }
                    this.RightItemsShown.Add(I);
                }
            }
            else if (this.LeftItems.Count > this.RightItems.Count)
            {
                int OverflowPos = this.RightItems.Count;
                while (OverflowPos < this.LeftItems.Count)
                {
                    Item I = this.LeftItems[OverflowPos++];
                    if (I == null || I.GetFieldText("name") == String.Empty || I.GetFieldText("name") == ".")
                    {
                        continue;
                    }
                    this.LeftItemsShown.Add(I);
                }
            }
            this.CurrentItem = ((this.LeftItemsShown.Count == 0) ? -1 : 0);
            this.PWD.Invoke(new AnonymousMethod(delegate() { this.PWD.Close(); }));
        }

        private void RemoveUnchangedItems()
        {
            this.btnRemoveUnchanged.Enabled = false;
            this.PWD = new PleaseWaitDialog(I18N.GetText("Dialog:RemoveUnchanged"));
            Thread T = new Thread(new ThreadStart(delegate() { this.RemoveUnchangedItemsWorker(); }));
            T.CurrentUICulture = Thread.CurrentThread.CurrentUICulture;
            T.Start();
            PWD.ShowDialog(this);
            this.Activate();
            this.PWD.Dispose();
            this.PWD = null;
            this.EnableNavigation();
            this.MarkItemChanges();
        }

        #endregion

        #region Item Display

        private string GetIconString(Item I)
        {
            string IconString = I.GetFieldText("icon");
            Image Icon = I.GetIcon();
            if (Icon != null)
            {
                MemoryStream MS = new MemoryStream();
                Icon.Save(MS, ImageFormat.Png);
                IconString += Convert.ToBase64String(MS.GetBuffer());
                MS.Close();
            }
            return IconString;
        }

        private void MarkItemChanges()
        {
            if (this.ieLeft.Item != null && this.ieRight.Item != null)
            {
                // Compare fields
                foreach (string Field in Item.AllFields)
                {
                    if (this.ieLeft.IsFieldShown(Field))
                    {
                        bool FieldChanged = (this.ieLeft.Item.GetFieldText(Field) != this.ieRight.Item.GetFieldText(Field));
                        this.ieLeft.MarkField(Field, FieldChanged);
                        this.ieRight.MarkField(Field, FieldChanged);
                    }
                }
                {
                    // Compare icon
                    bool IconChanged = (this.GetIconString(this.ieLeft.Item) != this.GetIconString(this.ieRight.Item));
                    this.ieLeft.MarkField("icon", IconChanged);
                    this.ieRight.MarkField("icon", IconChanged);
                }
            }
        }

        private void EnableNavigation()
        {
            this.ieLeft.Item = null;
            this.ieRight.Item = null;
            this.btnPrevious.Enabled = (this.CurrentItem > 0);
            this.btnNext.Enabled = false;
            Item LeftItem = null;
            Item RightItem = null;
            if (this.CurrentItem >= 0)
            {
                if (this.LeftItemsShown != null)
                {
                    if (this.CurrentItem < this.LeftItemsShown.Count)
                    {
                        LeftItem = this.LeftItemsShown[this.CurrentItem];
                    }
                    if (this.CurrentItem < this.LeftItemsShown.Count - 1)
                    {
                        this.btnNext.Enabled = true;
                    }
                }
                else if (this.LeftItems != null)
                {
                    if (this.CurrentItem < this.LeftItems.Count)
                    {
                        LeftItem = this.LeftItems[this.CurrentItem];
                    }
                    if (this.CurrentItem < this.LeftItems.Count - 1)
                    {
                        this.btnNext.Enabled = true;
                    }
                }
                if (this.RightItemsShown != null)
                {
                    if (this.CurrentItem < this.RightItemsShown.Count)
                    {
                        RightItem = this.RightItemsShown[this.CurrentItem];
                    }
                    if (this.CurrentItem < this.RightItemsShown.Count - 1)
                    {
                        this.btnNext.Enabled = true;
                    }
                }
                else if (this.RightItems != null)
                {
                    if (this.CurrentItem < this.RightItems.Count)
                    {
                        RightItem = this.RightItems[this.CurrentItem];
                    }
                    if (this.CurrentItem < this.RightItems.Count - 1)
                    {
                        this.btnNext.Enabled = true;
                    }
                }
            }
            else
            {
                this.btnNext.Enabled = false;
            }
            this.ieLeft.Item = LeftItem;
            this.ieRight.Item = RightItem;
        }

        #endregion

        #region Event Handlers

        private OpenFileDialog CreateLoadItemsDialog()
        {
            OpenFileDialog OFD = new OpenFileDialog();
            OFD.Title = I18N.GetText("OpenDialog:Title");
            OFD.Filter = I18N.GetText("OpenDialog:Filter");
            return OFD;
        }

        private void btnLoadItemSet1_Click(object sender, System.EventArgs e)
        {
            if (this.dlgLoadItems1 == null)
            {
                this.dlgLoadItems1 = this.CreateLoadItemsDialog();
            }
            if (this.dlgLoadItems1.ShowDialog(this) == DialogResult.OK)
            {
                this.LoadItems(this.dlgLoadItems1.FileName, this.ieLeft);
            }
        }

        private void btnLoadItemSet2_Click(object sender, System.EventArgs e)
        {
            if (this.dlgLoadItems2 == null)
            {
                this.dlgLoadItems2 = this.CreateLoadItemsDialog();
            }
            if (this.dlgLoadItems2.ShowDialog(this) == DialogResult.OK)
            {
                this.LoadItems(this.dlgLoadItems2.FileName, this.ieRight);
            }
        }

        private void btnPrevious_Click(object sender, System.EventArgs e)
        {
            --this.CurrentItem;
            this.EnableNavigation();
            this.MarkItemChanges();
        }

        private void btnNext_Click(object sender, System.EventArgs e)
        {
            ++this.CurrentItem;
            this.EnableNavigation();
            this.MarkItemChanges();
        }

        private void btnRemoveUnchanged_Click(object sender, System.EventArgs e) { this.RemoveUnchangedItems(); }

        private void ItemViewerSizeChanged(object sender, System.EventArgs e)
        {
            int WantedHeight = this.StartupHeight + Math.Max(this.ieLeft.Height, this.ieRight.Height) + 4;
            if (this.Height < WantedHeight)
            {
                this.Height = WantedHeight;
            }
        }

        #endregion
    }
}
