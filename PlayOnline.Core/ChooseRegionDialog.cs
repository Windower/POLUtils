// Copyright © 2004-2014 Tim Van Holder, Windower Team
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS"
// BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace PlayOnline.Core
{
    internal partial class ChooseRegionDialog : Form
    {
        public ChooseRegionDialog()
        {
            this.InitializeComponent();
            this.radJapan.Checked = (POL.SelectedRegion == POL.Region.Japan);
            this.radJapan.Enabled = ((POL.AvailableRegions & POL.Region.Japan) != 0);
            this.radNorthAmerica.Checked = (POL.SelectedRegion == POL.Region.NorthAmerica);
            this.radNorthAmerica.Enabled = ((POL.AvailableRegions & POL.Region.NorthAmerica) != 0);
            this.radEurope.Checked = (POL.SelectedRegion == POL.Region.Europe);
            this.radEurope.Enabled = ((POL.AvailableRegions & POL.Region.Europe) != 0);
        }

        #region Events

        private void btnOK_Click(object sender, System.EventArgs e)
        {
            if (this.radJapan.Checked)
            {
                POL.SelectedRegion = POL.Region.Japan;
            }
            if (this.radNorthAmerica.Checked)
            {
                POL.SelectedRegion = POL.Region.NorthAmerica;
            }
            if (this.radEurope.Checked)
            {
                POL.SelectedRegion = POL.Region.Europe;
            }
        }

        #endregion
    }
}
