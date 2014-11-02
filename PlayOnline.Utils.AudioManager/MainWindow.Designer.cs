// Copyright © 2004-2014 Tim Van Holder, Windower Team
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS"
// BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

namespace PlayOnline.Utils.AudioManager {

  public partial class MainWindow {

    #region Controls

    private System.Windows.Forms.Panel pnlInfoArea;
    private System.Windows.Forms.GroupBox grpFileInfo;
    private System.Windows.Forms.Label lblLocation;
    private System.Windows.Forms.TextBox txtLocation;
    private System.Windows.Forms.TabControl tabBrowsers;
    private System.Windows.Forms.TabPage tabMusicBrowser;
    private System.Windows.Forms.TreeView tvMusicBrowser;
    private System.Windows.Forms.TabPage tabSoundBrowser;
    private System.Windows.Forms.TreeView tvSoundBrowser;
    private System.Windows.Forms.ImageList ilMusicBrowserIcons;
    private System.Windows.Forms.ImageList ilSoundBrowserIcons;
    private System.Windows.Forms.Label lblFileType;
    private System.Windows.Forms.TextBox txtFileType;
    private System.Windows.Forms.Label lblFormat;
    private System.Windows.Forms.TextBox txtFormat;
    private System.Windows.Forms.TextBox txtFileLength;
    private System.Windows.Forms.Label lblFileLength;
    private System.Windows.Forms.TextBox txtTitle;
    private System.Windows.Forms.Label lblTitle;
    private System.Windows.Forms.TextBox txtComposer;
    private System.Windows.Forms.Label lblComposer;
    private System.Windows.Forms.SaveFileDialog dlgSaveWave;
    private System.Windows.Forms.Button btnDecode;
    private System.Windows.Forms.Button btnPlay;
    private System.Windows.Forms.Button btnPause;
    private System.Windows.Forms.Button btnStop;

    private System.ComponentModel.IContainer components;

    #endregion

    #region Windows Form Designer generated code

    protected override void Dispose(bool disposing) {
      if (disposing && components != null)
        components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent() {
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
      this.ilMusicBrowserIcons = new System.Windows.Forms.ImageList(this.components);
      this.pnlInfoArea = new System.Windows.Forms.Panel();
      this.chkBufferedPlayback = new System.Windows.Forms.CheckBox();
      this.btnStop = new System.Windows.Forms.Button();
      this.btnPause = new System.Windows.Forms.Button();
      this.btnPlay = new System.Windows.Forms.Button();
      this.btnDecode = new System.Windows.Forms.Button();
      this.grpFileInfo = new System.Windows.Forms.GroupBox();
      this.txtComposer = new System.Windows.Forms.TextBox();
      this.lblComposer = new System.Windows.Forms.Label();
      this.txtTitle = new System.Windows.Forms.TextBox();
      this.lblTitle = new System.Windows.Forms.Label();
      this.txtFileLength = new System.Windows.Forms.TextBox();
      this.lblFileLength = new System.Windows.Forms.Label();
      this.txtFormat = new System.Windows.Forms.TextBox();
      this.lblFormat = new System.Windows.Forms.Label();
      this.txtFileType = new System.Windows.Forms.TextBox();
      this.lblFileType = new System.Windows.Forms.Label();
      this.txtLocation = new System.Windows.Forms.TextBox();
      this.lblLocation = new System.Windows.Forms.Label();
      this.tabBrowsers = new System.Windows.Forms.TabControl();
      this.tabMusicBrowser = new System.Windows.Forms.TabPage();
      this.tvMusicBrowser = new System.Windows.Forms.TreeView();
      this.tabSoundBrowser = new System.Windows.Forms.TabPage();
      this.tvSoundBrowser = new System.Windows.Forms.TreeView();
      this.ilSoundBrowserIcons = new System.Windows.Forms.ImageList(this.components);
      this.dlgSaveWave = new System.Windows.Forms.SaveFileDialog();
      this.ttInfo = new System.Windows.Forms.ToolTip(this.components);
      this.pnlInfoArea.SuspendLayout();
      this.grpFileInfo.SuspendLayout();
      this.tabBrowsers.SuspendLayout();
      this.tabMusicBrowser.SuspendLayout();
      this.tabSoundBrowser.SuspendLayout();
      this.SuspendLayout();
      // 
      // ilMusicBrowserIcons
      // 
      this.ilMusicBrowserIcons.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
      resources.ApplyResources(this.ilMusicBrowserIcons, "ilMusicBrowserIcons");
      this.ilMusicBrowserIcons.TransparentColor = System.Drawing.Color.Transparent;
      // 
      // pnlInfoArea
      // 
      this.pnlInfoArea.Controls.Add(this.chkBufferedPlayback);
      this.pnlInfoArea.Controls.Add(this.btnStop);
      this.pnlInfoArea.Controls.Add(this.btnPause);
      this.pnlInfoArea.Controls.Add(this.btnPlay);
      this.pnlInfoArea.Controls.Add(this.btnDecode);
      this.pnlInfoArea.Controls.Add(this.grpFileInfo);
      resources.ApplyResources(this.pnlInfoArea, "pnlInfoArea");
      this.pnlInfoArea.Name = "pnlInfoArea";
      // 
      // chkBufferedPlayback
      // 
      resources.ApplyResources(this.chkBufferedPlayback, "chkBufferedPlayback");
      this.chkBufferedPlayback.Checked = true;
      this.chkBufferedPlayback.CheckState = System.Windows.Forms.CheckState.Checked;
      this.chkBufferedPlayback.Name = "chkBufferedPlayback";
      this.ttInfo.SetToolTip(this.chkBufferedPlayback, resources.GetString("chkBufferedPlayback.ToolTip"));
      this.chkBufferedPlayback.UseVisualStyleBackColor = true;
      // 
      // btnStop
      // 
      resources.ApplyResources(this.btnStop, "btnStop");
      this.btnStop.Name = "btnStop";
      this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
      // 
      // btnPause
      // 
      resources.ApplyResources(this.btnPause, "btnPause");
      this.btnPause.Name = "btnPause";
      this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
      // 
      // btnPlay
      // 
      resources.ApplyResources(this.btnPlay, "btnPlay");
      this.btnPlay.Name = "btnPlay";
      this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
      // 
      // btnDecode
      // 
      resources.ApplyResources(this.btnDecode, "btnDecode");
      this.btnDecode.Name = "btnDecode";
      this.btnDecode.Click += new System.EventHandler(this.btnDecode_Click);
      // 
      // grpFileInfo
      // 
      resources.ApplyResources(this.grpFileInfo, "grpFileInfo");
      this.grpFileInfo.Controls.Add(this.txtComposer);
      this.grpFileInfo.Controls.Add(this.lblComposer);
      this.grpFileInfo.Controls.Add(this.txtTitle);
      this.grpFileInfo.Controls.Add(this.lblTitle);
      this.grpFileInfo.Controls.Add(this.txtFileLength);
      this.grpFileInfo.Controls.Add(this.lblFileLength);
      this.grpFileInfo.Controls.Add(this.txtFormat);
      this.grpFileInfo.Controls.Add(this.lblFormat);
      this.grpFileInfo.Controls.Add(this.txtFileType);
      this.grpFileInfo.Controls.Add(this.lblFileType);
      this.grpFileInfo.Controls.Add(this.txtLocation);
      this.grpFileInfo.Controls.Add(this.lblLocation);
      this.grpFileInfo.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.grpFileInfo.Name = "grpFileInfo";
      this.grpFileInfo.TabStop = false;
      // 
      // txtComposer
      // 
      resources.ApplyResources(this.txtComposer, "txtComposer");
      this.txtComposer.Name = "txtComposer";
      this.txtComposer.ReadOnly = true;
      // 
      // lblComposer
      // 
      this.lblComposer.FlatStyle = System.Windows.Forms.FlatStyle.System;
      resources.ApplyResources(this.lblComposer, "lblComposer");
      this.lblComposer.Name = "lblComposer";
      // 
      // txtTitle
      // 
      resources.ApplyResources(this.txtTitle, "txtTitle");
      this.txtTitle.Name = "txtTitle";
      this.txtTitle.ReadOnly = true;
      // 
      // lblTitle
      // 
      this.lblTitle.FlatStyle = System.Windows.Forms.FlatStyle.System;
      resources.ApplyResources(this.lblTitle, "lblTitle");
      this.lblTitle.Name = "lblTitle";
      // 
      // txtFileLength
      // 
      resources.ApplyResources(this.txtFileLength, "txtFileLength");
      this.txtFileLength.Name = "txtFileLength";
      this.txtFileLength.ReadOnly = true;
      // 
      // lblFileLength
      // 
      this.lblFileLength.FlatStyle = System.Windows.Forms.FlatStyle.System;
      resources.ApplyResources(this.lblFileLength, "lblFileLength");
      this.lblFileLength.Name = "lblFileLength";
      // 
      // txtFormat
      // 
      resources.ApplyResources(this.txtFormat, "txtFormat");
      this.txtFormat.Name = "txtFormat";
      this.txtFormat.ReadOnly = true;
      // 
      // lblFormat
      // 
      this.lblFormat.FlatStyle = System.Windows.Forms.FlatStyle.System;
      resources.ApplyResources(this.lblFormat, "lblFormat");
      this.lblFormat.Name = "lblFormat";
      // 
      // txtFileType
      // 
      resources.ApplyResources(this.txtFileType, "txtFileType");
      this.txtFileType.Name = "txtFileType";
      this.txtFileType.ReadOnly = true;
      // 
      // lblFileType
      // 
      this.lblFileType.FlatStyle = System.Windows.Forms.FlatStyle.System;
      resources.ApplyResources(this.lblFileType, "lblFileType");
      this.lblFileType.Name = "lblFileType";
      // 
      // txtLocation
      // 
      resources.ApplyResources(this.txtLocation, "txtLocation");
      this.txtLocation.Name = "txtLocation";
      this.txtLocation.ReadOnly = true;
      // 
      // lblLocation
      // 
      this.lblLocation.FlatStyle = System.Windows.Forms.FlatStyle.System;
      resources.ApplyResources(this.lblLocation, "lblLocation");
      this.lblLocation.Name = "lblLocation";
      // 
      // tabBrowsers
      // 
      this.tabBrowsers.Controls.Add(this.tabMusicBrowser);
      this.tabBrowsers.Controls.Add(this.tabSoundBrowser);
      resources.ApplyResources(this.tabBrowsers, "tabBrowsers");
      this.tabBrowsers.ImageList = this.ilMusicBrowserIcons;
      this.tabBrowsers.Name = "tabBrowsers";
      this.tabBrowsers.SelectedIndex = 0;
      this.tabBrowsers.SelectedIndexChanged += new System.EventHandler(this.tabBrowsers_SelectedIndexChanged);
      // 
      // tabMusicBrowser
      // 
      this.tabMusicBrowser.Controls.Add(this.tvMusicBrowser);
      resources.ApplyResources(this.tabMusicBrowser, "tabMusicBrowser");
      this.tabMusicBrowser.Name = "tabMusicBrowser";
      // 
      // tvMusicBrowser
      // 
      resources.ApplyResources(this.tvMusicBrowser, "tvMusicBrowser");
      this.tvMusicBrowser.HideSelection = false;
      this.tvMusicBrowser.HotTracking = true;
      this.tvMusicBrowser.ImageList = this.ilMusicBrowserIcons;
      this.tvMusicBrowser.ItemHeight = 16;
      this.tvMusicBrowser.Name = "tvMusicBrowser";
      this.tvMusicBrowser.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.tvBrowser_AfterCollapse);
      this.tvMusicBrowser.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvBrowser_AfterSelect);
      this.tvMusicBrowser.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.tvBrowser_AfterExpand);
      // 
      // tabSoundBrowser
      // 
      this.tabSoundBrowser.Controls.Add(this.tvSoundBrowser);
      resources.ApplyResources(this.tabSoundBrowser, "tabSoundBrowser");
      this.tabSoundBrowser.Name = "tabSoundBrowser";
      // 
      // tvSoundBrowser
      // 
      resources.ApplyResources(this.tvSoundBrowser, "tvSoundBrowser");
      this.tvSoundBrowser.HideSelection = false;
      this.tvSoundBrowser.HotTracking = true;
      this.tvSoundBrowser.ImageList = this.ilSoundBrowserIcons;
      this.tvSoundBrowser.ItemHeight = 16;
      this.tvSoundBrowser.Name = "tvSoundBrowser";
      this.tvSoundBrowser.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.tvBrowser_AfterCollapse);
      this.tvSoundBrowser.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvBrowser_AfterSelect);
      this.tvSoundBrowser.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.tvBrowser_AfterExpand);
      // 
      // ilSoundBrowserIcons
      // 
      this.ilSoundBrowserIcons.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
      resources.ApplyResources(this.ilSoundBrowserIcons, "ilSoundBrowserIcons");
      this.ilSoundBrowserIcons.TransparentColor = System.Drawing.Color.Transparent;
      // 
      // dlgSaveWave
      // 
      this.dlgSaveWave.DefaultExt = "wav";
      resources.ApplyResources(this.dlgSaveWave, "dlgSaveWave");
      this.dlgSaveWave.RestoreDirectory = true;
      // 
      // MainWindow
      // 
      resources.ApplyResources(this, "$this");
      this.Controls.Add(this.tabBrowsers);
      this.Controls.Add(this.pnlInfoArea);
      this.Name = "MainWindow";
      this.Closed += new System.EventHandler(this.MainWindow_Closed);
      this.VisibleChanged += new System.EventHandler(this.MainWindow_VisibleChanged);
      this.pnlInfoArea.ResumeLayout(false);
      this.pnlInfoArea.PerformLayout();
      this.grpFileInfo.ResumeLayout(false);
      this.grpFileInfo.PerformLayout();
      this.tabBrowsers.ResumeLayout(false);
      this.tabMusicBrowser.ResumeLayout(false);
      this.tabSoundBrowser.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.CheckBox chkBufferedPlayback;
    private System.Windows.Forms.ToolTip ttInfo;

  }

}
