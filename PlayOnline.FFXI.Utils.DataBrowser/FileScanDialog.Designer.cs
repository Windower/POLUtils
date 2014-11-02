// Copyright © 2004-2014 Tim Van Holder, Windower Team
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS"
// BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

namespace PlayOnline.FFXI.Utils.DataBrowser {

  internal partial class FileScanDialog {

    #region Controls

    private System.Windows.Forms.ProgressBar prbScanProgress;
    private System.Windows.Forms.Label lblScanProgress;

    private System.ComponentModel.Container components = null;

    #endregion

    #region Windows Form Designer generated code

    protected override void Dispose(bool disposing) {
      if (disposing && components != null)
        components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent() {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FileScanDialog));
      this.prbScanProgress = new System.Windows.Forms.ProgressBar();
      this.lblScanProgress = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // prbScanProgress
      // 
      resources.ApplyResources(this.prbScanProgress, "prbScanProgress");
      this.prbScanProgress.Maximum = 1000;
      this.prbScanProgress.Name = "prbScanProgress";
      // 
      // lblScanProgress
      // 
      resources.ApplyResources(this.lblScanProgress, "lblScanProgress");
      this.lblScanProgress.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblScanProgress.Name = "lblScanProgress";
      // 
      // FileScanDialog
      // 
      resources.ApplyResources(this, "$this");
      this.Controls.Add(this.prbScanProgress);
      this.Controls.Add(this.lblScanProgress);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      this.Name = "FileScanDialog";
      this.ShowInTaskbar = false;
      this.ResumeLayout(false);

    }

    #endregion

  }

}
