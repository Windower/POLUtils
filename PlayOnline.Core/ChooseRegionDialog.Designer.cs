// Copyright © 2004-2014 Tim Van Holder, Windower Team
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS"
// BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

namespace PlayOnline.Core {

  internal partial class ChooseRegionDialog {

    #region Controls

    private System.Windows.Forms.Label lblExplanation;
    private System.Windows.Forms.RadioButton radJapan;
    private System.Windows.Forms.RadioButton radNorthAmerica;
    private System.Windows.Forms.RadioButton radEurope;
    private System.Windows.Forms.Button btnOK;

    private System.ComponentModel.Container components = null;

    #endregion

    #region Windows Form Designer generated code

    protected override void Dispose(bool disposing) {
      if (disposing && components != null)
        components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent() {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChooseRegionDialog));
      this.lblExplanation = new System.Windows.Forms.Label();
      this.radJapan = new System.Windows.Forms.RadioButton();
      this.radNorthAmerica = new System.Windows.Forms.RadioButton();
      this.radEurope = new System.Windows.Forms.RadioButton();
      this.btnOK = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // lblExplanation
      // 
      this.lblExplanation.AccessibleDescription = null;
      this.lblExplanation.AccessibleName = null;
      resources.ApplyResources(this.lblExplanation, "lblExplanation");
      this.lblExplanation.BackColor = System.Drawing.SystemColors.Control;
      this.lblExplanation.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblExplanation.Font = null;
      this.lblExplanation.Name = "lblExplanation";
      // 
      // radJapan
      // 
      this.radJapan.AccessibleDescription = null;
      this.radJapan.AccessibleName = null;
      resources.ApplyResources(this.radJapan, "radJapan");
      this.radJapan.BackgroundImage = null;
      this.radJapan.Font = null;
      this.radJapan.Name = "radJapan";
      // 
      // radNorthAmerica
      // 
      this.radNorthAmerica.AccessibleDescription = null;
      this.radNorthAmerica.AccessibleName = null;
      resources.ApplyResources(this.radNorthAmerica, "radNorthAmerica");
      this.radNorthAmerica.BackgroundImage = null;
      this.radNorthAmerica.Font = null;
      this.radNorthAmerica.Name = "radNorthAmerica";
      // 
      // radEurope
      // 
      this.radEurope.AccessibleDescription = null;
      this.radEurope.AccessibleName = null;
      resources.ApplyResources(this.radEurope, "radEurope");
      this.radEurope.BackgroundImage = null;
      this.radEurope.Font = null;
      this.radEurope.Name = "radEurope";
      // 
      // btnOK
      // 
      this.btnOK.AccessibleDescription = null;
      this.btnOK.AccessibleName = null;
      resources.ApplyResources(this.btnOK, "btnOK");
      this.btnOK.BackgroundImage = null;
      this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnOK.Font = null;
      this.btnOK.Name = "btnOK";
      this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
      // 
      // ChooseRegionDialog
      // 
      this.AccessibleDescription = null;
      this.AccessibleName = null;
      resources.ApplyResources(this, "$this");
      this.BackgroundImage = null;
      this.ControlBox = false;
      this.Controls.Add(this.btnOK);
      this.Controls.Add(this.radEurope);
      this.Controls.Add(this.radNorthAmerica);
      this.Controls.Add(this.radJapan);
      this.Controls.Add(this.lblExplanation);
      this.Font = null;
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.Icon = null;
      this.Name = "ChooseRegionDialog";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

  }

}
