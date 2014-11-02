// Copyright © 2004-2014 Tim Van Holder, Windower Team
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS"
// BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

namespace PlayOnline.FFXI.Utils.MacroManager {

  public partial class MainWindow {

    #region Controls

    private System.Windows.Forms.TreeView tvMacroTree;
    private System.Windows.Forms.ImageList ilBrowserIcons;
    private System.Windows.Forms.Panel pnlProperties;
    private System.Windows.Forms.ContextMenu mnuTreeContext;
    private System.Windows.Forms.TextBox txtCommand1;
    private System.Windows.Forms.TextBox txtCommand2;
    private System.Windows.Forms.TextBox txtCommand3;
    private System.Windows.Forms.TextBox txtCommand6;
    private System.Windows.Forms.TextBox txtCommand5;
    private System.Windows.Forms.TextBox txtCommand4;
    private System.Windows.Forms.GroupBox grpMacroCommands;
    private System.Windows.Forms.ContextMenu mnuTextContext;
    private System.Windows.Forms.MenuItem mnuTextCopy;
    private System.Windows.Forms.MenuItem mnuTextCut;
    private System.Windows.Forms.MenuItem mnuTextPaste;
    private System.Windows.Forms.MenuItem mnuTreeRename;
    private System.Windows.Forms.MenuItem mnuTreeClear;
    private System.Windows.Forms.MenuItem mnuTreeDelete;
    private System.Windows.Forms.MenuItem mnuTreeNew;
    private System.Windows.Forms.MenuItem mnuTreeNewFolder;
    private System.Windows.Forms.MenuItem mnuTreeNewMacro;
    private System.Windows.Forms.MenuItem mnuTreeSave;
    private System.Windows.Forms.MenuItem mnuTreeSep2;
    private System.Windows.Forms.MenuItem mnuTreeSep;
    private System.Windows.Forms.MenuItem mnuTreeCollapse;
    private System.Windows.Forms.MenuItem mnuTreeExpand;
    private System.Windows.Forms.MenuItem mnuTreeExpandAll;
    private System.Windows.Forms.MenuItem mnuTextClear;
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
      this.tvMacroTree = new System.Windows.Forms.TreeView();
      this.mnuTreeContext = new System.Windows.Forms.ContextMenu();
      this.mnuTreeClear = new System.Windows.Forms.MenuItem();
      this.mnuTreeDelete = new System.Windows.Forms.MenuItem();
      this.mnuTreeRename = new System.Windows.Forms.MenuItem();
      this.mnuTreeSave = new System.Windows.Forms.MenuItem();
      this.mnuTreeSep = new System.Windows.Forms.MenuItem();
      this.mnuTreeCollapse = new System.Windows.Forms.MenuItem();
      this.mnuTreeExpand = new System.Windows.Forms.MenuItem();
      this.mnuTreeExpandAll = new System.Windows.Forms.MenuItem();
      this.mnuTreeSep2 = new System.Windows.Forms.MenuItem();
      this.mnuTreeNew = new System.Windows.Forms.MenuItem();
      this.mnuTreeNewFolder = new System.Windows.Forms.MenuItem();
      this.mnuTreeNewMacro = new System.Windows.Forms.MenuItem();
      this.ilBrowserIcons = new System.Windows.Forms.ImageList(this.components);
      this.pnlProperties = new System.Windows.Forms.Panel();
      this.grpMacroCommands = new System.Windows.Forms.GroupBox();
      this.txtCommand6 = new System.Windows.Forms.TextBox();
      this.mnuTextContext = new System.Windows.Forms.ContextMenu();
      this.mnuTextClear = new System.Windows.Forms.MenuItem();
      this.mnuTextCopy = new System.Windows.Forms.MenuItem();
      this.mnuTextCut = new System.Windows.Forms.MenuItem();
      this.mnuTextPaste = new System.Windows.Forms.MenuItem();
      this.txtCommand5 = new System.Windows.Forms.TextBox();
      this.txtCommand4 = new System.Windows.Forms.TextBox();
      this.txtCommand3 = new System.Windows.Forms.TextBox();
      this.txtCommand2 = new System.Windows.Forms.TextBox();
      this.txtCommand1 = new System.Windows.Forms.TextBox();
      this.pnlProperties.SuspendLayout();
      this.grpMacroCommands.SuspendLayout();
      this.SuspendLayout();
      // 
      // tvMacroTree
      // 
      this.tvMacroTree.AllowDrop = true;
      this.tvMacroTree.ContextMenu = this.mnuTreeContext;
      resources.ApplyResources(this.tvMacroTree, "tvMacroTree");
      this.tvMacroTree.HideSelection = false;
      this.tvMacroTree.HotTracking = true;
      this.tvMacroTree.ImageList = this.ilBrowserIcons;
      this.tvMacroTree.LabelEdit = true;
      this.tvMacroTree.Name = "tvMacroTree";
      this.tvMacroTree.PathSeparator = "::";
      this.tvMacroTree.DragDrop += new System.Windows.Forms.DragEventHandler(this.tvMacroTree_DragDrop);
      this.tvMacroTree.DragOver += new System.Windows.Forms.DragEventHandler(this.tvMacroTree_DragOver);
      this.tvMacroTree.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.tvMacroTree_AfterLabelEdit);
      this.tvMacroTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvMacroTree_AfterSelect);
      this.tvMacroTree.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tvMacroTree_KeyUp);
      this.tvMacroTree.BeforeLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.tvMacroTree_BeforeLabelEdit);
      this.tvMacroTree.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.tvMacroTree_ItemDrag);
      // 
      // mnuTreeContext
      // 
      this.mnuTreeContext.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuTreeClear,
            this.mnuTreeDelete,
            this.mnuTreeRename,
            this.mnuTreeSave,
            this.mnuTreeSep,
            this.mnuTreeCollapse,
            this.mnuTreeExpand,
            this.mnuTreeExpandAll,
            this.mnuTreeSep2,
            this.mnuTreeNew});
      this.mnuTreeContext.Popup += new System.EventHandler(this.mnuTreeContext_Popup);
      // 
      // mnuTreeClear
      // 
      this.mnuTreeClear.Index = 0;
      resources.ApplyResources(this.mnuTreeClear, "mnuTreeClear");
      this.mnuTreeClear.Click += new System.EventHandler(this.mnuTreeClear_Click);
      // 
      // mnuTreeDelete
      // 
      this.mnuTreeDelete.Index = 1;
      resources.ApplyResources(this.mnuTreeDelete, "mnuTreeDelete");
      this.mnuTreeDelete.Click += new System.EventHandler(this.mnuTreeDelete_Click);
      // 
      // mnuTreeRename
      // 
      this.mnuTreeRename.Index = 2;
      resources.ApplyResources(this.mnuTreeRename, "mnuTreeRename");
      this.mnuTreeRename.Click += new System.EventHandler(this.mnuTreeRename_Click);
      // 
      // mnuTreeSave
      // 
      this.mnuTreeSave.Index = 3;
      resources.ApplyResources(this.mnuTreeSave, "mnuTreeSave");
      this.mnuTreeSave.Click += new System.EventHandler(this.mnuTreeSave_Click);
      // 
      // mnuTreeSep
      // 
      this.mnuTreeSep.Index = 4;
      resources.ApplyResources(this.mnuTreeSep, "mnuTreeSep");
      // 
      // mnuTreeCollapse
      // 
      this.mnuTreeCollapse.Index = 5;
      resources.ApplyResources(this.mnuTreeCollapse, "mnuTreeCollapse");
      this.mnuTreeCollapse.Click += new System.EventHandler(this.mnuTreeCollapse_Click);
      // 
      // mnuTreeExpand
      // 
      this.mnuTreeExpand.Index = 6;
      resources.ApplyResources(this.mnuTreeExpand, "mnuTreeExpand");
      this.mnuTreeExpand.Click += new System.EventHandler(this.mnuTreeExpand_Click);
      // 
      // mnuTreeExpandAll
      // 
      this.mnuTreeExpandAll.Index = 7;
      resources.ApplyResources(this.mnuTreeExpandAll, "mnuTreeExpandAll");
      this.mnuTreeExpandAll.Click += new System.EventHandler(this.mnuTreeExpandAll_Click);
      // 
      // mnuTreeSep2
      // 
      this.mnuTreeSep2.Index = 8;
      resources.ApplyResources(this.mnuTreeSep2, "mnuTreeSep2");
      // 
      // mnuTreeNew
      // 
      this.mnuTreeNew.Index = 9;
      this.mnuTreeNew.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuTreeNewFolder,
            this.mnuTreeNewMacro});
      resources.ApplyResources(this.mnuTreeNew, "mnuTreeNew");
      // 
      // mnuTreeNewFolder
      // 
      this.mnuTreeNewFolder.Index = 0;
      resources.ApplyResources(this.mnuTreeNewFolder, "mnuTreeNewFolder");
      this.mnuTreeNewFolder.Click += new System.EventHandler(this.mnuTreeNewFolder_Click);
      // 
      // mnuTreeNewMacro
      // 
      this.mnuTreeNewMacro.Index = 1;
      resources.ApplyResources(this.mnuTreeNewMacro, "mnuTreeNewMacro");
      this.mnuTreeNewMacro.Click += new System.EventHandler(this.mnuTreeNewMacro_Click);
      // 
      // ilBrowserIcons
      // 
      this.ilBrowserIcons.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
      resources.ApplyResources(this.ilBrowserIcons, "ilBrowserIcons");
      this.ilBrowserIcons.TransparentColor = System.Drawing.Color.Transparent;
      // 
      // pnlProperties
      // 
      this.pnlProperties.Controls.Add(this.grpMacroCommands);
      resources.ApplyResources(this.pnlProperties, "pnlProperties");
      this.pnlProperties.Name = "pnlProperties";
      // 
      // grpMacroCommands
      // 
      resources.ApplyResources(this.grpMacroCommands, "grpMacroCommands");
      this.grpMacroCommands.Controls.Add(this.txtCommand6);
      this.grpMacroCommands.Controls.Add(this.txtCommand5);
      this.grpMacroCommands.Controls.Add(this.txtCommand4);
      this.grpMacroCommands.Controls.Add(this.txtCommand3);
      this.grpMacroCommands.Controls.Add(this.txtCommand2);
      this.grpMacroCommands.Controls.Add(this.txtCommand1);
      this.grpMacroCommands.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.grpMacroCommands.Name = "grpMacroCommands";
      this.grpMacroCommands.TabStop = false;
      // 
      // txtCommand6
      // 
      this.txtCommand6.AcceptsTab = true;
      this.txtCommand6.ContextMenu = this.mnuTextContext;
      resources.ApplyResources(this.txtCommand6, "txtCommand6");
      this.txtCommand6.Name = "txtCommand6";
      this.txtCommand6.TextChanged += new System.EventHandler(this.txtCommand_TextChanged);
      // 
      // mnuTextContext
      // 
      this.mnuTextContext.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuTextClear,
            this.mnuTextCopy,
            this.mnuTextCut,
            this.mnuTextPaste});
      this.mnuTextContext.Popup += new System.EventHandler(this.mnuTextContext_Popup);
      // 
      // mnuTextClear
      // 
      this.mnuTextClear.Index = 0;
      resources.ApplyResources(this.mnuTextClear, "mnuTextClear");
      this.mnuTextClear.Click += new System.EventHandler(this.mnuTextClear_Click);
      // 
      // mnuTextCopy
      // 
      this.mnuTextCopy.Index = 1;
      resources.ApplyResources(this.mnuTextCopy, "mnuTextCopy");
      this.mnuTextCopy.Click += new System.EventHandler(this.mnuTextCopy_Click);
      // 
      // mnuTextCut
      // 
      this.mnuTextCut.Index = 2;
      resources.ApplyResources(this.mnuTextCut, "mnuTextCut");
      this.mnuTextCut.Click += new System.EventHandler(this.mnuTextCut_Click);
      // 
      // mnuTextPaste
      // 
      this.mnuTextPaste.Index = 3;
      resources.ApplyResources(this.mnuTextPaste, "mnuTextPaste");
      this.mnuTextPaste.Click += new System.EventHandler(this.mnuTextPaste_Click);
      // 
      // txtCommand5
      // 
      this.txtCommand5.AcceptsTab = true;
      this.txtCommand5.ContextMenu = this.mnuTextContext;
      resources.ApplyResources(this.txtCommand5, "txtCommand5");
      this.txtCommand5.Name = "txtCommand5";
      this.txtCommand5.TextChanged += new System.EventHandler(this.txtCommand_TextChanged);
      // 
      // txtCommand4
      // 
      this.txtCommand4.AcceptsTab = true;
      this.txtCommand4.ContextMenu = this.mnuTextContext;
      resources.ApplyResources(this.txtCommand4, "txtCommand4");
      this.txtCommand4.Name = "txtCommand4";
      this.txtCommand4.TextChanged += new System.EventHandler(this.txtCommand_TextChanged);
      // 
      // txtCommand3
      // 
      this.txtCommand3.AcceptsTab = true;
      this.txtCommand3.ContextMenu = this.mnuTextContext;
      resources.ApplyResources(this.txtCommand3, "txtCommand3");
      this.txtCommand3.Name = "txtCommand3";
      this.txtCommand3.TextChanged += new System.EventHandler(this.txtCommand_TextChanged);
      // 
      // txtCommand2
      // 
      this.txtCommand2.AcceptsTab = true;
      this.txtCommand2.ContextMenu = this.mnuTextContext;
      resources.ApplyResources(this.txtCommand2, "txtCommand2");
      this.txtCommand2.Name = "txtCommand2";
      this.txtCommand2.Tag = "";
      this.txtCommand2.TextChanged += new System.EventHandler(this.txtCommand_TextChanged);
      // 
      // txtCommand1
      // 
      this.txtCommand1.AcceptsTab = true;
      this.txtCommand1.ContextMenu = this.mnuTextContext;
      resources.ApplyResources(this.txtCommand1, "txtCommand1");
      this.txtCommand1.Name = "txtCommand1";
      this.txtCommand1.Tag = "";
      this.txtCommand1.TextChanged += new System.EventHandler(this.txtCommand_TextChanged);
      // 
      // MainWindow
      // 
      resources.ApplyResources(this, "$this");
      this.Controls.Add(this.tvMacroTree);
      this.Controls.Add(this.pnlProperties);
      this.MaximizeBox = false;
      this.Name = "MainWindow";
      this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
      this.pnlProperties.ResumeLayout(false);
      this.grpMacroCommands.ResumeLayout(false);
      this.grpMacroCommands.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

  }
}
