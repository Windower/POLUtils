// Copyright © 2004-2014 Tim Van Holder, Windower Team
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS"
// BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using System;
using System.IO;
using System.Threading;
using System.Text;
using System.Windows.Forms;
using PlayOnline.Core;
using PlayOnline.FFXI;

namespace PlayOnline.FFXI.Utils.DataBrowser
{
    internal class FileScanner
    {
        private FileScanDialog FSD = new FileScanDialog();

        public ThingList FileContents = null;

        public void ScanFile(IWin32Window ParentForm, string FileName)
        {
            lock (this.FSD)
            {
                if (FileName != null && File.Exists(FileName))
                {
                    this.FSD = new FileScanDialog();
                    Thread T = new Thread(new ThreadStart(delegate()
                    {
                        try
                        {
                            Application.DoEvents();
                            while (!this.FSD.Visible)
                            {
                                Thread.Sleep(0);
                                Application.DoEvents();
                            }
                            this.FSD.Invoke(new AnonymousMethod(delegate() { this.FSD.ResetProgress(); }));
                            this.FileContents = FileType.LoadAll(FileName,
                                new FileType.ProgressCallback(delegate(string Message, double PercentCompleted)
                                {
                                    this.FSD.Invoke(new AnonymousMethod(delegate()
                                    {
                                        this.FSD.SetProgress(Message, PercentCompleted);
                                    }));
                                }));
                        }
                        catch
                        {
                            this.FileContents = null;
                        }
                        finally
                        {
                            this.FSD.Invoke(new AnonymousMethod(delegate() { this.FSD.Finish(); }));
                        }
                    }));
                    T.CurrentUICulture = Thread.CurrentThread.CurrentUICulture;
                    T.Start();
                    if (this.FSD.ShowDialog(ParentForm) == DialogResult.Abort)
                    {
                        this.FSD.Finish();
                        this.FileContents = null;
                    }
                    if (T.IsAlive)
                    {
                        T.Abort();
                    }
                    this.FSD.Dispose();
                    this.FSD = null;
                }
            }
        }
    }
}
