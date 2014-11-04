// Copyright © 2004-2014 Tim Van Holder, Nevin Stepan, Windower Team
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS"
// BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using System;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;

namespace PlayOnline.Core
{
    // This probably should have a more fitting location.
    public delegate void AnonymousMethod();

    public struct AppID
    {
        public const string POLViewer = "1000";
        public const string FFXI = "0001";
        public const string TetraMaster = "0002";
        public const string FFXITC = "0015";
        // FrontMission Online = ?
        // That Mahjongg thing = ?
    }

    public class POL
    {
        [Flags]
        public enum Region
        {
            None = 0x00,
            Japan = 0x01,
            NorthAmerica = 0x02,
            Europe = 0x04,
        }

        private static Region AvailableRegions_ = Region.None;
        private static Region SelectedRegion_ = Region.None;

        public static void DetectRegions()
        {
            // Get configured region
            using (RegistryKey SettingsKey = POL.OpenPOLUtilsConfigKey(true))
            {
                if (SettingsKey != null)
                {
                    string UserRegion = SettingsKey.GetValue("Region", "None") as string;
                    try
                    {
                        POL.SelectedRegion_ = (Region)Enum.Parse(typeof (Region), UserRegion, true);
                    }
                    catch
                    {
                        POL.SelectedRegion_ = Region.None;
                    }
                }
            }
            // Check for installed POL software
            foreach (Region R in Enum.GetValues(typeof (Region)))
            {
                using (RegistryKey POLKey = POL.OpenRegistryKey(R, "InstallFolder"))
                {
                    if (POLKey != null)
                    {
                        POL.AvailableRegions_ |= R;
                    }
                }
            }
            // If user's choice is not available, clear that selection
            if ((POL.AvailableRegions_ & POL.SelectedRegion_) != POL.SelectedRegion_)
            {
                POL.SelectedRegion_ = Region.None;
            }
            // Select a region based on what's available
            if (POL.SelectedRegion_ == Region.None)
            {
                if ((POL.AvailableRegions_ & Region.NorthAmerica) != 0)
                {
                    POL.SelectedRegion_ = Region.NorthAmerica;
                }
                else if ((POL.AvailableRegions_ & Region.Europe) != 0)
                {
                    POL.SelectedRegion_ = Region.Europe;
                }
                else if ((POL.AvailableRegions_ & Region.Japan) != 0)
                {
                    POL.SelectedRegion_ = Region.Japan;
                }
            }
        }

        public static Region AvailableRegions
        {
            get
            {
                POL.DetectRegions();
                return POL.AvailableRegions_;
            }
        }

        public static Region SelectedRegion
        {
            get { return POL.SelectedRegion_; }
            set
            {
                if ((POL.AvailableRegions & value) == value)
                {
                    POL.SelectedRegion_ = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("SelectedRegion", value, I18N.GetText("POLRegionNotInstalled"));
                }
            }
        }

        public static bool MultipleRegionsAvailable
        {
            get
            {
                return (POL.AvailableRegions_ != Region.None && POL.AvailableRegions_ != Region.Japan &&
                        POL.AvailableRegions_ != Region.NorthAmerica && POL.AvailableRegions_ != Region.Europe);
            }
        }

        public static void ChooseRegion(Form Parent)
        {
            POL.DetectRegions();
            if (POL.MultipleRegionsAvailable)
            {
                using (ChooseRegionDialog CRD = new ChooseRegionDialog())
                {
                    CRD.ShowDialog(Parent);
                }
            }
            else // No multiple regions installed? No choice to be made then!
            {
                POL.SelectedRegion_ = POL.AvailableRegions_;
            }
            using (RegistryKey POLKey = POL.OpenPOLUtilsConfigKey(true))
            {
                if (POLKey != null)
                {
                    POLKey.SetValue("Region", POL.SelectedRegion_.ToString());
                }
            }
        }

        public static string GetApplicationPath(string ID) { return POL.GetApplicationPath(ID, POL.SelectedRegion_); }

        public static string GetApplicationPath(string ID, Region Region)
        {
            if ((POL.AvailableRegions & Region) == 0)
            {
                return null;
            }
            RegistryKey POLKey = POL.OpenRegistryKey(Region, "InstallFolder");
            if (POLKey == null)
            {
                return null;
            }
            string InstallPath = POLKey.GetValue(ID, null) as string;
            POLKey.Close();
            return InstallPath;
        }

        public static bool IsAppInstalled(string ID) { return POL.IsAppInstalled(ID, POL.SelectedRegion_); }

        public static bool IsAppInstalled(string ID, Region Region) { return (POL.GetApplicationPath(ID, Region) != null); }

        public static RegistryKey OpenAppConfigKey(string ID) { return POL.OpenAppConfigKey(ID, POL.SelectedRegion_); }

        public static RegistryKey OpenAppConfigKey(string ID, Region Region)
        {
            string BaseKey;
            switch (Region)
            {
            case Region.Europe:
                BaseKey = "SquareEnix";
                break;
            case Region.Japan:
                BaseKey = "SQUARE";
                break;
            case Region.NorthAmerica:
                BaseKey = "SquareEnix";
                break;
            default:
                return null;
            }
            string AppKey;
            if (ID == AppID.FFXI)
            {
                AppKey = "FinalFantasyXI";
            }
            else if (ID == AppID.TetraMaster)
            {
                AppKey = "TetraMaster";
            }
            else if (ID == AppID.POLViewer)
            {
                AppKey = "PlayOnlineViewer";
            }
            else
            {
                return null;
            }
            return POL.OpenRegistryKey(Region, Path.Combine(BaseKey, AppKey), true);
        }

        private static RegistryKey OpenRegistryKey(string KeyName)
        {
            return POL.OpenRegistryKey(POL.SelectedRegion_, KeyName, false);
        }

        private static RegistryKey OpenRegistryKey(string KeyName, bool Writable)
        {
            return POL.OpenRegistryKey(POL.SelectedRegion_, KeyName, Writable);
        }

        private static RegistryKey OpenRegistryKey(Region Region, string KeyName)
        {
            return POL.OpenRegistryKey(Region, KeyName, false);
        }

        private static RegistryKey OpenRegistryKey(Region Region, string KeyName, bool Writable)
        {
            string SubKey;
            {
                string POLKey = "PlayOnline";
                switch (Region)
                {
                case Region.Europe:
                    POLKey += "EU";
                    break;
                case Region.Japan:
                    break;
                case Region.NorthAmerica:
                    POLKey += "US";
                    break;
                default:
                    return null;
                }
                SubKey = Path.Combine(POLKey, KeyName);
            }
            try
            {
                using (RegistryKey Win64Root = Registry.LocalMachine.OpenSubKey(@"Software\WOW6432Node"))
                {
                    if (Win64Root != null)
                    {
                        RegistryKey Win64Key = Win64Root.OpenSubKey(SubKey, Writable);
                        if (Win64Key != null)
                        {
                            return Win64Key;
                        }
                    }
                }
            }
            catch {}
            try
            {
                return Registry.LocalMachine.OpenSubKey(Path.Combine("Software", SubKey), Writable);
            }
            catch
            {
                return null;
            }
        }

        public static RegistryKey OpenPOLUtilsConfigKey() { return POL.OpenPOLUtilsConfigKey(null, false); }

        public static RegistryKey OpenPOLUtilsConfigKey(bool MachineWide) { return POL.OpenPOLUtilsConfigKey(null, MachineWide); }

        public static RegistryKey OpenPOLUtilsConfigKey(string SubKey) { return POL.OpenPOLUtilsConfigKey(SubKey, false); }

        public static RegistryKey OpenPOLUtilsConfigKey(string SubKey, bool MachineWide)
        {
            RegistryKey Root = (MachineWide ? Registry.LocalMachine : Registry.CurrentUser);
            try
            {
                string KeyName = @"Software\Pebbles\POLUtils\";
                if (SubKey != null)
                {
                    KeyName += SubKey;
                }
                return Root.CreateSubKey(KeyName);
            }
            catch {}
            return null;
        }
    }
}
