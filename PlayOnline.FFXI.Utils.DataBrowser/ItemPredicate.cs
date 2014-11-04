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
using System.Data;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using PlayOnline.Core;
using PlayOnline.FFXI.Things;

namespace PlayOnline.FFXI.Utils.DataBrowser
{
    internal partial class ItemPredicate : UserControl
    {
        private enum Test
        {
            Contains,
            DoesntContain,
            StartsWith,
            EndsWith,
            Equals,
            MatchesRegexp,
            DoesntMatchRegexp
        }

        private class ItemField
        {
            public string Field
            {
                get { return this.Field_; }
            }

            private string Field_;

            public string Name
            {
                get { return this.Name_; }
            }

            private string Name_;

            public ItemField(string Field, string Name)
            {
                this.Field_ = Field;
                this.Name_ = Name;
            }
        }

        public ItemPredicate()
        {
            this.InitializeComponent();
            {
                // Add all item fields
                List<ItemField> ItemFields = new List<ItemField>();
                ItemFields.Add(new ItemField(null, I18N.GetText("ItemField:Any")));
                {
                    Item I = new Item();
                    foreach (string Field in I.GetAllFields())
                    {
                        ItemFields.Add(new ItemField(Field, I.GetFieldName(Field)));
                    }
                }
                this.cmbField.DataSource = ItemFields;
            }
            this.cmbField.SelectedIndex = 0; // Any Field
            this.cmbTest.Items.AddRange(NamedEnum.GetAll(typeof (Test)));
            this.cmbTest.SelectedIndex = 0; // Contains
        }

        public string ValidateQuery()
        {
            NamedEnum NE = this.cmbTest.SelectedItem as NamedEnum;
            if (NE == null)
            {
                return I18N.GetText("Query:NoQueryType");
            }
            Test T = (Test)NE.Value;
            switch (T)
            {
            case Test.StartsWith:
            case Test.EndsWith:
            case Test.Contains:
            case Test.DoesntContain:
                if (this.txtTestParameter.Text == String.Empty)
                {
                    return I18N.GetText("Query:NoEmptyString");
                }
                return null;
            case Test.Equals:
                return null;
            case Test.MatchesRegexp:
            case Test.DoesntMatchRegexp:
                if (this.txtTestParameter.Text == String.Empty)
                {
                    return I18N.GetText("Query:NoEmptyString");
                }
                try
                {
                    // Try to parse the regex
                    Regex RE = new Regex(this.txtTestParameter.Text, RegexOptions.Multiline | RegexOptions.ExplicitCapture);
                }
                catch
                {
                    return I18N.GetText("Query:BadRegexp");
                }
                return null;
            }
            return I18N.GetText("Query:BadQueryType");
        }

        #region Applying The Selected Predicate

        private bool MatchString(string S)
        {
            if (S == null)
            {
                return false;
            }
            NamedEnum NE = this.cmbTest.SelectedItem as NamedEnum;
            if (NE == null)
            {
                return false;
            }
            Test T = (Test)NE.Value;
            switch (T)
            {
            case Test.StartsWith:
                return S.StartsWith(this.txtTestParameter.Text);
            case Test.EndsWith:
                return S.EndsWith(this.txtTestParameter.Text);
            case Test.Equals:
                return (S == this.txtTestParameter.Text);
            case Test.Contains:
            case Test.DoesntContain:
            {
                int Pos = S.IndexOf(this.txtTestParameter.Text);
                return ((T == Test.Contains) ? (Pos >= 0) : (Pos < 0));
            }
            case Test.MatchesRegexp:
            case Test.DoesntMatchRegexp:
            {
                Regex RE = new Regex(this.txtTestParameter.Text, RegexOptions.Multiline | RegexOptions.ExplicitCapture);
                return ((T == Test.MatchesRegexp) ? RE.IsMatch(S) : !RE.IsMatch(S));
            }
            }
            return false;
        }

        public bool IsMatch(Item I)
        {
            if (this.cmbField.SelectedValue == null)
            {
                // Any Field
                foreach (string Field in I.GetFields())
                {
                    if (this.MatchString(I.GetFieldText(Field)))
                    {
                        return true;
                    }
                }
                return false;
            }
            else
            {
                return this.MatchString(I.GetFieldText(this.cmbField.SelectedValue as string));
            }
        }

        #endregion
    }
}
