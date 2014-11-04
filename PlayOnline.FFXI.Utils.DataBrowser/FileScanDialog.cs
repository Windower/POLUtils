// Copyright © 2004-2014 Tim Van Holder, Windower Team
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS"
// BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace PlayOnline.FFXI.Utils.DataBrowser
{
    internal partial class FileScanDialog : Form
    {
        public FileScanDialog()
        {
            InitializeComponent();
            this.DialogResult = DialogResult.Abort;
        }

        public void ResetProgress()
        {
            this.prbScanProgress.Text = String.Empty;
            this.prbScanProgress.Value = 0;
            this.prbScanProgress.Visible = true;
        }

        public void SetProgress(string Message, double PercentCompleted)
        {
            if (Message != null)
            {
                this.lblScanProgress.Text = Message;
            }
            this.prbScanProgress.Value = Math.Min((int)(PercentCompleted * this.prbScanProgress.Maximum),
                this.prbScanProgress.Maximum);
        }

        public void Finish()
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
