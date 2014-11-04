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
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Collections;
using System.Windows.Forms;
using PlayOnline.Core;

namespace PlayOnline.Utils.TetraViewer
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Icon = Icons.TetraMaster;
            TreeNode Root = new TreeNode("Tetra Master Data Files");
            this.tvDataFiles.Nodes.Add(Root);
            string TetraDir = POL.GetApplicationPath(AppID.TetraMaster);
            foreach (string DataFilePath in Directory.GetFiles(Path.Combine(TetraDir, "data"), "gW*.dat"))
            {
                TreeNode TN = new TreeNode(Path.GetFileName(DataFilePath));
                TN.Tag = DataFilePath;
                Root.Nodes.Add(TN);
                FileStream F = new FileStream(DataFilePath, FileMode.Open, FileAccess.Read);
                this.ReadTOC(F, TN);
            }
            Root.Expand();
        }

        private class TOCEntry
        {
            public Stream Source;
            public int Type;
            public string Name;
            public long Offset;
            public int Size;

            public TOCEntry(Stream S, long BaseOffset)
            {
                this.Source = S;
                BinaryReader BR = new BinaryReader(S, Encoding.ASCII);
                this.Type = BR.ReadInt32();
                this.Name = new string(BR.ReadChars(12));
                {
                    int end = this.Name.IndexOf('\0');
                    if (end >= 0)
                    {
                        this.Name = this.Name.Substring(0, end);
                    }
                }
                foreach (char c in Path.GetInvalidFileNameChars())
                {
                    this.Name = this.Name.Replace(c, '_');
                }
                this.Offset = BaseOffset + BR.ReadInt32();
                this.Size = BR.ReadInt32();
                BR.ReadInt32();
                BR.ReadInt32();
            }
        }

        private void ReadSubTOC(Stream S, TreeNode Root)
        {
            long BaseOffset = S.Position;
            TOCEntry TE = new TOCEntry(S, BaseOffset);
            while (TE.Type == 0x4000)
            {
                TreeNode TN = new TreeNode(TE.Name);
                TN.Tag = TE;
                Root.Nodes.Add(TN);
                TE = new TOCEntry(S, BaseOffset);
            }
        }

        private void ReadTOC(Stream S, TreeNode Root)
        {
            long BaseOffset = S.Position;
            TOCEntry TE = new TOCEntry(S, BaseOffset);
            while (TE.Type == 0x8000)
            {
                long Pos = S.Position;
                S.Seek(BaseOffset + TE.Offset, SeekOrigin.Begin);
                this.ReadSubTOC(S, Root);
                S.Seek(Pos, SeekOrigin.Begin);
                TE = new TOCEntry(S, BaseOffset);
            }
        }

        //private void ExportFile(ListViewItem LVI, string directory)
        //{
        //    string SavedStatus = this.sbrStatus.Text;
        //    TOCEntry TE = LVI.Tag as TOCEntry;
        //    string FileName = Path.Combine(directory, TE.Name + ".png");
        //    this.sbrStatus.Text = "Exporting: " + FileName;
        //    FileStream OutputFile = new FileStream(FileName, FileMode.Create, FileAccess.Write);
        //    TE.Source.Seek(TE.Offset, SeekOrigin.Begin);
        //    BinaryReader BR = new BinaryReader(TE.Source);
        //    BinaryWriter BW = new BinaryWriter(OutputFile);
        //    BW.Write(BR.ReadBytes(TE.Size));
        //    BW.Close();
        //    this.sbrStatus.Text = SavedStatus;
        //}

        private void ClearImage()
        {
            this.picViewer.BackgroundImage = null;
            this.picViewer.Image = null;
        }

        private void ShowImage()
        {
            if (this.tvDataFiles.SelectedNode == null)
            {
                return;
            }
            TOCEntry TE = this.tvDataFiles.SelectedNode.Tag as TOCEntry;
            if (TE == null)
            {
                return;
            }
            TE.Source.Seek(TE.Offset, SeekOrigin.Begin);
            try
            {
                Image I = null;
                {
                    // Create buffer and make the image from that - Image.FromStream(this.CurrentFile) does NOT work
                    BinaryReader BR = new BinaryReader(TE.Source);
                    byte[] ImageData = BR.ReadBytes(TE.Size);
                    MemoryStream MS = new MemoryStream(ImageData, false);
                    I = Image.FromStream(MS);
                    MS.Close();
                }
                if (this.mnuTiledImage.Checked)
                {
                    this.picViewer.BackgroundImage = I;
                }
                else
                {
                    this.picViewer.Image = I;
                }
                this.picViewer.SizeMode = (this.mnuStretchImage.Checked
                    ? PictureBoxSizeMode.StretchImage
                    : PictureBoxSizeMode.CenterImage);
                int bitdepth = 0;
                switch (I.PixelFormat)
                {
                case PixelFormat.Format1bppIndexed:
                    bitdepth = 1;
                    break;
                case PixelFormat.Format4bppIndexed:
                    bitdepth = 4;
                    break;
                case PixelFormat.Format8bppIndexed:
                    bitdepth = 8;
                    break;
                case PixelFormat.Format16bppGrayScale:
                case PixelFormat.Format16bppArgb1555:
                case PixelFormat.Format16bppRgb555:
                case PixelFormat.Format16bppRgb565:
                    bitdepth = 16;
                    break;
                case PixelFormat.Format24bppRgb:
                    bitdepth = 24;
                    break;
                case PixelFormat.Format32bppArgb:
                case PixelFormat.Format32bppPArgb:
                case PixelFormat.Format32bppRgb:
                    bitdepth = 32;
                    break;
                case PixelFormat.Format48bppRgb:
                    bitdepth = 48;
                    break;
                case PixelFormat.Format64bppArgb:
                case PixelFormat.Format64bppPArgb:
                    bitdepth = 64;
                    break;
                }
                //ImageConverter IC = new ImageConverter();
                this.sbrStatus.Text = String.Format("PNG Image - {0}x{1} {2}bpp", I.Width, I.Height, bitdepth);
            }
            catch {}
        }

        private void tvDataFiles_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
        {
            this.sbrStatus.Text = "";
            this.mnuExport.Visible = false;
            this.mnuExportAll.Visible = false;
            this.ClearImage();
            if (e.Node == null)
            {
                return;
            }
            if (e.Node.Parent != null && e.Node.Parent.Parent != null && e.Node.Nodes.Count == 0)
            {
                this.mnuExport.Visible = true;
            }
            if (e.Node.Parent != null && e.Node.Parent.Parent == null && e.Node.Nodes.Count != 0)
            {
                this.mnuExportAll.Visible = true;
            }
            this.ShowImage();
        }

        private void ImageOption_Click(object sender, System.EventArgs e)
        {
            this.ClearImage();
            foreach (MenuItem MI in this.mnuPictureContext.MenuItems)
            {
                MI.Checked = (MI == sender);
            }
            this.ShowImage();
        }
    }
}
