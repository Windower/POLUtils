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
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using PlayOnline.Core;
using PlayOnline.Core.Audio;

namespace PlayOnline.Utils.AudioManager
{
    public partial class InfoBuilder : Form
    {
        public InfoBuilder()
        {
            InitializeComponent();
            this.prbApplication.Select();
        }

        private TreeNode TargetNode_;

        public TreeNode TargetNode
        {
            get { return this.TargetNode_; }
            set { this.TargetNode_ = value; }
        }

        private string FileTypeName_;

        public string FileTypeName
        {
            get { return this.FileTypeName_; }
            set { this.FileTypeName_ = value; }
        }

        private string ResourceName_;

        public string ResourceName
        {
            get { return this.ResourceName_; }
            set { this.ResourceName_ = value; }
        }

        private string FilePattern_;

        public string FilePattern
        {
            get { return this.FilePattern_; }
            set { this.FilePattern_ = value; }
        }

        public void Run()
        {
            Stream InfoData = Assembly.GetExecutingAssembly().GetManifestResourceStream(this.ResourceName_);
            if (InfoData != null)
            {
                XmlReader XR = new XmlTextReader(InfoData);
                try
                {
                    XmlDocument XD = new XmlDocument();
                    XD.Load(XR);
                    XmlNodeList Apps = XD.SelectNodes("/pol-audio-data/application");
                    this.txtApplication.Text = null;
                    this.prbApplication.Value = 0;
                    this.prbApplication.Maximum = Apps.Count;
                    foreach (XmlNode App in Apps)
                    {
                        try
                        {
                            string AppName = App.Attributes["name"].InnerText;
                            this.txtApplication.Text = String.Format("[{0}/{1}] {2}", this.prbApplication.Value + 1,
                                this.prbApplication.Maximum, AppName);
                            this.txtDirectory.Text = "Scanning...";
                            this.txtFile.Text = "Scanning...";
                            this.prbDirectory.Value = 0;
                            this.prbFile.Value = 0;
                            Application.DoEvents();
                            string AppPath = POL.GetApplicationPath(App.Attributes["id"].InnerText);
                            if (AppPath != null)
                            {
                                TreeNode AppNode = new TreeNode(AppName);
                                AppNode.ImageIndex = 1;
                                AppNode.SelectedImageIndex = 1;
                                AppNode.Tag = AppPath;
                                XmlNodeList DataPaths = App.SelectNodes("data-path");
                                // Precompute totals for directories/files
                                this.prbDirectory.Maximum = 0;
                                this.prbFile.Maximum = 0;
                                foreach (XmlNode DataPath in DataPaths)
                                {
                                    this.PreScanDataPath(Path.Combine(AppPath,
                                        DataPath.InnerText.Replace('/', Path.DirectorySeparatorChar)));
                                }
                                ++this.prbApplication.Value;
                                // Now do a full scan
                                foreach (XmlNode DataPath in DataPaths)
                                {
                                    this.ScanDataPath(App,
                                        Path.Combine(AppPath, DataPath.InnerText.Replace('/', Path.DirectorySeparatorChar)), AppNode);
                                }
                                if (AppNode.Nodes.Count > 0)
                                {
                                    this.TargetNode_.Nodes.Add(AppNode);
                                }
                            }
                            else
                            {
                                ++this.prbApplication.Value;
                            }
                        }
                        catch (Exception E)
                        {
                            Console.WriteLine(E.ToString());
                        }
                    }
                }
                catch (Exception E)
                {
                    Console.WriteLine(E.ToString());
                }
                XR.Close();
                InfoData.Close();
            }
        }

        private void PreScanDataPath(string DataPath)
        {
            if (!Directory.Exists(DataPath))
            {
                return;
            }
            ++this.prbDirectory.Maximum;
            this.prbFile.Maximum += Directory.GetFiles(DataPath, this.FilePattern_).Length;
            foreach (string SubDir in Directory.GetDirectories(DataPath))
            {
                this.PreScanDataPath(SubDir);
            }
        }

        private void ScanDataPath(XmlNode App, string DataPath, TreeNode AppNode)
        {
            if (!Directory.Exists(DataPath))
            {
                return;
            }
            this.txtDirectory.Text = String.Format("[{0}/{1}] {2}", this.prbDirectory.Value + 1, this.prbDirectory.Maximum, DataPath);
            Application.DoEvents();
            foreach (string File in Directory.GetFiles(DataPath, this.FilePattern_))
            {
                this.txtFile.Text = String.Format("[{0}/{1}] {2}", this.prbFile.Value + 1, this.prbFile.Maximum,
                    Path.GetFileName(File));
                Application.DoEvents();
                AudioFile AF = new AudioFile(File);
                if (AF.Type != AudioFileType.Unknown)
                {
                    FileInfo FI = new FileInfo(App, AF);
                    TreeNode FileNode =
                        new TreeNode(String.Format("[{0}] {1}", AF.ID, ((FI.Title == null) ? Path.GetFileName(File) : FI.Title)));
                    FileNode.ImageIndex = 3;
                    FileNode.SelectedImageIndex = 3;
                    FileNode.Tag = FI;
                    AppNode.Nodes.Add(FileNode);
                }
                ++this.prbFile.Value;
            }
            ++this.prbDirectory.Value;
            // Recurse
            foreach (string SubDir in Directory.GetDirectories(DataPath))
            {
                TreeNode DirNode = new TreeNode(Path.GetFileName(SubDir));
                DirNode.ImageIndex = 1;
                DirNode.SelectedImageIndex = 1;
                FileInfo FI = new FileInfo(App, SubDir);
                DirNode.Tag = FI;
                if (FI.Title != null)
                {
                    DirNode.Text += String.Format(" - {0}", FI.Title);
                }
                this.ScanDataPath(App, SubDir, DirNode);
                if (DirNode.Nodes.Count > 0)
                {
                    AppNode.Nodes.Add(DirNode);
                }
            }
        }

        private bool Running = false;

        private void InfoBuilder_VisibleChanged(object sender, System.EventArgs e)
        {
            if (this.TargetNode == null)
            {
                throw new InvalidOperationException("No target node set.");
            }
            lock (this)
            {
                if (this.Running)
                {
                    return;
                }
                this.Running = true;
            }
            this.Show();
            Application.DoEvents();
            this.Activate();
            Application.DoEvents();
            this.Refresh();
            Application.DoEvents();
            this.Run();
            this.Close();
        }
    }
}
