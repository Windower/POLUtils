// Copyright © 2004-2014 Tim Van Holder, Windower Team
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS"
// BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Xsl;

namespace ItemListUpgrade
{
    internal class Program
    {
        private OpenFileDialog dlgOldFile = new OpenFileDialog();
        private SaveFileDialog dlgNewFile = new SaveFileDialog();

        private Program()
        {
            // Prepare dialogs
            this.dlgOldFile.DefaultExt = "xml";
            this.dlgOldFile.Filter = this.GetText("FileFilter");
            this.dlgOldFile.Title = this.GetText("Title:OldFile");
            this.dlgNewFile.DefaultExt = "xml";
            this.dlgNewFile.Filter = this.GetText("FileFilter");
            this.dlgNewFile.Title = this.GetText("Title:NewFile");
        }

        private void Run()
        {
            if (this.dlgOldFile.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            if (this.dlgNewFile.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            this.PerformUpgrade(this.dlgOldFile.FileName, this.dlgNewFile.FileName);
        }

        #region I18N

        private ResourceManager Resources = new ResourceManager("Messages", Assembly.GetExecutingAssembly());

        private string GetText(string Name)
        {
            string ResourceString = this.Resources.GetObject(Name, CultureInfo.CurrentUICulture) as string;
            if (ResourceString == null)
            {
                ResourceString = this.Resources.GetObject(Name, CultureInfo.InvariantCulture) as string;
            }
            if (ResourceString == null)
            {
                return Name;
            }
            else
            {
                return ResourceString;
            }
        }

        #endregion

        #region Applying the XSLT transform

        private XslCompiledTransform UpgradeTransform = null;

        private void PrepareTransform()
        {
            if (this.UpgradeTransform != null)
            {
                return;
            }
            try
            {
                this.UpgradeTransform = new XslCompiledTransform();
                XmlReader XR = new XmlTextReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("ItemListUpgrade.xslt"));
                this.UpgradeTransform.Load(XR);
                XR.Close();
            }
            catch
            {
                this.UpgradeTransform = null;
            }
        }

        private void PerformUpgrade(string OldListFile, string NewListFile)
        {
            this.PrepareTransform();
            if (this.UpgradeTransform != null)
            {
                try
                {
                    XmlDocument XD = new XmlDocument();
                    XD.Load(OldListFile);
                    XmlWriter XW = XmlTextWriter.Create(NewListFile, this.UpgradeTransform.OutputSettings);
                    this.UpgradeTransform.Transform(XD, XW);
                    XW.Close();
                    MessageBox.Show(null, this.GetText("UpgradeSuccess"), this.GetText("Title:UpgradeComplete"),
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception E)
                {
                    MessageBox.Show(null, String.Format(this.GetText("UpgradeFailed"), E.Message),
                        this.GetText("Title:UpgradeFailed"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show(null, this.GetText("PrepareFailed"), this.GetText("Title:UpgradeFailed"), MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        #endregion

        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Program P = new Program();
            P.Run();
        }
    }
}
