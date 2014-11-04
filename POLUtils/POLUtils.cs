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
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;
using PlayOnline.Core;

namespace POLUtils
{
    internal class CultureChoice
    {
        public string Name
        {
            get
            {
                if (this.Culture_.LCID == CultureInfo.InvariantCulture.LCID)
                {
                    return "Default";
                }
                return String.Format("[{0}] {1}", this.Culture_.Name, this.Culture_.NativeName);
            }
        }

        private CultureInfo Culture_;

        public CultureInfo Culture
        {
            get { return this.Culture_; }
        }

        public CultureChoice(CultureInfo Culture) { this.Culture_ = Culture; }

        private static CultureChoice Current_ = null;

        public static CultureChoice Current
        {
            get { return CultureChoice.Current_; }
            set
            {
                CultureChoice.Current_ = value;
                Thread.CurrentThread.CurrentUICulture = value.Culture;
                using (RegistryKey SettingsKey = POL.OpenPOLUtilsConfigKey())
                {
                    if (SettingsKey != null)
                    {
                        SettingsKey.SetValue("UI Culture", value.Culture.Name);
                    }
                }
            }
        }
    }

    public class POLUtils
    {
        internal static bool KeepGoing;

        internal static ArrayList AvailableCultures;

        private static void KaBOOM(object sender, UnhandledExceptionEventArgs args)
        {
            if (args.IsTerminating)
            {
                MessageBox.Show(
                    "POLUtils has encountered an exception and needs to close.\nPlease report this to Pebbles:\n\n" +
                    args.ExceptionObject.ToString(), "Oops", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else if (!(args.ExceptionObject is ThreadAbortException))
            {
                MessageBox.Show(
                    "POLUtils has encountered an exception.\nPlease report this to Pebbles:\n\n" + args.ExceptionObject.ToString(),
                    "Oops", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        [STAThread]
        public static int Main(string[] Arguments)
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(POLUtils.KaBOOM);
            Application.EnableVisualStyles();
            if (POL.AvailableRegions == POL.Region.None)
            {
                MessageBox.Show(I18N.GetText("Text:NoPOL"), I18N.GetText("Caption:NoPOL"), MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
            {
                POLUtils.AvailableCultures = new ArrayList();
                string LastCulture = String.Empty;
                using (RegistryKey SettingsKey = POL.OpenPOLUtilsConfigKey())
                {
                    if (SettingsKey != null)
                    {
                        LastCulture = SettingsKey.GetValue("UI Culture", null) as string;
                        if (LastCulture == null)
                        {
                            LastCulture = String.Empty;
                        }
                    }
                }
                // Detect Available Languages
                POLUtils.AvailableCultures.Add(new CultureChoice(CultureInfo.InvariantCulture));
                foreach (CultureInfo CI in CultureInfo.GetCultures(CultureTypes.AllCultures))
                {
                    if (CI.Name != String.Empty && Directory.Exists(Path.Combine(Application.StartupPath, CI.Name)))
                    {
                        CultureChoice CC = new CultureChoice(CI);
                        POLUtils.AvailableCultures.Add(CC);
                        if (LastCulture != String.Empty && LastCulture == CC.Culture.Name)
                        {
                            CultureChoice.Current = CC;
                        }
                    }
                }
                if (CultureChoice.Current == null) // if none configured, default to invariant culture
                {
                    CultureChoice.Current = POLUtils.AvailableCultures[0] as CultureChoice;
                }
                // The loop is for the benefit of language change
                POLUtils.KeepGoing = true;
                while (POLUtils.KeepGoing)
                {
                    POLUtils.KeepGoing = false;
                    Application.Run(new POLUtilsUI());
                }
            }
            return 0;
        }
    }
}
