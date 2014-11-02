// Copyright © 2004-2014 Tim Van Holder, Nevin Stepan, Windower Team
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS"
// BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

using PlayOnline.Core;
using PlayOnline.FFXI;
using PlayOnline.FFXI.Things;

namespace MassExtractor {

  internal class Program {

    private static bool useTest = false;

    private static void ExtractFile(int FileNumber, string OutputFile) {
    string ROMPath;
      if (useTest == true)
    ROMPath = FFXI.GetFilePath(FileNumber, AppID.FFXITC);
      else
    ROMPath = FFXI.GetFilePath(FileNumber);
      if (ROMPath == null || !File.Exists(ROMPath)) {
	Console.ForegroundColor = ConsoleColor.Red;
	Console.WriteLine(I18N.GetText("BadFileID"), FileNumber, OutputFile);
	Console.ForegroundColor = ConsoleColor.White;
	return;
      }
      Program.ExtractFile(ROMPath, OutputFile);
    }

    private static void ExtractFile(string ROMPath, string OutputFile) {
      try {
	Console.Write(I18N.GetText("Extracting"), Path.GetFileName(OutputFile));
      ThingList KnownData = FileType.LoadAll(ROMPath, null);
	Console.ForegroundColor = ConsoleColor.White;
	Console.Write(I18N.GetText("Load"));
	if (KnownData != null) {
	  Console.ForegroundColor = ConsoleColor.Green;
	  Console.Write(I18N.GetText("OK"));
	bool SaveOK = KnownData.Save(OutputFile);
	  Console.ForegroundColor = ConsoleColor.White;
	  Console.Write(I18N.GetText("Save"));
	  if (SaveOK) {
	    Console.ForegroundColor = ConsoleColor.Green;
	    Console.WriteLine(I18N.GetText("OK"));
	  }
	  else {
	    Console.ForegroundColor = ConsoleColor.Red;
	    Console.WriteLine(I18N.GetText("FAILED"));
	  }
	  KnownData.Clear();
	}
	else {
	  Console.ForegroundColor = ConsoleColor.Red;
	  Console.WriteLine(I18N.GetText("FAILED"));
	}
      }
      catch (Exception E) {
	Console.ForegroundColor = ConsoleColor.Red;
	Console.WriteLine(I18N.GetText("Exception"), E.Message);
      }
      finally {
	Console.ForegroundColor = ConsoleColor.White;
      }
    }

    [STAThread]
    static int Main() {
      Console.Title = I18N.GetText("ConsoleTitle");
      Console.ForegroundColor = ConsoleColor.White;
      try {
      string FFXIFolder = null;
	if (FFXIFolder == null) FFXIFolder = POL.GetApplicationPath(AppID.FFXI, POL.Region.Japan);
	if (FFXIFolder == null) FFXIFolder = POL.GetApplicationPath(AppID.FFXI, POL.Region.NorthAmerica);
	if (FFXIFolder == null) FFXIFolder = POL.GetApplicationPath(AppID.FFXI, POL.Region.Europe);
	if (FFXIFolder == null) {
	  Console.ForegroundColor = ConsoleColor.Red;
	  Console.WriteLine(I18N.GetText("NoFFXI"));
	  return 1;
	}
      string OutputFolder = null;
      bool   ScanAllFiles = false;
	{
	string[] Args = Environment.GetCommandLineArgs();
    for (int count = 0; count < Args.Length; count++) {
      if (Args[count].ToLower() == "-test") {
        useTest = true;
        try {
          FFXIFolder = null;
          if (FFXIFolder == null) FFXIFolder = POL.GetApplicationPath(AppID.FFXITC, POL.Region.Japan);
          if (FFXIFolder == null) FFXIFolder = POL.GetApplicationPath(AppID.FFXITC, POL.Region.NorthAmerica);
          if (FFXIFolder == null) FFXIFolder = POL.GetApplicationPath(AppID.FFXITC, POL.Region.Europe);
          if (FFXIFolder == null) {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(I18N.GetText("NoFFXITC"));
            return 1;
          }
        }
        catch (Exception E) {
          Console.ForegroundColor = ConsoleColor.Red;
          Console.WriteLine(I18N.GetText("Exception"), E.Message);
          return 1;
        }
        finally { Console.ResetColor();}
    }
    else if (Args[count].ToLower() == "-full-scan")
      ScanAllFiles = true;
    else
      OutputFolder = Args[count];
    }
	  /*if (Args.Length > 1) {
	    OutputFolder = Args[1];
	    if (OutputFolder == "-full-scan") {
	      OutputFolder = null;
	      ScanAllFiles = true;
	      if (Args.Length > 2)
		OutputFolder = Args[2];
	    }
	  }*/
	  if (OutputFolder == null) {
	    Console.ForegroundColor = ConsoleColor.Red;
	    Console.WriteLine(I18N.GetText("Usage"), Args[0]);
	    return 1;
	  }
	  OutputFolder = Path.GetFullPath(OutputFolder);
	  if (!Directory.Exists(OutputFolder)) {
	    Console.WriteLine(I18N.GetText("Creating"), OutputFolder);
	    Directory.CreateDirectory(OutputFolder);
	  }
	}
      DateTime ExtractionStart = DateTime.Now;
	Console.WriteLine(I18N.GetText("SourceFolder"), FFXIFolder);
	Console.WriteLine(I18N.GetText("TargetFolder"), OutputFolder);
	Console.WriteLine(String.Format(I18N.GetText("StartTime"), ExtractionStart));
	if (ScanAllFiles)
	  Program.DoFullFileScan(FFXIFolder, OutputFolder);
	else { // Process "known" files
	  Directory.SetCurrentDirectory(OutputFolder);
	  Console.Write(I18N.GetText("DumpFileTable"));
	  { // Dump File Table
	  StreamWriter FileTable = new StreamWriter("file-table.csv", false, Encoding.UTF8);
	    FileTable.WriteLine("\"File ID\"\t\"ROMDir\"\t\"Dir\"\t\"File\"");
	    for (byte i = 1; i < 20; ++i) {
	    string Suffix = "";
	    string DataDir = FFXIFolder;
	      if (i > 1) {
		Suffix = i.ToString();
		DataDir = Path.Combine(DataDir, "Rom" + Suffix);
	      }
	    string VTableFile = Path.Combine(DataDir, String.Format("VTABLE{0}.DAT", Suffix));
	    string FTableFile = Path.Combine(DataDir, String.Format("FTABLE{0}.DAT", Suffix));
	      if (i == 1) // add the Rom now (not needed for the *TABLE.DAT, but needed for the other DAT paths)
		DataDir = Path.Combine(DataDir, "Rom");
	      if (System.IO.File.Exists(VTableFile) && System.IO.File.Exists(FTableFile)) {
		try {
		BinaryReader VBR = new BinaryReader(new FileStream(VTableFile, FileMode.Open, FileAccess.Read, FileShare.Read));
		BinaryReader FBR = new BinaryReader(new FileStream(FTableFile, FileMode.Open, FileAccess.Read, FileShare.Read));
		long FileCount = VBR.BaseStream.Length;
		  for (long j = 0; j < FileCount; ++j) {
		    if (VBR.ReadByte() == i) {
		      FBR.BaseStream.Seek(2 * j, SeekOrigin.Begin);
		    ushort FileDir = FBR.ReadUInt16();
		    short Dir = (short) (FileDir / 0x80);
		    byte File = (byte)  (FileDir % 0x80);
		      FileTable.WriteLine("{0}\t\"{1}\"\t\"{2}\"\t\"{3}\"", j, Path.GetFileName(DataDir), Dir.ToString(), Path.ChangeExtension(File.ToString(), ".dat"));
		    }
		  }
		  FBR.Close();
		  VBR.Close();
		}
		catch { }
	      }
	    }
	    FileTable.Close();
	    Console.ForegroundColor = ConsoleColor.Green;
	    Console.WriteLine(I18N.GetText("OK"));
	    Console.ForegroundColor = ConsoleColor.White;
      }
      //resources files
      if (!Directory.Exists("resources"))
          Directory.CreateDirectory("resources");

      Program.ExtractFile(81, "resources/spellAbilCommon.xml");

      Program.ExtractFile(73, "resources/generalE.xml");
      Program.ExtractFile(74, "resources/usableE.xml");
      Program.ExtractFile(75, "resources/weaponE.xml");
      Program.ExtractFile(76, "resources/armorE.xml");
      Program.ExtractFile(55668, "resources/armor2E.xml");
      Program.ExtractFile(77, "resources/puppetE.xml");
      Program.ExtractFile(91, "resources/currencyE.xml");
      Program.ExtractFile(55667, "resources/slipE.xml");
      Program.ExtractFile(55701, "resources/abilE.xml");
      Program.ExtractFile(55465, "resources/areaE.xml");
      Program.ExtractFile(55702, "resources/spellE.xml");
      Program.ExtractFile(55726, "resources/statusE.xml");

      Program.ExtractFile(56235, "resources/generalF.xml");
      Program.ExtractFile(56211, "resources/general2F.xml");
      Program.ExtractFile(56236, "resources/usableF.xml");
      Program.ExtractFile(56237, "resources/weaponF.xml");
      Program.ExtractFile(56238, "resources/armorF.xml");
      Program.ExtractFile(56208, "resources/armor2F.xml");
      Program.ExtractFile(56239, "resources/puppetF.xml");
      Program.ExtractFile(56240, "resources/currencyF.xml");
      Program.ExtractFile(56207, "resources/slipF.xml");
      Program.ExtractFile(56241, "resources/abilF.xml");
      Program.ExtractFile(56195, "resources/areaF.xml");
      Program.ExtractFile(56242, "resources/spellF.xml");
      Program.ExtractFile(56272, "resources/statusF.xml");

      Program.ExtractFile(55815, "resources/generalD.xml");
      Program.ExtractFile(55791, "resources/general2D.xml");
      Program.ExtractFile(55816, "resources/usableD.xml");
      Program.ExtractFile(55817, "resources/weaponD.xml");
      Program.ExtractFile(55818, "resources/armorD.xml");
      Program.ExtractFile(55788, "resources/armor2D.xml");
      Program.ExtractFile(55819, "resources/puppetD.xml");
      Program.ExtractFile(55820, "resources/currencyD.xml");
      Program.ExtractFile(55787, "resources/slipD.xml");
      Program.ExtractFile(55821, "resources/abilD.xml");
      Program.ExtractFile(55775, "resources/areaD.xml");
      Program.ExtractFile(55822, "resources/spellD.xml");
      Program.ExtractFile(55852, "resources/statusD.xml");

      Program.ExtractFile(4, "resources/generalJ.xml");
      Program.ExtractFile(55551, "resources/general2J.xml");
      Program.ExtractFile(5, "resources/usableJ.xml");
      Program.ExtractFile(6, "resources/weaponJ.xml");
      Program.ExtractFile(7, "resources/armorJ.xml");
      Program.ExtractFile(55548, "resources/armor2J.xml");
      Program.ExtractFile(8, "resources/puppetJ.xml");
      Program.ExtractFile(9, "resources/currencyJ.xml");
      Program.ExtractFile(55547, "resources/slipJ.xml");
      Program.ExtractFile(55581, "resources/abilJ.xml");
      Program.ExtractFile(55535, "resources/areaJ.xml");
      Program.ExtractFile(55582, "resources/spellJ.xml");
      Program.ExtractFile(55605, "resources/statusJ.xml");
	  // Interesting Data
	  Program.ExtractFile(11, "old-spells-1.xml");
	  Program.ExtractFile(73, "items-general.xml");
	   Program.ExtractFile(55671, "items-general2.xml");
	  Program.ExtractFile(74, "items-usable.xml");
	  Program.ExtractFile(75, "items-weapons.xml");
	  Program.ExtractFile(76, "items-armor.xml");
      Program.ExtractFile(55668, "items-armor2.xml");
	  Program.ExtractFile(77, "items-puppet.xml");
	  Program.ExtractFile(82, "quests.xml");
	  Program.ExtractFile(85, "old-abilities.xml");
	  Program.ExtractFile(86, "old-spells-2.xml");
      Program.ExtractFile(87, "old-statuses.xml");
      Program.ExtractFile(91, "items-currency.xml");
      Program.ExtractFile(55667, "items-voucher-slip.xml");
	  // Dialog Tables
	  for (ushort i = 0; i < 0x100; ++i)
	    Program.ExtractFile(6420 + i, String.Format("dialog-table-{0:000}.xml", i));
      // Whitegate's 2nd dialog table
      Program.ExtractFile(57945, "dialog-table-50-2.xml");
	  // Mob Lists
	  for (ushort i = 0; i < 0x100; ++i)
	    Program.ExtractFile(6720 + i, String.Format("mob-list-{0:000}.xml", i));
	  for (ushort i = 0; i < 0x100; ++i)
	    Program.ExtractFile(86491 + i, String.Format("mob-list-{0:000}.xml", i+255));
	  for (ushort i = 0; i < 0x100; ++i)
	    Program.ExtractFile(67910 + i, String.Format("mob-list2-{0:000}.xml", i));
	  // String Tables
	  Program.ExtractFile(55465, "area-names.xml");
	  Program.ExtractFile(55466, "area-names-search.xml");
	  Program.ExtractFile(55467, "job-names.xml");
	  Program.ExtractFile(55468, "job-names-short.xml");
	  Program.ExtractFile(55469, "race-names.xml");
	  Program.ExtractFile(55470, "character-selection-strings.xml");
	  Program.ExtractFile(55471, "equipment-locations.xml");
	  // More String Tables
	  Program.ExtractFile(55645, "various-1.xml");
	  Program.ExtractFile(55646, "error-messages.xml");
	  Program.ExtractFile(55647, "pol-messages.xml");
	  Program.ExtractFile(55648, "ingame-messages-1.xml");
	  Program.ExtractFile(55649, "ingame-messages-2.xml");
	  Program.ExtractFile(55650, "chat-filter-types.xml");
	  Program.ExtractFile(55651, "menu-item-description.xml");
	  Program.ExtractFile(55652, "menu-item-text.xml");
	  Program.ExtractFile(55653, "various-2.xml");
	  Program.ExtractFile(55654, "region-names.xml");
	  Program.ExtractFile(55657, "weather-types.xml");
	  Program.ExtractFile(55658, "day-names.xml");
	  Program.ExtractFile(55659, "directions.xml");
	  Program.ExtractFile(55660, "moon-phases.xml");
	  Program.ExtractFile(55661, "area-names-alternate.xml");
	  // String Tables That Used To Be Special Formats
	  Program.ExtractFile(55695, "key-items.xml");
	  Program.ExtractFile(55701, "ability-names.xml");
	  Program.ExtractFile(55702, "spell-names.xml");
	  Program.ExtractFile(55704, "titles.xml");
	  Program.ExtractFile(55706, "quests-sandoria.xml");
	  Program.ExtractFile(55707, "quests-bastok.xml");
	  Program.ExtractFile(55708, "quests-windurst.xml");
	  Program.ExtractFile(55709, "quests-jeuno.xml");
	  Program.ExtractFile(55710, "quests-other.xml");
	  Program.ExtractFile(55711, "quests-zilart.xml");
	  Program.ExtractFile(55712, "quests-ahtuhrgan.xml");
	  Program.ExtractFile(55715, "missions-sandoria.xml");
	  Program.ExtractFile(55716, "missions-bastok.xml");
	  Program.ExtractFile(55717, "missions-windurst.xml");
	  Program.ExtractFile(55718, "missions-zilart.xml");
	  Program.ExtractFile(55719, "missions-promathia.xml");
	  Program.ExtractFile(55720, "missions-assault.xml");
	  Program.ExtractFile(55721, "missions-ahtuhrgan.xml");
	  Program.ExtractFile(55722, "quests-goddess.xml");
	  Program.ExtractFile(55723, "missions-goddess.xml");
      Program.ExtractFile(55724, "missions-campaign.xml");
      Program.ExtractFile(55713, "quests-abyssea.xml");
      Program.ExtractFile(55735, "missions-prophecy.xml");
      Program.ExtractFile(55736, "missions-moogle.xml");
      Program.ExtractFile(55737, "missions-shantotto.xml");
	  Program.ExtractFile(55725, "status-names.xml");
	  Program.ExtractFile(55733, "ability-descriptions.xml");
	  Program.ExtractFile(55734, "spell-descriptions.xml");
	}
      DateTime ExtractionEnd = DateTime.Now;
	Console.WriteLine(String.Format(I18N.GetText("EndTime"), ExtractionEnd));
	Console.WriteLine(String.Format(I18N.GetText("ElapsedTime"), ExtractionEnd - ExtractionStart));
	return 0;
      }
      catch (Exception E) {
	Console.ForegroundColor = ConsoleColor.Red;
	Console.WriteLine(I18N.GetText("Exception"), E.Message);
	return 1;
      }
      finally { Console.ResetColor(); }
    }

    private static void DoFullFileScan(string FFXIFolder, string OutputFolder) {
      Directory.SetCurrentDirectory(FFXIFolder);
      for (int i = 1; i < 20; ++i) {
      string ROMFolder = "Rom";
	if (i > 1)
	  ROMFolder += i.ToString();
	if (Directory.Exists(ROMFolder)) {
	  for (int j = 0; j < 1000; ++j) {
	  string ROMSubFolder = Path.Combine(ROMFolder, j.ToString());
	    if (Directory.Exists(ROMSubFolder)) {
	      Console.WriteLine(I18N.GetText("Scanning"), ROMSubFolder);
	    long Files      = 0;
	    long KnownFiles = 0;
	      for (int k = 0; k < 128; ++k) {
	      string ROMFile = Path.Combine(ROMSubFolder, String.Format("{0}.DAT", k));
		if (File.Exists(ROMFile)) {
		  try {
		  ThingList KnownData = FileType.LoadAll(ROMFile, null);
		    if (KnownData != null && KnownData.Count > 0) {
		      Console.WriteLine(I18N.GetText("ExtractingAll"), KnownData.Count, ROMFile);
		      ++KnownFiles;
		    ThingList<Graphic> Images = new ThingList<Graphic>();
		    ThingList NonImages = new ThingList();
		      foreach (IThing T in KnownData) {
			if (T is Graphic)
			  Images.Add(T as Graphic);
			else
			  NonImages.Add(T);
		      }
		      KnownData.Clear();
		      if (Images.Count == 1) {
		      Image I = Images[0].GetFieldValue("image") as Image;
			if (I != null) {
			string Category  = Images[0].GetFieldText("category");
			string ID        = Images[0].GetFieldText("id");
			string ImageFile = String.Format("{0}-{1}-{2} - ({3}) {4}.png", i, j, k, Category, ID);
			  I.Save(Path.Combine(OutputFolder, ImageFile), ImageFormat.Png);
			}
		      }
		      else if (Images.Count > 0) {
		      string ImageFolder = Path.Combine(OutputFolder, String.Format("{0}-{1}-{2}", i, j, k));
		      int    ImageIndex  = 0;
			foreach (Graphic G in Images) {
			Image I = G.GetFieldValue("image") as Image;
			  if (I != null) {
			    if (!Directory.Exists(ImageFolder))
			      Directory.CreateDirectory(ImageFolder);
			  string Category  = G.GetFieldText("category");
			  string ID        = G.GetFieldText("id");
			  string ImageFile = String.Format("{0} - ({1}) {2}.png", ++ImageIndex, Category, ID);
			    I.Save(Path.Combine(ImageFolder, ImageFile), ImageFormat.Png);
			  }
			}
		      }
		      Images.Clear();
		      if (NonImages.Count > 0)
			NonImages.Save(Path.Combine(OutputFolder, String.Format("{0}-{1}-{2}.xml", i, j, k)));
		      NonImages.Clear();
		    }
		    ++Files;
		  }
		  catch (Exception E) {
		    Console.ForegroundColor = ConsoleColor.Red;
		    Console.WriteLine(I18N.GetText("Exception"), E.Message);
		    Console.ForegroundColor = ConsoleColor.White;
		  }
		}
	      }
	      Console.WriteLine(" => {0} of {1} file(s) contained recogniseable data", KnownFiles, Files);
	    }
	  }
	}
      }
    }

  }

}
