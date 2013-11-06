// $Id: Item.Designer.cs 757 2010-07-04 13:05:45Z tim.vanholder $

// Copyright © 2004-2010 Tim Van Holder
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS"
// BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

namespace PlayOnline.FFXI.PropertyPages {

  partial class Item {

    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing) {
      if (disposing && components != null)
	components.Dispose();
      base.Dispose(disposing);
    }

    #region Component Designer generated code

    private void InitializeComponent() {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Item));
      this.ieEditor = new PlayOnline.FFXI.ItemEditor();
      this.SuspendLayout();
      // 
      // ieEditor
      // 
      this.ieEditor.BackColor = System.Drawing.Color.Transparent;
      this.ieEditor.Item = null;
      resources.ApplyResources(this.ieEditor, "ieEditor");
      this.ieEditor.Name = "ieEditor";
      // 
      // Item
      // 
      resources.ApplyResources(this, "$this");
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.ieEditor);
      this.Name = "Item";
      this.TabName = "Item";
      this.ResumeLayout(false);

    }

    #endregion

    private ItemEditor ieEditor;


  }

}
