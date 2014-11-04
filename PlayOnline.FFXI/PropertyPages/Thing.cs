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
using System.Data;
using System.Text;
using System.Windows.Forms;
using PlayOnline.Core;

namespace PlayOnline.FFXI.PropertyPages
{
    public partial class Thing : IThing
    {
        public Thing(Things.IThing T)
        {
            InitializeComponent();
            this.picIcon.Image = T.GetIcon();
            if (this.picIcon.Image == null)
            {
                this.picIcon.Image = Icons.POLViewer.ToBitmap();
            }
            this.lblText.Text = T.ToString();
            this.lblTypeName.Text = T.TypeName;
            this.AddFieldEntries(T);
        }

        private void AddFieldEntries(Things.IThing T)
        {
            List<String> AllFields = T.GetAllFields();
            foreach (string FieldName in AllFields)
            {
                if (T.HasField(FieldName))
                {
                    object V = T.GetFieldValue(FieldName);
                    if (V is string && (V as string).Contains("\n"))
                    {
                        int LineCount = 0;
                        foreach (string Line in (V as string).Split('\n'))
                        {
                            ListViewItem LVI = this.lstFields.Items.Add(String.Format("{0} [{1}]", FieldName, ++LineCount));
                            LVI.Tag = Line;
                            LVI.SubItems.Add(Line);
                        }
                    }
                    else
                    {
                        ListViewItem LVI = this.lstFields.Items.Add(FieldName);
                        LVI.Tag = V;
                        LVI.SubItems.Add(T.GetFieldText(FieldName));
                        if (V is Things.IThing)
                        {
                            LVI.Font = new Font(LVI.Font, FontStyle.Underline);
                        }
                    }
                }
                else
                {
                    this.lstFields.Items.Add(FieldName).ForeColor = SystemColors.GrayText;
                }
            }
        }

        private void lstFields_ItemActivate(object sender, EventArgs e)
        {
            if (this.lstFields.SelectedItems != null && this.lstFields.SelectedItems.Count > 0)
            {
                Things.IThing T = this.lstFields.SelectedItems[0].Tag as Things.IThing;
                if (T != null)
                {
                    using (ThingPropertyPages TPP = new ThingPropertyPages(T))
                    {
                        TPP.ShowDialog(this);
                    }
                }
            }
        }
    }
}
