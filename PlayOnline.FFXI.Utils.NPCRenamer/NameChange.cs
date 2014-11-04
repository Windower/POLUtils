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
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using PlayOnline.Core;

namespace PlayOnline.FFXI.Utils.NPCRenamer
{
    internal class NameChange
    {
        private uint ID_;
        private string Old_;
        private string New_;

        public uint ID
        {
            get { return this.ID_; }
        }

        public string Old
        {
            get { return this.Old_; }
        }

        public string New
        {
            get { return this.New_; }
        }

        public string Area
        {
            get { return FFXIResourceManager.GetAreaName((ushort)((this.ID_ >> 12) & 0xff)); }
        }

        private NameChange(uint ID, string Old, string New)
        {
            this.ID_ = ID;
            this.Old_ = Old;
            this.New_ = New;
        }

        public static void Add(uint ID, string Old, string New)
        {
            NameChange NC = new NameChange(ID, Old, New);
            foreach (NameChange PNC in NameChange.Pending)
            {
                if (PNC.ID == NC.ID)
                {
                    NC.Old_ = PNC.Old_;
                    NameChange.Pending.Remove(PNC);
                    break;
                }
            }
            if (NC.Old_ == NC.New_)
            {
                return; // No change -> no need to add a change
            }
            NameChange.Pending.Add(NC);
        }

        // The 2 Global Lists

        public static List<NameChange> Applied = new List<NameChange>();
        public static List<NameChange> Pending = new List<NameChange>();

        public static void Apply(NameChange NC)
        {
            if (!NameChange.Pending.Contains(NC))
            {
                return;
            }
            {
                string DATFileName = FFXI.GetFilePath(6720 + (ushort)((NC.ID_ >> 12) & 0xff));
                if (DATFileName != null)
                {
                    FileStream DATFile = new FileStream(DATFileName, FileMode.Open, FileAccess.ReadWrite);
                    BinaryReader BR = new BinaryReader(DATFile, Encoding.ASCII);
                    bool Found = false;
                    while (DATFile.Position < DATFile.Length)
                    {
                        string Name = new string(BR.ReadChars(0x1C)).TrimEnd('\0');
                        uint ID = BR.ReadUInt32();
                        if (ID == NC.ID_)
                        {
                            Found = true;
                            if (Name != NC.Old_)
                            {
                                if (
                                    MessageBox.Show(String.Format(I18N.GetText("Message:OldNameMismatch"), NC.Old_, NC.New_, Name),
                                        I18N.GetText("Title:OldNameMismatch"), MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                                        MessageBoxDefaultButton.Button2) == DialogResult.No)
                                {
                                    return;
                                }
                            }
                            break;
                        }
                    }
                    if (!Found)
                    {
                        MessageBox.Show(
                            String.Format(I18N.GetText("Message:EntryNotFound"), NC.ID_, NC.Old_, NC.New_, DATFileName),
                            I18N.GetText("Title:EntryNotFound"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    string NewName = NC.New_.PadRight(0x1C, '\0');
                    DATFile.Position -= 0x20;
                    DATFile.Write(Encoding.ASCII.GetBytes(NewName), 0, 0x1C);
                    DATFile.Close();
                }
            }
            NameChange.Pending.Remove(NC);
            foreach (NameChange ANC in NameChange.Applied)
            {
                if (ANC.ID == NC.ID)
                {
                    if (ANC.Old_ == NC.New_) // Reverting existing applied change
                    {
                        NameChange.Applied.Remove(ANC);
                    }
                    else
                    {
                        ANC.New_ = NC.New_;
                    }
                    return;
                }
            }
            NameChange.Applied.Add(NC);
        }

        public static void Revert(NameChange NC) { NameChange.Add(NC.ID, NC.New_, NC.Old_); }

        // XML Stuff

        public static XmlDocument CreateChangeset()
        {
            XmlDocument Changeset = new XmlDocument();
            Changeset.AppendChild(Changeset.CreateProcessingInstruction("xml", "version=\"1.0\" encoding=\"utf-8\""));
            Changeset.AppendChild(Changeset.CreateElement("ffxi-npc-name-changeset"));
            return Changeset;
        }

        public void AddToChangeset(XmlDocument Changeset)
        {
            XmlElement XThis = Changeset.CreateElement("name-change");
            XThis.Attributes.Append(Changeset.CreateAttribute("id"));
            XThis.Attributes.Append(Changeset.CreateAttribute("old"));
            XThis.Attributes.Append(Changeset.CreateAttribute("new"));
            XThis.Attributes["id"].InnerText = XmlConvert.ToString(this.ID_);
            XThis.Attributes["old"].InnerText = this.Old_;
            XThis.Attributes["new"].InnerText = this.New_;
            Changeset.DocumentElement.AppendChild(XThis);
        }

        public static void LoadChangeset(string FileName, bool RevertChanges)
        {
            XmlDocument Changeset = new XmlDocument();
            try
            {
                Changeset.Load(FileName);
            }
            catch (Exception E)
            {
                MessageBox.Show(String.Format(I18N.GetText("Message:ChangesetLoadError"), E.Message),
                    I18N.GetText("Title:ChangesetLoadError"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            foreach (XmlNode XN in Changeset.DocumentElement.ChildNodes)
            {
                XmlElement XNameChange = XN as XmlElement;
                if (XNameChange != null && XNameChange.Name == "name-change" && XNameChange.HasAttribute("id") &&
                    XNameChange.HasAttribute("old") && XNameChange.HasAttribute("new"))
                {
                    uint ID = XmlConvert.ToUInt32(XNameChange.Attributes["id"].InnerText);
                    string Old = XNameChange.Attributes["old"].InnerText;
                    string New = XNameChange.Attributes["new"].InnerText;
                    if (RevertChanges)
                    {
                        NameChange.Add(ID, New, Old);
                    }
                    else
                    {
                        NameChange.Add(ID, Old, New);
                    }
                }
            }
        }

        public static void LoadChangeset(string FileName) { NameChange.LoadChangeset(FileName, false); }

        // The history file

        private static string HistoryFileName =
            Path.Combine(
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    Path.Combine("Pebbles", "POLUtils")), "npc-name-change-history.xml");

        public static void LoadHistory()
        {
            NameChange.Applied.Clear();
            if (File.Exists(NameChange.HistoryFileName))
            {
                try
                {
                    XmlDocument History = new XmlDocument();
                    History.Load(NameChange.HistoryFileName);
                    foreach (XmlNode XN in History.DocumentElement.ChildNodes)
                    {
                        XmlElement XNameChange = XN as XmlElement;
                        if (XNameChange != null && XNameChange.Name == "name-change" && XNameChange.HasAttribute("id") &&
                            XNameChange.HasAttribute("old") && XNameChange.HasAttribute("new"))
                        {
                            uint ID = XmlConvert.ToUInt32(XNameChange.Attributes["id"].InnerText);
                            string Old = XNameChange.Attributes["old"].InnerText;
                            string New = XNameChange.Attributes["new"].InnerText;
                            NameChange.Applied.Add(new NameChange(ID, Old, New));
                        }
                    }
                }
                catch (Exception E)
                {
                    MessageBox.Show(String.Format(I18N.GetText("Message:HistoryLoadError"), E.Message),
                        I18N.GetText("Title:HistoryLoadError"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public static void SaveHistory()
        {
            try
            {
                XmlDocument History = NameChange.CreateChangeset();
                foreach (NameChange NC in NameChange.Applied)
                {
                    NC.AddToChangeset(History);
                }
                string HistoryFolder = Path.GetDirectoryName(NameChange.HistoryFileName);
                if (!Directory.Exists(HistoryFolder))
                {
                    Directory.CreateDirectory(HistoryFolder);
                }
                History.Save(NameChange.HistoryFileName);
            }
            catch (Exception E)
            {
                MessageBox.Show(String.Format(I18N.GetText("Message:HistorySaveError"), E.Message),
                    I18N.GetText("Title:HistorySaveError"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
