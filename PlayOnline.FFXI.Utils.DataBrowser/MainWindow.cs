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
using System.Drawing.Imaging;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using PlayOnline.Core;
using PlayOnline.FFXI.Things;

namespace PlayOnline.FFXI.Utils.DataBrowser
{
    public partial class MainWindow : Form
    {
        #region Custom Menu Items

        private class ROMMenuItem : MenuItem
        {
            public ROMMenuItem(string Text, int FileID, EventHandler OnClick)
                : base(Text, OnClick)
            {
                if (FFXI.GetFilePath(FileID, out this.App_, out this.Dir_, out this.File_))
                {
                    this.UpdatePath();
                }
                else
                {
                    this.Enabled = false;
                }
            }

            public ROMMenuItem(string Text, byte App, byte Dir, byte File, EventHandler OnClick)
                : base(Text, OnClick)
            {
                this.App_ = App;
                this.Dir_ = Dir;
                this.File_ = File;
                this.UpdatePath();
            }

            private void UpdatePath()
            {
                this.Path_ = FFXI.GetFilePath(this.App_, this.Dir_, this.File_);
                this.Enabled = File.Exists(this.Path_);
            }

            public byte ROMApp
            {
                get { return this.App_; }
            }

            public short ROMDir
            {
                get { return this.Dir_; }
            }

            public byte ROMFile
            {
                get { return this.File_; }
            }

            public string ROMPath
            {
                get { return this.Path_; }
            }

            private byte App_;
            private short Dir_;
            private byte File_;
            private string Path_;
        }

        private class CategoryMenuItem : MenuItem
        {
            private bool SubItemsAdded_ = false;
            private XmlNode XCategory_ = null;
            private EventHandler ROMMenuItemClick_ = null;

            public CategoryMenuItem(XmlNode XCategory, EventHandler ROMMenuItemClick)
                : base()
            {
                this.XCategory_ = XCategory;
                this.ROMMenuItemClick_ = ROMMenuItemClick;
                this.Text = this.BuildItemName(XCategory.SelectSingleNode("./name"));
                this.MenuItems.Add(String.Empty); // dummy entry - replaced on first popup
            }

            protected override void OnPopup(EventArgs e)
            {
                if (!this.SubItemsAdded_)
                {
                    this.MenuItems.Clear();
                    foreach (XmlNode XN in this.XCategory_.ChildNodes)
                    {
                        if (XN is XmlElement)
                        {
                            if (XN.Name == "category")
                            {
                                // Only add it if there's stuff in it
                                if (XN.SelectSingleNode("./category | ./rom-file") != null)
                                {
                                    this.MenuItems.Add(new CategoryMenuItem(XN, this.ROMMenuItemClick_));
                                }
                            }
                            else if (XN.Name == "separator")
                            {
                                this.MenuItems.Add("-");
                            }
                            else if (XN.Name == "rom-file")
                            {
                                // Don't add it unless it points somewhere (note: can cause empty category items)
                                if (XN.Attributes["id"] != null)
                                {
                                    this.MenuItems.Add(new ROMMenuItem(this.BuildItemName(XN),
                                        XmlConvert.ToInt32(XN.Attributes["id"].InnerText), this.ROMMenuItemClick_));
                                }
                                else if (XN.Attributes["app"] != null && XN.Attributes["dir"] != null &&
                                         XN.Attributes["file"] != null)
                                {
                                    byte ROMApp = XmlConvert.ToByte(XN.Attributes["app"].InnerText);
                                    byte ROMDir = XmlConvert.ToByte(XN.Attributes["dir"].InnerText);
                                    byte ROMFile = XmlConvert.ToByte(XN.Attributes["file"].InnerText);
                                    this.MenuItems.Add(new ROMMenuItem(this.BuildItemName(XN), ROMApp, ROMDir, ROMFile,
                                        this.ROMMenuItemClick_));
                                }
                            }
                        }
                    }
                    this.SubItemsAdded_ = true;
                }
                base.OnPopup(e);
            }

            private string BuildItemName(XmlNode XName)
            {
                if (XName == null)
                {
                    return "???";
                }
                string ItemName = String.Empty;
                foreach (XmlNode XN in XName.ChildNodes)
                {
                    if (XN is XmlText)
                    {
                        ItemName += XN.InnerText;
                    }
                    else if (XN is XmlElement)
                    {
                        XmlElement XE = XN as XmlElement;
                        if (XE.Name == "i18n-string" && XE.HasAttribute("id"))
                        {
                            ItemName += I18N.GetText(XE.Attributes["id"].InnerText);
                        }
                        else if (XE.Name == "area-name" && XE.HasAttribute("id"))
                        {
                            ushort ID = 0;
                            try
                            {
                                ID = ushort.Parse(XE.Attributes["id"].InnerText);
                            }
                            catch {}
                            ItemName += FFXIResourceManager.GetAreaName(ID);
                        }
                        else if (XE.Name == "region-name" && XE.HasAttribute("id"))
                        {
                            ushort ID = 0;
                            try
                            {
                                ID = ushort.Parse(XE.Attributes["id"].InnerText);
                            }
                            catch {}
                            ItemName += FFXIResourceManager.GetRegionName(ID);
                        }
                        else
                        {
                            ItemName += '?' + XE.Name + '?';
                        }
                    }
                }
                return ItemName;
            }
        }

        #endregion

        private ThingList<Item> LoadedItems_ = null;
        private ThingList LoadedThings_ = null;
        private PleaseWaitDialog PWD = null;
        private ImageList ListIcons_ = new ImageList();
        private Dictionary<string, ListView> ListViews_ = new Dictionary<string, ListView>();
        private TabControl ListViewTabs_ = null;

        public MainWindow(string appID)
        {
            this.InitializeComponent();
            this.Icon = Icons.CheckedPage;
            try
            {
                this.ilBrowserIcons.Images.Add(Icons.DocFolder);
                this.ilBrowserIcons.Images.Add(Icons.FolderClosed);
                this.ilBrowserIcons.Images.Add(Icons.FolderOpen);
                this.ilBrowserIcons.Images.Add(Icons.ConfigFile);
            }
            catch (Exception E)
            {
                Console.WriteLine("{0}", E.ToString());
                this.tvDataFiles.ImageList = null;
            }
            for (int i = 1; i < 20; ++i)
            {
                string DataDir = Path.Combine(POL.GetApplicationPath(appID), "Rom");
                if (i > 1)
                {
                    DataDir += i.ToString();
                }
                if (Directory.Exists(DataDir))
                {
                    TreeNode Root = this.tvDataFiles.Nodes.Add(I18N.GetText(String.Format("FFXI{0}", i)));
                    Root.ImageIndex = Root.SelectedImageIndex = 0;
                    Root.Tag = DataDir;
                    Root.Nodes.Add("<dummy>").Tag = Root;
                }
            }
            this.InitializeROMMenus();
            this.ResetViewers();
        }

        private ListView MaybeAddListView(string Key)
        {
            if (this.ListViews_.ContainsKey(Key))
            {
                return this.ListViews_[Key];
            }
            ListView LV = new ListView();
            // Set up the listview the way we want it
            LV.AllowColumnReorder = true;
            LV.ContextMenu = this.mnuEntryListContext;
            LV.Dock = DockStyle.Fill;
            LV.FullRowSelect = true;
            LV.GridLines = true;
            LV.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            LV.View = View.Details;
            LV.ColumnClick += new ColumnClickEventHandler(ListViewColumnSorter.ListView_ColumnClick);
            LV.DoubleClick += new EventHandler(ListView_DoubleClick);
            LV.KeyDown += new KeyEventHandler(ListView_KeyDown);
            // Add the always-present first column (so that the "real" data is always in subitems)
            LV.Columns.Add("<type>", "[Data Type]", 60, HorizontalAlignment.Left, -1);
            if (this.ListViews_.Count == 0)
            {
                this.pnlGeneralContents.Controls.Add(LV);
            }
            else
            {
                if (this.ListViewTabs_ == null)
                {
                    // First switch to tabbed display mode
                    this.ListViewTabs_ = new TabControl();
                    this.ListViewTabs_.Dock = DockStyle.Fill;
                    foreach (string OldKey in this.ListViews_.Keys)
                    {
                        TabPage TP = new TabPage(OldKey);
                        TP.Controls.Add(this.ListViews_[OldKey]);
                        this.ListViewTabs_.TabPages.Add(TP);
                    }
                    this.pnlGeneralContents.Controls.Clear();
                    this.pnlGeneralContents.Controls.Add(ListViewTabs_);
                }
                {
                    TabPage TP = new TabPage(Key);
                    TP.Controls.Add(LV);
                    this.ListViewTabs_.TabPages.Add(TP);
                }
            }
            this.ListViews_[Key] = LV;
            return LV;
        }

        private void ResetViewers()
        {
            // Clear the entire right-hand pane
            this.pnlNoViewers.Visible = false;
            this.tabViewers.Visible = false;
            // Reset the viewer tabs
            this.tabViewers.TabPages.Clear();
            this.pnlGeneralContents.Controls.Clear();
            if (this.ListViewTabs_ != null)
            {
                this.ListViewTabs_.TabPages.Clear();
            }
            this.ListViewTabs_ = null;
            // Reset all applicable viewer tab contents
            foreach (ListView LV in this.ListViews_.Values)
            {
                LV.Clear();
            }
            this.ListViews_.Clear();
            this.ListIcons_.Images.Clear();
            this.chkShowIcons.Checked = false;
            this.btnThingListSaveImages.Enabled = false;
            this.cmbItems.Items.Clear();
            this.cmbImageChooser.Items.Clear();
            this.picImageViewer.Image = null;
            this.picImageViewer.Tag = null;
        }

        private void LoadFile(string FileName)
        {
            this.ResetViewers();
            this.LoadedThings_ = null;
            if (FileName != null && File.Exists(FileName))
            {
                this.Enabled = false;
                FileScanner FS = new FileScanner();
                FS.ScanFile(this, FileName);
                this.LoadedThings_ = FS.FileContents;
                this.LoadedItems_ = new ThingList<Item>();
                if (FS.FileContents != null)
                {
                    int LoadCount = 0;
                    if (FS.FileContents.Count > 0)
                    {
                        this.tabViewers.TabPages.Add(this.tabViewerGeneral);
                        this.tabViewers.Visible = true;
                        Application.DoEvents();
                        foreach (IThing T in FS.FileContents)
                        {
                            ListView LV = this.MaybeAddListView(T.TypeName);
                            int IconIndex = -1;
                            {
                                Image Icon = T.GetIcon();
                                if (Icon != null)
                                {
                                    IconIndex = this.ListIcons_.Images.Count;
                                    this.ListIcons_.Images.Add(Icon);
                                }
                            }
                            ListViewItem LVI = LV.Items.Add(T.TypeName, IconIndex);
                            LVI.Tag = T;
                            for (int i = 1; i < LV.Columns.Count; ++i)
                            {
                                LVI.SubItems.Add("");
                            }
                            foreach (string Field in T.GetFields())
                            {
                                if (!LV.Columns.ContainsKey(Field))
                                {
                                    LV.Columns.Add(Field, T.GetFieldName(Field), 60, HorizontalAlignment.Left, -1);
                                    LVI.SubItems.Add("");
                                }
                                LVI.SubItems[LV.Columns[Field].Index].Text = T.GetFieldText(Field);
                            }
                            if (T is Item)
                            {
                                if (this.cmbItems.Items.Count == 0)
                                {
                                    this.tabViewers.TabPages.Add(this.tabViewerItems);
                                }
                                this.cmbItems.Items.Add(T);
                                this.LoadedItems_.Add(T as Item);
                            }
                            else if (T is Graphic)
                            {
                                if (this.cmbImageChooser.Items.Count == 0)
                                {
                                    this.tabViewers.TabPages.Add(this.tabViewerImages);
                                }
                                this.cmbImageChooser.Items.Add(T);
                            }
                            if ((++LoadCount % 100) == 0)
                            {
                                Application.DoEvents();
                            }
                        }
                        this.btnThingListSaveImages.Enabled = (this.ListIcons_.Images.Count != 0);
                        this.chkShowIcons.Enabled = (this.ListIcons_.Images.Count != 0);
                        Application.DoEvents();
                        foreach (ListView LV in this.ListViews_.Values)
                        {
                            if (this.ListIcons_.Images.Count == 0)
                            {
                                LV.SmallImageList = null;
                            }
                            LV.HeaderStyle = ColumnHeaderStyle.Clickable;
                        }
                        this.ResizeListColumns();
                    }
                    if (this.cmbImageChooser.Items.Count > 0)
                    {
                        this.cmbImageChooser.SelectedItem = null;
                        this.tabViewers.SelectedTab = this.tabViewerImages;
                        this.cmbImageChooser.Select();
                        this.cmbImageChooser.SelectedIndex = 0;
                        Application.DoEvents();
                    }
                    if (this.cmbItems.Items.Count > 0)
                    {
                        this.cmbItems.SelectedItem = null;
                        this.tabViewers.SelectedTab = this.tabViewerItems;
                        this.cmbItems.Select();
                        this.cmbItems.SelectedIndex = 0;
                        Application.DoEvents();
                    }
                }
                if (!this.tabViewers.Visible)
                {
                    this.pnlNoViewers.Visible = true;
                }
                this.Enabled = true;
            }
        }

        private void ResizeListColumns()
        {
            foreach (ListView LV in this.ListViews_.Values)
            {
                foreach (ColumnHeader CH in LV.Columns)
                {
                    CH.Width = -2;
                    CH.Width += 2;
                }
                Application.DoEvents();
            }
        }

        #region Image Viewer Events

        private void cmbImageChooser_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            Graphic G = this.cmbImageChooser.SelectedItem as Graphic;
            if (G != null)
            {
                this.picImageViewer.Image = G.GetIcon();
                this.picImageViewer.Tag = G.ToString();
            }
        }

        #region Context Menu Events

        private PictureBox GetSourcePicture(MenuItem SourceMenu)
        {
            if (SourceMenu != null)
            {
                ContextMenu CM = SourceMenu.GetContextMenu();
                if (CM != null)
                {
                    return CM.SourceControl as PictureBox;
                }
            }
            return null;
        }

        private void SetPictureSizeMode(PictureBox PB, PictureBoxSizeMode SizeMode)
        {
            if (PB != null)
            {
                PB.SizeMode = SizeMode;
            }
        }

        private void SetPictureBackground(PictureBox PB, Color BackColor)
        {
            if (PB != null)
            {
                PB.BackColor = BackColor;
            }
        }

        private void SavePicture(PictureBox PB)
        {
            if (PB != null)
            {
                this.dlgSavePicture.FileName = PB.Tag as string;
                if (this.dlgSavePicture.ShowDialog() == DialogResult.OK)
                {
                    ImageFormat IF = ImageFormat.Bmp;
                    switch (this.dlgSavePicture.FilterIndex)
                    {
                    case 1:
                        IF = ImageFormat.Bmp;
                        break;
                    case 2:
                        IF = ImageFormat.Png;
                        break;
                    }
                    PB.Image.Save(this.dlgSavePicture.FileName, IF);
                }
            }
        }

        private void mnuPCModeNormal_Click(object sender, System.EventArgs e)
        {
            this.mnuPCModeNormal.Checked = true;
            this.mnuPCModeCentered.Checked = false;
            this.mnuPCModeStretched.Checked = false;
            this.mnuPCModeZoomed.Checked = false;
            this.SetPictureSizeMode(this.GetSourcePicture(sender as MenuItem), PictureBoxSizeMode.Normal);
        }

        private void mnuPCModeCentered_Click(object sender, System.EventArgs e)
        {
            this.mnuPCModeNormal.Checked = false;
            this.mnuPCModeCentered.Checked = true;
            this.mnuPCModeStretched.Checked = false;
            this.mnuPCModeZoomed.Checked = false;
            this.SetPictureSizeMode(this.GetSourcePicture(sender as MenuItem), PictureBoxSizeMode.CenterImage);
        }

        private void mnuPCModeStretched_Click(object sender, System.EventArgs e)
        {
            this.mnuPCModeNormal.Checked = false;
            this.mnuPCModeCentered.Checked = false;
            this.mnuPCModeStretched.Checked = true;
            this.mnuPCModeZoomed.Checked = false;
            this.SetPictureSizeMode(this.GetSourcePicture(sender as MenuItem), PictureBoxSizeMode.StretchImage);
        }

        private void mnuPCModeZoomed_Click(object sender, EventArgs e)
        {
            this.mnuPCModeNormal.Checked = false;
            this.mnuPCModeCentered.Checked = false;
            this.mnuPCModeStretched.Checked = false;
            this.mnuPCModeZoomed.Checked = true;
            this.SetPictureSizeMode(this.GetSourcePicture(sender as MenuItem), PictureBoxSizeMode.Zoom);
        }

        private void mnuPCBackgroundBlack_Click(object sender, System.EventArgs e)
        {
            this.mnuPCBackgroundBlack.Checked = true;
            this.mnuPCBackgroundWhite.Checked = false;
            this.mnuPCBackgroundTransparent.Checked = false;
            this.SetPictureBackground(this.GetSourcePicture(sender as MenuItem), Color.Black);
        }

        private void mnuPCBackgroundWhite_Click(object sender, System.EventArgs e)
        {
            this.mnuPCBackgroundBlack.Checked = false;
            this.mnuPCBackgroundWhite.Checked = true;
            this.mnuPCBackgroundTransparent.Checked = false;
            this.SetPictureBackground(this.GetSourcePicture(sender as MenuItem), Color.White);
        }

        private void mnuPCBackgroundTransparent_Click(object sender, System.EventArgs e)
        {
            this.mnuPCBackgroundBlack.Checked = false;
            this.mnuPCBackgroundWhite.Checked = false;
            this.mnuPCBackgroundTransparent.Checked = true;
            this.SetPictureBackground(this.GetSourcePicture(sender as MenuItem), Color.Transparent);
        }

        private void mnuPCSaveAs_Click(object sender, System.EventArgs e)
        {
            this.SavePicture(this.GetSourcePicture(sender as MenuItem));
        }

        #endregion

        #endregion

        #region Item Data Viewer Events

        private void btnFindItems_Click(object sender, System.EventArgs e)
        {
            using (ItemFindDialog IFD = new ItemFindDialog(this.LoadedItems_))
            {
                if (IFD.ShowDialog(this) == DialogResult.OK && IFD.SelectedItem != null)
                {
                    this.cmbItems.SelectedItem = IFD.SelectedItem;
                }
            }
        }

        private void ieItemViewer_SizeChanged(object sender, System.EventArgs e)
        {
            if (this.Height < this.ieItemViewer.Height + 128)
            {
                this.Height = this.ieItemViewer.Height + 128;
            }
        }

        private void cmbItems_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            this.ieItemViewer.Item = this.cmbItems.SelectedItem as Item;
            // Annoyingly we cannot set the selected item in the listview - hopefully a datagrid does allow it...
        }

        #endregion

        #region General Viewer Events

        private void CopyEntry(ListView LV)
        {
            string FullText = String.Empty;
            foreach (ListViewItem LVI in LV.SelectedItems)
            {
                string ItemText = String.Empty;
                foreach (ListViewItem.ListViewSubItem LVSI in LVI.SubItems)
                {
                    if (ItemText != String.Empty)
                    {
                        ItemText += '\t';
                    }
                    ItemText += LVSI.Text;
                }
                if (FullText != String.Empty)
                {
                    FullText += '\n';
                }
                FullText += ItemText;
            }
            if (FullText != String.Empty)
            {
                Clipboard.SetDataObject(FullText, true);
            }
        }

        private void ListView_DoubleClick(object sender, EventArgs e)
        {
            ListView LV = sender as ListView;
            foreach (ListViewItem LVI in LV.SelectedItems)
            {
                ThingPropertyPages TPP = new ThingPropertyPages(LVI.Tag as IThing);
                TPP.Show(this);
            }
        }

        private void ListView_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            ListView LV = sender as ListView;
            // Support Ctrl-Ins in addition to the Ctrl-C supported by the context menu
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.Insert)
            {
                this.CopyEntry(LV);
            }
        }

        private void ExportThings(ThingList ThingsToExport)
        {
            this.dlgExportFile.FileName = "";
            if (this.dlgExportFile.ShowDialog() == DialogResult.OK)
            {
                this.PWD = new PleaseWaitDialog(I18N.GetText("Dialog:ExportFileContents"));
                Thread T = new Thread(new ThreadStart(delegate()
                    {
                        ThingsToExport.Save(this.dlgExportFile.FileName);
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

        private void btnThingListExportXML_Click(object sender, EventArgs e) { this.ExportThings(this.LoadedThings_); }

        private FolderBrowserDialog dlgBrowseFolder = null;

        private void PrepareFolderBrowser(string Description)
        {
            if (this.dlgBrowseFolder == null)
            {
                this.dlgBrowseFolder = new FolderBrowserDialog();
                this.dlgBrowseFolder.Description = Description;
                string Location = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "POLUtils");
                if (!Directory.Exists(Location))
                {
                    Directory.CreateDirectory(Location);
                }
                this.dlgBrowseFolder.SelectedPath = Location;
            }
        }

        private void btnThingListSaveImages_Click(object sender, EventArgs e)
        {
            this.PrepareFolderBrowser(I18N.GetText("Description:BrowseImageExportFolder"));
            if (this.dlgBrowseFolder.ShowDialog() == DialogResult.OK)
            {
                PleaseWaitDialog PWD = new PleaseWaitDialog(I18N.GetText("Dialog:SaveAllImages"));
                Thread T = new Thread(new ThreadStart(delegate()
                    {
                        Application.DoEvents();
                        foreach (IThing X in this.LoadedThings_)
                        {
                            Image I = X.GetIcon();
                            // FIXME: Assumes no IThing has more than one image
                            if (I != null)
                            {
                                string ImageFileName = X.ToString() + ".png";
                                foreach (char C in Path.GetInvalidFileNameChars())
                                {
                                    ImageFileName = ImageFileName.Replace(C, '_');
                                }
                                I.Save( Path.Combine(this.dlgBrowseFolder.SelectedPath, ImageFileName), ImageFormat.Png);
                            }
                            Application.DoEvents();
                        }
                        PWD.Invoke(new AnonymousMethod(delegate() { PWD.Close(); }));
                    }));
                T.CurrentUICulture = Thread.CurrentThread.CurrentUICulture;
                T.Start();
                PWD.ShowDialog(this);
                this.Activate();
                PWD.Dispose();
                PWD = null;
            }
        }

        private void chkShowIcons_CheckedChanged(object sender, EventArgs e)
        {
            foreach (ListView LV in this.ListViews_.Values)
            {
                LV.SmallImageList = (this.chkShowIcons.Checked ? this.ListIcons_ : null);
            }
        }

        #region Context Menu Events

        private void mnuELCProperties_Click(object sender, EventArgs e)
        {
            ListView LV = this.mnuEntryListContext.SourceControl as ListView;
            if (LV != null)
            {
                foreach (ListViewItem LVI in LV.SelectedItems)
                {
                    ThingPropertyPages TPP = new ThingPropertyPages(LVI.Tag as IThing);
                    TPP.Show(this);
                }
            }
        }

        private void mnuELCCopyRow_Click(object sender, System.EventArgs e)
        {
            ListView LV = this.mnuEntryListContext.SourceControl as ListView;
            if (LV != null)
            {
                this.CopyEntry(LV);
            }
        }

        private void CopyEntryFieldMenuItem_Click(object sender, System.EventArgs e)
        {
            ListView LV = this.mnuEntryListContext.SourceControl as ListView;
            MenuItem MI = sender as MenuItem;
            if (LV != null && MI != null)
            {
                string CopyText = String.Empty;
                foreach (ListViewItem LVI in LV.SelectedItems)
                {
                    if (CopyText != "")
                    {
                        CopyText += "\r\n";
                    }
                    CopyText += LVI.SubItems[MI.Index].Text;
                }
                Clipboard.SetDataObject(CopyText, true);
            }
        }

        private void mnuStringTableContext_Popup(object sender, System.EventArgs e)
        {
            ListView LV = this.mnuEntryListContext.SourceControl as ListView;
            if (LV != null && LV.SelectedItems.Count > 0)
            {
                // Set up sub-menu with all available columns
                this.mnuELCCopyField.MenuItems.Clear();
                foreach (ColumnHeader CH in LV.Columns)
                {
                    this.mnuELCCopyField.MenuItems.Add(CH.Index,
                        new MenuItem(CH.Text, new EventHandler(this.CopyEntryFieldMenuItem_Click)));
                }
                this.mnuELCCopyField.Enabled = true;
            }
            else
            {
                this.mnuELCCopyField.Enabled = false;
            }
        }

        private void mnuELCEAll_Click(object sender, EventArgs e)
        {
            ListView LV = this.mnuEntryListContext.SourceControl as ListView;
            if (LV != null)
            {
                ThingList ThingsToExport = new ThingList();
                foreach (ListViewItem LVI in LV.Items)
                {
                    if (LVI.Tag is IThing)
                    {
                        ThingsToExport.Add(LVI.Tag as IThing);
                    }
                }
                if (ThingsToExport.Count > 0)
                {
                    this.ExportThings(ThingsToExport);
                }
            }
        }

        private void mnuELCESelected_Click(object sender, EventArgs e)
        {
            ListView LV = this.mnuEntryListContext.SourceControl as ListView;
            if (LV != null)
            {
                ThingList ThingsToExport = new ThingList();
                foreach (ListViewItem LVI in LV.SelectedItems)
                {
                    if (LVI.Tag is IThing)
                    {
                        ThingsToExport.Add(LVI.Tag as IThing);
                    }
                }
                if (ThingsToExport.Count > 0)
                {
                    this.ExportThings(ThingsToExport);
                }
            }
        }

        #endregion

        #endregion

        #region TreeView Events

        private void tvDataFiles_AfterCollapse(object sender, System.Windows.Forms.TreeViewEventArgs e)
        {
            if (e.Node.ImageIndex == 2)
            {
                e.Node.ImageIndex = e.Node.SelectedImageIndex = 1;
            }
        }

        private void tvDataFiles_AfterExpand(object sender, System.Windows.Forms.TreeViewEventArgs e)
        {
            if (e.Node.ImageIndex == 1)
            {
                e.Node.ImageIndex = e.Node.SelectedImageIndex = 2;
            }
        }

        private void tvDataFiles_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
        {
            this.LoadFile(this.tvDataFiles.SelectedNode.Tag as string);
        }

        private void tvDataFiles_BeforeExpand(object sender, System.Windows.Forms.TreeViewCancelEventArgs e)
        {
            if (e.Node.FirstNode != null && e.Node.FirstNode.Tag == e.Node)
            {
                e.Node.Nodes.Clear();
                if (e.Node.Parent == null)
                {
                    for (int i = 0; i < 0x200; ++i)
                    {
                        string SubDir = String.Format("{0}", i);
                        string SubDirPath = Path.Combine(e.Node.Tag as string, SubDir);
                        if (Directory.Exists(SubDirPath))
                        {
                            TreeNode SubDirNode = e.Node.Nodes.Add(SubDir);
                            SubDirNode.ImageIndex = SubDirNode.SelectedImageIndex = 1;
                            SubDirNode.Tag = SubDirPath;
                            SubDirNode.Nodes.Add("<dummy>").Tag = SubDirNode;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < 0x80; ++i)
                    {
                        string DataFile = String.Format("{0}.DAT", i);
                        string DataFilePath = Path.Combine(e.Node.Tag as string, DataFile);
                        if (File.Exists(DataFilePath))
                        {
                            TreeNode DataFileNode = e.Node.Nodes.Add(DataFile);
                            DataFileNode.ImageIndex = DataFileNode.SelectedImageIndex = 3;
                            DataFileNode.Tag = DataFilePath;
                        }
                    }
                }
            }
        }

        #endregion

        #region Main Menu Events

        private void InitializeROMMenus()
        {
            XmlDocument XD = new XmlDocument();
            try
            {
                Stream InfoData = this.GetType().Assembly.GetManifestResourceStream("ROMFileMappings.xml");
                if (InfoData != null)
                {
                    XD.Load(new XmlTextReader(InfoData));
                    InfoData.Close();
                }
            }
            catch {}
            if (XD.DocumentElement == null)
            {
                return;
            }
            XmlNodeList Categories = XD.DocumentElement.SelectNodes("./category");
            int MenuPos = 0;
            foreach (XmlNode XN in Categories)
            {
                if (XN is XmlElement)
                {
                    this.mnuMain.MenuItems.Add(MenuPos++,
                        new CategoryMenuItem(XN as XmlElement, new EventHandler(this.ROMMenuItem_Click)));
                }
            }
        }

        private void SelectEntry(int App, int Dir, int File)
        {
            if (App < 0 || App > this.tvDataFiles.Nodes.Count)
            {
                return;
            }
            TreeNode AppNode = this.tvDataFiles.Nodes[App];
            AppNode.Expand();
            TreeNode DirNode = null;
            string DirNodeText = String.Format("{0}", Dir);
            foreach (TreeNode TN in AppNode.Nodes)
            {
                if (TN.Text == DirNodeText)
                {
                    DirNode = TN;
                    break;
                }
            }
            if (DirNode != null)
            {
                DirNode.Expand();
                TreeNode FileNode = null;
                string FileNodeText = String.Format("{0}.DAT", File);
                foreach (TreeNode TN in DirNode.Nodes)
                {
                    if (TN.Text == FileNodeText)
                    {
                        FileNode = TN;
                        break;
                    }
                }
                if (FileNode != null)
                {
                    this.tvDataFiles.SelectedNode = FileNode;
                }
            }
        }

        private void ROMMenuItem_Click(object sender, System.EventArgs e)
        {
            ROMMenuItem RMI = sender as ROMMenuItem;
            if (RMI != null)
            {
                this.SelectEntry(RMI.ROMApp, RMI.ROMDir, RMI.ROMFile);
            }
        }

        #endregion

        #region Other Events

        private void btnReloadFile_Click(object sender, EventArgs e) { this.LoadFile(this.tvDataFiles.SelectedNode.Tag as string); }

        #endregion
    }
}
