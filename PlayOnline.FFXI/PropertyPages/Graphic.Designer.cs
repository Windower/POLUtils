// Copyright © 2004-2014 Tim Van Holder, Windower Team
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS"
// BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

namespace PlayOnline.FFXI.PropertyPages {

  partial class Graphic {

    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing) {
      if (disposing && components != null)
	components.Dispose();
      base.Dispose(disposing);
    }

    #region Component Designer generated code

    private void InitializeComponent() {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Graphic));
      this.btnSelectColor = new System.Windows.Forms.Button();
      this.lblBackColor = new System.Windows.Forms.Label();
      this.cmbViewMode = new System.Windows.Forms.ComboBox();
      this.lblViewMode = new System.Windows.Forms.Label();
      this.picImage = new System.Windows.Forms.PictureBox();
      this.dlgChooseColor = new System.Windows.Forms.ColorDialog();
      this.cmbBackColor = new System.Windows.Forms.ComboBox();
      this.btnSave = new System.Windows.Forms.Button();
      this.dlgSaveImage = new System.Windows.Forms.SaveFileDialog();
      ((System.ComponentModel.ISupportInitialize) (this.picImage)).BeginInit();
      this.SuspendLayout();
      // 
      // btnSelectColor
      // 
      resources.ApplyResources(this.btnSelectColor, "btnSelectColor");
      this.btnSelectColor.Name = "btnSelectColor";
      this.btnSelectColor.UseVisualStyleBackColor = true;
      this.btnSelectColor.Click += new System.EventHandler(this.btnSelectColor_Click);
      // 
      // lblBackColor
      // 
      resources.ApplyResources(this.lblBackColor, "lblBackColor");
      this.lblBackColor.Name = "lblBackColor";
      // 
      // cmbViewMode
      // 
      resources.ApplyResources(this.cmbViewMode, "cmbViewMode");
      this.cmbViewMode.DisplayMember = "Name";
      this.cmbViewMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cmbViewMode.Name = "cmbViewMode";
      this.cmbViewMode.ValueMember = "Value";
      this.cmbViewMode.SelectedIndexChanged += new System.EventHandler(this.cmbViewMode_SelectedIndexChanged);
      // 
      // lblViewMode
      // 
      resources.ApplyResources(this.lblViewMode, "lblViewMode");
      this.lblViewMode.Name = "lblViewMode";
      // 
      // picImage
      // 
      resources.ApplyResources(this.picImage, "picImage");
      this.picImage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.picImage.Name = "picImage";
      this.picImage.TabStop = false;
      // 
      // dlgChooseColor
      // 
      this.dlgChooseColor.AnyColor = true;
      this.dlgChooseColor.FullOpen = true;
      // 
      // cmbBackColor
      // 
      resources.ApplyResources(this.cmbBackColor, "cmbBackColor");
      this.cmbBackColor.DisplayMember = "Name";
      this.cmbBackColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cmbBackColor.Items.AddRange(new object[] {
            resources.GetString("cmbBackColor.Items"),
            resources.GetString("cmbBackColor.Items1")});
      this.cmbBackColor.Name = "cmbBackColor";
      this.cmbBackColor.ValueMember = "Value";
      this.cmbBackColor.SelectedIndexChanged += new System.EventHandler(this.cmbBackColor_SelectedIndexChanged);
      // 
      // btnSave
      // 
      resources.ApplyResources(this.btnSave, "btnSave");
      this.btnSave.Name = "btnSave";
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
      // 
      // dlgSaveImage
      // 
      resources.ApplyResources(this.dlgSaveImage, "dlgSaveImage");
      // 
      // Graphic
      // 
      resources.ApplyResources(this, "$this");
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.btnSave);
      this.Controls.Add(this.cmbBackColor);
      this.Controls.Add(this.btnSelectColor);
      this.Controls.Add(this.lblBackColor);
      this.Controls.Add(this.cmbViewMode);
      this.Controls.Add(this.lblViewMode);
      this.Controls.Add(this.picImage);
      this.Name = "Graphic";
      this.TabName = "Graphic";
      ((System.ComponentModel.ISupportInitialize) (this.picImage)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button btnSelectColor;
    private System.Windows.Forms.Label lblBackColor;
    private System.Windows.Forms.ComboBox cmbViewMode;
    private System.Windows.Forms.Label lblViewMode;
    private System.Windows.Forms.PictureBox picImage;
    private System.Windows.Forms.ColorDialog dlgChooseColor;
    private System.Windows.Forms.ComboBox cmbBackColor;
    private System.Windows.Forms.Button btnSave;
    private System.Windows.Forms.SaveFileDialog dlgSaveImage;


  }

}
