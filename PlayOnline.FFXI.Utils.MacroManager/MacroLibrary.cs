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
using System.IO;
using System.Text;
using System.Xml;

namespace PlayOnline.FFXI
{
    public class MacroLibrary : SpecialMacroFolder
    {
        private string FileName_;

        public override bool CanSave
        {
            get { return (this.FileName_ != null); }
        }

        private MacroLibrary()
            : base("Macro Library") {}

        public static MacroFolder Load()
        {
            MacroLibrary ML = new MacroLibrary();
            {
                string SettingsDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    Path.Combine("Pebbles", "POLUtils"));
                if (!Directory.Exists(SettingsDir))
                {
                    Directory.CreateDirectory(SettingsDir);
                }
                ML.FileName_ = Path.Combine(SettingsDir, "macro-library.xml");
            }
            if (File.Exists(ML.FileName_))
            {
                XmlDocument XD = new XmlDocument();
                XD.Load(ML.FileName_);
                MacroLibrary.LoadFromXml(ML, XD.DocumentElement);
            }
            return ML;
        }

        private static void LoadFromXml(MacroFolder MF, XmlElement FolderNode)
        {
            if (FolderNode.Attributes["name"] != null)
            {
                MF.Name = FolderNode.Attributes["name"].InnerText;
            }
            {
                // Load contained macros
                XmlNodeList Macros = FolderNode.SelectNodes("macro");
                foreach (XmlNode MacroNode in Macros)
                {
                    if (MacroNode is XmlElement)
                    {
                        MF.Macros.Add(Macro.LoadFromXml(MacroNode as XmlElement));
                    }
                }
            }
            {
                // Load contained folders
                XmlNodeList SubFolders = FolderNode.SelectNodes("folder");
                foreach (XmlNode SubFolderNode in SubFolders)
                {
                    if (SubFolderNode is XmlElement)
                    {
                        MacroFolder SubFolder = new MacroFolder();
                        MacroLibrary.LoadFromXml(SubFolder, SubFolderNode as XmlElement);
                        MF.Folders.Add(SubFolder);
                    }
                }
            }
        }

        public override bool Save()
        {
            try
            {
                XmlDocument XDoc = new XmlDocument();
                XDoc.PreserveWhitespace = true;
                XDoc.AppendChild(XDoc.CreateXmlDeclaration("1.0", "utf-8", null));
                XDoc.AppendChild(XDoc.CreateComment("NOTE: Editing this file by hand is NOT recommended"));
                MacroLibrary.WriteToXml(this, XDoc, XDoc);
                XmlTextWriter XW = new XmlTextWriter(this.FileName_, Encoding.UTF8);
                XW.Formatting = Formatting.Indented;
                XDoc.WriteTo(XW);
                XW.Close();
                return true;
            }
            catch (Exception E)
            {
                Console.WriteLine(E.ToString());
            }
            return false;
        }

        private static void WriteToXml(MacroFolder MF, XmlDocument XDoc, XmlNode Parent)
        {
            XmlElement XFolder = XDoc.CreateElement("folder");
            if (MF.Name != null && MF.Name != String.Empty)
            {
                XmlAttribute XName = XDoc.CreateAttribute("name");
                XName.InnerText = MF.Name;
                XFolder.Attributes.Append(XName);
            }
            foreach (MacroFolder SubFolder in MF.Folders)
            {
                MacroLibrary.WriteToXml(SubFolder, XDoc, XFolder);
            }
            foreach (Macro M in MF.Macros)
            {
                M.WriteToXml(XDoc, XFolder);
            }
            Parent.AppendChild(XFolder);
        }
    }
}
