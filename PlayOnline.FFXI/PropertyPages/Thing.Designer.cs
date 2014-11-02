// Copyright © 2004-2014 Tim Van Holder, Windower Team
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS"
// BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

namespace PlayOnline.FFXI.PropertyPages {

  partial class Thing {

    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing) {
      if (disposing && components != null)
	components.Dispose();
      base.Dispose(disposing);
    }

    #region Component Designer generated code

    private void InitializeComponent() {
      System.Windows.Forms.ColumnHeader colFieldName;
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Thing));
      System.Windows.Forms.ColumnHeader colFieldValue;
      this.lblText = new System.Windows.Forms.Label();
      this.lstFields = new System.Windows.Forms.ListView();
      this.lblFields = new System.Windows.Forms.Label();
      this.lblTypeName = new System.Windows.Forms.Label();
      this.lblType = new System.Windows.Forms.Label();
      this.picIcon = new System.Windows.Forms.PictureBox();
      colFieldName = new System.Windows.Forms.ColumnHeader();
      colFieldValue = new System.Windows.Forms.ColumnHeader();
      ((System.ComponentModel.ISupportInitialize) (this.picIcon)).BeginInit();
      this.SuspendLayout();
      // 
      // colFieldName
      // 
      resources.ApplyResources(colFieldName, "colFieldName");
      // 
      // colFieldValue
      // 
      resources.ApplyResources(colFieldValue, "colFieldValue");
      // 
      // lblText
      // 
      resources.ApplyResources(this.lblText, "lblText");
      this.lblText.AutoEllipsis = true;
      this.lblText.Name = "lblText";
      // 
      // lstFields
      // 
      resources.ApplyResources(this.lstFields, "lstFields");
      this.lstFields.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            colFieldName,
            colFieldValue});
      this.lstFields.FullRowSelect = true;
      this.lstFields.Name = "lstFields";
      this.lstFields.UseCompatibleStateImageBehavior = false;
      this.lstFields.View = System.Windows.Forms.View.Details;
      this.lstFields.ItemActivate += new System.EventHandler(this.lstFields_ItemActivate);
      // 
      // lblFields
      // 
      resources.ApplyResources(this.lblFields, "lblFields");
      this.lblFields.Name = "lblFields";
      // 
      // lblTypeName
      // 
      resources.ApplyResources(this.lblTypeName, "lblTypeName");
      this.lblTypeName.AutoEllipsis = true;
      this.lblTypeName.Name = "lblTypeName";
      // 
      // lblType
      // 
      resources.ApplyResources(this.lblType, "lblType");
      this.lblType.Name = "lblType";
      // 
      // picIcon
      // 
      resources.ApplyResources(this.picIcon, "picIcon");
      this.picIcon.Name = "picIcon";
      this.picIcon.TabStop = false;
      // 
      // Thing
      // 
      resources.ApplyResources(this, "$this");
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.lblText);
      this.Controls.Add(this.lstFields);
      this.Controls.Add(this.lblFields);
      this.Controls.Add(this.lblTypeName);
      this.Controls.Add(this.lblType);
      this.Controls.Add(this.picIcon);
      this.Name = "Thing";
      this.TabName = "General";
      ((System.ComponentModel.ISupportInitialize) (this.picIcon)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label lblText;
    private System.Windows.Forms.ListView lstFields;
    private System.Windows.Forms.Label lblFields;
    private System.Windows.Forms.Label lblTypeName;
    private System.Windows.Forms.Label lblType;
    private System.Windows.Forms.PictureBox picIcon;


  }

}
