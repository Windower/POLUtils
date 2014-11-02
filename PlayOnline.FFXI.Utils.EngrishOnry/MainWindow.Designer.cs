// Copyright © 2004-2014 Tim Van Holder, Windower Team
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS"
// BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

namespace PlayOnline.FFXI.Utils.EngrishOnry {

  public partial class MainWindow {

    #region Windows Form Designer generated code

    private System.ComponentModel.Container components = null;

    protected override void Dispose(bool disposing) {
      if (disposing && components != null)
        components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent() {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
      this.pnlLog = new System.Windows.Forms.Panel();
      this.lblActivityLog = new System.Windows.Forms.Label();
      this.rtbActivityLog = new System.Windows.Forms.RichTextBox();
      this.mnuConfigSpellData = new System.Windows.Forms.ContextMenu();
      this.mnuTranslateSpellNames = new System.Windows.Forms.MenuItem();
      this.mnuTranslateSpellDescriptions = new System.Windows.Forms.MenuItem();
      this.mnuConfigItemData = new System.Windows.Forms.ContextMenu();
      this.mnuTranslateItemNames = new System.Windows.Forms.MenuItem();
      this.mnuTranslateItemDescriptions = new System.Windows.Forms.MenuItem();
      this.mnuConfigAutoTrans = new System.Windows.Forms.ContextMenu();
      this.mnuPreserveJapaneseATCompletion = new System.Windows.Forms.MenuItem();
      this.mnuEnglishATCompletionOnly = new System.Windows.Forms.MenuItem();
      this.pnlActions = new System.Windows.Forms.Panel();
      this.btnConfigAbilities = new System.Windows.Forms.Button();
      this.lblAbilities = new System.Windows.Forms.Label();
      this.btnRestoreAbilities = new System.Windows.Forms.Button();
      this.btnTranslateAbilities = new System.Windows.Forms.Button();
      this.btnConfigDialogTables = new System.Windows.Forms.Button();
      this.btnConfigStringTables = new System.Windows.Forms.Button();
      this.btnConfigAutoTrans = new System.Windows.Forms.Button();
      this.btnConfigItemData = new System.Windows.Forms.Button();
      this.btnConfigSpellData = new System.Windows.Forms.Button();
      this.lblItemData = new System.Windows.Forms.Label();
      this.lblAutoTranslator = new System.Windows.Forms.Label();
      this.lblSpellData = new System.Windows.Forms.Label();
      this.lblStringTables = new System.Windows.Forms.Label();
      this.lblDialogTables = new System.Windows.Forms.Label();
      this.btnRestoreSpellData = new System.Windows.Forms.Button();
      this.btnTranslateSpellData = new System.Windows.Forms.Button();
      this.btnRestoreStringTables = new System.Windows.Forms.Button();
      this.btnTranslateStringTables = new System.Windows.Forms.Button();
      this.btnRestoreDialogTables = new System.Windows.Forms.Button();
      this.btnTranslateDialogTables = new System.Windows.Forms.Button();
      this.btnRestoreAutoTrans = new System.Windows.Forms.Button();
      this.btnTranslateAutoTrans = new System.Windows.Forms.Button();
      this.btnRestoreItemData = new System.Windows.Forms.Button();
      this.btnTranslateItemData = new System.Windows.Forms.Button();
      this.mnuConfigAbilities = new System.Windows.Forms.ContextMenu();
      this.mnuTranslateAbilityNames = new System.Windows.Forms.MenuItem();
      this.mnuTranslateAbilityDescriptions = new System.Windows.Forms.MenuItem();
      this.pnlLog.SuspendLayout();
      this.pnlActions.SuspendLayout();
      this.SuspendLayout();
      // 
      // pnlLog
      // 
      this.pnlLog.Controls.Add(this.lblActivityLog);
      this.pnlLog.Controls.Add(this.rtbActivityLog);
      resources.ApplyResources(this.pnlLog, "pnlLog");
      this.pnlLog.Name = "pnlLog";
      // 
      // lblActivityLog
      // 
      resources.ApplyResources(this.lblActivityLog, "lblActivityLog");
      this.lblActivityLog.Name = "lblActivityLog";
      // 
      // rtbActivityLog
      // 
      resources.ApplyResources(this.rtbActivityLog, "rtbActivityLog");
      this.rtbActivityLog.Name = "rtbActivityLog";
      this.rtbActivityLog.ReadOnly = true;
      // 
      // mnuConfigSpellData
      // 
      this.mnuConfigSpellData.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuTranslateSpellNames,
            this.mnuTranslateSpellDescriptions});
      // 
      // mnuTranslateSpellNames
      // 
      this.mnuTranslateSpellNames.Checked = true;
      this.mnuTranslateSpellNames.Index = 0;
      resources.ApplyResources(this.mnuTranslateSpellNames, "mnuTranslateSpellNames");
      this.mnuTranslateSpellNames.Click += new System.EventHandler(this.mnuTranslateSpellNames_Click);
      // 
      // mnuTranslateSpellDescriptions
      // 
      this.mnuTranslateSpellDescriptions.Checked = true;
      this.mnuTranslateSpellDescriptions.Index = 1;
      resources.ApplyResources(this.mnuTranslateSpellDescriptions, "mnuTranslateSpellDescriptions");
      this.mnuTranslateSpellDescriptions.Click += new System.EventHandler(this.mnuTranslateSpellDescriptions_Click);
      // 
      // mnuConfigItemData
      // 
      this.mnuConfigItemData.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuTranslateItemNames,
            this.mnuTranslateItemDescriptions});
      // 
      // mnuTranslateItemNames
      // 
      this.mnuTranslateItemNames.Checked = true;
      this.mnuTranslateItemNames.Index = 0;
      resources.ApplyResources(this.mnuTranslateItemNames, "mnuTranslateItemNames");
      this.mnuTranslateItemNames.Click += new System.EventHandler(this.mnuTranslateItemNames_Click);
      // 
      // mnuTranslateItemDescriptions
      // 
      this.mnuTranslateItemDescriptions.Checked = true;
      this.mnuTranslateItemDescriptions.Index = 1;
      resources.ApplyResources(this.mnuTranslateItemDescriptions, "mnuTranslateItemDescriptions");
      this.mnuTranslateItemDescriptions.Click += new System.EventHandler(this.mnuTranslateItemDescriptions_Click);
      // 
      // mnuConfigAutoTrans
      // 
      this.mnuConfigAutoTrans.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuPreserveJapaneseATCompletion,
            this.mnuEnglishATCompletionOnly});
      // 
      // mnuPreserveJapaneseATCompletion
      // 
      this.mnuPreserveJapaneseATCompletion.Checked = true;
      this.mnuPreserveJapaneseATCompletion.Index = 0;
      resources.ApplyResources(this.mnuPreserveJapaneseATCompletion, "mnuPreserveJapaneseATCompletion");
      this.mnuPreserveJapaneseATCompletion.Click += new System.EventHandler(this.mnuPreserveJapaneseATCompletion_Click);
      // 
      // mnuEnglishATCompletionOnly
      // 
      this.mnuEnglishATCompletionOnly.Index = 1;
      resources.ApplyResources(this.mnuEnglishATCompletionOnly, "mnuEnglishATCompletionOnly");
      this.mnuEnglishATCompletionOnly.Click += new System.EventHandler(this.mnuEnglishATCompletionOnly_Click);
      // 
      // pnlActions
      // 
      this.pnlActions.Controls.Add(this.btnConfigAbilities);
      this.pnlActions.Controls.Add(this.lblAbilities);
      this.pnlActions.Controls.Add(this.btnRestoreAbilities);
      this.pnlActions.Controls.Add(this.btnTranslateAbilities);
      this.pnlActions.Controls.Add(this.btnConfigDialogTables);
      this.pnlActions.Controls.Add(this.btnConfigStringTables);
      this.pnlActions.Controls.Add(this.btnConfigAutoTrans);
      this.pnlActions.Controls.Add(this.btnConfigItemData);
      this.pnlActions.Controls.Add(this.btnConfigSpellData);
      this.pnlActions.Controls.Add(this.lblItemData);
      this.pnlActions.Controls.Add(this.lblAutoTranslator);
      this.pnlActions.Controls.Add(this.lblSpellData);
      this.pnlActions.Controls.Add(this.lblStringTables);
      this.pnlActions.Controls.Add(this.lblDialogTables);
      this.pnlActions.Controls.Add(this.btnRestoreSpellData);
      this.pnlActions.Controls.Add(this.btnTranslateSpellData);
      this.pnlActions.Controls.Add(this.btnRestoreStringTables);
      this.pnlActions.Controls.Add(this.btnTranslateStringTables);
      this.pnlActions.Controls.Add(this.btnRestoreDialogTables);
      this.pnlActions.Controls.Add(this.btnTranslateDialogTables);
      this.pnlActions.Controls.Add(this.btnRestoreAutoTrans);
      this.pnlActions.Controls.Add(this.btnTranslateAutoTrans);
      this.pnlActions.Controls.Add(this.btnRestoreItemData);
      this.pnlActions.Controls.Add(this.btnTranslateItemData);
      resources.ApplyResources(this.pnlActions, "pnlActions");
      this.pnlActions.Name = "pnlActions";
      // 
      // btnConfigAbilities
      // 
      resources.ApplyResources(this.btnConfigAbilities, "btnConfigAbilities");
      this.btnConfigAbilities.Name = "btnConfigAbilities";
      this.btnConfigAbilities.Click += new System.EventHandler(this.btnConfigAbilities_Click);
      // 
      // lblAbilities
      // 
      this.lblAbilities.FlatStyle = System.Windows.Forms.FlatStyle.System;
      resources.ApplyResources(this.lblAbilities, "lblAbilities");
      this.lblAbilities.Name = "lblAbilities";
      // 
      // btnRestoreAbilities
      // 
      resources.ApplyResources(this.btnRestoreAbilities, "btnRestoreAbilities");
      this.btnRestoreAbilities.Name = "btnRestoreAbilities";
      this.btnRestoreAbilities.Click += new System.EventHandler(this.btnRestoreAbilities_Click);
      // 
      // btnTranslateAbilities
      // 
      resources.ApplyResources(this.btnTranslateAbilities, "btnTranslateAbilities");
      this.btnTranslateAbilities.Name = "btnTranslateAbilities";
      this.btnTranslateAbilities.Click += new System.EventHandler(this.btnTranslateAbilities_Click);
      // 
      // btnConfigDialogTables
      // 
      resources.ApplyResources(this.btnConfigDialogTables, "btnConfigDialogTables");
      this.btnConfigDialogTables.Name = "btnConfigDialogTables";
      // 
      // btnConfigStringTables
      // 
      resources.ApplyResources(this.btnConfigStringTables, "btnConfigStringTables");
      this.btnConfigStringTables.Name = "btnConfigStringTables";
      // 
      // btnConfigAutoTrans
      // 
      resources.ApplyResources(this.btnConfigAutoTrans, "btnConfigAutoTrans");
      this.btnConfigAutoTrans.Name = "btnConfigAutoTrans";
      this.btnConfigAutoTrans.Click += new System.EventHandler(this.btnConfigAutoTrans_Click);
      // 
      // btnConfigItemData
      // 
      resources.ApplyResources(this.btnConfigItemData, "btnConfigItemData");
      this.btnConfigItemData.Name = "btnConfigItemData";
      this.btnConfigItemData.Click += new System.EventHandler(this.btnConfigItemData_Click);
      // 
      // btnConfigSpellData
      // 
      resources.ApplyResources(this.btnConfigSpellData, "btnConfigSpellData");
      this.btnConfigSpellData.Name = "btnConfigSpellData";
      this.btnConfigSpellData.Click += new System.EventHandler(this.btnConfigSpellData_Click);
      // 
      // lblItemData
      // 
      this.lblItemData.FlatStyle = System.Windows.Forms.FlatStyle.System;
      resources.ApplyResources(this.lblItemData, "lblItemData");
      this.lblItemData.Name = "lblItemData";
      // 
      // lblAutoTranslator
      // 
      this.lblAutoTranslator.FlatStyle = System.Windows.Forms.FlatStyle.System;
      resources.ApplyResources(this.lblAutoTranslator, "lblAutoTranslator");
      this.lblAutoTranslator.Name = "lblAutoTranslator";
      // 
      // lblSpellData
      // 
      this.lblSpellData.FlatStyle = System.Windows.Forms.FlatStyle.System;
      resources.ApplyResources(this.lblSpellData, "lblSpellData");
      this.lblSpellData.Name = "lblSpellData";
      // 
      // lblStringTables
      // 
      this.lblStringTables.FlatStyle = System.Windows.Forms.FlatStyle.System;
      resources.ApplyResources(this.lblStringTables, "lblStringTables");
      this.lblStringTables.Name = "lblStringTables";
      // 
      // lblDialogTables
      // 
      this.lblDialogTables.FlatStyle = System.Windows.Forms.FlatStyle.System;
      resources.ApplyResources(this.lblDialogTables, "lblDialogTables");
      this.lblDialogTables.Name = "lblDialogTables";
      // 
      // btnRestoreSpellData
      // 
      resources.ApplyResources(this.btnRestoreSpellData, "btnRestoreSpellData");
      this.btnRestoreSpellData.Name = "btnRestoreSpellData";
      this.btnRestoreSpellData.Click += new System.EventHandler(this.btnRestoreSpellData_Click);
      // 
      // btnTranslateSpellData
      // 
      resources.ApplyResources(this.btnTranslateSpellData, "btnTranslateSpellData");
      this.btnTranslateSpellData.Name = "btnTranslateSpellData";
      this.btnTranslateSpellData.Click += new System.EventHandler(this.btnTranslateSpellData_Click);
      // 
      // btnRestoreStringTables
      // 
      resources.ApplyResources(this.btnRestoreStringTables, "btnRestoreStringTables");
      this.btnRestoreStringTables.Name = "btnRestoreStringTables";
      this.btnRestoreStringTables.Click += new System.EventHandler(this.btnRestoreStringTables_Click);
      // 
      // btnTranslateStringTables
      // 
      resources.ApplyResources(this.btnTranslateStringTables, "btnTranslateStringTables");
      this.btnTranslateStringTables.Name = "btnTranslateStringTables";
      this.btnTranslateStringTables.Click += new System.EventHandler(this.btnTranslateStringTables_Click);
      // 
      // btnRestoreDialogTables
      // 
      resources.ApplyResources(this.btnRestoreDialogTables, "btnRestoreDialogTables");
      this.btnRestoreDialogTables.Name = "btnRestoreDialogTables";
      this.btnRestoreDialogTables.Click += new System.EventHandler(this.btnRestoreDialogTables_Click);
      // 
      // btnTranslateDialogTables
      // 
      resources.ApplyResources(this.btnTranslateDialogTables, "btnTranslateDialogTables");
      this.btnTranslateDialogTables.Name = "btnTranslateDialogTables";
      this.btnTranslateDialogTables.Click += new System.EventHandler(this.btnTranslateDialogTables_Click);
      // 
      // btnRestoreAutoTrans
      // 
      resources.ApplyResources(this.btnRestoreAutoTrans, "btnRestoreAutoTrans");
      this.btnRestoreAutoTrans.Name = "btnRestoreAutoTrans";
      this.btnRestoreAutoTrans.Click += new System.EventHandler(this.btnRestoreAutoTrans_Click);
      // 
      // btnTranslateAutoTrans
      // 
      resources.ApplyResources(this.btnTranslateAutoTrans, "btnTranslateAutoTrans");
      this.btnTranslateAutoTrans.Name = "btnTranslateAutoTrans";
      this.btnTranslateAutoTrans.Click += new System.EventHandler(this.btnTranslateAutoTrans_Click);
      // 
      // btnRestoreItemData
      // 
      resources.ApplyResources(this.btnRestoreItemData, "btnRestoreItemData");
      this.btnRestoreItemData.Name = "btnRestoreItemData";
      this.btnRestoreItemData.Click += new System.EventHandler(this.btnRestoreItemData_Click);
      // 
      // btnTranslateItemData
      // 
      resources.ApplyResources(this.btnTranslateItemData, "btnTranslateItemData");
      this.btnTranslateItemData.Name = "btnTranslateItemData";
      this.btnTranslateItemData.Click += new System.EventHandler(this.btnTranslateItemData_Click);
      // 
      // mnuConfigAbilities
      // 
      this.mnuConfigAbilities.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuTranslateAbilityNames,
            this.mnuTranslateAbilityDescriptions});
      // 
      // mnuTranslateAbilityNames
      // 
      this.mnuTranslateAbilityNames.Checked = true;
      this.mnuTranslateAbilityNames.Index = 0;
      resources.ApplyResources(this.mnuTranslateAbilityNames, "mnuTranslateAbilityNames");
      // 
      // mnuTranslateAbilityDescriptions
      // 
      this.mnuTranslateAbilityDescriptions.Checked = true;
      this.mnuTranslateAbilityDescriptions.Index = 1;
      resources.ApplyResources(this.mnuTranslateAbilityDescriptions, "mnuTranslateAbilityDescriptions");
      // 
      // MainWindow
      // 
      resources.ApplyResources(this, "$this");
      this.Controls.Add(this.pnlLog);
      this.Controls.Add(this.pnlActions);
      this.Name = "MainWindow";
      this.pnlLog.ResumeLayout(false);
      this.pnlActions.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    // Controls
    private System.Windows.Forms.Label lblActivityLog;
    private System.Windows.Forms.RichTextBox rtbActivityLog;
    private System.Windows.Forms.ContextMenu mnuConfigSpellData;
    private System.Windows.Forms.ContextMenu mnuConfigItemData;
    private System.Windows.Forms.MenuItem mnuTranslateSpellNames;
    private System.Windows.Forms.MenuItem mnuTranslateSpellDescriptions;
    private System.Windows.Forms.ContextMenu mnuConfigAutoTrans;
    private System.Windows.Forms.Panel pnlLog;
    private System.Windows.Forms.Panel pnlActions;
    private System.Windows.Forms.Button btnConfigAutoTrans;
    private System.Windows.Forms.Button btnConfigItemData;
    private System.Windows.Forms.Button btnConfigSpellData;
    private System.Windows.Forms.Label lblItemData;
    private System.Windows.Forms.Label lblAutoTranslator;
    private System.Windows.Forms.Label lblSpellData;
    private System.Windows.Forms.Label lblStringTables;
    private System.Windows.Forms.Label lblDialogTables;
    private System.Windows.Forms.Button btnRestoreSpellData;
    private System.Windows.Forms.Button btnTranslateSpellData;
    private System.Windows.Forms.Button btnRestoreStringTables;
    private System.Windows.Forms.Button btnTranslateStringTables;
    private System.Windows.Forms.Button btnRestoreDialogTables;
    private System.Windows.Forms.Button btnTranslateDialogTables;
    private System.Windows.Forms.Button btnRestoreAutoTrans;
    private System.Windows.Forms.Button btnTranslateAutoTrans;
    private System.Windows.Forms.Button btnRestoreItemData;
    private System.Windows.Forms.Button btnTranslateItemData;
    private System.Windows.Forms.MenuItem mnuTranslateItemNames;
    private System.Windows.Forms.MenuItem mnuTranslateItemDescriptions;
    private System.Windows.Forms.MenuItem mnuPreserveJapaneseATCompletion;
    private System.Windows.Forms.MenuItem mnuEnglishATCompletionOnly;
    private System.Windows.Forms.Button btnConfigStringTables;
    private System.Windows.Forms.Button btnConfigDialogTables;
    private System.Windows.Forms.Button btnConfigAbilities;
    private System.Windows.Forms.Label lblAbilities;
    private System.Windows.Forms.Button btnRestoreAbilities;
    private System.Windows.Forms.Button btnTranslateAbilities;
    private System.Windows.Forms.ContextMenu mnuConfigAbilities;
    private System.Windows.Forms.MenuItem mnuTranslateAbilityNames;
    private System.Windows.Forms.MenuItem mnuTranslateAbilityDescriptions;

  }

}
