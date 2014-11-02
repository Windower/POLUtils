// Copyright © 2004-2014 Tim Van Holder, Windower Team
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS"
// BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

namespace PlayOnline.FFXI {

  public partial class PleaseWaitDialog {

    #region Controls

    private System.Windows.Forms.Label lblMessage;

    private System.ComponentModel.Container components = null;

    #endregion

    #region Windows Form Designer generated code

    protected override void Dispose(bool disposing) {
      if (disposing && components != null)
        components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent() {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PleaseWaitDialog));
      this.lblMessage = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // lblMessage
      // 
      this.lblMessage.AccessibleDescription = resources.GetString("lblMessage.AccessibleDescription");
      this.lblMessage.AccessibleName = resources.GetString("lblMessage.AccessibleName");
      resources.ApplyResources(this.lblMessage, "lblMessage");
      this.lblMessage.BackColor = System.Drawing.Color.Transparent;
      this.lblMessage.Name = "lblMessage";
      this.lblMessage.RightToLeft = ((System.Windows.Forms.RightToLeft) (resources.GetObject("lblMessage.RightToLeft")));
      this.lblMessage.UseMnemonic = false;
      // 
      // PleaseWaitDialog
      // 
      this.AccessibleDescription = resources.GetString("$this.AccessibleDescription");
      this.AccessibleName = resources.GetString("$this.AccessibleName");
      resources.ApplyResources(this, "$this");
      this.BackgroundImage = ((System.Drawing.Image) (resources.GetObject("$this.BackgroundImage")));
      this.ControlBox = false;
      this.Controls.Add(this.lblMessage);
      this.Font = ((System.Drawing.Font) (resources.GetObject("$this.Font")));
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      this.Icon = ((System.Drawing.Icon) (resources.GetObject("$this.Icon")));
      this.ImeMode = ((System.Windows.Forms.ImeMode) (resources.GetObject("$this.ImeMode")));
      this.Name = "PleaseWaitDialog";
      this.RightToLeft = ((System.Windows.Forms.RightToLeft) (resources.GetObject("$this.RightToLeft")));
      this.ShowInTaskbar = false;
      this.ResumeLayout(false);

    }

    #endregion

  }

}
