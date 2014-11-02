// Copyright © 2004-2014 Tim Van Holder, Windower Team
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS"
// BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

namespace PlayOnline.FFXI.Utils.DataBrowser {

  internal partial class ItemPredicate {

    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing) {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    #region Component Designer generated code

    private void InitializeComponent() {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ItemPredicate));
      this.cmbTest = new System.Windows.Forms.ComboBox();
      this.txtTestParameter = new System.Windows.Forms.TextBox();
      this.cmbField = new System.Windows.Forms.ComboBox();
      this.SuspendLayout();
      // 
      // cmbTest
      // 
      this.cmbTest.DisplayMember = "Name";
      this.cmbTest.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cmbTest.FormattingEnabled = true;
      resources.ApplyResources(this.cmbTest, "cmbTest");
      this.cmbTest.Name = "cmbTest";
      this.cmbTest.ValueMember = "Value";
      // 
      // txtTestParameter
      // 
      resources.ApplyResources(this.txtTestParameter, "txtTestParameter");
      this.txtTestParameter.Name = "txtTestParameter";
      // 
      // cmbField
      // 
      this.cmbField.DisplayMember = "Name";
      this.cmbField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cmbField.FormattingEnabled = true;
      resources.ApplyResources(this.cmbField, "cmbField");
      this.cmbField.Name = "cmbField";
      this.cmbField.ValueMember = "Field";
      // 
      // ItemPredicate
      // 
      this.BackColor = System.Drawing.Color.Transparent;
      this.Controls.Add(this.cmbField);
      this.Controls.Add(this.cmbTest);
      this.Controls.Add(this.txtTestParameter);
      this.Name = "ItemPredicate";
      resources.ApplyResources(this, "$this");
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.ComboBox cmbField;
    private System.Windows.Forms.ComboBox cmbTest;
    private System.Windows.Forms.TextBox txtTestParameter;

  }

}
