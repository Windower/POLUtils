// Copyright © 2004-2014 Tim Van Holder, Windower Team
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS"
// BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

namespace PlayOnline.FFXI.Utils.DataBrowser {

  public partial class MainWindow {

    private System.ComponentModel.IContainer components;

    #region Windows Form Designer generated code

    protected override void Dispose(bool disposing) {
      if (disposing && components != null)
        components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent() {
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
      this.tvDataFiles = new System.Windows.Forms.TreeView();
      this.ilBrowserIcons = new System.Windows.Forms.ImageList(this.components);
      this.splSplitter = new System.Windows.Forms.Splitter();
      this.mnuPictureContext = new System.Windows.Forms.ContextMenu();
      this.mnuPCMode = new System.Windows.Forms.MenuItem();
      this.mnuPCModeNormal = new System.Windows.Forms.MenuItem();
      this.mnuPCModeCentered = new System.Windows.Forms.MenuItem();
      this.mnuPCModeStretched = new System.Windows.Forms.MenuItem();
      this.mnuPCModeZoomed = new System.Windows.Forms.MenuItem();
      this.mnuPCBackground = new System.Windows.Forms.MenuItem();
      this.mnuPCBackgroundBlack = new System.Windows.Forms.MenuItem();
      this.mnuPCBackgroundWhite = new System.Windows.Forms.MenuItem();
      this.mnuPCBackgroundTransparent = new System.Windows.Forms.MenuItem();
      this.mnuPCSaveAs = new System.Windows.Forms.MenuItem();
      this.dlgSavePicture = new System.Windows.Forms.SaveFileDialog();
      this.mnuEntryListContext = new System.Windows.Forms.ContextMenu();
      this.mnuELCProperties = new System.Windows.Forms.MenuItem();
      this.mnuELCSep = new System.Windows.Forms.MenuItem();
      this.mnuELCCopyRow = new System.Windows.Forms.MenuItem();
      this.mnuELCCopyField = new System.Windows.Forms.MenuItem();
      this.mnuELCSep2 = new System.Windows.Forms.MenuItem();
      this.mnuELCExport = new System.Windows.Forms.MenuItem();
      this.mnuELCEAll = new System.Windows.Forms.MenuItem();
      this.mnuELCESelected = new System.Windows.Forms.MenuItem();
      this.mnuMain = new System.Windows.Forms.MainMenu(this.components);
      this.pnlViewerArea = new System.Windows.Forms.Panel();
      this.tabViewers = new System.Windows.Forms.TabControl();
      this.tabViewerItems = new System.Windows.Forms.ThemedTabPage();
      this.ieItemViewer = new PlayOnline.FFXI.ItemEditor();
      this.grpMainItemActions = new System.Windows.Forms.GroupBox();
      this.cmbItems = new System.Windows.Forms.ComboBox();
      this.btnFindItems = new System.Windows.Forms.Button();
      this.tabViewerImages = new System.Windows.Forms.ThemedTabPage();
      this.picImageViewer = new System.Windows.Forms.PictureBox();
      this.pnlImageChooser = new System.Windows.Forms.Panel();
      this.cmbImageChooser = new System.Windows.Forms.ComboBox();
      this.lblImageChooser = new System.Windows.Forms.Label();
      this.tabViewerGeneral = new System.Windows.Forms.ThemedTabPage();
      this.pnlThingListActions = new System.Windows.Forms.Panel();
      this.chkShowIcons = new System.Windows.Forms.CheckBox();
      this.btnThingListSaveImages = new System.Windows.Forms.Button();
      this.btnThingListExportXML = new System.Windows.Forms.Button();
      this.pnlNoViewers = new System.Windows.Forms.Panel();
      this.btnReloadFile = new System.Windows.Forms.Button();
      this.lblNoViewers = new System.Windows.Forms.Label();
      this.dlgExportFile = new System.Windows.Forms.SaveFileDialog();
      this.pnlGeneralContents = new System.Windows.Forms.Panel();
      this.pnlViewerArea.SuspendLayout();
      this.tabViewers.SuspendLayout();
      this.tabViewerItems.SuspendLayout();
      this.grpMainItemActions.SuspendLayout();
      this.tabViewerImages.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize) (this.picImageViewer)).BeginInit();
      this.pnlImageChooser.SuspendLayout();
      this.tabViewerGeneral.SuspendLayout();
      this.pnlThingListActions.SuspendLayout();
      this.pnlNoViewers.SuspendLayout();
      this.SuspendLayout();
      // 
      // tvDataFiles
      // 
      resources.ApplyResources(this.tvDataFiles, "tvDataFiles");
      this.tvDataFiles.HideSelection = false;
      this.tvDataFiles.ImageList = this.ilBrowserIcons;
      this.tvDataFiles.ItemHeight = 16;
      this.tvDataFiles.Name = "tvDataFiles";
      this.tvDataFiles.PathSeparator = "/";
      this.tvDataFiles.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.tvDataFiles_AfterCollapse);
      this.tvDataFiles.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvDataFiles_BeforeExpand);
      this.tvDataFiles.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvDataFiles_AfterSelect);
      this.tvDataFiles.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.tvDataFiles_AfterExpand);
      // 
      // ilBrowserIcons
      // 
      this.ilBrowserIcons.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
      resources.ApplyResources(this.ilBrowserIcons, "ilBrowserIcons");
      this.ilBrowserIcons.TransparentColor = System.Drawing.Color.Transparent;
      // 
      // splSplitter
      // 
      resources.ApplyResources(this.splSplitter, "splSplitter");
      this.splSplitter.Name = "splSplitter";
      this.splSplitter.TabStop = false;
      // 
      // mnuPictureContext
      // 
      this.mnuPictureContext.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuPCMode,
            this.mnuPCBackground,
            this.mnuPCSaveAs});
      // 
      // mnuPCMode
      // 
      this.mnuPCMode.Index = 0;
      this.mnuPCMode.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuPCModeNormal,
            this.mnuPCModeCentered,
            this.mnuPCModeStretched,
            this.mnuPCModeZoomed});
      resources.ApplyResources(this.mnuPCMode, "mnuPCMode");
      // 
      // mnuPCModeNormal
      // 
      this.mnuPCModeNormal.Checked = true;
      this.mnuPCModeNormal.Index = 0;
      this.mnuPCModeNormal.RadioCheck = true;
      resources.ApplyResources(this.mnuPCModeNormal, "mnuPCModeNormal");
      this.mnuPCModeNormal.Click += new System.EventHandler(this.mnuPCModeNormal_Click);
      // 
      // mnuPCModeCentered
      // 
      this.mnuPCModeCentered.Index = 1;
      this.mnuPCModeCentered.RadioCheck = true;
      resources.ApplyResources(this.mnuPCModeCentered, "mnuPCModeCentered");
      this.mnuPCModeCentered.Click += new System.EventHandler(this.mnuPCModeCentered_Click);
      // 
      // mnuPCModeStretched
      // 
      this.mnuPCModeStretched.Index = 2;
      this.mnuPCModeStretched.RadioCheck = true;
      resources.ApplyResources(this.mnuPCModeStretched, "mnuPCModeStretched");
      this.mnuPCModeStretched.Click += new System.EventHandler(this.mnuPCModeStretched_Click);
      // 
      // mnuPCModeZoomed
      // 
      this.mnuPCModeZoomed.Index = 3;
      resources.ApplyResources(this.mnuPCModeZoomed, "mnuPCModeZoomed");
      this.mnuPCModeZoomed.Click += new System.EventHandler(this.mnuPCModeZoomed_Click);
      // 
      // mnuPCBackground
      // 
      this.mnuPCBackground.Index = 1;
      this.mnuPCBackground.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuPCBackgroundBlack,
            this.mnuPCBackgroundWhite,
            this.mnuPCBackgroundTransparent});
      resources.ApplyResources(this.mnuPCBackground, "mnuPCBackground");
      // 
      // mnuPCBackgroundBlack
      // 
      this.mnuPCBackgroundBlack.Index = 0;
      this.mnuPCBackgroundBlack.RadioCheck = true;
      resources.ApplyResources(this.mnuPCBackgroundBlack, "mnuPCBackgroundBlack");
      this.mnuPCBackgroundBlack.Click += new System.EventHandler(this.mnuPCBackgroundBlack_Click);
      // 
      // mnuPCBackgroundWhite
      // 
      this.mnuPCBackgroundWhite.Index = 1;
      this.mnuPCBackgroundWhite.RadioCheck = true;
      resources.ApplyResources(this.mnuPCBackgroundWhite, "mnuPCBackgroundWhite");
      this.mnuPCBackgroundWhite.Click += new System.EventHandler(this.mnuPCBackgroundWhite_Click);
      // 
      // mnuPCBackgroundTransparent
      // 
      this.mnuPCBackgroundTransparent.Checked = true;
      this.mnuPCBackgroundTransparent.Index = 2;
      this.mnuPCBackgroundTransparent.RadioCheck = true;
      resources.ApplyResources(this.mnuPCBackgroundTransparent, "mnuPCBackgroundTransparent");
      this.mnuPCBackgroundTransparent.Click += new System.EventHandler(this.mnuPCBackgroundTransparent_Click);
      // 
      // mnuPCSaveAs
      // 
      this.mnuPCSaveAs.Index = 2;
      resources.ApplyResources(this.mnuPCSaveAs, "mnuPCSaveAs");
      this.mnuPCSaveAs.Click += new System.EventHandler(this.mnuPCSaveAs_Click);
      // 
      // dlgSavePicture
      // 
      resources.ApplyResources(this.dlgSavePicture, "dlgSavePicture");
      // 
      // mnuEntryListContext
      // 
      this.mnuEntryListContext.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuELCProperties,
            this.mnuELCSep,
            this.mnuELCCopyRow,
            this.mnuELCCopyField,
            this.mnuELCSep2,
            this.mnuELCExport});
      this.mnuEntryListContext.Popup += new System.EventHandler(this.mnuStringTableContext_Popup);
      // 
      // mnuELCProperties
      // 
      this.mnuELCProperties.Index = 0;
      resources.ApplyResources(this.mnuELCProperties, "mnuELCProperties");
      this.mnuELCProperties.Click += new System.EventHandler(this.mnuELCProperties_Click);
      // 
      // mnuELCSep
      // 
      this.mnuELCSep.Index = 1;
      resources.ApplyResources(this.mnuELCSep, "mnuELCSep");
      // 
      // mnuELCCopyRow
      // 
      this.mnuELCCopyRow.Index = 2;
      resources.ApplyResources(this.mnuELCCopyRow, "mnuELCCopyRow");
      this.mnuELCCopyRow.Click += new System.EventHandler(this.mnuELCCopyRow_Click);
      // 
      // mnuELCCopyField
      // 
      this.mnuELCCopyField.Index = 3;
      resources.ApplyResources(this.mnuELCCopyField, "mnuELCCopyField");
      // 
      // mnuELCSep2
      // 
      this.mnuELCSep2.Index = 4;
      resources.ApplyResources(this.mnuELCSep2, "mnuELCSep2");
      // 
      // mnuELCExport
      // 
      this.mnuELCExport.Index = 5;
      this.mnuELCExport.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuELCEAll,
            this.mnuELCESelected});
      resources.ApplyResources(this.mnuELCExport, "mnuELCExport");
      // 
      // mnuELCEAll
      // 
      this.mnuELCEAll.Index = 0;
      resources.ApplyResources(this.mnuELCEAll, "mnuELCEAll");
      this.mnuELCEAll.Click += new System.EventHandler(this.mnuELCEAll_Click);
      // 
      // mnuELCESelected
      // 
      this.mnuELCESelected.Index = 1;
      resources.ApplyResources(this.mnuELCESelected, "mnuELCESelected");
      this.mnuELCESelected.Click += new System.EventHandler(this.mnuELCESelected_Click);
      // 
      // pnlViewerArea
      // 
      this.pnlViewerArea.Controls.Add(this.tabViewers);
      this.pnlViewerArea.Controls.Add(this.pnlNoViewers);
      resources.ApplyResources(this.pnlViewerArea, "pnlViewerArea");
      this.pnlViewerArea.Name = "pnlViewerArea";
      // 
      // tabViewers
      // 
      this.tabViewers.Controls.Add(this.tabViewerItems);
      this.tabViewers.Controls.Add(this.tabViewerImages);
      this.tabViewers.Controls.Add(this.tabViewerGeneral);
      resources.ApplyResources(this.tabViewers, "tabViewers");
      this.tabViewers.Name = "tabViewers";
      this.tabViewers.SelectedIndex = 0;
      // 
      // tabViewerItems
      // 
      this.tabViewerItems.Controls.Add(this.ieItemViewer);
      this.tabViewerItems.Controls.Add(this.grpMainItemActions);
      resources.ApplyResources(this.tabViewerItems, "tabViewerItems");
      this.tabViewerItems.Name = "tabViewerItems";
      this.tabViewerItems.UseVisualStyleBackColor = true;
      // 
      // ieItemViewer
      // 
      this.ieItemViewer.BackColor = System.Drawing.Color.Transparent;
      this.ieItemViewer.Item = null;
      resources.ApplyResources(this.ieItemViewer, "ieItemViewer");
      this.ieItemViewer.Name = "ieItemViewer";
      this.ieItemViewer.SizeChanged += new System.EventHandler(this.ieItemViewer_SizeChanged);
      // 
      // grpMainItemActions
      // 
      this.grpMainItemActions.Controls.Add(this.cmbItems);
      this.grpMainItemActions.Controls.Add(this.btnFindItems);
      resources.ApplyResources(this.grpMainItemActions, "grpMainItemActions");
      this.grpMainItemActions.Name = "grpMainItemActions";
      this.grpMainItemActions.TabStop = false;
      // 
      // cmbItems
      // 
      resources.ApplyResources(this.cmbItems, "cmbItems");
      this.cmbItems.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cmbItems.FormattingEnabled = true;
      this.cmbItems.Name = "cmbItems";
      this.cmbItems.SelectedIndexChanged += new System.EventHandler(this.cmbItems_SelectedIndexChanged);
      // 
      // btnFindItems
      // 
      resources.ApplyResources(this.btnFindItems, "btnFindItems");
      this.btnFindItems.Name = "btnFindItems";
      this.btnFindItems.Click += new System.EventHandler(this.btnFindItems_Click);
      // 
      // tabViewerImages
      // 
      this.tabViewerImages.Controls.Add(this.picImageViewer);
      this.tabViewerImages.Controls.Add(this.pnlImageChooser);
      resources.ApplyResources(this.tabViewerImages, "tabViewerImages");
      this.tabViewerImages.Name = "tabViewerImages";
      this.tabViewerImages.UseVisualStyleBackColor = true;
      // 
      // picImageViewer
      // 
      this.picImageViewer.BackColor = System.Drawing.Color.Transparent;
      this.picImageViewer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.picImageViewer.ContextMenu = this.mnuPictureContext;
      resources.ApplyResources(this.picImageViewer, "picImageViewer");
      this.picImageViewer.Name = "picImageViewer";
      this.picImageViewer.TabStop = false;
      // 
      // pnlImageChooser
      // 
      this.pnlImageChooser.Controls.Add(this.cmbImageChooser);
      this.pnlImageChooser.Controls.Add(this.lblImageChooser);
      resources.ApplyResources(this.pnlImageChooser, "pnlImageChooser");
      this.pnlImageChooser.Name = "pnlImageChooser";
      // 
      // cmbImageChooser
      // 
      resources.ApplyResources(this.cmbImageChooser, "cmbImageChooser");
      this.cmbImageChooser.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cmbImageChooser.FormattingEnabled = true;
      this.cmbImageChooser.Name = "cmbImageChooser";
      this.cmbImageChooser.SelectedIndexChanged += new System.EventHandler(this.cmbImageChooser_SelectedIndexChanged);
      // 
      // lblImageChooser
      // 
      this.lblImageChooser.BackColor = System.Drawing.Color.Transparent;
      resources.ApplyResources(this.lblImageChooser, "lblImageChooser");
      this.lblImageChooser.Name = "lblImageChooser";
      // 
      // tabViewerGeneral
      // 
      this.tabViewerGeneral.Controls.Add(this.pnlGeneralContents);
      this.tabViewerGeneral.Controls.Add(this.pnlThingListActions);
      resources.ApplyResources(this.tabViewerGeneral, "tabViewerGeneral");
      this.tabViewerGeneral.Name = "tabViewerGeneral";
      this.tabViewerGeneral.UseVisualStyleBackColor = true;
      // 
      // pnlThingListActions
      // 
      this.pnlThingListActions.Controls.Add(this.chkShowIcons);
      this.pnlThingListActions.Controls.Add(this.btnThingListSaveImages);
      this.pnlThingListActions.Controls.Add(this.btnThingListExportXML);
      resources.ApplyResources(this.pnlThingListActions, "pnlThingListActions");
      this.pnlThingListActions.Name = "pnlThingListActions";
      // 
      // chkShowIcons
      // 
      resources.ApplyResources(this.chkShowIcons, "chkShowIcons");
      this.chkShowIcons.Name = "chkShowIcons";
      this.chkShowIcons.UseVisualStyleBackColor = true;
      this.chkShowIcons.CheckedChanged += new System.EventHandler(this.chkShowIcons_CheckedChanged);
      // 
      // btnThingListSaveImages
      // 
      resources.ApplyResources(this.btnThingListSaveImages, "btnThingListSaveImages");
      this.btnThingListSaveImages.Name = "btnThingListSaveImages";
      this.btnThingListSaveImages.UseVisualStyleBackColor = true;
      this.btnThingListSaveImages.Click += new System.EventHandler(this.btnThingListSaveImages_Click);
      // 
      // btnThingListExportXML
      // 
      resources.ApplyResources(this.btnThingListExportXML, "btnThingListExportXML");
      this.btnThingListExportXML.Name = "btnThingListExportXML";
      this.btnThingListExportXML.UseVisualStyleBackColor = true;
      this.btnThingListExportXML.Click += new System.EventHandler(this.btnThingListExportXML_Click);
      // 
      // pnlNoViewers
      // 
      this.pnlNoViewers.Controls.Add(this.btnReloadFile);
      this.pnlNoViewers.Controls.Add(this.lblNoViewers);
      resources.ApplyResources(this.pnlNoViewers, "pnlNoViewers");
      this.pnlNoViewers.Name = "pnlNoViewers";
      // 
      // btnReloadFile
      // 
      resources.ApplyResources(this.btnReloadFile, "btnReloadFile");
      this.btnReloadFile.Name = "btnReloadFile";
      this.btnReloadFile.UseVisualStyleBackColor = true;
      this.btnReloadFile.Click += new System.EventHandler(this.btnReloadFile_Click);
      // 
      // lblNoViewers
      // 
      resources.ApplyResources(this.lblNoViewers, "lblNoViewers");
      this.lblNoViewers.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblNoViewers.Name = "lblNoViewers";
      // 
      // dlgExportFile
      // 
      this.dlgExportFile.DefaultExt = "xml";
      resources.ApplyResources(this.dlgExportFile, "dlgExportFile");
      // 
      // pnlGeneralContents
      // 
      resources.ApplyResources(this.pnlGeneralContents, "pnlGeneralContents");
      this.pnlGeneralContents.Name = "pnlGeneralContents";
      // 
      // MainWindow
      // 
      resources.ApplyResources(this, "$this");
      this.Controls.Add(this.pnlViewerArea);
      this.Controls.Add(this.splSplitter);
      this.Controls.Add(this.tvDataFiles);
      this.Menu = this.mnuMain;
      this.Name = "MainWindow";
      this.pnlViewerArea.ResumeLayout(false);
      this.tabViewers.ResumeLayout(false);
      this.tabViewerItems.ResumeLayout(false);
      this.grpMainItemActions.ResumeLayout(false);
      this.tabViewerImages.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize) (this.picImageViewer)).EndInit();
      this.pnlImageChooser.ResumeLayout(false);
      this.tabViewerGeneral.ResumeLayout(false);
      this.pnlThingListActions.ResumeLayout(false);
      this.pnlThingListActions.PerformLayout();
      this.pnlNoViewers.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TreeView tvDataFiles;
    private System.Windows.Forms.ImageList ilBrowserIcons;
    private System.Windows.Forms.Splitter splSplitter;
    private System.Windows.Forms.ContextMenu mnuPictureContext;
    private System.Windows.Forms.MenuItem mnuPCBackgroundBlack;
    private System.Windows.Forms.MenuItem mnuPCBackgroundWhite;
    private System.Windows.Forms.MenuItem mnuPCBackgroundTransparent;
    private System.Windows.Forms.SaveFileDialog dlgSavePicture;
    private System.Windows.Forms.MainMenu mnuMain;
    private System.Windows.Forms.MenuItem mnuPCMode;
    private System.Windows.Forms.MenuItem mnuPCModeNormal;
    private System.Windows.Forms.MenuItem mnuPCModeCentered;
    private System.Windows.Forms.MenuItem mnuPCModeStretched;
    private System.Windows.Forms.MenuItem mnuPCBackground;
    private System.Windows.Forms.MenuItem mnuPCSaveAs;
    private System.Windows.Forms.ContextMenu mnuEntryListContext;
    private System.Windows.Forms.Panel pnlViewerArea;
    private System.Windows.Forms.TabControl tabViewers;
    private System.Windows.Forms.PictureBox picImageViewer;
    private System.Windows.Forms.Panel pnlImageChooser;
    private System.Windows.Forms.Label lblImageChooser;
    private System.Windows.Forms.ComboBox cmbImageChooser;
    private System.Windows.Forms.Panel pnlNoViewers;
    private System.Windows.Forms.Label lblNoViewers;
    private System.Windows.Forms.Button btnFindItems;
    private System.Windows.Forms.GroupBox grpMainItemActions;
    private System.Windows.Forms.ComboBox cmbItems;
    private PlayOnline.FFXI.ItemEditor ieItemViewer;
    private System.Windows.Forms.MenuItem mnuELCCopyRow;
    private System.Windows.Forms.MenuItem mnuELCCopyField;
    private System.Windows.Forms.SaveFileDialog dlgExportFile;
    private System.Windows.Forms.ThemedTabPage tabViewerImages;
    private System.Windows.Forms.ThemedTabPage tabViewerItems;
    private System.Windows.Forms.ThemedTabPage tabViewerGeneral;
    private System.Windows.Forms.MenuItem mnuPCModeZoomed;
    private System.Windows.Forms.MenuItem mnuELCProperties;
    private System.Windows.Forms.MenuItem mnuELCSep;
    private System.Windows.Forms.Panel pnlThingListActions;
    private System.Windows.Forms.Button btnThingListExportXML;
    private System.Windows.Forms.MenuItem mnuELCSep2;
    private System.Windows.Forms.MenuItem mnuELCExport;
    private System.Windows.Forms.MenuItem mnuELCEAll;
    private System.Windows.Forms.MenuItem mnuELCESelected;
    private System.Windows.Forms.Button btnThingListSaveImages;
    private System.Windows.Forms.Button btnReloadFile;
    private System.Windows.Forms.CheckBox chkShowIcons;
    private System.Windows.Forms.Panel pnlGeneralContents;

  }

}
