// Copyright © 2004-2014 Tim Van Holder, Windower Team
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS"
// BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

namespace PlayOnline.Utils.AudioManager {

  public partial class InfoBuilder {

    #region Controls

    private System.Windows.Forms.ProgressBar prbApplication;
    private System.Windows.Forms.Label lblApplication;
    private System.Windows.Forms.TextBox txtApplication;
    private System.Windows.Forms.TextBox txtDirectory;
    private System.Windows.Forms.Label lblDirectory;
    private System.Windows.Forms.ProgressBar prbDirectory;
    private System.Windows.Forms.TextBox txtFile;
    private System.Windows.Forms.Label lblFile;
    private System.Windows.Forms.ProgressBar prbFile;
    private System.ComponentModel.Container components = null;

    #endregion

    #region Windows Form Designer generated code

    protected override void Dispose(bool disposing) {
      if (disposing && components != null)
        components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent() {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InfoBuilder));
      this.prbApplication = new System.Windows.Forms.ProgressBar();
      this.lblApplication = new System.Windows.Forms.Label();
      this.txtApplication = new System.Windows.Forms.TextBox();
      this.txtDirectory = new System.Windows.Forms.TextBox();
      this.lblDirectory = new System.Windows.Forms.Label();
      this.prbDirectory = new System.Windows.Forms.ProgressBar();
      this.txtFile = new System.Windows.Forms.TextBox();
      this.lblFile = new System.Windows.Forms.Label();
      this.prbFile = new System.Windows.Forms.ProgressBar();
      this.SuspendLayout();
      // 
      // prbApplication
      // 
      this.prbApplication.AccessibleDescription = null;
      this.prbApplication.AccessibleName = null;
      resources.ApplyResources(this.prbApplication, "prbApplication");
      this.prbApplication.BackgroundImage = null;
      this.prbApplication.Font = null;
      this.prbApplication.Name = "prbApplication";
      // 
      // lblApplication
      // 
      this.lblApplication.AccessibleDescription = null;
      this.lblApplication.AccessibleName = null;
      resources.ApplyResources(this.lblApplication, "lblApplication");
      this.lblApplication.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblApplication.Font = null;
      this.lblApplication.Name = "lblApplication";
      // 
      // txtApplication
      // 
      this.txtApplication.AccessibleDescription = null;
      this.txtApplication.AccessibleName = null;
      resources.ApplyResources(this.txtApplication, "txtApplication");
      this.txtApplication.BackgroundImage = null;
      this.txtApplication.Font = null;
      this.txtApplication.Name = "txtApplication";
      this.txtApplication.ReadOnly = true;
      // 
      // txtDirectory
      // 
      this.txtDirectory.AccessibleDescription = null;
      this.txtDirectory.AccessibleName = null;
      resources.ApplyResources(this.txtDirectory, "txtDirectory");
      this.txtDirectory.BackgroundImage = null;
      this.txtDirectory.Font = null;
      this.txtDirectory.Name = "txtDirectory";
      this.txtDirectory.ReadOnly = true;
      // 
      // lblDirectory
      // 
      this.lblDirectory.AccessibleDescription = null;
      this.lblDirectory.AccessibleName = null;
      resources.ApplyResources(this.lblDirectory, "lblDirectory");
      this.lblDirectory.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblDirectory.Font = null;
      this.lblDirectory.Name = "lblDirectory";
      // 
      // prbDirectory
      // 
      this.prbDirectory.AccessibleDescription = null;
      this.prbDirectory.AccessibleName = null;
      resources.ApplyResources(this.prbDirectory, "prbDirectory");
      this.prbDirectory.BackgroundImage = null;
      this.prbDirectory.Font = null;
      this.prbDirectory.Name = "prbDirectory";
      // 
      // txtFile
      // 
      this.txtFile.AccessibleDescription = null;
      this.txtFile.AccessibleName = null;
      resources.ApplyResources(this.txtFile, "txtFile");
      this.txtFile.BackgroundImage = null;
      this.txtFile.Font = null;
      this.txtFile.Name = "txtFile";
      this.txtFile.ReadOnly = true;
      // 
      // lblFile
      // 
      this.lblFile.AccessibleDescription = null;
      this.lblFile.AccessibleName = null;
      resources.ApplyResources(this.lblFile, "lblFile");
      this.lblFile.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblFile.Font = null;
      this.lblFile.Name = "lblFile";
      // 
      // prbFile
      // 
      this.prbFile.AccessibleDescription = null;
      this.prbFile.AccessibleName = null;
      resources.ApplyResources(this.prbFile, "prbFile");
      this.prbFile.BackgroundImage = null;
      this.prbFile.Font = null;
      this.prbFile.Name = "prbFile";
      // 
      // InfoBuilder
      // 
      this.AccessibleDescription = null;
      this.AccessibleName = null;
      resources.ApplyResources(this, "$this");
      this.BackgroundImage = null;
      this.ControlBox = false;
      this.Controls.Add(this.txtFile);
      this.Controls.Add(this.txtDirectory);
      this.Controls.Add(this.txtApplication);
      this.Controls.Add(this.lblFile);
      this.Controls.Add(this.prbFile);
      this.Controls.Add(this.lblDirectory);
      this.Controls.Add(this.prbDirectory);
      this.Controls.Add(this.lblApplication);
      this.Controls.Add(this.prbApplication);
      this.Font = null;
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.Icon = null;
      this.Name = "InfoBuilder";
      this.ShowInTaskbar = false;
      this.VisibleChanged += new System.EventHandler(this.InfoBuilder_VisibleChanged);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

  }

}
