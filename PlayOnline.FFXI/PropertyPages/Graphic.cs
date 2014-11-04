// Copyright © 2004-2014 Tim Van Holder, Windower Team
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS"
// BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Data;
using System.Text;
using System.Windows.Forms;
using PlayOnline.Core;

namespace PlayOnline.FFXI.PropertyPages
{
    public partial class Graphic : IThing
    {
        public Graphic(Things.Graphic G)
        {
            InitializeComponent();
            this.dlgSaveImage.FileName = G.ToString() + ".png";
            this.picImage.Image = G.GetFieldValue("image") as Image;
            this.cmbViewMode.Items.Add(new NamedEnum(PictureBoxSizeMode.Normal));
            this.cmbViewMode.Items.Add(new NamedEnum(PictureBoxSizeMode.CenterImage));
            this.cmbViewMode.Items.Add(new NamedEnum(PictureBoxSizeMode.StretchImage));
            this.cmbViewMode.Items.Add(new NamedEnum(PictureBoxSizeMode.Zoom));
            this.cmbViewMode.SelectedIndex = 0;
            this.cmbBackColor.SelectedIndex = 0;
        }

        private Color SolidColor_ = Color.White;

        private void cmbViewMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            NamedEnum NE = this.cmbViewMode.SelectedItem as NamedEnum;
            this.picImage.SizeMode = (PictureBoxSizeMode)NE.Value;
        }

        private void cmbBackColor_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.btnSelectColor.Enabled = (this.cmbBackColor.SelectedIndex != 0);
            switch (this.cmbBackColor.SelectedIndex)
            {
            case 0:
                this.picImage.BackColor = Color.Transparent;
                break;
            case 1:
                this.picImage.BackColor = this.SolidColor_;
                break;
            default:
                this.picImage.BackColor = Color.Black;
                break;
            }
        }

        private void btnSelectColor_Click(object sender, EventArgs e)
        {
            this.dlgChooseColor.Color = this.SolidColor_;
            if (this.dlgChooseColor.ShowDialog(this) == DialogResult.OK)
            {
                this.SolidColor_ = this.dlgChooseColor.Color;
                this.picImage.BackColor = this.SolidColor_;
                // FIXME: Persist custom colors?
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (this.dlgSaveImage.ShowDialog(this) == DialogResult.OK)
            {
                this.picImage.Image.Save(this.dlgSaveImage.FileName, ImageFormat.Png);
            }
        }
    }
}
