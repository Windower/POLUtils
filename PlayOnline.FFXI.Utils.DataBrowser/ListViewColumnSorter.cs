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
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using PlayOnline.Core;

namespace PlayOnline.FFXI.Utils.DataBrowser
{
    internal class ListViewColumnSorter : IComparer
    {
        public int Column = 0;
        public bool Numeric = false;
        public SortOrder Order = SortOrder.Ascending;

        public int Compare(object x, object y)
        {
            if (this.Order == SortOrder.None)
            {
                return 0;
            }
            ListViewItem LVI1 = x as ListViewItem;
            ListViewItem LVI2 = y as ListViewItem;
            if (LVI1 == null || LVI2 == null || LVI1.SubItems.Count <= this.Column || LVI2.SubItems.Count <= this.Column)
            {
                return 0;
            }
            int result = 0;
            if (this.Numeric)
            {
                try
                {
                    double L1 = double.Parse(LVI1.SubItems[Column].Text);
                    double L2 = double.Parse(LVI2.SubItems[Column].Text);
                    result = L1.CompareTo(L2);
                }
                catch {}
            }
            else
            {
                result = LVI1.SubItems[this.Column].Text.CompareTo(LVI2.SubItems[this.Column].Text);
            }
            if (this.Order == SortOrder.Descending)
            {
                result *= -1;
            }
            return result;
        }

        public static void ListView_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            ListView LV = sender as ListView;
            if (LV == null)
            {
                return;
            }
            ListViewColumnSorter S = LV.ListViewItemSorter as ListViewColumnSorter;
            if (S == null)
            {
                S = new ListViewColumnSorter();
                LV.ListViewItemSorter = S;
                S.Column = -1;
            }
            if (S.Column == e.Column)
            {
                S.Order = ((S.Order == SortOrder.Ascending) ? SortOrder.Descending : SortOrder.Ascending);
            }
            else
            {
                S.Column = e.Column;
                S.Order = SortOrder.Ascending;
            }
            LV.Sort();
        }
    }
}
