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
using System.Collections;
using System.IO;
using System.Text;
using System.Windows.Forms;
using PlayOnline.Core;

namespace PlayOnline.FFXI.Utils.MacroManager
{
    public partial class MainWindow : Form
    {
        private class ATMenuItem : MenuItem
        {
            public ATMenuItem(uint ResourceID, EventHandler OnClick)
                : base(FFXIResourceManager.GetResourceString(ResourceID), OnClick)
            {
                this.ResourceID_ = ResourceID;
            }

            private uint ResourceID_;

            public uint ResourceID
            {
                get { return this.ResourceID_; }
            }
        }

        public MainWindow()
        {
            this.InitializeComponent();
            // Set Up Icons
            this.Icon = Icons.Joystick;
            if (this.Icon == null)
            {
                this.Icon = Icons.CheckedPage;
            }
            try
            {
                this.ilBrowserIcons.Images.Add(Icons.DocFolder);
                this.ilBrowserIcons.Images.Add(Icons.People);
                this.ilBrowserIcons.Images.Add(Icons.FolderOpen);
                this.ilBrowserIcons.Images.Add(Icons.FolderClosed);
                this.ilBrowserIcons.Images.Add(Icons.ConfigFile);
                this.ilBrowserIcons.Images.Add(Icons.TextFile);
            }
            catch (Exception E)
            {
                Console.WriteLine(E.ToString());
                this.tvMacroTree.ImageList = null;
            }
            // Prepare textbox context menu
            this.CreateTextInsertMenu();
            // Set Up Library & Character Trees
            this.AddMacroLibraryNode(this.tvMacroTree);
            Game.Clear(); // Force fresh data
            foreach (Character C in Game.Characters)
            {
                this.AddCharacterNode(C, this.tvMacroTree);
            }
        }

        private void AddMacroBar(string FileName, TreeNode Parent)
        {
            try
            {
                MacroFolder MF = null;
                if (File.Exists(FileName))
                    // MacroFolder.LoadFromDATFile() creates an empty macro folder tree if the file doesn't exist, so test this first.
                {
                    MF = MacroSet.Load(FileName, "Dropped Macro Bar").Clone();
                    // Clone() turns it into a regular MacroFolder instead of a MacroSet
                }
                if (MF == null)
                {
                    throw new InvalidOperationException("Only valid macro bar files (mcr*.dat) can be dropped onto the macro tree.");
                }
                this.AddMacroFolderNode(MF, Parent);
                MacroFolder ParentFolder = Parent.Tag as MacroFolder;
                ParentFolder.Folders.Add(MF);
                this.tvMacroTree.SelectedNode = Parent;
                Parent.ExpandAll();
            }
            catch (Exception E)
            {
                MessageBox.Show(this, E.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RefreshMacroNode(TreeNode MacroNode)
        {
            Macro M = MacroNode.Tag as Macro;
            MacroNode.Text = M.Name;
            if (this.tvMacroTree.SelectedNode == MacroNode)
            {
                this.ShowProperties(M);
            }
        }

        private void ShowProperties(Macro M)
        {
            this.txtCommand1.Enabled = (M != null);
            this.txtCommand2.Enabled = (M != null);
            this.txtCommand3.Enabled = (M != null);
            this.txtCommand4.Enabled = (M != null);
            this.txtCommand5.Enabled = (M != null);
            this.txtCommand6.Enabled = (M != null);
            if (M == null)
            {
                this.txtCommand1.Clear();
                this.txtCommand2.Clear();
                this.txtCommand3.Clear();
                this.txtCommand4.Clear();
                this.txtCommand5.Clear();
                this.txtCommand6.Clear();
            }
            else
            {
                this.txtCommand1.Text = M.Commands[0];
                this.txtCommand2.Text = M.Commands[1];
                this.txtCommand3.Text = M.Commands[2];
                this.txtCommand4.Text = M.Commands[3];
                this.txtCommand5.Text = M.Commands[4];
                this.txtCommand6.Text = M.Commands[5];
            }
        }

        #region TreeNode Actions

        private void DoClear(TreeNode TN, bool Confirm)
        {
            if (TN.Tag is Character)
            {
                Character C = TN.Tag as Character;
                if (!Confirm ||
                    MessageBox.Show(this, String.Format("Are you sure you want to clear all macro bars for {0}?", C.Name),
                        "Confirm Clear", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) ==
                    DialogResult.Yes)
                {
                    foreach (TreeNode SubNode in TN.Nodes)
                    {
                        this.DoClear(SubNode, false);
                    }
                }
            }
            else if (TN.Tag is MacroFolder)
            {
                MacroFolder MF = TN.Tag as MacroFolder;
                if (!Confirm ||
                    MessageBox.Show(this,
                        String.Format("Are you sure you want to clear the '{0}' macro folder (and all its contents)?", MF.Name),
                        "Confirm Clear", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) ==
                    DialogResult.Yes)
                {
                    if (!MF.Locked)
                    {
                        TN.Text = String.Empty;
                        MF.Name = String.Empty;
                    }
                    foreach (TreeNode SubNode in TN.Nodes)
                    {
                        this.DoClear(SubNode, false);
                    }
                }
            }
            else if (TN.Tag is Macro)
            {
                Macro M = TN.Tag as Macro;
                if (!Confirm || M.Empty ||
                    MessageBox.Show(this, String.Format("Are you sure you want to clear the '{0}' macro?", M.Name), "Confirm Clear",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    M.Clear();
                    this.RefreshMacroNode(TN);
                }
            }
        }

        private void DoDelete(TreeNode TN, bool Confirm)
        {
            if (TN.Tag is Macro)
            {
                Macro M = TN.Tag as Macro;
                if (!Confirm || M.Empty ||
                    MessageBox.Show(this, String.Format("Are you sure you want to delete the '{0}' macro?", M.Name),
                        "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) ==
                    DialogResult.Yes)
                {
                    TreeNode ParentNode = TN.Parent;
                    (ParentNode.Tag as MacroFolder).Macros.Remove(M);
                    ParentNode.Nodes.Remove(TN);
                }
            }
            else if (TN.Tag is MacroFolder)
            {
                MacroFolder MF = TN.Tag as MacroFolder;
                if (!Confirm ||
                    MessageBox.Show(this,
                        String.Format("Are you sure you want to delete the '{0}' macro folder (and all its contents)?", MF.Name),
                        "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) ==
                    DialogResult.Yes)
                {
                    TreeNode ParentNode = TN.Parent;
                    (ParentNode.Tag as MacroFolder).Folders.Remove(MF);
                    ParentNode.Nodes.Remove(TN);
                }
            }
        }

        private void DoNewFolder(TreeNode TN, bool Confirm)
        {
            MacroFolder NewFolder = new MacroFolder("New Folder");
            (TN.Tag as MacroFolder).Folders.Add(NewFolder);
            this.tvMacroTree.SelectedNode = this.AddMacroFolderNode(NewFolder, TN);
            this.tvMacroTree.SelectedNode.BeginEdit();
        }

        private void DoNewMacro(TreeNode TN, bool Confirm)
        {
            Macro NewMacro = new Macro("New Macro");
            (TN.Tag as MacroFolder).Macros.Add(NewMacro);
            this.tvMacroTree.SelectedNode = this.AddMacroNode(NewMacro, TN);
            this.tvMacroTree.SelectedNode.BeginEdit();
        }

        private void DoSave(TreeNode TN, bool Confirm)
        {
            if (TN.Tag is MacroFolder)
            {
                MacroFolder MF = TN.Tag as MacroFolder;
                if (MF.CanSave)
                {
                    MF.Save();
                }
            }
        }

        private bool IsClearAllowed(TreeNode TN)
        {
            if (TN.Tag is MacroFolder || TN.Tag is Macro)
            {
                return true;
            }
            return false;
        }

        private bool IsDeleteAllowed(TreeNode TN)
        {
            if (TN.Tag is CharacterMacros || TN.Tag is MacroBook || TN.Tag is MacroLibrary || TN.Tag is MacroSet)
            {
                return false;
            }
            else if (TN.Tag is Macro)
            {
                MacroFolder OwnerFolder = TN.Parent.Tag as MacroFolder;
                return (OwnerFolder != null && !OwnerFolder.Locked);
            }
            else if (TN.Tag is MacroFolder)
            {
                return !(TN.Tag as MacroFolder).Locked;
            }
            return false;
        }

        private bool IsRenameAllowed(TreeNode TN)
        {
            if (TN.Tag is CharacterMacros || TN.Tag is MacroBook || TN.Tag is Macro)
            {
                return true;
            }
            else if (TN.Tag is MacroFolder)
            {
                return !(TN.Tag as MacroFolder).Locked;
            }
            return false;
        }

        private bool IsSaveAllowed(TreeNode TN)
        {
            if (TN.Tag is MacroFolder)
            {
                return (TN.Tag as MacroFolder).CanSave;
            }
            return false;
        }

        #endregion

        #region TreeView Creation

        private void AddMacroLibraryNode(TreeView Parent) { this.AddMacroFolderNode(MacroLibrary.Load(), this.tvMacroTree); }

        private void AddCharacterNode(Character C, TreeView Parent) { this.AddMacroFolderNode(new CharacterMacros(C), Parent); }

        private TreeNode AddMacroFolderNode(MacroFolder Folder, TreeNode Parent)
        {
            TreeNode MacroFolderNode = new TreeNode(Folder.Name, 3, 2);
            MacroFolderNode.Tag = Folder;
            foreach (MacroFolder MF in Folder.Folders)
            {
                this.AddMacroFolderNode(MF, MacroFolderNode);
            }
            foreach (Macro       M  in Folder.Macros)
            {
                this.AddMacroNode(M, MacroFolderNode);
            }
            Parent.Nodes.Add(MacroFolderNode);
            return MacroFolderNode;
        }

        private void AddMacroFolderNode(MacroFolder Folder, TreeView Parent)
        {
            TreeNode MacroFolderNode = new TreeNode(Folder.Name, 0, 0);
            MacroFolderNode.Tag = Folder;
            foreach (MacroFolder MF in Folder.Folders)
            {
                this.AddMacroFolderNode(MF, MacroFolderNode);
            }
            foreach (Macro       M  in Folder.Macros)
            {
                this.AddMacroNode(M, MacroFolderNode);
            }
            Parent.Nodes.Add(MacroFolderNode);
        }

        private TreeNode AddMacroNode(Macro M, TreeNode Parent)
        {
            TreeNode MacroNode = new TreeNode(M.Name, 4, 4);
            MacroNode.Tag = M;
            Parent.Nodes.Add(MacroNode);
            return MacroNode;
        }

        #endregion

        #region TextBox Context Menu Creation

        private void CreateTextInsertMenu()
        {
            this.mnuTextContext.MenuItems.Add("-");
            MenuItem InsertMenu = this.mnuTextContext.MenuItems.Add("&Insert");
            this.AddAutoTransMenuItems(InsertMenu.MenuItems.Add("&Auto-Translator Text"));
            this.AddFacesMenuItems(InsertMenu.MenuItems.Add("&Faces"));
            this.AddAlphabetMenuItems(InsertMenu.MenuItems.Add("&Alphabets"));
            this.AddSpecialCharMenuItems(InsertMenu.MenuItems.Add("&Special Characters"));
        }

        private void AddAutoTransMenuItems(MenuItem ParentMenu)
        {
#if false
            Hashtable CategoryMenus = new Hashtable();
            if (AutoTranslator.Data != null)
            {
                foreach (AutoTranslator.MessageGroup MG in AutoTranslator.Data)
                {
                    MenuItem CategoryMenu = CategoryMenus[MG.Category] as MenuItem;
                    if (CategoryMenu == null)
                    {
                        CategoryMenu = ParentMenu.MenuItems.Add("&Unknown Category");
                        switch (MG.Category)
                        {
                        case 2:
                            CategoryMenu.Text = "&English Messages";
                            break;
                        case 4:
                            CategoryMenu.Text = "&Japanese Messages";
                            break;
                        }
                        CategoryMenus[MG.Category] = CategoryMenu;
                    }
                    MenuItem GroupMenu = CategoryMenu.MenuItems.Add(MG.Title);
                    GroupMenu.MenuItems.Add(MG.Description).Enabled = false;
                    foreach (AutoTranslator.Message M in MG.Messages)
                    {
                        GroupMenu.MenuItems.Add(new ATMenuItem(M.ResID, new EventHandler(this.AutoTransMenuItem_Click)));
                    }
                }
            }
#else
            ParentMenu.MenuItems.Add("Temporarily Disabled").Enabled = false;
#endif
        }

        private void AddFacesMenuItems(MenuItem FaceMenu)
        {
            FaceMenu.MenuItems.Add("＼(^_^ ) ( ^_^)／", new EventHandler(this.InsertMenuItem_Click));
            FaceMenu.MenuItems.Add("＼(・_・ )( ・_・)／", new EventHandler(this.InsertMenuItem_Click));
            FaceMenu.MenuItems.Add("( ^_^)／", new EventHandler(this.InsertMenuItem_Click));
            FaceMenu.MenuItems.Add("＼（＾ｏ＾）／", new EventHandler(this.InsertMenuItem_Click));
            FaceMenu.MenuItems.Add("＼(＾ｏ＾)／", new EventHandler(this.InsertMenuItem_Click));
            FaceMenu.MenuItems.Add("＼(^_^ ) ( ^_^)／", new EventHandler(this.InsertMenuItem_Click));
            FaceMenu.MenuItems.Add("m(_ _)m", new EventHandler(this.InsertMenuItem_Click));
            FaceMenu.MenuItems.Add("_(._.)_", new EventHandler(this.InsertMenuItem_Click));
            FaceMenu.MenuItems.Add("＜(。_。)＞", new EventHandler(this.InsertMenuItem_Click));
            FaceMenu.MenuItems.Add("(シ_ _)シ", new EventHandler(this.InsertMenuItem_Click));
            FaceMenu.MenuItems.Add("ヘ(-_-ヘ", new EventHandler(this.InsertMenuItem_Click));
            FaceMenu.MenuItems.Add("へ（＾＾へ）））））", new EventHandler(this.InsertMenuItem_Click));
            FaceMenu.MenuItems.Add("（（（（（ノ＾＾）ノ", new EventHandler(this.InsertMenuItem_Click));
            FaceMenu.MenuItems.Add("Ψ('-'*)", new EventHandler(this.InsertMenuItem_Click));
            FaceMenu.MenuItems.Add("♪♪ｖ(⌒ｏ⌒)ｖ♪♪", new EventHandler(this.InsertMenuItem_Click));
            FaceMenu.MenuItems.Add("┗|・o・|┛", new EventHandler(this.InsertMenuItem_Click));
            FaceMenu.MenuItems.Add("┗|⌒O⌒|┛", new EventHandler(this.InsertMenuItem_Click));
            FaceMenu.MenuItems.Add("(TωT)/~~~　BYE　BYE", new EventHandler(this.InsertMenuItem_Click));
        }

        private void AddAlphabetMenuItems(MenuItem ParentMenu)
        {
            {
                // Greek
                MenuItem MI = ParentMenu.MenuItems.Add("&Greek");
                {
                    MenuItem MI2 = MI.MenuItems.Add("&Lowercase");
                    for (int i = 0x03B1; i <= 0x03C1; ++i)
                    {
                        MI2.MenuItems.Add(new string((char)i, 1), new EventHandler(this.InsertMenuItem_Click));
                    }
                    // Skip second form of sigma (not in shift-jis)
                    for (int i = 0x03C3; i <= 0x03C9; ++i)
                    {
                        MI2.MenuItems.Add(new string((char)i, 1), new EventHandler(this.InsertMenuItem_Click));
                    }
                }
                {
                    MenuItem MI2 = MI.MenuItems.Add("&Uppercase");
                    for (int i = 0x0391; i <= 0x03A1; ++i)
                    {
                        MI2.MenuItems.Add(new string((char)i, 1), new EventHandler(this.InsertMenuItem_Click));
                    }
                    // Skip second form of sigma (not in shift-jis)
                    for (int i = 0x03A3; i <= 0x03A9; ++i)
                    {
                        MI2.MenuItems.Add(new string((char)i, 1), new EventHandler(this.InsertMenuItem_Click));
                    }
                }
            }
            {
                // Japanese
                MenuItem MI = ParentMenu.MenuItems.Add("&Japanese");
                {
                    MenuItem MI2 = MI.MenuItems.Add("&Hiragana");
                }
                {
                    MenuItem MI2 = MI.MenuItems.Add("&Katakana");
                }
                {
                    MenuItem MI2 = MI.MenuItems.Add("&Punctuation && Symbols");
                    for (int i = 0x3000; i <= 0x3003; ++i)
                    {
                        MI2.MenuItems.Add(new string((char)i, 1), new EventHandler(this.InsertMenuItem_Click));
                    }
                    for (int i = 0x3005; i <= 0x3015; ++i)
                    {
                        MI2.MenuItems.Add(new string((char)i, 1), new EventHandler(this.InsertMenuItem_Click));
                    }
                    MI2.MenuItems.Add("\x301D", new EventHandler(this.InsertMenuItem_Click));
                    MI2.MenuItems.Add("\x301F", new EventHandler(this.InsertMenuItem_Click));
                }
            }
            {
                // Latin
                MenuItem MI = ParentMenu.MenuItems.Add("&Latin");
                {
                    MenuItem MI2 = MI.MenuItems.Add("&Lowercase");
                    for (int i = 0x0061; i <= 0x007A; ++i)
                    {
                        MI2.MenuItems.Add(new string((char)i, 1), new EventHandler(this.InsertMenuItem_Click));
                    }
                }
                {
                    MenuItem MI2 = MI.MenuItems.Add("&Uppercase");
                    for (int i = 0x0041; i <= 0x005A; ++i)
                    {
                        MI2.MenuItems.Add(new string((char)i, 1), new EventHandler(this.InsertMenuItem_Click));
                    }
                }
                {
                    MenuItem MI2 = MI.MenuItems.Add("&Numbers");
                    for (int i = 0x0030; i <= 0x0039; ++i)
                    {
                        MI2.MenuItems.Add(new string((char)i, 1), new EventHandler(this.InsertMenuItem_Click));
                    }
                }
                {
                    MenuItem MI2 = MI.MenuItems.Add("&Punctuation && Symbols");
                    for (int i = 0x0020; i <= 0x002F; ++i)
                    {
                        MI2.MenuItems.Add(new string((char)i, 1), new EventHandler(this.InsertMenuItem_Click));
                    }
                    for (int i = 0x003A; i <= 0x0040; ++i)
                    {
                        MI2.MenuItems.Add(new string((char)i, 1), new EventHandler(this.InsertMenuItem_Click));
                    }
                    for (int i = 0x005B; i <= 0x0060; ++i)
                    {
                        MI2.MenuItems.Add(new string((char)i, 1), new EventHandler(this.InsertMenuItem_Click));
                    }
                    for (int i = 0x007B; i <= 0x007E; ++i)
                    {
                        MI2.MenuItems.Add(new string((char)i, 1), new EventHandler(this.InsertMenuItem_Click));
                    }
                    foreach (MenuItem MI3 in MI2.MenuItems)
                    {
                        // Avoid activating the special Menu caption formatting
                        if (MI3.Text == "-")
                        {
                            MI3.Text = "- ";
                        }
                        else if (MI3.Text == "&")
                        {
                            MI3.Text = "&&";
                        }
                    }
                    MI2.MenuItems.Add("\x2010", new EventHandler(this.InsertMenuItem_Click));
                    MI2.MenuItems.Add("\x2015", new EventHandler(this.InsertMenuItem_Click));
                    MI2.MenuItems.Add("\x2018", new EventHandler(this.InsertMenuItem_Click));
                    MI2.MenuItems.Add("\x2019", new EventHandler(this.InsertMenuItem_Click));
                    MI2.MenuItems.Add("\x201C", new EventHandler(this.InsertMenuItem_Click));
                    MI2.MenuItems.Add("\x201D", new EventHandler(this.InsertMenuItem_Click));
                    MI2.MenuItems.Add("\x2020", new EventHandler(this.InsertMenuItem_Click));
                    MI2.MenuItems.Add("\x2021", new EventHandler(this.InsertMenuItem_Click));
                    MI2.MenuItems.Add("\x2025", new EventHandler(this.InsertMenuItem_Click));
                    MI2.MenuItems.Add("\x2026", new EventHandler(this.InsertMenuItem_Click));
                    MI2.MenuItems.Add("\x2030", new EventHandler(this.InsertMenuItem_Click));
                    MI2.MenuItems.Add("\x2032", new EventHandler(this.InsertMenuItem_Click));
                    MI2.MenuItems.Add("\x2033", new EventHandler(this.InsertMenuItem_Click));
                    MI2.MenuItems.Add("\x203B", new EventHandler(this.InsertMenuItem_Click));
                }
            }
            {
                // Wide ASCII
                MenuItem MI = ParentMenu.MenuItems.Add("&Wide ASCII");
                {
                    MenuItem MI2 = MI.MenuItems.Add("&Lowercase");
                    for (int i = 0xFF41; i <= 0xFF5A; ++i)
                    {
                        MI2.MenuItems.Add(new string((char)i, 1), new EventHandler(this.InsertMenuItem_Click));
                    }
                }
                {
                    MenuItem MI2 = MI.MenuItems.Add("&Uppercase");
                    for (int i = 0xFF21; i <= 0xFF3A; ++i)
                    {
                        MI2.MenuItems.Add(new string((char)i, 1), new EventHandler(this.InsertMenuItem_Click));
                    }
                }
                {
                    MenuItem MI2 = MI.MenuItems.Add("&Numbers");
                    for (int i = 0xFF10; i <= 0xFF19; ++i)
                    {
                        MI2.MenuItems.Add(new string((char)i, 1), new EventHandler(this.InsertMenuItem_Click));
                    }
                }
                {
                    MenuItem MI2 = MI.MenuItems.Add("&Punctuation && Symbols");
                    for (int i = 0xFF01; i <= 0xFF0F; ++i)
                    {
                        MI2.MenuItems.Add(new string((char)i, 1), new EventHandler(this.InsertMenuItem_Click));
                    }
                    for (int i = 0xFF1A; i <= 0xFF20; ++i)
                    {
                        MI2.MenuItems.Add(new string((char)i, 1), new EventHandler(this.InsertMenuItem_Click));
                    }
                    for (int i = 0xFF3B; i <= 0xFF40; ++i)
                    {
                        MI2.MenuItems.Add(new string((char)i, 1), new EventHandler(this.InsertMenuItem_Click));
                    }
                    for (int i = 0xFF5B; i <= 0xFF5E; ++i)
                    {
                        MI2.MenuItems.Add(new string((char)i, 1), new EventHandler(this.InsertMenuItem_Click));
                    }
                }
            }
        }

        private void AddSpecialCharMenuItems(MenuItem ParentMenu)
        {
            {
                // Circled Numbers
                MenuItem MI = ParentMenu.MenuItems.Add("&Circled Numbers");
                for (int i = 0x2460; i <= 0x2473; ++i)
                {
                    MI.MenuItems.Add(new string((char)i, 1), new EventHandler(this.InsertMenuItem_Click));
                }
            }
            {
                // Dingbats
                MenuItem MI = ParentMenu.MenuItems.Add("&Dingbats");
                MI.MenuItems.Add("\x25A0", new EventHandler(this.InsertMenuItem_Click));
                MI.MenuItems.Add("\x25A1", new EventHandler(this.InsertMenuItem_Click));
                MI.MenuItems.Add("\x25B2", new EventHandler(this.InsertMenuItem_Click));
                MI.MenuItems.Add("\x25B3", new EventHandler(this.InsertMenuItem_Click));
                MI.MenuItems.Add("\x25BC", new EventHandler(this.InsertMenuItem_Click));
                MI.MenuItems.Add("\x25BD", new EventHandler(this.InsertMenuItem_Click));
                MI.MenuItems.Add("\x25C6", new EventHandler(this.InsertMenuItem_Click));
                MI.MenuItems.Add("\x25C7", new EventHandler(this.InsertMenuItem_Click));
                MI.MenuItems.Add("\x25CB", new EventHandler(this.InsertMenuItem_Click));
                MI.MenuItems.Add("\x25CE", new EventHandler(this.InsertMenuItem_Click));
                MI.MenuItems.Add("\x25CF", new EventHandler(this.InsertMenuItem_Click));
                MI.MenuItems.Add("\x25EF", new EventHandler(this.InsertMenuItem_Click));
                MI.MenuItems.Add("\x2605", new EventHandler(this.InsertMenuItem_Click));
                MI.MenuItems.Add("\x2606", new EventHandler(this.InsertMenuItem_Click));
                MI.MenuItems.Add("\x2640", new EventHandler(this.InsertMenuItem_Click));
                MI.MenuItems.Add("\x2642", new EventHandler(this.InsertMenuItem_Click));
            }
            {
                // Line Drawing
                MenuItem MI = ParentMenu.MenuItems.Add("&Line Drawing");
                MI.MenuItems.Add("\x2500", new EventHandler(this.InsertMenuItem_Click));
                MI.MenuItems.Add("\x2501", new EventHandler(this.InsertMenuItem_Click));
                MI.MenuItems.Add("\x2502", new EventHandler(this.InsertMenuItem_Click));
                MI.MenuItems.Add("\x2503", new EventHandler(this.InsertMenuItem_Click));
                MI.MenuItems.Add("\x250C", new EventHandler(this.InsertMenuItem_Click));
                MI.MenuItems.Add("\x250F", new EventHandler(this.InsertMenuItem_Click));
                MI.MenuItems.Add("\x2510", new EventHandler(this.InsertMenuItem_Click));
                MI.MenuItems.Add("\x2513", new EventHandler(this.InsertMenuItem_Click));
                MI.MenuItems.Add("\x2514", new EventHandler(this.InsertMenuItem_Click));
                MI.MenuItems.Add("\x2517", new EventHandler(this.InsertMenuItem_Click));
                MI.MenuItems.Add("\x2518", new EventHandler(this.InsertMenuItem_Click));
                MI.MenuItems.Add("\x251B", new EventHandler(this.InsertMenuItem_Click));
                MI.MenuItems.Add("\x251C", new EventHandler(this.InsertMenuItem_Click));
                MI.MenuItems.Add("\x251D", new EventHandler(this.InsertMenuItem_Click));
                MI.MenuItems.Add("\x2520", new EventHandler(this.InsertMenuItem_Click));
                MI.MenuItems.Add("\x2523", new EventHandler(this.InsertMenuItem_Click));
                MI.MenuItems.Add("\x2524", new EventHandler(this.InsertMenuItem_Click));
                MI.MenuItems.Add("\x2525", new EventHandler(this.InsertMenuItem_Click));
                MI.MenuItems.Add("\x2528", new EventHandler(this.InsertMenuItem_Click));
                MI.MenuItems.Add("\x252B", new EventHandler(this.InsertMenuItem_Click));
                MI.MenuItems.Add("\x252C", new EventHandler(this.InsertMenuItem_Click));
                MI.MenuItems.Add("\x252F", new EventHandler(this.InsertMenuItem_Click));
                MI.MenuItems.Add("\x2530", new EventHandler(this.InsertMenuItem_Click));
                MI.MenuItems.Add("\x2533", new EventHandler(this.InsertMenuItem_Click));
                MI.MenuItems.Add("\x2534", new EventHandler(this.InsertMenuItem_Click));
                MI.MenuItems.Add("\x2537", new EventHandler(this.InsertMenuItem_Click));
                MI.MenuItems.Add("\x2538", new EventHandler(this.InsertMenuItem_Click));
                MI.MenuItems.Add("\x253B", new EventHandler(this.InsertMenuItem_Click));
                MI.MenuItems.Add("\x253C", new EventHandler(this.InsertMenuItem_Click));
                MI.MenuItems.Add("\x253F", new EventHandler(this.InsertMenuItem_Click));
                MI.MenuItems.Add("\x2542", new EventHandler(this.InsertMenuItem_Click));
                MI.MenuItems.Add("\x254B", new EventHandler(this.InsertMenuItem_Click));
            }
            {
                // Mathematical & Musical Symbols
                MenuItem MI = ParentMenu.MenuItems.Add("&Math && Music");
                {
                    // Math Operators
                    MenuItem MI2 = MI.MenuItems.Add("&Operators");
                    MI2.MenuItems.Add("\x2200", new EventHandler(this.InsertMenuItem_Click));
                    MI2.MenuItems.Add("\x2202", new EventHandler(this.InsertMenuItem_Click));
                    MI2.MenuItems.Add("\x2203", new EventHandler(this.InsertMenuItem_Click));
                    MI2.MenuItems.Add("\x2207", new EventHandler(this.InsertMenuItem_Click));
                    MI2.MenuItems.Add("\x2208", new EventHandler(this.InsertMenuItem_Click));
                    MI2.MenuItems.Add("\x220B", new EventHandler(this.InsertMenuItem_Click));
                    MI2.MenuItems.Add("\x2211", new EventHandler(this.InsertMenuItem_Click));
                    MI2.MenuItems.Add("\x221A", new EventHandler(this.InsertMenuItem_Click));
                    MI2.MenuItems.Add("\x221D", new EventHandler(this.InsertMenuItem_Click));
                    MI2.MenuItems.Add("\x221E", new EventHandler(this.InsertMenuItem_Click));
                    MI2.MenuItems.Add("\x221F", new EventHandler(this.InsertMenuItem_Click));
                    MI2.MenuItems.Add("\x2220", new EventHandler(this.InsertMenuItem_Click));
                    MI2.MenuItems.Add("\x2225", new EventHandler(this.InsertMenuItem_Click));
                    MI2.MenuItems.Add("\x2227", new EventHandler(this.InsertMenuItem_Click));
                    MI2.MenuItems.Add("\x2228", new EventHandler(this.InsertMenuItem_Click));
                    MI2.MenuItems.Add("\x2229", new EventHandler(this.InsertMenuItem_Click));
                    MI2.MenuItems.Add("\x222A", new EventHandler(this.InsertMenuItem_Click));
                    MI2.MenuItems.Add("\x222B", new EventHandler(this.InsertMenuItem_Click));
                    MI2.MenuItems.Add("\x222C", new EventHandler(this.InsertMenuItem_Click));
                    MI2.MenuItems.Add("\x222E", new EventHandler(this.InsertMenuItem_Click));
                    MI2.MenuItems.Add("\x2234", new EventHandler(this.InsertMenuItem_Click));
                    MI2.MenuItems.Add("\x2235", new EventHandler(this.InsertMenuItem_Click));
                    MI2.MenuItems.Add("\x223D", new EventHandler(this.InsertMenuItem_Click));
                    MI2.MenuItems.Add("\x2252", new EventHandler(this.InsertMenuItem_Click));
                    MI2.MenuItems.Add("\x2260", new EventHandler(this.InsertMenuItem_Click));
                    MI2.MenuItems.Add("\x2261", new EventHandler(this.InsertMenuItem_Click));
                    MI2.MenuItems.Add("\x2266", new EventHandler(this.InsertMenuItem_Click));
                    MI2.MenuItems.Add("\x2267", new EventHandler(this.InsertMenuItem_Click));
                    MI2.MenuItems.Add("\x226A", new EventHandler(this.InsertMenuItem_Click));
                    MI2.MenuItems.Add("\x226B", new EventHandler(this.InsertMenuItem_Click));
                    MI2.MenuItems.Add("\x2282", new EventHandler(this.InsertMenuItem_Click));
                    MI2.MenuItems.Add("\x2283", new EventHandler(this.InsertMenuItem_Click));
                    MI2.MenuItems.Add("\x2286", new EventHandler(this.InsertMenuItem_Click));
                    MI2.MenuItems.Add("\x2287", new EventHandler(this.InsertMenuItem_Click));
                    MI2.MenuItems.Add("\x22A5", new EventHandler(this.InsertMenuItem_Click));
                    MI2.MenuItems.Add("\x22BF", new EventHandler(this.InsertMenuItem_Click));
                }
                {
                    // Arrows
                    MenuItem MI2 = MI.MenuItems.Add("&Arrows");
                    MI2.MenuItems.Add("\x2190", new EventHandler(this.InsertMenuItem_Click)); // Left
                    MI2.MenuItems.Add("\x2191", new EventHandler(this.InsertMenuItem_Click)); // Up
                    MI2.MenuItems.Add("\x2192", new EventHandler(this.InsertMenuItem_Click)); // Right
                    MI2.MenuItems.Add("\x2193", new EventHandler(this.InsertMenuItem_Click)); // Down
                    MI2.MenuItems.Add("\x21D2", new EventHandler(this.InsertMenuItem_Click)); // =>
                    MI2.MenuItems.Add("\x21D4", new EventHandler(this.InsertMenuItem_Click)); // <=>
                }
                {
                    // Musical Symbols
                    MenuItem MI2 = MI.MenuItems.Add("&Musical Symbols");
                    MI2.MenuItems.Add("\x266A", new EventHandler(this.InsertMenuItem_Click)); // Single note
                    MI2.MenuItems.Add("\x266D", new EventHandler(this.InsertMenuItem_Click)); // Flat
                    MI2.MenuItems.Add("\x266F", new EventHandler(this.InsertMenuItem_Click)); // Sharp
                }
            }
            {
                // Roman Numerals
                MenuItem MI = ParentMenu.MenuItems.Add("&Roman Numerals");
                {
                    MenuItem MI2 = MI.MenuItems.Add("&Lowercase");
                    for (int i = 0x2170; i <= 0x2179; ++i)
                    {
                        MI2.MenuItems.Add(new string((char)i, 1), new EventHandler(this.InsertMenuItem_Click));
                    }
                }
                {
                    MenuItem MI2 = MI.MenuItems.Add("&Uppercase");
                    for (int i = 0x2160; i <= 0x2169; ++i)
                    {
                        MI2.MenuItems.Add(new string((char)i, 1), new EventHandler(this.InsertMenuItem_Click));
                    }
                }
            }
            {
                // Unit Indicators
                MenuItem MI = ParentMenu.MenuItems.Add("&Unit Indicators");
                MI.MenuItems.Add("\x2103", new EventHandler(this.InsertMenuItem_Click)); // °C
                MI.MenuItems.Add("\x338E", new EventHandler(this.InsertMenuItem_Click)); // mg
                MI.MenuItems.Add("\x338F", new EventHandler(this.InsertMenuItem_Click)); // kg
                MI.MenuItems.Add("\x339C", new EventHandler(this.InsertMenuItem_Click)); // mm
                MI.MenuItems.Add("\x339D", new EventHandler(this.InsertMenuItem_Click)); // cm
                MI.MenuItems.Add("\x339E", new EventHandler(this.InsertMenuItem_Click)); // km
                MI.MenuItems.Add("\x33A1", new EventHandler(this.InsertMenuItem_Click)); // m²
                MI.MenuItems.Add("\x33C4", new EventHandler(this.InsertMenuItem_Click)); // cc
                MI.MenuItems.Add("\x33CD", new EventHandler(this.InsertMenuItem_Click)); // KK
            }
            {
                // Various
                MenuItem MI = ParentMenu.MenuItems.Add("&Various");
                {
                    MenuItem MI2 = MI.MenuItems.Add("&Asian Symbols");
                    MI2.MenuItems.Add("\x3231", new EventHandler(this.InsertMenuItem_Click));
                    MI2.MenuItems.Add("\x3232", new EventHandler(this.InsertMenuItem_Click));
                    MI2.MenuItems.Add("\x3239", new EventHandler(this.InsertMenuItem_Click));
                    MI2.MenuItems.Add("\x32A4", new EventHandler(this.InsertMenuItem_Click));
                    MI2.MenuItems.Add("\x32A5", new EventHandler(this.InsertMenuItem_Click));
                    MI2.MenuItems.Add("\x32A6", new EventHandler(this.InsertMenuItem_Click));
                    MI2.MenuItems.Add("\x32A7", new EventHandler(this.InsertMenuItem_Click));
                    MI2.MenuItems.Add("\x32A8", new EventHandler(this.InsertMenuItem_Click));
                    MI2.MenuItems.Add("\x3303", new EventHandler(this.InsertMenuItem_Click));
                    MI2.MenuItems.Add("\x330D", new EventHandler(this.InsertMenuItem_Click));
                    MI2.MenuItems.Add("\x3314", new EventHandler(this.InsertMenuItem_Click));
                    MI2.MenuItems.Add("\x3318", new EventHandler(this.InsertMenuItem_Click));
                    MI2.MenuItems.Add("\x3322", new EventHandler(this.InsertMenuItem_Click));
                    MI2.MenuItems.Add("\x3323", new EventHandler(this.InsertMenuItem_Click));
                    MI2.MenuItems.Add("\x3326", new EventHandler(this.InsertMenuItem_Click));
                    MI2.MenuItems.Add("\x3327", new EventHandler(this.InsertMenuItem_Click));
                    MI2.MenuItems.Add("\x332B", new EventHandler(this.InsertMenuItem_Click));
                    MI2.MenuItems.Add("\x3336", new EventHandler(this.InsertMenuItem_Click));
                    MI2.MenuItems.Add("\x333B", new EventHandler(this.InsertMenuItem_Click));
                    MI2.MenuItems.Add("\x3349", new EventHandler(this.InsertMenuItem_Click));
                    MI2.MenuItems.Add("\x334A", new EventHandler(this.InsertMenuItem_Click));
                    MI2.MenuItems.Add("\x334D", new EventHandler(this.InsertMenuItem_Click));
                    MI2.MenuItems.Add("\x3351", new EventHandler(this.InsertMenuItem_Click));
                    MI2.MenuItems.Add("\x3357", new EventHandler(this.InsertMenuItem_Click));
                    MI2.MenuItems.Add("\x337B", new EventHandler(this.InsertMenuItem_Click));
                    MI2.MenuItems.Add("\x337C", new EventHandler(this.InsertMenuItem_Click));
                    MI2.MenuItems.Add("\x337D", new EventHandler(this.InsertMenuItem_Click));
                    MI2.MenuItems.Add("\x337E", new EventHandler(this.InsertMenuItem_Click));
                }
                {
                    MenuItem MI2 = MI.MenuItems.Add("&Combined Letters");
                    MI2.MenuItems.Add("\x2116", new EventHandler(this.InsertMenuItem_Click)); // "numero"
                    MI2.MenuItems.Add("\x2121", new EventHandler(this.InsertMenuItem_Click)); // "telephone"
                }
                {
                    MenuItem MI2 = MI.MenuItems.Add("&Other");
                    MI2.MenuItems.Add("\x2312", new EventHandler(this.InsertMenuItem_Click));
                }
            }
        }

        #endregion

        #region Menu Events

        private void AutoTransMenuItem_Click(object sender, EventArgs e)
        {
            ATMenuItem MI = sender as ATMenuItem;
            if (MI != null)
            {
                ContextMenu TopMenu = MI.GetContextMenu();
                if (TopMenu != null)
                {
                    TextBoxBase Owner = TopMenu.SourceControl as TextBoxBase;
                    if (Owner != null)
                    {
                        string NewText = Owner.Text.Substring(0, Owner.SelectionStart);
                        NewText += String.Format("{0}[{1:X8}] {2}{3}", FFXIEncoding.SpecialMarkerStart, MI.ResourceID, MI.Text,
                            FFXIEncoding.SpecialMarkerEnd);
                        int caretpos = NewText.Length;
                        NewText += Owner.Text.Substring(Owner.SelectionStart + Owner.SelectionLength);
                        Owner.Text = NewText;
                        Owner.SelectionStart = caretpos;
                        Owner.SelectionLength = 0;
                    }
                }
            }
        }

        private void InsertMenuItem_Click(object sender, EventArgs e)
        {
            MenuItem MI = sender as MenuItem;
            if (MI != null)
            {
                ContextMenu TopMenu = MI.GetContextMenu();
                if (TopMenu != null)
                {
                    TextBoxBase Owner = TopMenu.SourceControl as TextBoxBase;
                    if (Owner != null)
                    {
                        string MenuText = MI.Text;
                        if (MenuText == "&&" || MenuText == "- ")
                        {
                            MenuText = MenuText.Substring(0, 1);
                        }
                        string NewText = Owner.Text.Substring(0, Owner.SelectionStart) + MenuText;
                        int caretpos = NewText.Length;
                        NewText += Owner.Text.Substring(Owner.SelectionStart + Owner.SelectionLength);
                        Owner.Text = NewText;
                        Owner.SelectionStart = caretpos;
                        Owner.SelectionLength = 0;
                    }
                }
            }
        }

        private void mnuTextContext_Popup(object sender, System.EventArgs e)
        {
            // Currently, there's nothing to dynamically alter in this menu
        }

        private void mnuTextClear_Click(object sender, System.EventArgs e)
        {
            MenuItem MI = sender as MenuItem;
            if (MI != null)
            {
                ContextMenu TopMenu = MI.GetContextMenu();
                if (TopMenu != null)
                {
                    TextBoxBase Owner = TopMenu.SourceControl as TextBoxBase;
                    if (Owner != null)
                    {
                        Owner.Clear();
                    }
                }
            }
        }

        private void mnuTextCopy_Click(object sender, System.EventArgs e)
        {
            MenuItem MI = sender as MenuItem;
            if (MI != null)
            {
                ContextMenu TopMenu = MI.GetContextMenu();
                if (TopMenu != null)
                {
                    TextBoxBase Owner = TopMenu.SourceControl as TextBoxBase;
                    if (Owner != null)
                    {
                        Clipboard.SetDataObject(Owner.SelectedText, true);
                    }
                }
            }
        }

        private void mnuTextCut_Click(object sender, System.EventArgs e)
        {
            MenuItem MI = sender as MenuItem;
            if (MI != null)
            {
                ContextMenu TopMenu = MI.GetContextMenu();
                if (TopMenu != null)
                {
                    TextBoxBase Owner = TopMenu.SourceControl as TextBoxBase;
                    if (Owner != null)
                    {
                        Clipboard.SetDataObject(Owner.SelectedText, true);
                        if (Owner.SelectedText != String.Empty)
                        {
                            string NewText = Owner.Text.Substring(0, Owner.SelectionStart);
                            NewText += Owner.Text.Substring(Owner.SelectionStart + Owner.SelectionLength);
                            int caretpos = Owner.SelectionStart;
                            Owner.Text = NewText;
                            Owner.SelectionStart = caretpos;
                            Owner.SelectionLength = 0;
                        }
                    }
                }
            }
        }

        private void mnuTextPaste_Click(object sender, System.EventArgs e)
        {
            MenuItem MI = sender as MenuItem;
            if (MI != null)
            {
                ContextMenu TopMenu = MI.GetContextMenu();
                if (TopMenu != null)
                {
                    TextBoxBase Owner = TopMenu.SourceControl as TextBoxBase;
                    if (Owner != null)
                    {
                        string NewText = Owner.Text.Substring(0, Owner.SelectionStart);
                        NewText += Clipboard.GetDataObject().GetData(typeof (String));
                        int caretpos = NewText.Length;
                        NewText += Owner.Text.Substring(Owner.SelectionStart + Owner.SelectionLength);
                        Owner.Text = NewText;
                        Owner.SelectionStart = caretpos;
                        Owner.SelectionLength = 0;
                    }
                }
            }
        }

        private TreeNode ContextNode = null;

        private void mnuTreeContext_Popup(object sender, System.EventArgs e)
        {
            this.ContextNode = this.tvMacroTree.GetNodeAt(this.tvMacroTree.PointToClient(Control.MousePosition));
            this.mnuTreeRename.Enabled = this.IsRenameAllowed(this.ContextNode);
            this.mnuTreeClear.Enabled = this.IsClearAllowed(this.ContextNode);
            this.mnuTreeDelete.Enabled = this.IsDeleteAllowed(this.ContextNode);
            this.mnuTreeSave.Enabled = this.IsSaveAllowed(this.ContextNode);
            // The "New" portion is optional; use Visible rather than Enabled
            this.mnuTreeSep2.Visible =
                this.mnuTreeNew.Visible = (this.ContextNode.Tag is MacroFolder && !(this.ContextNode.Tag as MacroFolder).Locked);
        }

        private void mnuTreeClear_Click(object sender, System.EventArgs e) { this.DoClear(this.ContextNode, true); }

        private void mnuTreeCollapse_Click(object sender, System.EventArgs e) { this.ContextNode.Collapse(); }

        private void mnuTreeDelete_Click(object sender, System.EventArgs e) { this.DoDelete(this.ContextNode, true); }

        private void mnuTreeExpand_Click(object sender, System.EventArgs e) { this.ContextNode.Expand(); }

        private void mnuTreeExpandAll_Click(object sender, System.EventArgs e) { this.ContextNode.ExpandAll(); }

        private void mnuTreeNewFolder_Click(object sender, System.EventArgs e) { this.DoNewFolder(this.ContextNode, true); }

        private void mnuTreeNewMacro_Click(object sender, System.EventArgs e) { this.DoNewMacro(this.ContextNode, true); }

        private void mnuTreeRename_Click(object sender, System.EventArgs e) { this.ContextNode.BeginEdit(); }

        private void mnuTreeSave_Click(object sender, System.EventArgs e) { this.DoSave(this.ContextNode, true); }

        #endregion

        #region TreeView Events

        private void tvMacroTree_BeforeLabelEdit(object sender, System.Windows.Forms.NodeLabelEditEventArgs e)
        {
            if (!this.IsRenameAllowed(e.Node))
            {
                e.CancelEdit = true;
            }
        }

        private void tvMacroTree_AfterLabelEdit(object sender, System.Windows.Forms.NodeLabelEditEventArgs e)
        {
            if (e.Label == null) // Use made no changes to the label text
            {
                return;
            }
            Character C = e.Node.Tag as Character;
            if (C != null)
            {
                C.Name = e.Label;
                return;
            }
            MacroFolder MF = e.Node.Tag as MacroFolder;
            if (MF != null)
            {
                MF.Name = e.Label;
                return;
            }
            Macro M = e.Node.Tag as Macro;
            if (M != null)
            {
                M.Name = e.Label;
                return;
            }
        }

        private void tvMacroTree_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
        {
            if (this.tvMacroTree.SelectedNode != null)
            {
                this.ShowProperties(this.tvMacroTree.SelectedNode.Tag as Macro);
            }
        }

        private void tvMacroTree_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (this.tvMacroTree.SelectedNode != null)
            {
                if (!e.Shift && !e.Control && !e.Alt)
                {
                    // Unmodified Keys
                    if (e.KeyData == Keys.F2) // F2 = Rename
                    {
                        this.tvMacroTree.SelectedNode.BeginEdit();
                    }
                    else if (e.KeyData == Keys.Delete)
                    {
                        if (this.IsDeleteAllowed(this.tvMacroTree.SelectedNode))
                        {
                            this.DoDelete(this.tvMacroTree.SelectedNode, true);
                        }
                        else if (this.IsClearAllowed(this.tvMacroTree.SelectedNode))
                        {
                            this.DoClear(this.tvMacroTree.SelectedNode, true);
                        }
                    }
                }
            }
        }

        #endregion

        #region Drag & Drop Events

        private bool CanBeMoved(TreeNode TN)
        {
            if (TN == null || TN.Tag == null)
            {
                return false;
            }
            else if (TN.Tag is SpecialMacroFolder)
            {
                return false;
            }
            else if (TN.Tag is Macro || TN.Tag is MacroFolder)
            {
                return this.CanAddOrRemoveItem(TN.Parent);
            }
            else
            {
                return false;
            }
        }

        private bool CanAddOrRemoveItem(TreeNode TN)
        {
            if (TN == null || TN.Tag == null)
            {
                return false;
            }
            else if (TN.Tag is MacroLibrary)
            {
                return true;
            }
            else if (TN.Tag is MacroFolder && !(TN.Tag is SpecialMacroFolder))
            {
                return !(TN.Tag as MacroFolder).Locked;
            }
            else
            {
                return false;
            }
        }

        private bool IsLocatedBelow(TreeNode Node, TreeNode PossibleAncestor)
        {
            if (Node == null || PossibleAncestor == null || Node == PossibleAncestor)
            {
                return false;
            }
            foreach (TreeNode TN in PossibleAncestor.Nodes)
            {
                if (TN == Node || this.IsLocatedBelow(Node, TN))
                {
                    return true;
                }
            }
            return false;
        }

        private void KillSubTree(TreeNode Node)
        {
            TreeNode Owner = Node.Parent;
            Node.Remove();
            if (Owner != null && Owner.Tag is MacroFolder)
            {
                MacroFolder MF = Owner.Tag as MacroFolder;
                if (Node.Tag is Macro)
                {
                    MF.Macros.Remove(Node.Tag as Macro);
                }
                else if (Node.Tag is MacroFolder)
                {
                    MF.Folders.Remove(Node.Tag as MacroFolder);
                }
            }
        }

        private void tvMacroTree_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        {
            TreeNode DropNode = this.tvMacroTree.GetNodeAt(this.tvMacroTree.PointToClient(new Point(e.X, e.Y)));
            if (DropNode == null)
            {
                return;
            }
            if (e.Data.GetDataPresent("FileDrop"))
            {
                try
                {
                    String[] FileNames = e.Data.GetData("FileDrop") as String[];
                    foreach (String MacroBarFile in FileNames)
                    {
                        this.AddMacroBar(MacroBarFile, DropNode);
                    }
                }
                catch {}
            }
            else if (e.Data.GetDataPresent(typeof (TreeNode)))
            {
                TreeNode DragNode = e.Data.GetData(typeof (TreeNode)) as TreeNode;
                if (DropNode.Tag is Macro)
                {
                    if (DragNode.Tag is Macro)
                    {
                        Macro Source = DragNode.Tag as Macro;
                        Macro Target = DropNode.Tag as Macro;
                        if (Target.Empty ||
                            MessageBox.Show(this,
                                String.Format("Are you sure you want to replace the '{0}' macro with the '{1}' macro?", Target.Name,
                                    Source.Name), "Confirm Replace", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                                MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                        {
                            Target.Name = Source.Name;
                            for (int i = 0; i < 6; ++i)
                            {
                                Target.Commands[i] = Source.Commands[i];
                            }
                            this.RefreshMacroNode(DropNode);
                            this.tvMacroTree.SelectedNode = DropNode;
                            if (e.Effect == DragDropEffects.Move)
                            {
                                this.KillSubTree(DragNode);
                            }
                        }
                    }
                }
                else if (DropNode.Tag is MacroFolder)
                {
                    MacroFolder DropFolder = DropNode.Tag as MacroFolder;
                    if (DragNode.Tag is Macro)
                    {
                        Macro DragMacro = (DragNode.Tag as Macro).Clone();
                        DropFolder.Macros.Add(DragMacro);
                        this.tvMacroTree.SelectedNode = this.AddMacroNode(DragMacro, DropNode);
                        if (e.Effect == DragDropEffects.Move)
                        {
                            this.KillSubTree(DragNode);
                        }
                    }
                    else if (DragNode.Tag is MacroFolder)
                    {
                        MacroFolder DragFolder = (DragNode.Tag as MacroFolder).Clone();
                        DropFolder.Folders.Add(DragFolder);
                        this.tvMacroTree.SelectedNode = this.AddMacroFolderNode(DragFolder, DropNode);
                        if (e.Effect == DragDropEffects.Move)
                        {
                            this.KillSubTree(DragNode);
                        }
                    }
                }
            }
        }

        private void tvMacroTree_DragOver(object sender, System.Windows.Forms.DragEventArgs e)
        {
            e.Effect = DragDropEffects.None;
            TreeNode DropNode = this.tvMacroTree.GetNodeAt(this.tvMacroTree.PointToClient(new Point(e.X, e.Y)));
            if (DropNode == null)
            {
                return;
            }
            if (e.Data.GetDataPresent(typeof (TreeNode)))
            {
                TreeNode DragNode = e.Data.GetData(typeof (TreeNode)) as TreeNode;
                if (DropNode != DragNode && !this.IsLocatedBelow(DropNode, DragNode))
                {
                    // Don't allow invalid tree moves
                    if ((DropNode.Tag is Macro && DragNode.Tag is Macro) || this.CanAddOrRemoveItem(DropNode))
                    {
                        e.Effect = DragDropEffects.Copy;
                        if ((e.AllowedEffect & DragDropEffects.Move) != 0 && (e.KeyState & 0x2c) != 0x08)
                        {
                            e.Effect = DragDropEffects.Move;
                        }
                    }
                }
            }
            else if (e.Data.GetDataPresent("FileDrop") && this.CanAddOrRemoveItem(DropNode))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void tvMacroTree_ItemDrag(object sender, System.Windows.Forms.ItemDragEventArgs e)
        {
            TreeNode DragNode = e.Item as TreeNode;
            if (DragNode == null || DragNode.Tag == null)
            {
                return;
            }
            else if (DragNode.Tag is MacroLibrary) // Can't drag a macro library
            {
                return;
            }
            else if (DragNode.Tag is Macro || DragNode.Tag is MacroFolder)
            {
                DragDropEffects AllowedEffects = DragDropEffects.Copy;
                if (this.CanBeMoved(DragNode))
                {
                    AllowedEffects |= DragDropEffects.Move;
                }
                this.DoDragDrop(DragNode, AllowedEffects);
            }
        }

        #endregion

        #region Other Events

        private void txtCommand_TextChanged(object sender, System.EventArgs e)
        {
            TextBoxBase TBB = sender as TextBoxBase;
            if (TBB != null && TBB.Enabled)
            {
                Macro M = this.tvMacroTree.SelectedNode.Tag as Macro;
                if (M != null)
                {
                    M.Commands[TBB.TabIndex] = TBB.Text;
                }
            }
        }

        #endregion
    }
}
