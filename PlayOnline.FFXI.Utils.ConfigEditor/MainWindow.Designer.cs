// Copyright © 2004-2014 Tim Van Holder, Windower Team
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS"
// BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

namespace PlayOnline.FFXI.Utils.ConfigEditor {

  public partial class MainWindow {

    private System.ComponentModel.Container components = null;

    protected override void Dispose(bool disposing) {
      if (disposing && components != null)
        components.Dispose();
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private void InitializeComponent() {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
      this.grpGlobalConfig = new System.Windows.Forms.GroupBox();
      this.txtSoundEffects = new System.Windows.Forms.TextBox();
      this.lblSoundEffects = new System.Windows.Forms.Label();
      this.txt3DHeight = new System.Windows.Forms.TextBox();
      this.txt3DWidth = new System.Windows.Forms.TextBox();
      this.txtGUIHeight = new System.Windows.Forms.TextBox();
      this.txtGUIWidth = new System.Windows.Forms.TextBox();
      this.lbl3DX = new System.Windows.Forms.Label();
      this.lblGUIX = new System.Windows.Forms.Label();
      this.lbl3DResolution = new System.Windows.Forms.Label();
      this.lblGUIResolution = new System.Windows.Forms.Label();
      this.lblWarning = new System.Windows.Forms.Label();
      this.picWarning = new System.Windows.Forms.PictureBox();
      this.grpCharConfig = new System.Windows.Forms.GroupBox();
      this.txtSample = new System.Windows.Forms.Label();
      this.lblColor16 = new System.Windows.Forms.Label();
      this.lblColor8 = new System.Windows.Forms.Label();
      this.lblColor15 = new System.Windows.Forms.Label();
      this.lblColor23 = new System.Windows.Forms.Label();
      this.lblColor7 = new System.Windows.Forms.Label();
      this.lblColor12 = new System.Windows.Forms.Label();
      this.lblColor13 = new System.Windows.Forms.Label();
      this.lblColor14 = new System.Windows.Forms.Label();
      this.lblColor11 = new System.Windows.Forms.Label();
      this.lblColor10 = new System.Windows.Forms.Label();
      this.lblColor9 = new System.Windows.Forms.Label();
      this.lblColor20 = new System.Windows.Forms.Label();
      this.lblColor21 = new System.Windows.Forms.Label();
      this.lblColor22 = new System.Windows.Forms.Label();
      this.lblColor19 = new System.Windows.Forms.Label();
      this.lblColor18 = new System.Windows.Forms.Label();
      this.lblColor17 = new System.Windows.Forms.Label();
      this.lblColor4 = new System.Windows.Forms.Label();
      this.lblColor5 = new System.Windows.Forms.Label();
      this.lblColor6 = new System.Windows.Forms.Label();
      this.lblColor3 = new System.Windows.Forms.Label();
      this.lblColor2 = new System.Windows.Forms.Label();
      this.lblColor1 = new System.Windows.Forms.Label();
      this.cmbCharacters = new System.Windows.Forms.ComboBox();
      this.btnClose = new System.Windows.Forms.Button();
      this.btnCancel = new System.Windows.Forms.Button();
      this.btnApply = new System.Windows.Forms.Button();
      this.dlgChooseColor = new System.Windows.Forms.ColorDialog();
      this.grpGlobalConfig.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize) (this.picWarning)).BeginInit();
      this.grpCharConfig.SuspendLayout();
      this.SuspendLayout();
      // 
      // grpGlobalConfig
      // 
      this.grpGlobalConfig.AccessibleDescription = null;
      this.grpGlobalConfig.AccessibleName = null;
      resources.ApplyResources(this.grpGlobalConfig, "grpGlobalConfig");
      this.grpGlobalConfig.BackgroundImage = null;
      this.grpGlobalConfig.Controls.Add(this.txtSoundEffects);
      this.grpGlobalConfig.Controls.Add(this.lblSoundEffects);
      this.grpGlobalConfig.Controls.Add(this.txt3DHeight);
      this.grpGlobalConfig.Controls.Add(this.txt3DWidth);
      this.grpGlobalConfig.Controls.Add(this.txtGUIHeight);
      this.grpGlobalConfig.Controls.Add(this.txtGUIWidth);
      this.grpGlobalConfig.Controls.Add(this.lbl3DX);
      this.grpGlobalConfig.Controls.Add(this.lblGUIX);
      this.grpGlobalConfig.Controls.Add(this.lbl3DResolution);
      this.grpGlobalConfig.Controls.Add(this.lblGUIResolution);
      this.grpGlobalConfig.Controls.Add(this.lblWarning);
      this.grpGlobalConfig.Controls.Add(this.picWarning);
      this.grpGlobalConfig.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.grpGlobalConfig.Font = null;
      this.grpGlobalConfig.Name = "grpGlobalConfig";
      this.grpGlobalConfig.TabStop = false;
      // 
      // txtSoundEffects
      // 
      this.txtSoundEffects.AccessibleDescription = null;
      this.txtSoundEffects.AccessibleName = null;
      resources.ApplyResources(this.txtSoundEffects, "txtSoundEffects");
      this.txtSoundEffects.BackgroundImage = null;
      this.txtSoundEffects.Font = null;
      this.txtSoundEffects.Name = "txtSoundEffects";
      this.txtSoundEffects.TextChanged += new System.EventHandler(this.Something_Changed);
      // 
      // lblSoundEffects
      // 
      this.lblSoundEffects.AccessibleDescription = null;
      this.lblSoundEffects.AccessibleName = null;
      resources.ApplyResources(this.lblSoundEffects, "lblSoundEffects");
      this.lblSoundEffects.Font = null;
      this.lblSoundEffects.Name = "lblSoundEffects";
      // 
      // txt3DHeight
      // 
      this.txt3DHeight.AccessibleDescription = null;
      this.txt3DHeight.AccessibleName = null;
      resources.ApplyResources(this.txt3DHeight, "txt3DHeight");
      this.txt3DHeight.BackgroundImage = null;
      this.txt3DHeight.Font = null;
      this.txt3DHeight.Name = "txt3DHeight";
      this.txt3DHeight.TextChanged += new System.EventHandler(this.Something_Changed);
      // 
      // txt3DWidth
      // 
      this.txt3DWidth.AccessibleDescription = null;
      this.txt3DWidth.AccessibleName = null;
      resources.ApplyResources(this.txt3DWidth, "txt3DWidth");
      this.txt3DWidth.BackgroundImage = null;
      this.txt3DWidth.Font = null;
      this.txt3DWidth.Name = "txt3DWidth";
      this.txt3DWidth.TextChanged += new System.EventHandler(this.Something_Changed);
      // 
      // txtGUIHeight
      // 
      this.txtGUIHeight.AccessibleDescription = null;
      this.txtGUIHeight.AccessibleName = null;
      resources.ApplyResources(this.txtGUIHeight, "txtGUIHeight");
      this.txtGUIHeight.BackgroundImage = null;
      this.txtGUIHeight.Font = null;
      this.txtGUIHeight.Name = "txtGUIHeight";
      this.txtGUIHeight.TextChanged += new System.EventHandler(this.Something_Changed);
      // 
      // txtGUIWidth
      // 
      this.txtGUIWidth.AccessibleDescription = null;
      this.txtGUIWidth.AccessibleName = null;
      resources.ApplyResources(this.txtGUIWidth, "txtGUIWidth");
      this.txtGUIWidth.BackgroundImage = null;
      this.txtGUIWidth.Font = null;
      this.txtGUIWidth.Name = "txtGUIWidth";
      this.txtGUIWidth.TextChanged += new System.EventHandler(this.Something_Changed);
      // 
      // lbl3DX
      // 
      this.lbl3DX.AccessibleDescription = null;
      this.lbl3DX.AccessibleName = null;
      resources.ApplyResources(this.lbl3DX, "lbl3DX");
      this.lbl3DX.Font = null;
      this.lbl3DX.Name = "lbl3DX";
      // 
      // lblGUIX
      // 
      this.lblGUIX.AccessibleDescription = null;
      this.lblGUIX.AccessibleName = null;
      resources.ApplyResources(this.lblGUIX, "lblGUIX");
      this.lblGUIX.Font = null;
      this.lblGUIX.Name = "lblGUIX";
      // 
      // lbl3DResolution
      // 
      this.lbl3DResolution.AccessibleDescription = null;
      this.lbl3DResolution.AccessibleName = null;
      resources.ApplyResources(this.lbl3DResolution, "lbl3DResolution");
      this.lbl3DResolution.Font = null;
      this.lbl3DResolution.Name = "lbl3DResolution";
      // 
      // lblGUIResolution
      // 
      this.lblGUIResolution.AccessibleDescription = null;
      this.lblGUIResolution.AccessibleName = null;
      resources.ApplyResources(this.lblGUIResolution, "lblGUIResolution");
      this.lblGUIResolution.Font = null;
      this.lblGUIResolution.Name = "lblGUIResolution";
      // 
      // lblWarning
      // 
      this.lblWarning.AccessibleDescription = null;
      this.lblWarning.AccessibleName = null;
      resources.ApplyResources(this.lblWarning, "lblWarning");
      this.lblWarning.Font = null;
      this.lblWarning.Name = "lblWarning";
      // 
      // picWarning
      // 
      this.picWarning.AccessibleDescription = null;
      this.picWarning.AccessibleName = null;
      resources.ApplyResources(this.picWarning, "picWarning");
      this.picWarning.BackgroundImage = null;
      this.picWarning.Font = null;
      this.picWarning.ImageLocation = null;
      this.picWarning.Name = "picWarning";
      this.picWarning.TabStop = false;
      // 
      // grpCharConfig
      // 
      this.grpCharConfig.AccessibleDescription = null;
      this.grpCharConfig.AccessibleName = null;
      resources.ApplyResources(this.grpCharConfig, "grpCharConfig");
      this.grpCharConfig.BackgroundImage = null;
      this.grpCharConfig.Controls.Add(this.txtSample);
      this.grpCharConfig.Controls.Add(this.lblColor16);
      this.grpCharConfig.Controls.Add(this.lblColor8);
      this.grpCharConfig.Controls.Add(this.lblColor15);
      this.grpCharConfig.Controls.Add(this.lblColor23);
      this.grpCharConfig.Controls.Add(this.lblColor7);
      this.grpCharConfig.Controls.Add(this.lblColor12);
      this.grpCharConfig.Controls.Add(this.lblColor13);
      this.grpCharConfig.Controls.Add(this.lblColor14);
      this.grpCharConfig.Controls.Add(this.lblColor11);
      this.grpCharConfig.Controls.Add(this.lblColor10);
      this.grpCharConfig.Controls.Add(this.lblColor9);
      this.grpCharConfig.Controls.Add(this.lblColor20);
      this.grpCharConfig.Controls.Add(this.lblColor21);
      this.grpCharConfig.Controls.Add(this.lblColor22);
      this.grpCharConfig.Controls.Add(this.lblColor19);
      this.grpCharConfig.Controls.Add(this.lblColor18);
      this.grpCharConfig.Controls.Add(this.lblColor17);
      this.grpCharConfig.Controls.Add(this.lblColor4);
      this.grpCharConfig.Controls.Add(this.lblColor5);
      this.grpCharConfig.Controls.Add(this.lblColor6);
      this.grpCharConfig.Controls.Add(this.lblColor3);
      this.grpCharConfig.Controls.Add(this.lblColor2);
      this.grpCharConfig.Controls.Add(this.lblColor1);
      this.grpCharConfig.Controls.Add(this.cmbCharacters);
      this.grpCharConfig.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.grpCharConfig.Font = null;
      this.grpCharConfig.Name = "grpCharConfig";
      this.grpCharConfig.TabStop = false;
      // 
      // txtSample
      // 
      this.txtSample.AccessibleDescription = null;
      this.txtSample.AccessibleName = null;
      resources.ApplyResources(this.txtSample, "txtSample");
      this.txtSample.BackColor = System.Drawing.Color.Black;
      this.txtSample.ForeColor = System.Drawing.Color.White;
      this.txtSample.Name = "txtSample";
      // 
      // lblColor16
      // 
      this.lblColor16.AccessibleDescription = null;
      this.lblColor16.AccessibleName = null;
      resources.ApplyResources(this.lblColor16, "lblColor16");
      this.lblColor16.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.lblColor16.Font = null;
      this.lblColor16.Name = "lblColor16";
      this.lblColor16.DoubleClick += new System.EventHandler(this.ColorLabel_DoubleClick);
      this.lblColor16.MouseLeave += new System.EventHandler(this.ColorLabel_MouseLeave);
      this.lblColor16.MouseEnter += new System.EventHandler(this.ColorLabel_MouseEnter);
      // 
      // lblColor8
      // 
      this.lblColor8.AccessibleDescription = null;
      this.lblColor8.AccessibleName = null;
      resources.ApplyResources(this.lblColor8, "lblColor8");
      this.lblColor8.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.lblColor8.Font = null;
      this.lblColor8.Name = "lblColor8";
      this.lblColor8.DoubleClick += new System.EventHandler(this.ColorLabel_DoubleClick);
      this.lblColor8.MouseLeave += new System.EventHandler(this.ColorLabel_MouseLeave);
      this.lblColor8.MouseEnter += new System.EventHandler(this.ColorLabel_MouseEnter);
      // 
      // lblColor15
      // 
      this.lblColor15.AccessibleDescription = null;
      this.lblColor15.AccessibleName = null;
      resources.ApplyResources(this.lblColor15, "lblColor15");
      this.lblColor15.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.lblColor15.Font = null;
      this.lblColor15.Name = "lblColor15";
      this.lblColor15.DoubleClick += new System.EventHandler(this.ColorLabel_DoubleClick);
      this.lblColor15.MouseLeave += new System.EventHandler(this.ColorLabel_MouseLeave);
      this.lblColor15.MouseEnter += new System.EventHandler(this.ColorLabel_MouseEnter);
      // 
      // lblColor23
      // 
      this.lblColor23.AccessibleDescription = null;
      this.lblColor23.AccessibleName = null;
      resources.ApplyResources(this.lblColor23, "lblColor23");
      this.lblColor23.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.lblColor23.Font = null;
      this.lblColor23.Name = "lblColor23";
      this.lblColor23.DoubleClick += new System.EventHandler(this.ColorLabel_DoubleClick);
      this.lblColor23.MouseLeave += new System.EventHandler(this.ColorLabel_MouseLeave);
      this.lblColor23.MouseEnter += new System.EventHandler(this.ColorLabel_MouseEnter);
      // 
      // lblColor7
      // 
      this.lblColor7.AccessibleDescription = null;
      this.lblColor7.AccessibleName = null;
      resources.ApplyResources(this.lblColor7, "lblColor7");
      this.lblColor7.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.lblColor7.Font = null;
      this.lblColor7.Name = "lblColor7";
      this.lblColor7.DoubleClick += new System.EventHandler(this.ColorLabel_DoubleClick);
      this.lblColor7.MouseLeave += new System.EventHandler(this.ColorLabel_MouseLeave);
      this.lblColor7.MouseEnter += new System.EventHandler(this.ColorLabel_MouseEnter);
      // 
      // lblColor12
      // 
      this.lblColor12.AccessibleDescription = null;
      this.lblColor12.AccessibleName = null;
      resources.ApplyResources(this.lblColor12, "lblColor12");
      this.lblColor12.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.lblColor12.Font = null;
      this.lblColor12.Name = "lblColor12";
      this.lblColor12.DoubleClick += new System.EventHandler(this.ColorLabel_DoubleClick);
      this.lblColor12.MouseLeave += new System.EventHandler(this.ColorLabel_MouseLeave);
      this.lblColor12.MouseEnter += new System.EventHandler(this.ColorLabel_MouseEnter);
      // 
      // lblColor13
      // 
      this.lblColor13.AccessibleDescription = null;
      this.lblColor13.AccessibleName = null;
      resources.ApplyResources(this.lblColor13, "lblColor13");
      this.lblColor13.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.lblColor13.Font = null;
      this.lblColor13.Name = "lblColor13";
      this.lblColor13.DoubleClick += new System.EventHandler(this.ColorLabel_DoubleClick);
      this.lblColor13.MouseLeave += new System.EventHandler(this.ColorLabel_MouseLeave);
      this.lblColor13.MouseEnter += new System.EventHandler(this.ColorLabel_MouseEnter);
      // 
      // lblColor14
      // 
      this.lblColor14.AccessibleDescription = null;
      this.lblColor14.AccessibleName = null;
      resources.ApplyResources(this.lblColor14, "lblColor14");
      this.lblColor14.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.lblColor14.Font = null;
      this.lblColor14.Name = "lblColor14";
      this.lblColor14.DoubleClick += new System.EventHandler(this.ColorLabel_DoubleClick);
      this.lblColor14.MouseLeave += new System.EventHandler(this.ColorLabel_MouseLeave);
      this.lblColor14.MouseEnter += new System.EventHandler(this.ColorLabel_MouseEnter);
      // 
      // lblColor11
      // 
      this.lblColor11.AccessibleDescription = null;
      this.lblColor11.AccessibleName = null;
      resources.ApplyResources(this.lblColor11, "lblColor11");
      this.lblColor11.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.lblColor11.Font = null;
      this.lblColor11.Name = "lblColor11";
      this.lblColor11.DoubleClick += new System.EventHandler(this.ColorLabel_DoubleClick);
      this.lblColor11.MouseLeave += new System.EventHandler(this.ColorLabel_MouseLeave);
      this.lblColor11.MouseEnter += new System.EventHandler(this.ColorLabel_MouseEnter);
      // 
      // lblColor10
      // 
      this.lblColor10.AccessibleDescription = null;
      this.lblColor10.AccessibleName = null;
      resources.ApplyResources(this.lblColor10, "lblColor10");
      this.lblColor10.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.lblColor10.Font = null;
      this.lblColor10.Name = "lblColor10";
      this.lblColor10.DoubleClick += new System.EventHandler(this.ColorLabel_DoubleClick);
      this.lblColor10.MouseLeave += new System.EventHandler(this.ColorLabel_MouseLeave);
      this.lblColor10.MouseEnter += new System.EventHandler(this.ColorLabel_MouseEnter);
      // 
      // lblColor9
      // 
      this.lblColor9.AccessibleDescription = null;
      this.lblColor9.AccessibleName = null;
      resources.ApplyResources(this.lblColor9, "lblColor9");
      this.lblColor9.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.lblColor9.Font = null;
      this.lblColor9.Name = "lblColor9";
      this.lblColor9.DoubleClick += new System.EventHandler(this.ColorLabel_DoubleClick);
      this.lblColor9.MouseLeave += new System.EventHandler(this.ColorLabel_MouseLeave);
      this.lblColor9.MouseEnter += new System.EventHandler(this.ColorLabel_MouseEnter);
      // 
      // lblColor20
      // 
      this.lblColor20.AccessibleDescription = null;
      this.lblColor20.AccessibleName = null;
      resources.ApplyResources(this.lblColor20, "lblColor20");
      this.lblColor20.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.lblColor20.Font = null;
      this.lblColor20.Name = "lblColor20";
      this.lblColor20.DoubleClick += new System.EventHandler(this.ColorLabel_DoubleClick);
      this.lblColor20.MouseLeave += new System.EventHandler(this.ColorLabel_MouseLeave);
      this.lblColor20.MouseEnter += new System.EventHandler(this.ColorLabel_MouseEnter);
      // 
      // lblColor21
      // 
      this.lblColor21.AccessibleDescription = null;
      this.lblColor21.AccessibleName = null;
      resources.ApplyResources(this.lblColor21, "lblColor21");
      this.lblColor21.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.lblColor21.Font = null;
      this.lblColor21.Name = "lblColor21";
      this.lblColor21.DoubleClick += new System.EventHandler(this.ColorLabel_DoubleClick);
      this.lblColor21.MouseLeave += new System.EventHandler(this.ColorLabel_MouseLeave);
      this.lblColor21.MouseEnter += new System.EventHandler(this.ColorLabel_MouseEnter);
      // 
      // lblColor22
      // 
      this.lblColor22.AccessibleDescription = null;
      this.lblColor22.AccessibleName = null;
      resources.ApplyResources(this.lblColor22, "lblColor22");
      this.lblColor22.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.lblColor22.Font = null;
      this.lblColor22.Name = "lblColor22";
      this.lblColor22.DoubleClick += new System.EventHandler(this.ColorLabel_DoubleClick);
      this.lblColor22.MouseLeave += new System.EventHandler(this.ColorLabel_MouseLeave);
      this.lblColor22.MouseEnter += new System.EventHandler(this.ColorLabel_MouseEnter);
      // 
      // lblColor19
      // 
      this.lblColor19.AccessibleDescription = null;
      this.lblColor19.AccessibleName = null;
      resources.ApplyResources(this.lblColor19, "lblColor19");
      this.lblColor19.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.lblColor19.Font = null;
      this.lblColor19.Name = "lblColor19";
      this.lblColor19.DoubleClick += new System.EventHandler(this.ColorLabel_DoubleClick);
      this.lblColor19.MouseLeave += new System.EventHandler(this.ColorLabel_MouseLeave);
      this.lblColor19.MouseEnter += new System.EventHandler(this.ColorLabel_MouseEnter);
      // 
      // lblColor18
      // 
      this.lblColor18.AccessibleDescription = null;
      this.lblColor18.AccessibleName = null;
      resources.ApplyResources(this.lblColor18, "lblColor18");
      this.lblColor18.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.lblColor18.Font = null;
      this.lblColor18.Name = "lblColor18";
      this.lblColor18.DoubleClick += new System.EventHandler(this.ColorLabel_DoubleClick);
      this.lblColor18.MouseLeave += new System.EventHandler(this.ColorLabel_MouseLeave);
      this.lblColor18.MouseEnter += new System.EventHandler(this.ColorLabel_MouseEnter);
      // 
      // lblColor17
      // 
      this.lblColor17.AccessibleDescription = null;
      this.lblColor17.AccessibleName = null;
      resources.ApplyResources(this.lblColor17, "lblColor17");
      this.lblColor17.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.lblColor17.Font = null;
      this.lblColor17.Name = "lblColor17";
      this.lblColor17.DoubleClick += new System.EventHandler(this.ColorLabel_DoubleClick);
      this.lblColor17.MouseLeave += new System.EventHandler(this.ColorLabel_MouseLeave);
      this.lblColor17.MouseEnter += new System.EventHandler(this.ColorLabel_MouseEnter);
      // 
      // lblColor4
      // 
      this.lblColor4.AccessibleDescription = null;
      this.lblColor4.AccessibleName = null;
      resources.ApplyResources(this.lblColor4, "lblColor4");
      this.lblColor4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.lblColor4.Font = null;
      this.lblColor4.Name = "lblColor4";
      this.lblColor4.DoubleClick += new System.EventHandler(this.ColorLabel_DoubleClick);
      this.lblColor4.MouseLeave += new System.EventHandler(this.ColorLabel_MouseLeave);
      this.lblColor4.MouseEnter += new System.EventHandler(this.ColorLabel_MouseEnter);
      // 
      // lblColor5
      // 
      this.lblColor5.AccessibleDescription = null;
      this.lblColor5.AccessibleName = null;
      resources.ApplyResources(this.lblColor5, "lblColor5");
      this.lblColor5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.lblColor5.Font = null;
      this.lblColor5.Name = "lblColor5";
      this.lblColor5.DoubleClick += new System.EventHandler(this.ColorLabel_DoubleClick);
      this.lblColor5.MouseLeave += new System.EventHandler(this.ColorLabel_MouseLeave);
      this.lblColor5.MouseEnter += new System.EventHandler(this.ColorLabel_MouseEnter);
      // 
      // lblColor6
      // 
      this.lblColor6.AccessibleDescription = null;
      this.lblColor6.AccessibleName = null;
      resources.ApplyResources(this.lblColor6, "lblColor6");
      this.lblColor6.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.lblColor6.Font = null;
      this.lblColor6.Name = "lblColor6";
      this.lblColor6.DoubleClick += new System.EventHandler(this.ColorLabel_DoubleClick);
      this.lblColor6.MouseLeave += new System.EventHandler(this.ColorLabel_MouseLeave);
      this.lblColor6.MouseEnter += new System.EventHandler(this.ColorLabel_MouseEnter);
      // 
      // lblColor3
      // 
      this.lblColor3.AccessibleDescription = null;
      this.lblColor3.AccessibleName = null;
      resources.ApplyResources(this.lblColor3, "lblColor3");
      this.lblColor3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.lblColor3.Font = null;
      this.lblColor3.Name = "lblColor3";
      this.lblColor3.DoubleClick += new System.EventHandler(this.ColorLabel_DoubleClick);
      this.lblColor3.MouseLeave += new System.EventHandler(this.ColorLabel_MouseLeave);
      this.lblColor3.MouseEnter += new System.EventHandler(this.ColorLabel_MouseEnter);
      // 
      // lblColor2
      // 
      this.lblColor2.AccessibleDescription = null;
      this.lblColor2.AccessibleName = null;
      resources.ApplyResources(this.lblColor2, "lblColor2");
      this.lblColor2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.lblColor2.Font = null;
      this.lblColor2.Name = "lblColor2";
      this.lblColor2.DoubleClick += new System.EventHandler(this.ColorLabel_DoubleClick);
      this.lblColor2.MouseLeave += new System.EventHandler(this.ColorLabel_MouseLeave);
      this.lblColor2.MouseEnter += new System.EventHandler(this.ColorLabel_MouseEnter);
      // 
      // lblColor1
      // 
      this.lblColor1.AccessibleDescription = null;
      this.lblColor1.AccessibleName = null;
      resources.ApplyResources(this.lblColor1, "lblColor1");
      this.lblColor1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.lblColor1.Font = null;
      this.lblColor1.Name = "lblColor1";
      this.lblColor1.DoubleClick += new System.EventHandler(this.ColorLabel_DoubleClick);
      this.lblColor1.MouseLeave += new System.EventHandler(this.ColorLabel_MouseLeave);
      this.lblColor1.MouseEnter += new System.EventHandler(this.ColorLabel_MouseEnter);
      // 
      // cmbCharacters
      // 
      this.cmbCharacters.AccessibleDescription = null;
      this.cmbCharacters.AccessibleName = null;
      resources.ApplyResources(this.cmbCharacters, "cmbCharacters");
      this.cmbCharacters.BackgroundImage = null;
      this.cmbCharacters.DisplayMember = "CharacterName";
      this.cmbCharacters.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cmbCharacters.Font = null;
      this.cmbCharacters.FormattingEnabled = true;
      this.cmbCharacters.Name = "cmbCharacters";
      this.cmbCharacters.SelectedIndexChanged += new System.EventHandler(this.cmbCharacters_SelectedIndexChanged);
      // 
      // btnClose
      // 
      this.btnClose.AccessibleDescription = null;
      this.btnClose.AccessibleName = null;
      resources.ApplyResources(this.btnClose, "btnClose");
      this.btnClose.BackgroundImage = null;
      this.btnClose.Font = null;
      this.btnClose.Name = "btnClose";
      this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
      // 
      // btnCancel
      // 
      this.btnCancel.AccessibleDescription = null;
      this.btnCancel.AccessibleName = null;
      resources.ApplyResources(this.btnCancel, "btnCancel");
      this.btnCancel.BackgroundImage = null;
      this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnCancel.Font = null;
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
      // 
      // btnApply
      // 
      this.btnApply.AccessibleDescription = null;
      this.btnApply.AccessibleName = null;
      resources.ApplyResources(this.btnApply, "btnApply");
      this.btnApply.BackgroundImage = null;
      this.btnApply.Font = null;
      this.btnApply.Name = "btnApply";
      this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
      // 
      // dlgChooseColor
      // 
      this.dlgChooseColor.AnyColor = true;
      this.dlgChooseColor.FullOpen = true;
      // 
      // MainWindow
      // 
      this.AccessibleDescription = null;
      this.AccessibleName = null;
      resources.ApplyResources(this, "$this");
      this.BackgroundImage = null;
      this.Controls.Add(this.btnApply);
      this.Controls.Add(this.btnCancel);
      this.Controls.Add(this.btnClose);
      this.Controls.Add(this.grpCharConfig);
      this.Controls.Add(this.grpGlobalConfig);
      this.Font = null;
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.Icon = null;
      this.MaximizeBox = false;
      this.Name = "MainWindow";
      this.grpGlobalConfig.ResumeLayout(false);
      this.grpGlobalConfig.PerformLayout();
      ((System.ComponentModel.ISupportInitialize) (this.picWarning)).EndInit();
      this.grpCharConfig.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.GroupBox grpGlobalConfig;
    private System.Windows.Forms.Label lblGUIResolution;
    private System.Windows.Forms.TextBox txtGUIWidth;
    private System.Windows.Forms.Label lblGUIX;
    private System.Windows.Forms.TextBox txtGUIHeight;
    private System.Windows.Forms.Label lbl3DResolution;
    private System.Windows.Forms.TextBox txt3DWidth;
    private System.Windows.Forms.Label lbl3DX;
    private System.Windows.Forms.TextBox txt3DHeight;
    private System.Windows.Forms.PictureBox picWarning;
    private System.Windows.Forms.Label lblWarning;
    private System.Windows.Forms.GroupBox grpCharConfig;
    private System.Windows.Forms.ComboBox cmbCharacters;
    private System.Windows.Forms.Button btnClose;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.Button btnApply;
    private System.Windows.Forms.Label lblColor1;
    private System.Windows.Forms.Label lblColor2;
    private System.Windows.Forms.Label lblColor3;
    private System.Windows.Forms.Label lblColor6;
    private System.Windows.Forms.Label lblColor5;
    private System.Windows.Forms.Label lblColor4;
    private System.Windows.Forms.Label lblColor20;
    private System.Windows.Forms.Label lblColor21;
    private System.Windows.Forms.Label lblColor22;
    private System.Windows.Forms.Label lblColor19;
    private System.Windows.Forms.Label lblColor18;
    private System.Windows.Forms.Label lblColor17;
    private System.Windows.Forms.Label lblColor12;
    private System.Windows.Forms.Label lblColor13;
    private System.Windows.Forms.Label lblColor14;
    private System.Windows.Forms.Label lblColor11;
    private System.Windows.Forms.Label lblColor10;
    private System.Windows.Forms.Label lblColor9;
    private System.Windows.Forms.Label lblColor15;
    private System.Windows.Forms.Label lblColor23;
    private System.Windows.Forms.Label lblColor7;
    private System.Windows.Forms.Label lblColor16;
    private System.Windows.Forms.Label lblColor8;
    private System.Windows.Forms.ColorDialog dlgChooseColor;
    private System.Windows.Forms.TextBox txtSoundEffects;
    private System.Windows.Forms.Label lblSoundEffects;
    private System.Windows.Forms.Label txtSample;

  }

}
