// Copyright © 2004-2014 Tim Van Holder, Windower Team
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS"
// BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

namespace PlayOnline.FFXI.Utils.StrangeApparatus {

  partial class MainWindow {

    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing) {
      if (disposing && components != null)
	components.Dispose();
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private void InitializeComponent() {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
      this.btnGenerateCodes = new System.Windows.Forms.Button();
      this.txtCharacterName = new System.Windows.Forms.TextBox();
      this.lblCharacterName = new System.Windows.Forms.Label();
      this.lvCodes = new System.Windows.Forms.ListView();
      this.colArea = new System.Windows.Forms.ColumnHeader();
      this.colElement = new System.Windows.Forms.ColumnHeader();
      this.colChipColor = new System.Windows.Forms.ColumnHeader();
      this.colCode = new System.Windows.Forms.ColumnHeader();
      this.SuspendLayout();
      // 
      // btnGenerateCodes
      // 
      resources.ApplyResources(this.btnGenerateCodes, "btnGenerateCodes");
      this.btnGenerateCodes.Name = "btnGenerateCodes";
      this.btnGenerateCodes.UseVisualStyleBackColor = true;
      this.btnGenerateCodes.Click += new System.EventHandler(this.btnGenerateCodes_Click);
      // 
      // txtCharacterName
      // 
      resources.ApplyResources(this.txtCharacterName, "txtCharacterName");
      this.txtCharacterName.Name = "txtCharacterName";
      // 
      // lblCharacterName
      // 
      resources.ApplyResources(this.lblCharacterName, "lblCharacterName");
      this.lblCharacterName.Name = "lblCharacterName";
      // 
      // lvCodes
      // 
      this.lvCodes.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colArea,
            this.colElement,
            this.colChipColor,
            this.colCode});
      this.lvCodes.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
      resources.ApplyResources(this.lvCodes, "lvCodes");
      this.lvCodes.Name = "lvCodes";
      this.lvCodes.UseCompatibleStateImageBehavior = false;
      this.lvCodes.View = System.Windows.Forms.View.Details;
      // 
      // colArea
      // 
      resources.ApplyResources(this.colArea, "colArea");
      // 
      // colElement
      // 
      resources.ApplyResources(this.colElement, "colElement");
      // 
      // colChipColor
      // 
      resources.ApplyResources(this.colChipColor, "colChipColor");
      // 
      // colCode
      // 
      resources.ApplyResources(this.colCode, "colCode");
      // 
      // MainWindow
      // 
      this.AcceptButton = this.btnGenerateCodes;
      resources.ApplyResources(this, "$this");
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.lvCodes);
      this.Controls.Add(this.lblCharacterName);
      this.Controls.Add(this.txtCharacterName);
      this.Controls.Add(this.btnGenerateCodes);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.Name = "MainWindow";
      this.ShowIcon = false;
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button btnGenerateCodes;
    private System.Windows.Forms.TextBox txtCharacterName;
    private System.Windows.Forms.Label lblCharacterName;
    private System.Windows.Forms.ListView lvCodes;
    private System.Windows.Forms.ColumnHeader colArea;
    private System.Windows.Forms.ColumnHeader colElement;
    private System.Windows.Forms.ColumnHeader colChipColor;
    private System.Windows.Forms.ColumnHeader colCode;

  }

}