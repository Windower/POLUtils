// Copyright © 2004-2014 Tim Van Holder, Windower Team
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS"
// BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

namespace PlayOnline.FFXI.Utils.NPCRenamer {

  partial class NameChanges {

    protected override void Dispose(bool disposing) {
      if (disposing && components != null)
        components.Dispose();
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NameChanges));
      this.lstNameChanges = new System.Windows.Forms.ListView();
      this.chArea = new System.Windows.Forms.ColumnHeader();
      this.chOldName = new System.Windows.Forms.ColumnHeader();
      this.chNewName = new System.Windows.Forms.ColumnHeader();
      this.mnuNameChangeContext = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.mnuWriteSelected = new System.Windows.Forms.ToolStripMenuItem();
      this.mnuRevertSelected = new System.Windows.Forms.ToolStripMenuItem();
      this.mnuSaveSelected = new System.Windows.Forms.ToolStripMenuItem();
      this.btnDiscardPending = new System.Windows.Forms.Button();
      this.btnWritePending = new System.Windows.Forms.Button();
      this.btnClose = new System.Windows.Forms.Button();
      this.btnRevertAll = new System.Windows.Forms.Button();
      this.btnForgetAll = new System.Windows.Forms.Button();
      this.btnApplySet = new System.Windows.Forms.Button();
      this.btnUnapplySet = new System.Windows.Forms.Button();
      this.dlgLoadChangeset = new System.Windows.Forms.OpenFileDialog();
      this.dlgSaveChangeset = new System.Windows.Forms.SaveFileDialog();
      this.prbWriteChanges = new System.Windows.Forms.ProgressBar();
      this.mnuNameChangeContext.SuspendLayout();
      this.SuspendLayout();
      // 
      // lstNameChanges
      // 
      resources.ApplyResources(this.lstNameChanges, "lstNameChanges");
      this.lstNameChanges.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chArea,
            this.chOldName,
            this.chNewName});
      this.lstNameChanges.ContextMenuStrip = this.mnuNameChangeContext;
      this.lstNameChanges.FullRowSelect = true;
      this.lstNameChanges.Name = "lstNameChanges";
      this.lstNameChanges.UseCompatibleStateImageBehavior = false;
      this.lstNameChanges.View = System.Windows.Forms.View.Details;
      this.lstNameChanges.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstNameChanges_KeyDown);
      // 
      // chArea
      // 
      resources.ApplyResources(this.chArea, "chArea");
      // 
      // chOldName
      // 
      resources.ApplyResources(this.chOldName, "chOldName");
      // 
      // chNewName
      // 
      resources.ApplyResources(this.chNewName, "chNewName");
      // 
      // mnuNameChangeContext
      // 
      this.mnuNameChangeContext.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuWriteSelected,
            this.mnuRevertSelected,
            this.mnuSaveSelected});
      this.mnuNameChangeContext.Name = "mnuNameChangeContext";
      this.mnuNameChangeContext.ShowImageMargin = false;
      resources.ApplyResources(this.mnuNameChangeContext, "mnuNameChangeContext");
      this.mnuNameChangeContext.Opening += new System.ComponentModel.CancelEventHandler(this.mnuNameChangeContext_Opening);
      // 
      // mnuWriteSelected
      // 
      this.mnuWriteSelected.Name = "mnuWriteSelected";
      resources.ApplyResources(this.mnuWriteSelected, "mnuWriteSelected");
      this.mnuWriteSelected.Click += new System.EventHandler(this.mnuWriteSelected_Click);
      // 
      // mnuRevertSelected
      // 
      this.mnuRevertSelected.Name = "mnuRevertSelected";
      resources.ApplyResources(this.mnuRevertSelected, "mnuRevertSelected");
      this.mnuRevertSelected.Click += new System.EventHandler(this.mnuRevertSelected_Click);
      // 
      // mnuSaveSelected
      // 
      this.mnuSaveSelected.Name = "mnuSaveSelected";
      resources.ApplyResources(this.mnuSaveSelected, "mnuSaveSelected");
      this.mnuSaveSelected.Click += new System.EventHandler(this.mnuSaveSelected_Click);
      // 
      // btnDiscardPending
      // 
      resources.ApplyResources(this.btnDiscardPending, "btnDiscardPending");
      this.btnDiscardPending.Name = "btnDiscardPending";
      this.btnDiscardPending.UseVisualStyleBackColor = true;
      this.btnDiscardPending.Click += new System.EventHandler(this.btnDiscardPending_Click);
      // 
      // btnWritePending
      // 
      resources.ApplyResources(this.btnWritePending, "btnWritePending");
      this.btnWritePending.Name = "btnWritePending";
      this.btnWritePending.UseVisualStyleBackColor = true;
      this.btnWritePending.Click += new System.EventHandler(this.btnWritePending_Click);
      // 
      // btnClose
      // 
      resources.ApplyResources(this.btnClose, "btnClose");
      this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnClose.Name = "btnClose";
      this.btnClose.UseVisualStyleBackColor = true;
      this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
      // 
      // btnRevertAll
      // 
      resources.ApplyResources(this.btnRevertAll, "btnRevertAll");
      this.btnRevertAll.Name = "btnRevertAll";
      this.btnRevertAll.UseVisualStyleBackColor = true;
      this.btnRevertAll.Click += new System.EventHandler(this.btnRevertAll_Click);
      // 
      // btnForgetAll
      // 
      resources.ApplyResources(this.btnForgetAll, "btnForgetAll");
      this.btnForgetAll.Name = "btnForgetAll";
      this.btnForgetAll.UseVisualStyleBackColor = true;
      this.btnForgetAll.Click += new System.EventHandler(this.btnForgetAll_Click);
      // 
      // btnApplySet
      // 
      resources.ApplyResources(this.btnApplySet, "btnApplySet");
      this.btnApplySet.Name = "btnApplySet";
      this.btnApplySet.UseVisualStyleBackColor = true;
      this.btnApplySet.Click += new System.EventHandler(this.btnApplySet_Click);
      // 
      // btnUnapplySet
      // 
      resources.ApplyResources(this.btnUnapplySet, "btnUnapplySet");
      this.btnUnapplySet.Name = "btnUnapplySet";
      this.btnUnapplySet.UseVisualStyleBackColor = true;
      this.btnUnapplySet.Click += new System.EventHandler(this.btnUnapplySet_Click);
      // 
      // dlgLoadChangeset
      // 
      this.dlgLoadChangeset.DefaultExt = "xml";
      resources.ApplyResources(this.dlgLoadChangeset, "dlgLoadChangeset");
      this.dlgLoadChangeset.SupportMultiDottedExtensions = true;
      // 
      // dlgSaveChangeset
      // 
      this.dlgSaveChangeset.DefaultExt = "xml";
      resources.ApplyResources(this.dlgSaveChangeset, "dlgSaveChangeset");
      this.dlgSaveChangeset.SupportMultiDottedExtensions = true;
      // 
      // prbWriteChanges
      // 
      resources.ApplyResources(this.prbWriteChanges, "prbWriteChanges");
      this.prbWriteChanges.Name = "prbWriteChanges";
      this.prbWriteChanges.Step = 1;
      this.prbWriteChanges.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
      // 
      // NameChanges
      // 
      resources.ApplyResources(this, "$this");
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.btnClose;
      this.Controls.Add(this.prbWriteChanges);
      this.Controls.Add(this.btnWritePending);
      this.Controls.Add(this.btnForgetAll);
      this.Controls.Add(this.btnApplySet);
      this.Controls.Add(this.btnUnapplySet);
      this.Controls.Add(this.btnRevertAll);
      this.Controls.Add(this.lstNameChanges);
      this.Controls.Add(this.btnClose);
      this.Controls.Add(this.btnDiscardPending);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
      this.Name = "NameChanges";
      this.mnuNameChangeContext.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ListView lstNameChanges;
    private System.Windows.Forms.ColumnHeader chArea;
    private System.Windows.Forms.ColumnHeader chOldName;
    private System.Windows.Forms.ColumnHeader chNewName;
    private System.Windows.Forms.ContextMenuStrip mnuNameChangeContext;
    private System.Windows.Forms.ToolStripMenuItem mnuRevertSelected;
    private System.Windows.Forms.ToolStripMenuItem mnuWriteSelected;
    private System.Windows.Forms.ToolStripMenuItem mnuSaveSelected;
    private System.Windows.Forms.Button btnDiscardPending;
    private System.Windows.Forms.Button btnWritePending;
    private System.Windows.Forms.Button btnClose;
    private System.Windows.Forms.Button btnRevertAll;
    private System.Windows.Forms.Button btnForgetAll;
    private System.Windows.Forms.Button btnApplySet;
    private System.Windows.Forms.Button btnUnapplySet;
    private System.ComponentModel.IContainer components;
    private System.Windows.Forms.OpenFileDialog dlgLoadChangeset;
    private System.Windows.Forms.SaveFileDialog dlgSaveChangeset;
    private System.Windows.Forms.ProgressBar prbWriteChanges;

  }

}