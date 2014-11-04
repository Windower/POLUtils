// Copyright © 2004-2014 Tim Van Holder, Nevin Stepan, Windower Team
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
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using PlayOnline.Core;
using PlayOnline.FFXI;

namespace PlayOnline.FFXI.Utils.EngrishOnry
{
    public partial class MainWindow : System.Windows.Forms.Form
    {
        public MainWindow()
        {
            this.InitializeComponent();
            this.Icon = Icons.POLViewer;
        }

        #region General Support Routines

        private void AddLogEntry(string Text)
        {
            this.rtbActivityLog.Text += String.Format("[{0}] {1}\n", DateTime.Now.ToString(), Text);
            Application.DoEvents();
        }

        private void LogFailure(string Action) { this.AddLogEntry(String.Format(I18N.GetText("ActionFailed"), Action)); }

        private string MakeBackupFolder(bool CreateIfNeeded)
        {
            try
            {
                string BackupFolder = Path.Combine(POL.GetApplicationPath(AppID.FFXI), "EngrishOnry-Backup");
                if (CreateIfNeeded && !Directory.Exists(BackupFolder))
                {
                    this.AddLogEntry(String.Format(I18N.GetText("CreateBackupDir"), BackupFolder));
                    Directory.CreateDirectory(BackupFolder);
                }
                return BackupFolder;
            }
            catch
            {
                this.LogFailure("MakeBackupFolder");
                return null;
            }
        }

        private string MakeBackupFolder() { return this.MakeBackupFolder(true); }

        private bool BackupFile(int FileNumber)
        {
            try
            {
                string BackupName = Path.Combine(this.MakeBackupFolder(), String.Format("{0}.dat", FileNumber));
                if (!File.Exists(BackupName))
                {
                    string OriginalName = FFXI.GetFilePath(FileNumber);
                    this.AddLogEntry(String.Format(I18N.GetText("BackingUp"), OriginalName));
                    File.Copy(OriginalName, BackupName);
                }
                return true;
            }
            catch
            {
                this.LogFailure("BackupFile");
                return false;
            }
        }

        private bool RestoreFile(int FileNumber)
        {
            try
            {
                string BackupName = Path.Combine(this.MakeBackupFolder(), String.Format("{0}.dat", FileNumber));
                if (File.Exists(BackupName))
                {
                    string OriginalName = FFXI.GetFilePath(FileNumber);
                    this.AddLogEntry(String.Format(I18N.GetText("Restoring"), OriginalName));
                    File.Copy(BackupName, OriginalName, true);
                }
                return true;
            }
            catch
            {
                this.LogFailure("RestoreFile");
                return false;
            }
        }

        private void SwapFile(int JPFileNumber, int ENFileNumber)
        {
            try
            {
                if (!this.BackupFile(JPFileNumber))
                {
                    return;
                }
                string SourceFile = FFXI.GetFilePath(ENFileNumber);
                string TargetFile = FFXI.GetFilePath(JPFileNumber);
                this.AddLogEntry(String.Format(I18N.GetText("ReplaceJPROM"), TargetFile));
                this.AddLogEntry(String.Format(I18N.GetText("SourceENROM"), SourceFile));
                File.Copy(SourceFile, TargetFile, true);
            }
            catch
            {
                this.LogFailure("SwapFile");
            }
        }

        #endregion

        #region Item Data Translation

        private void TranslateItemFile(int JPFileNumber, int ENFileNumber)
        {
            if (!this.mnuTranslateItemNames.Checked && !this.mnuTranslateItemDescriptions.Checked)
            {
                return;
            }
            if (!this.BackupFile(JPFileNumber))
            {
                return;
            }
            try
            {
                string JFileName = FFXI.GetFilePath(JPFileNumber);
                string EFileName = FFXI.GetFilePath(ENFileNumber);
                this.AddLogEntry(String.Format(I18N.GetText("TranslatingItems"), JFileName));
                this.AddLogEntry(String.Format(I18N.GetText("UsingEnglishFile"), EFileName));
                FileStream JStream = new FileStream(JFileName, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
                FileStream EStream = new FileStream(EFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                if ((JStream.Length % 0xc00) != 0)
                {
                    this.AddLogEntry(I18N.GetText("ItemFileSizeBad"));
                    goto TranslationDone;
                }
                if (JStream.Length != EStream.Length)
                {
                    this.AddLogEntry(I18N.GetText("FileSizeMismatch"));
                    goto TranslationDone;
                }
                Things.Item.Type T;
                {
                    BinaryReader BR = new BinaryReader(JStream);
                    Things.Item.DeduceType(BR, out T);
                    BR = new BinaryReader(EStream);
                    Things.Item.Type ET;
                    Things.Item.DeduceType(BR, out ET);
                    if (T != ET)
                    {
                        this.AddLogEntry(I18N.GetText("ItemTypeMismatch"));
                        goto TranslationDone;
                    }
                }
                ushort StringBase = 0;
                switch (T)
                {
                case Things.Item.Type.Armor:
                    StringBase = 36;
                    break;
                case Things.Item.Type.Item:
                    StringBase = 20;
                    break;
                case Things.Item.Type.PuppetItem:
                    StringBase = 24;
                    break;
                case Things.Item.Type.UsableItem:
                    StringBase = 16;
                    break;
                case Things.Item.Type.Weapon:
                    StringBase = 44;
                    break;
                case Things.Item.Type.Slip:
                    StringBase = 80;
                    break;
                default:
                    this.AddLogEntry(I18N.GetText("ItemTypeBad"));
                    goto TranslationDone;
                }
                long ItemCount = JStream.Length / 0xc00;
                byte[] JData = new byte[0x280];
                byte[] EData = new byte[0x280];
                MemoryStream JMemStream = new MemoryStream(JData, true);
                MemoryStream EMemStream = new MemoryStream(EData, false);
                FFXIEncoding E = new FFXIEncoding();
                for (long i = 0; i < ItemCount; ++i)
                {
                    JStream.Seek(i * 0xc00, SeekOrigin.Begin);
                    JStream.Read(JData, 0, 0x280);
                    EStream.Seek(i * 0xc00, SeekOrigin.Begin);
                    EStream.Read(EData, 0, 0x280);
                    FFXIEncryption.Rotate(JData, 5);
                    FFXIEncryption.Rotate(EData, 5);
                    // Read English or Japanese Name
                    List<byte> Name = new List<byte>();
                    {
                        MemoryStream MS = null;
                        if (this.mnuTranslateItemNames.Checked)
                        {
                            MS = EMemStream;
                        }
                        else
                        {
                            MS = JMemStream;
                        }
                        MS.Position = StringBase + 4;
                        BinaryReader BR = new BinaryReader(MS);
                        MS.Position = StringBase + 0x1c + BR.ReadUInt32();
                        while (MS.Position < 0x280)
                        {
                            int B = MS.ReadByte();
                            if (B <= 0)
                            {
                                break;
                            }
                            Name.Add((byte)B);
                        }
                        Name.Add(0);
                        while (Name.Count % 4 != 0)
                        {
                            Name.Add(0);
                        }
                    }
                    // Read English or Japanese Description
                    List<byte> Description = new List<byte>();
                    {
                        MemoryStream MS = null;
                        if (this.mnuTranslateItemDescriptions.Checked)
                        {
                            EMemStream.Position = StringBase + 4 + 8 * 4;
                            MS = EMemStream;
                        }
                        else
                        {
                            JMemStream.Position = StringBase + 4 + 8 * 1;
                            MS = JMemStream;
                        }
                        BinaryReader BR = new BinaryReader(MS);
                        MS.Position = StringBase + 0x1c + BR.ReadUInt32();
                        while (MS.Position < 0x280)
                        {
                            int B = MS.ReadByte();
                            if (B <= 0)
                            {
                                break;
                            }
                            Description.Add((byte)B);
                        }
                        Description.Add(0);
                        while (Description.Count % 4 != 0)
                        {
                            Description.Add(0);
                        }
                    }
                    {
                        // Construct a new string table
                        BinaryWriter BW = new BinaryWriter(JMemStream);
                        Array.Clear(JData, StringBase, 0x280 - StringBase);
                        JMemStream.Position = StringBase;
                        uint NameOffset = 0x14; // Right after the string table header
                        uint DescOffset = NameOffset + (uint)Name.Count + 28;
                        BW.Write((uint)2); // String Count
                        BW.Write(NameOffset); // Entry #1
                        BW.Write((uint)0);
                        BW.Write(DescOffset); // Entry #2
                        BW.Write((uint)0);
                        // String #1 - Padding + text bytes
                        BW.Write((uint)1);
                        BW.Write((ulong)0);
                        BW.Write((ulong)0);
                        BW.Write((ulong)0);
                        BW.Write(Name.ToArray());
                        // String #2 - Padding + text bytes
                        BW.Write((uint)1);
                        BW.Write((ulong)0);
                        BW.Write((ulong)0);
                        BW.Write((ulong)0);
                        BW.Write(Description.ToArray());
                        // End marker
                        BW.Write((uint)1);
                    }
                    // Update file data
                    FFXIEncryption.Rotate(JData, 3);
                    JStream.Seek(i * 0xc00, SeekOrigin.Begin);
                    JStream.Write(JData, 0, 0x280);
                }
                TranslationDone:
                JStream.Close();
                EStream.Close();
            }
            catch
            {
                this.LogFailure("TranslateItemFile");
            }
        }

        private void btnTranslateItemData_Click(object sender, System.EventArgs e)
        {
            this.TranslateItemFile(4, 73);
            this.TranslateItemFile(5, 74);
            this.TranslateItemFile(6, 75);
            this.TranslateItemFile(7, 76);
            this.TranslateItemFile(8, 77);
            this.AddLogEntry(I18N.GetText("ItemTranslateDone"));
        }

        private void btnRestoreItemData_Click(object sender, System.EventArgs e)
        {
            this.RestoreFile(4);
            this.RestoreFile(5);
            this.RestoreFile(6);
            this.RestoreFile(7);
            this.RestoreFile(8);
            this.AddLogEntry(I18N.GetText("ItemRestoreDone"));
        }

        private void btnConfigItemData_Click(object sender, System.EventArgs e)
        {
            this.mnuConfigItemData.Show(this.btnConfigItemData, new Point(0, this.btnConfigItemData.Height));
        }

        private void mnuTranslateItemNames_Click(object sender, System.EventArgs e)
        {
            this.mnuTranslateItemNames.Checked = !this.mnuTranslateItemNames.Checked;
        }

        private void mnuTranslateItemDescriptions_Click(object sender, System.EventArgs e)
        {
            this.mnuTranslateItemDescriptions.Checked = !this.mnuTranslateItemDescriptions.Checked;
        }

        #endregion

        #region Spell Data Translation

        private void TranslateSpellFile(int FileNumber)
        {
            if (!this.mnuTranslateSpellNames.Checked && !this.mnuTranslateSpellDescriptions.Checked)
            {
                return;
            }
            if (!this.BackupFile(FileNumber))
            {
                return;
            }
            try
            {
                string FileName = FFXI.GetFilePath(FileNumber);
                this.AddLogEntry(String.Format(I18N.GetText("TranslatingSpells"), FileName));
                FileStream SpellStream = new FileStream(FileName, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
                if ((SpellStream.Length % 0x400) != 0)
                {
                    this.AddLogEntry(I18N.GetText("SpellFileSizeBad"));
                    goto TranslationDone;
                }
                long SpellCount = SpellStream.Length / 0x400;
                byte[] TextBlock = new byte[0x128];
                for (long i = 0; i < SpellCount; ++i)
                {
                    SpellStream.Seek(i * 0x400 + 0x29, SeekOrigin.Begin);
                    SpellStream.Read(TextBlock, 0, 0x128);
                    if (this.mnuTranslateSpellNames.Checked)
                    {
                        Array.Copy(TextBlock, 0x14, TextBlock, 0x00, 0x14); // Copy english name
                    }
                    if (this.mnuTranslateSpellDescriptions.Checked)
                    {
                        Array.Copy(TextBlock, 0xa8, TextBlock, 0x28, 0x80); // Copy english description
                    }
                    SpellStream.Seek(i * 0x400 + 0x29, SeekOrigin.Begin);
                    SpellStream.Write(TextBlock, 0, 0x128);
                }
                TranslationDone:
                SpellStream.Close();
            }
            catch
            {
                this.LogFailure("TranslateSpellFile");
            }
        }

        private void btnTranslateSpellData_Click(object sender, System.EventArgs e)
        {
            this.TranslateSpellFile(11);
            this.TranslateSpellFile(86);
            this.AddLogEntry(I18N.GetText("SpellTranslateDone"));
        }

        private void btnRestoreSpellData_Click(object sender, System.EventArgs e)
        {
            this.RestoreFile(11);
            this.RestoreFile(86);
            this.AddLogEntry(I18N.GetText("SpellRestoreDone"));
        }

        private void btnConfigSpellData_Click(object sender, System.EventArgs e)
        {
            this.mnuConfigSpellData.Show(this.btnConfigSpellData, new Point(0, this.btnConfigSpellData.Height));
        }

        private void mnuTranslateSpellNames_Click(object sender, System.EventArgs e)
        {
            this.mnuTranslateSpellNames.Checked = !this.mnuTranslateSpellNames.Checked;
        }

        private void mnuTranslateSpellDescriptions_Click(object sender, System.EventArgs e)
        {
            this.mnuTranslateSpellDescriptions.Checked = !this.mnuTranslateSpellDescriptions.Checked;
        }

        #endregion

        #region Ability Data Translation

        private void TranslateAbilityFile(int JPFileNumber, int ENFileNumber)
        {
            if (!this.mnuTranslateAbilityNames.Checked && !this.mnuTranslateAbilityDescriptions.Checked)
            {
                return;
            }
            if (!this.BackupFile(JPFileNumber))
            {
                return;
            }
            try
            {
                string JFileName = FFXI.GetFilePath(JPFileNumber);
                string EFileName = FFXI.GetFilePath(ENFileNumber);
                this.AddLogEntry(String.Format(I18N.GetText("TranslatingAbilities"), JFileName));
                this.AddLogEntry(String.Format(I18N.GetText("UsingEnglishFile"), EFileName));
                FileStream JFileStream = new FileStream(JFileName, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
                FileStream EFileStream = new FileStream(EFileName, FileMode.Open, FileAccess.Read);
                if ((JFileStream.Length % 0x400) != 0 || (EFileStream.Length % 0x400) != 0)
                {
                    this.AddLogEntry(I18N.GetText("AbilityFileSizeBad"));
                    goto TranslationDone;
                }
                if (JFileStream.Length != EFileStream.Length)
                {
                    this.AddLogEntry(I18N.GetText("FileSizeMismatch"));
                    goto TranslationDone;
                }
                long AbilityCount = JFileStream.Length / 0x400;
                byte[] JTextBlock = new byte[0x120];
                byte[] ETextBlock = new byte[0x120];
                for (long i = 0; i < AbilityCount; ++i)
                {
                    JFileStream.Seek(i * 0x400 + 0xa, SeekOrigin.Begin);
                    JFileStream.Read(JTextBlock, 0, 0x120);
                    EFileStream.Seek(i * 0x400 + 0xa, SeekOrigin.Begin);
                    EFileStream.Read(ETextBlock, 0, 0x120);
                    if (this.mnuTranslateAbilityNames.Checked)
                    {
                        Array.Copy(ETextBlock, 0x00, JTextBlock, 0x00, 0x020);
                    }
                    if (this.mnuTranslateAbilityDescriptions.Checked)
                    {
                        Array.Copy(ETextBlock, 0x20, JTextBlock, 0x20, 0x100);
                    }
                    JFileStream.Seek(i * 0x400 + 0xa, SeekOrigin.Begin);
                    JFileStream.Write(JTextBlock, 0, 0x120);
                }
                TranslationDone:
                JFileStream.Close();
                EFileStream.Close();
            }
            catch
            {
                this.LogFailure("TranslateAbilityFile");
            }
        }

        private void btnTranslateAbilities_Click(object sender, System.EventArgs e)
        {
            this.TranslateAbilityFile(10, 85);
            this.AddLogEntry(I18N.GetText("AbilityTranslateDone"));
        }

        private void btnRestoreAbilities_Click(object sender, System.EventArgs e)
        {
            this.RestoreFile(10);
            this.AddLogEntry(I18N.GetText("AbilityRestoreDone"));
        }

        private void btnConfigAbilities_Click(object sender, System.EventArgs e)
        {
            this.mnuConfigAbilities.Show(this.btnConfigAbilities, new Point(0, this.btnConfigAbilities.Height));
        }

        #endregion

        #region Auto-Translator Translation

        private void TranslateAutoTranslatorFile(int JPFileNumber, int ENFileNumber)
        {
            if (!this.BackupFile(JPFileNumber))
            {
                return;
            }
            try
            {
                string JFileName = FFXI.GetFilePath(JPFileNumber);
                string EFileName = FFXI.GetFilePath(ENFileNumber);
                this.AddLogEntry(String.Format(I18N.GetText("TranslatingAutoTrans"), JFileName));
                this.AddLogEntry(String.Format(I18N.GetText("UsingEnglishFile"), EFileName));
                FileStream ATStream = new FileStream(JFileName, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
                MemoryStream NewStream = new MemoryStream();
                BinaryReader JBR = new BinaryReader(ATStream);
                BinaryWriter JBW = new BinaryWriter(NewStream);
                BinaryReader EBR = new BinaryReader(new FileStream(EFileName, FileMode.Open, FileAccess.Read));
                FFXIEncoding E = new FFXIEncoding();
                while (ATStream.Position != ATStream.Length)
                {
                    {
                        // Validate & Copy ID
                        uint JID = JBR.ReadUInt32();
                        uint EID = EBR.ReadUInt32();
                        if ((JID & 0xffff) != 0x102)
                        {
                            this.AddLogEntry(String.Format(I18N.GetText("ATBadJPID"), JID));
                            goto TranslationDone;
                        }
                        if ((EID & 0xffff) != 0x202)
                        {
                            this.AddLogEntry(String.Format(I18N.GetText("ATBadENID"), EID));
                            goto TranslationDone;
                        }
                        if ((EID & 0xffff0000) != (JID & 0xffff0000))
                        {
                            this.AddLogEntry(String.Format(I18N.GetText("ATIDMismatch"), JID, EID));
                            goto TranslationDone;
                        }
                        JBW.Write(JID);
                    }
                    // Group name -> use English
                    JBW.Write(EBR.ReadBytes(32));
                    JBR.BaseStream.Position += 32;
                    // Completion Text -> based on config
                    if (this.mnuPreserveJapaneseATCompletion.Checked)
                    {
                        JBW.Write(JBR.ReadBytes(32));
                        EBR.BaseStream.Position += 32;
                    }
                    else
                    {
                        byte[] EnglishCompletion = E.GetBytes(E.GetString(EBR.ReadBytes(32)).ToLowerInvariant());
                        // we want it lowercase
                        if (EnglishCompletion.Length != 32)
                        {
                            this.AddLogEntry(String.Format(I18N.GetText("ATLowercaseProblem"), EnglishCompletion.Length));
                            goto TranslationDone;
                        }
                        JBW.Write(EnglishCompletion);
                        JBR.BaseStream.Position += 32;
                    }
                    uint JEntryCount = JBR.ReadUInt32();
                    uint EEntryCount = EBR.ReadUInt32();
                    if (JEntryCount != EEntryCount)
                    {
                        this.AddLogEntry(String.Format(I18N.GetText("ATCountMismatch"), JEntryCount, EEntryCount));
                        goto TranslationDone;
                    }
                    JBW.Write(JEntryCount);
                    long EntryBytesPos = JBW.BaseStream.Position;
                    JBW.Write((uint)0);
                    JBR.BaseStream.Position += 4;
                    EBR.BaseStream.Position += 4;
                    for (uint i = 0; i < JEntryCount; ++i)
                    {
                        // Validate & Copy ID
                        uint JID = JBR.ReadUInt32();
                        uint EID = EBR.ReadUInt32();
                        if ((JID & 0xffff) != 0x102)
                        {
                            this.AddLogEntry(String.Format(I18N.GetText("ATBadJPID"), JID));
                            goto TranslationDone;
                        }
                        if ((EID & 0xffff) != 0x202)
                        {
                            this.AddLogEntry(String.Format(I18N.GetText("ATBadENID"), EID));
                            goto TranslationDone;
                        }
                        if ((EID & 0xffff0000) != (JID & 0xffff0000))
                        {
                            this.AddLogEntry(String.Format(I18N.GetText("ATIDMismatch"), JID, EID));
                            goto TranslationDone;
                        }
                        JBW.Write(JID);
                        // Display text -> use English
                        byte[] EnglishText = EBR.ReadBytes(EBR.ReadByte());
                        JBW.Write((byte)EnglishText.Length);
                        JBW.Write(EnglishText);
                        JBR.BaseStream.Position += 1 + JBR.ReadByte();
                        // Completion Text -> based on config
                        if (this.mnuPreserveJapaneseATCompletion.Checked)
                        {
                            byte[] JapaneseText = JBR.ReadBytes(JBR.ReadByte());
                            JBW.Write((byte)JapaneseText.Length);
                            JBW.Write(JapaneseText);
                        }
                        else
                        {
                            byte[] LowerEnglishText = E.GetBytes(E.GetString(EnglishText).ToLowerInvariant());
                            JBW.Write((byte)LowerEnglishText.Length);
                            JBW.Write(LowerEnglishText);
                            JBR.BaseStream.Position += 1 + JBR.ReadByte();
                        }
                    }
                    long EndOfGroupPos = JBW.BaseStream.Position;
                    JBW.BaseStream.Position = EntryBytesPos;
                    JBW.Write((uint)(EndOfGroupPos - EntryBytesPos - 4));
                    JBW.BaseStream.Position = EndOfGroupPos;
                }
                ATStream.Seek(0, SeekOrigin.Begin);
                ATStream.Write(NewStream.ToArray(), 0, (int)NewStream.Length);
                ATStream.SetLength(NewStream.Length);
                TranslationDone:
                ATStream.Close();
                NewStream.Close();
                EBR.Close();
            }
            catch
            {
                this.LogFailure("TranslateAutoTranslator");
            }
        }

        private void btnTranslateAutoTrans_Click(object sender, System.EventArgs e)
        {
            this.TranslateAutoTranslatorFile(55545, 55665);
            this.AddLogEntry(I18N.GetText("AutoTransTranslateDone"));
        }

        private void btnRestoreAutoTrans_Click(object sender, System.EventArgs e)
        {
            this.RestoreFile(55545);
            this.AddLogEntry(I18N.GetText("AutoTransRestoreDone"));
        }

        private void btnConfigAutoTrans_Click(object sender, System.EventArgs e)
        {
            this.mnuConfigAutoTrans.Show(this.btnConfigAutoTrans, new Point(0, this.btnConfigAutoTrans.Height));
        }

        private void mnuPreserveJapaneseATCompletion_Click(object sender, System.EventArgs e)
        {
            this.mnuPreserveJapaneseATCompletion.Checked = true;
            this.mnuEnglishATCompletionOnly.Checked = false;
        }

        private void mnuEnglishATCompletionOnly_Click(object sender, System.EventArgs e)
        {
            this.mnuEnglishATCompletionOnly.Checked = true;
            this.mnuPreserveJapaneseATCompletion.Checked = false;
        }

        #endregion

        #region Dialog Tables / String Tables

        private void btnTranslateDialogTables_Click(object sender, System.EventArgs e)
        {
            for (ushort i = 0; i < 0x100; ++i)
            {
                this.SwapFile(6120 + i, 6420 + i);
            }
            this.AddLogEntry(I18N.GetText("DialogTableTranslateDone"));
        }

        private void btnRestoreDialogTables_Click(object sender, System.EventArgs e)
        {
            for (ushort i = 0; i < 0x100; ++i)
            {
                this.RestoreFile(6120 + i);
            }
            this.AddLogEntry(I18N.GetText("DialogTableRestoreDone"));
        }

        private void btnTranslateStringTables_Click(object sender, System.EventArgs e)
        {
            this.SwapFile(55525, 55645);
            this.SwapFile(55526, 55646);
            this.SwapFile(55527, 55647);
            this.SwapFile(55528, 55648);
            this.SwapFile(55529, 55649);
            this.SwapFile(55530, 55650);
            this.SwapFile(55531, 55651);
            this.SwapFile(55532, 55652);
            this.SwapFile(55533, 55653);
            this.SwapFile(55534, 55654);
            this.SwapFile(55535, 55465); // out of sequence in the EN side of things
            this.SwapFile(55536, 55467); // out of sequence in the EN side of things
            this.SwapFile(55537, 55657);
            this.SwapFile(55538, 55658);
            this.SwapFile(55539, 55659);
            this.SwapFile(55540, 55660);
            this.AddLogEntry(I18N.GetText("StringTableTranslateDone"));
        }

        private void btnRestoreStringTables_Click(object sender, System.EventArgs e)
        {
            this.RestoreFile(55525);
            this.RestoreFile(55526);
            this.RestoreFile(55527);
            this.RestoreFile(55528);
            this.RestoreFile(55529);
            this.RestoreFile(55530);
            this.RestoreFile(55531);
            this.RestoreFile(55532);
            this.RestoreFile(55533);
            this.RestoreFile(55534);
            this.RestoreFile(55535);
            this.RestoreFile(55536);
            this.RestoreFile(55537);
            this.RestoreFile(55538);
            this.RestoreFile(55539);
            this.RestoreFile(55540);
            this.AddLogEntry(I18N.GetText("StringTableRestoreDone"));
        }

        #endregion
    }
}
