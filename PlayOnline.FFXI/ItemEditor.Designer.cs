// Copyright © 2004-2014 Tim Van Holder, Windower Team
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS"
// BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

namespace PlayOnline.FFXI {

  public partial class ItemEditor {

    private System.ComponentModel.IContainer components;

    protected override void Dispose(bool disposing) {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    #region Component Designer generated code

    private void InitializeComponent() {
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ItemEditor));
      this.grpEquipmentInfo = new System.Windows.Forms.GroupBox();
      this.txtRaces = new System.Windows.Forms.TextBox();
      this.txtSlots = new System.Windows.Forms.TextBox();
      this.txtJobs = new System.Windows.Forms.TextBox();
      this.txtLevel = new System.Windows.Forms.TextBox();
      this.lblRaces = new System.Windows.Forms.Label();
      this.lblSlots = new System.Windows.Forms.Label();
      this.lblJobs = new System.Windows.Forms.Label();
      this.lblLevel = new System.Windows.Forms.Label();
      this.grpCommonInfo = new System.Windows.Forms.GroupBox();
      this.txtValidTargets = new System.Windows.Forms.TextBox();
      this.txtName = new System.Windows.Forms.TextBox();
      this.txtDescription = new System.Windows.Forms.TextBox();
      this.txtStackSize = new System.Windows.Forms.TextBox();
      this.txtFlags = new System.Windows.Forms.TextBox();
      this.txtType = new System.Windows.Forms.TextBox();
      this.txtID = new System.Windows.Forms.TextBox();
      this.picIcon = new System.Windows.Forms.PictureBox();
      this.lblValidTargets = new System.Windows.Forms.Label();
      this.lblDescription = new System.Windows.Forms.Label();
      this.lblName = new System.Windows.Forms.Label();
      this.lblStackSize = new System.Windows.Forms.Label();
      this.lblFlags = new System.Windows.Forms.Label();
      this.lblType = new System.Windows.Forms.Label();
      this.lblID = new System.Windows.Forms.Label();
      this.ttToolTip = new System.Windows.Forms.ToolTip(this.components);
      this.grpWeaponInfo = new System.Windows.Forms.GroupBox();
      this.txtJugSize = new System.Windows.Forms.TextBox();
      this.lblJugSize = new System.Windows.Forms.Label();
      this.txtDPS = new System.Windows.Forms.TextBox();
      this.txtDelay = new System.Windows.Forms.TextBox();
      this.txtDamage = new System.Windows.Forms.TextBox();
      this.txtSkill = new System.Windows.Forms.TextBox();
      this.lblDPS = new System.Windows.Forms.Label();
      this.lblDelay = new System.Windows.Forms.Label();
      this.lblDamage = new System.Windows.Forms.Label();
      this.lblSkill = new System.Windows.Forms.Label();
      this.grpFurnitureInfo = new System.Windows.Forms.GroupBox();
      this.txtStorage = new System.Windows.Forms.TextBox();
      this.txtElement = new System.Windows.Forms.TextBox();
      this.lblStorage = new System.Windows.Forms.Label();
      this.lblElement = new System.Windows.Forms.Label();
      this.grpShieldInfo = new System.Windows.Forms.GroupBox();
      this.txtShieldSize = new System.Windows.Forms.TextBox();
      this.lblShieldSize = new System.Windows.Forms.Label();
      this.grpEnchantmentInfo = new System.Windows.Forms.GroupBox();
      this.txtCastTime = new System.Windows.Forms.TextBox();
      this.txtReuseTimer = new System.Windows.Forms.TextBox();
      this.txtEquipDelay = new System.Windows.Forms.TextBox();
      this.txtMaxCharges = new System.Windows.Forms.TextBox();
      this.lblCastTime = new System.Windows.Forms.Label();
      this.lblEquipDelay = new System.Windows.Forms.Label();
      this.lblReuseTimer = new System.Windows.Forms.Label();
      this.lblMaxCharges = new System.Windows.Forms.Label();
      this.grpLogStrings = new System.Windows.Forms.GroupBox();
      this.txtPlural = new System.Windows.Forms.TextBox();
      this.txtSingular = new System.Windows.Forms.TextBox();
      this.lblPlural = new System.Windows.Forms.Label();
      this.lblSingular = new System.Windows.Forms.Label();
      this.grpUsableItemInfo = new System.Windows.Forms.GroupBox();
      this.txtActivationTime = new System.Windows.Forms.TextBox();
      this.lblActivationTime = new System.Windows.Forms.Label();
      this.grpPuppetItemInfo = new System.Windows.Forms.GroupBox();
      this.txtPuppetSlot = new System.Windows.Forms.TextBox();
      this.txtElementCharge = new System.Windows.Forms.TextBox();
      this.lblPuppetSlot = new System.Windows.Forms.Label();
      this.lblElementCharge = new System.Windows.Forms.Label();
      this.txtResourceID = new System.Windows.Forms.TextBox();
      this.lblResourceID = new System.Windows.Forms.Label();
      this.grpEquipmentInfo.SuspendLayout();
      this.grpCommonInfo.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize) (this.picIcon)).BeginInit();
      this.grpWeaponInfo.SuspendLayout();
      this.grpFurnitureInfo.SuspendLayout();
      this.grpShieldInfo.SuspendLayout();
      this.grpEnchantmentInfo.SuspendLayout();
      this.grpLogStrings.SuspendLayout();
      this.grpUsableItemInfo.SuspendLayout();
      this.grpPuppetItemInfo.SuspendLayout();
      this.SuspendLayout();
      // 
      // grpEquipmentInfo
      // 
      this.grpEquipmentInfo.BackColor = System.Drawing.Color.Transparent;
      this.grpEquipmentInfo.Controls.Add(this.txtRaces);
      this.grpEquipmentInfo.Controls.Add(this.txtSlots);
      this.grpEquipmentInfo.Controls.Add(this.txtJobs);
      this.grpEquipmentInfo.Controls.Add(this.txtLevel);
      this.grpEquipmentInfo.Controls.Add(this.lblRaces);
      this.grpEquipmentInfo.Controls.Add(this.lblSlots);
      this.grpEquipmentInfo.Controls.Add(this.lblJobs);
      this.grpEquipmentInfo.Controls.Add(this.lblLevel);
      resources.ApplyResources(this.grpEquipmentInfo, "grpEquipmentInfo");
      this.grpEquipmentInfo.Name = "grpEquipmentInfo";
      this.grpEquipmentInfo.TabStop = false;
      // 
      // txtRaces
      // 
      resources.ApplyResources(this.txtRaces, "txtRaces");
      this.txtRaces.Name = "txtRaces";
      this.txtRaces.ReadOnly = true;
      // 
      // txtSlots
      // 
      resources.ApplyResources(this.txtSlots, "txtSlots");
      this.txtSlots.Name = "txtSlots";
      this.txtSlots.ReadOnly = true;
      // 
      // txtJobs
      // 
      resources.ApplyResources(this.txtJobs, "txtJobs");
      this.txtJobs.Name = "txtJobs";
      this.txtJobs.ReadOnly = true;
      // 
      // txtLevel
      // 
      resources.ApplyResources(this.txtLevel, "txtLevel");
      this.txtLevel.Name = "txtLevel";
      this.txtLevel.ReadOnly = true;
      // 
      // lblRaces
      // 
      resources.ApplyResources(this.lblRaces, "lblRaces");
      this.lblRaces.Name = "lblRaces";
      // 
      // lblSlots
      // 
      resources.ApplyResources(this.lblSlots, "lblSlots");
      this.lblSlots.Name = "lblSlots";
      // 
      // lblJobs
      // 
      resources.ApplyResources(this.lblJobs, "lblJobs");
      this.lblJobs.Name = "lblJobs";
      // 
      // lblLevel
      // 
      resources.ApplyResources(this.lblLevel, "lblLevel");
      this.lblLevel.Name = "lblLevel";
      // 
      // grpCommonInfo
      // 
      this.grpCommonInfo.BackColor = System.Drawing.Color.Transparent;
      this.grpCommonInfo.Controls.Add(this.txtResourceID);
      this.grpCommonInfo.Controls.Add(this.lblResourceID);
      this.grpCommonInfo.Controls.Add(this.txtValidTargets);
      this.grpCommonInfo.Controls.Add(this.txtName);
      this.grpCommonInfo.Controls.Add(this.txtDescription);
      this.grpCommonInfo.Controls.Add(this.txtStackSize);
      this.grpCommonInfo.Controls.Add(this.txtFlags);
      this.grpCommonInfo.Controls.Add(this.txtType);
      this.grpCommonInfo.Controls.Add(this.txtID);
      this.grpCommonInfo.Controls.Add(this.picIcon);
      this.grpCommonInfo.Controls.Add(this.lblValidTargets);
      this.grpCommonInfo.Controls.Add(this.lblDescription);
      this.grpCommonInfo.Controls.Add(this.lblName);
      this.grpCommonInfo.Controls.Add(this.lblStackSize);
      this.grpCommonInfo.Controls.Add(this.lblFlags);
      this.grpCommonInfo.Controls.Add(this.lblType);
      this.grpCommonInfo.Controls.Add(this.lblID);
      resources.ApplyResources(this.grpCommonInfo, "grpCommonInfo");
      this.grpCommonInfo.Name = "grpCommonInfo";
      this.grpCommonInfo.TabStop = false;
      // 
      // txtValidTargets
      // 
      resources.ApplyResources(this.txtValidTargets, "txtValidTargets");
      this.txtValidTargets.Name = "txtValidTargets";
      this.txtValidTargets.ReadOnly = true;
      // 
      // txtName
      // 
      resources.ApplyResources(this.txtName, "txtName");
      this.txtName.Name = "txtName";
      this.txtName.ReadOnly = true;
      // 
      // txtDescription
      // 
      resources.ApplyResources(this.txtDescription, "txtDescription");
      this.txtDescription.Name = "txtDescription";
      this.txtDescription.ReadOnly = true;
      // 
      // txtStackSize
      // 
      resources.ApplyResources(this.txtStackSize, "txtStackSize");
      this.txtStackSize.Name = "txtStackSize";
      this.txtStackSize.ReadOnly = true;
      // 
      // txtFlags
      // 
      resources.ApplyResources(this.txtFlags, "txtFlags");
      this.txtFlags.Name = "txtFlags";
      this.txtFlags.ReadOnly = true;
      // 
      // txtType
      // 
      resources.ApplyResources(this.txtType, "txtType");
      this.txtType.Name = "txtType";
      this.txtType.ReadOnly = true;
      // 
      // txtID
      // 
      resources.ApplyResources(this.txtID, "txtID");
      this.txtID.Name = "txtID";
      this.txtID.ReadOnly = true;
      // 
      // picIcon
      // 
      this.picIcon.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      resources.ApplyResources(this.picIcon, "picIcon");
      this.picIcon.Name = "picIcon";
      this.picIcon.TabStop = false;
      // 
      // lblValidTargets
      // 
      resources.ApplyResources(this.lblValidTargets, "lblValidTargets");
      this.lblValidTargets.Name = "lblValidTargets";
      // 
      // lblDescription
      // 
      resources.ApplyResources(this.lblDescription, "lblDescription");
      this.lblDescription.Name = "lblDescription";
      // 
      // lblName
      // 
      resources.ApplyResources(this.lblName, "lblName");
      this.lblName.Name = "lblName";
      // 
      // lblStackSize
      // 
      resources.ApplyResources(this.lblStackSize, "lblStackSize");
      this.lblStackSize.Name = "lblStackSize";
      // 
      // lblFlags
      // 
      resources.ApplyResources(this.lblFlags, "lblFlags");
      this.lblFlags.Name = "lblFlags";
      // 
      // lblType
      // 
      resources.ApplyResources(this.lblType, "lblType");
      this.lblType.Name = "lblType";
      // 
      // lblID
      // 
      resources.ApplyResources(this.lblID, "lblID");
      this.lblID.Name = "lblID";
      // 
      // grpWeaponInfo
      // 
      this.grpWeaponInfo.BackColor = System.Drawing.Color.Transparent;
      this.grpWeaponInfo.Controls.Add(this.txtJugSize);
      this.grpWeaponInfo.Controls.Add(this.lblJugSize);
      this.grpWeaponInfo.Controls.Add(this.txtDPS);
      this.grpWeaponInfo.Controls.Add(this.txtDelay);
      this.grpWeaponInfo.Controls.Add(this.txtDamage);
      this.grpWeaponInfo.Controls.Add(this.txtSkill);
      this.grpWeaponInfo.Controls.Add(this.lblDPS);
      this.grpWeaponInfo.Controls.Add(this.lblDelay);
      this.grpWeaponInfo.Controls.Add(this.lblDamage);
      this.grpWeaponInfo.Controls.Add(this.lblSkill);
      resources.ApplyResources(this.grpWeaponInfo, "grpWeaponInfo");
      this.grpWeaponInfo.Name = "grpWeaponInfo";
      this.grpWeaponInfo.TabStop = false;
      // 
      // txtJugSize
      // 
      resources.ApplyResources(this.txtJugSize, "txtJugSize");
      this.txtJugSize.Name = "txtJugSize";
      this.txtJugSize.ReadOnly = true;
      // 
      // lblJugSize
      // 
      resources.ApplyResources(this.lblJugSize, "lblJugSize");
      this.lblJugSize.Name = "lblJugSize";
      // 
      // txtDPS
      // 
      resources.ApplyResources(this.txtDPS, "txtDPS");
      this.txtDPS.Name = "txtDPS";
      this.txtDPS.ReadOnly = true;
      // 
      // txtDelay
      // 
      resources.ApplyResources(this.txtDelay, "txtDelay");
      this.txtDelay.Name = "txtDelay";
      this.txtDelay.ReadOnly = true;
      // 
      // txtDamage
      // 
      resources.ApplyResources(this.txtDamage, "txtDamage");
      this.txtDamage.Name = "txtDamage";
      this.txtDamage.ReadOnly = true;
      // 
      // txtSkill
      // 
      resources.ApplyResources(this.txtSkill, "txtSkill");
      this.txtSkill.Name = "txtSkill";
      this.txtSkill.ReadOnly = true;
      // 
      // lblDPS
      // 
      resources.ApplyResources(this.lblDPS, "lblDPS");
      this.lblDPS.Name = "lblDPS";
      // 
      // lblDelay
      // 
      resources.ApplyResources(this.lblDelay, "lblDelay");
      this.lblDelay.Name = "lblDelay";
      // 
      // lblDamage
      // 
      resources.ApplyResources(this.lblDamage, "lblDamage");
      this.lblDamage.Name = "lblDamage";
      // 
      // lblSkill
      // 
      resources.ApplyResources(this.lblSkill, "lblSkill");
      this.lblSkill.Name = "lblSkill";
      // 
      // grpFurnitureInfo
      // 
      this.grpFurnitureInfo.BackColor = System.Drawing.Color.Transparent;
      this.grpFurnitureInfo.Controls.Add(this.txtStorage);
      this.grpFurnitureInfo.Controls.Add(this.txtElement);
      this.grpFurnitureInfo.Controls.Add(this.lblStorage);
      this.grpFurnitureInfo.Controls.Add(this.lblElement);
      resources.ApplyResources(this.grpFurnitureInfo, "grpFurnitureInfo");
      this.grpFurnitureInfo.Name = "grpFurnitureInfo";
      this.grpFurnitureInfo.TabStop = false;
      // 
      // txtStorage
      // 
      resources.ApplyResources(this.txtStorage, "txtStorage");
      this.txtStorage.Name = "txtStorage";
      this.txtStorage.ReadOnly = true;
      // 
      // txtElement
      // 
      resources.ApplyResources(this.txtElement, "txtElement");
      this.txtElement.Name = "txtElement";
      this.txtElement.ReadOnly = true;
      // 
      // lblStorage
      // 
      resources.ApplyResources(this.lblStorage, "lblStorage");
      this.lblStorage.Name = "lblStorage";
      // 
      // lblElement
      // 
      resources.ApplyResources(this.lblElement, "lblElement");
      this.lblElement.Name = "lblElement";
      // 
      // grpShieldInfo
      // 
      this.grpShieldInfo.BackColor = System.Drawing.Color.Transparent;
      this.grpShieldInfo.Controls.Add(this.txtShieldSize);
      this.grpShieldInfo.Controls.Add(this.lblShieldSize);
      resources.ApplyResources(this.grpShieldInfo, "grpShieldInfo");
      this.grpShieldInfo.Name = "grpShieldInfo";
      this.grpShieldInfo.TabStop = false;
      // 
      // txtShieldSize
      // 
      resources.ApplyResources(this.txtShieldSize, "txtShieldSize");
      this.txtShieldSize.Name = "txtShieldSize";
      this.txtShieldSize.ReadOnly = true;
      // 
      // lblShieldSize
      // 
      resources.ApplyResources(this.lblShieldSize, "lblShieldSize");
      this.lblShieldSize.Name = "lblShieldSize";
      // 
      // grpEnchantmentInfo
      // 
      this.grpEnchantmentInfo.BackColor = System.Drawing.Color.Transparent;
      this.grpEnchantmentInfo.Controls.Add(this.txtCastTime);
      this.grpEnchantmentInfo.Controls.Add(this.txtReuseTimer);
      this.grpEnchantmentInfo.Controls.Add(this.txtEquipDelay);
      this.grpEnchantmentInfo.Controls.Add(this.txtMaxCharges);
      this.grpEnchantmentInfo.Controls.Add(this.lblCastTime);
      this.grpEnchantmentInfo.Controls.Add(this.lblEquipDelay);
      this.grpEnchantmentInfo.Controls.Add(this.lblReuseTimer);
      this.grpEnchantmentInfo.Controls.Add(this.lblMaxCharges);
      resources.ApplyResources(this.grpEnchantmentInfo, "grpEnchantmentInfo");
      this.grpEnchantmentInfo.Name = "grpEnchantmentInfo";
      this.grpEnchantmentInfo.TabStop = false;
      // 
      // txtCastTime
      // 
      resources.ApplyResources(this.txtCastTime, "txtCastTime");
      this.txtCastTime.Name = "txtCastTime";
      this.txtCastTime.ReadOnly = true;
      // 
      // txtReuseTimer
      // 
      resources.ApplyResources(this.txtReuseTimer, "txtReuseTimer");
      this.txtReuseTimer.Name = "txtReuseTimer";
      this.txtReuseTimer.ReadOnly = true;
      // 
      // txtEquipDelay
      // 
      resources.ApplyResources(this.txtEquipDelay, "txtEquipDelay");
      this.txtEquipDelay.Name = "txtEquipDelay";
      this.txtEquipDelay.ReadOnly = true;
      // 
      // txtMaxCharges
      // 
      resources.ApplyResources(this.txtMaxCharges, "txtMaxCharges");
      this.txtMaxCharges.Name = "txtMaxCharges";
      this.txtMaxCharges.ReadOnly = true;
      // 
      // lblCastTime
      // 
      resources.ApplyResources(this.lblCastTime, "lblCastTime");
      this.lblCastTime.Name = "lblCastTime";
      // 
      // lblEquipDelay
      // 
      resources.ApplyResources(this.lblEquipDelay, "lblEquipDelay");
      this.lblEquipDelay.Name = "lblEquipDelay";
      // 
      // lblReuseTimer
      // 
      resources.ApplyResources(this.lblReuseTimer, "lblReuseTimer");
      this.lblReuseTimer.Name = "lblReuseTimer";
      // 
      // lblMaxCharges
      // 
      resources.ApplyResources(this.lblMaxCharges, "lblMaxCharges");
      this.lblMaxCharges.Name = "lblMaxCharges";
      // 
      // grpLogStrings
      // 
      this.grpLogStrings.BackColor = System.Drawing.Color.Transparent;
      this.grpLogStrings.Controls.Add(this.txtPlural);
      this.grpLogStrings.Controls.Add(this.txtSingular);
      this.grpLogStrings.Controls.Add(this.lblPlural);
      this.grpLogStrings.Controls.Add(this.lblSingular);
      resources.ApplyResources(this.grpLogStrings, "grpLogStrings");
      this.grpLogStrings.Name = "grpLogStrings";
      this.grpLogStrings.TabStop = false;
      // 
      // txtPlural
      // 
      resources.ApplyResources(this.txtPlural, "txtPlural");
      this.txtPlural.Name = "txtPlural";
      this.txtPlural.ReadOnly = true;
      // 
      // txtSingular
      // 
      resources.ApplyResources(this.txtSingular, "txtSingular");
      this.txtSingular.Name = "txtSingular";
      this.txtSingular.ReadOnly = true;
      // 
      // lblPlural
      // 
      resources.ApplyResources(this.lblPlural, "lblPlural");
      this.lblPlural.Name = "lblPlural";
      // 
      // lblSingular
      // 
      resources.ApplyResources(this.lblSingular, "lblSingular");
      this.lblSingular.Name = "lblSingular";
      // 
      // grpUsableItemInfo
      // 
      this.grpUsableItemInfo.BackColor = System.Drawing.Color.Transparent;
      this.grpUsableItemInfo.Controls.Add(this.txtActivationTime);
      this.grpUsableItemInfo.Controls.Add(this.lblActivationTime);
      resources.ApplyResources(this.grpUsableItemInfo, "grpUsableItemInfo");
      this.grpUsableItemInfo.Name = "grpUsableItemInfo";
      this.grpUsableItemInfo.TabStop = false;
      // 
      // txtActivationTime
      // 
      resources.ApplyResources(this.txtActivationTime, "txtActivationTime");
      this.txtActivationTime.Name = "txtActivationTime";
      this.txtActivationTime.ReadOnly = true;
      // 
      // lblActivationTime
      // 
      resources.ApplyResources(this.lblActivationTime, "lblActivationTime");
      this.lblActivationTime.Name = "lblActivationTime";
      // 
      // grpPuppetItemInfo
      // 
      this.grpPuppetItemInfo.BackColor = System.Drawing.Color.Transparent;
      this.grpPuppetItemInfo.Controls.Add(this.txtPuppetSlot);
      this.grpPuppetItemInfo.Controls.Add(this.txtElementCharge);
      this.grpPuppetItemInfo.Controls.Add(this.lblPuppetSlot);
      this.grpPuppetItemInfo.Controls.Add(this.lblElementCharge);
      resources.ApplyResources(this.grpPuppetItemInfo, "grpPuppetItemInfo");
      this.grpPuppetItemInfo.Name = "grpPuppetItemInfo";
      this.grpPuppetItemInfo.TabStop = false;
      // 
      // txtPuppetSlot
      // 
      resources.ApplyResources(this.txtPuppetSlot, "txtPuppetSlot");
      this.txtPuppetSlot.Name = "txtPuppetSlot";
      this.txtPuppetSlot.ReadOnly = true;
      // 
      // txtElementCharge
      // 
      resources.ApplyResources(this.txtElementCharge, "txtElementCharge");
      this.txtElementCharge.Name = "txtElementCharge";
      this.txtElementCharge.ReadOnly = true;
      // 
      // lblPuppetSlot
      // 
      resources.ApplyResources(this.lblPuppetSlot, "lblPuppetSlot");
      this.lblPuppetSlot.Name = "lblPuppetSlot";
      // 
      // lblElementCharge
      // 
      resources.ApplyResources(this.lblElementCharge, "lblElementCharge");
      this.lblElementCharge.Name = "lblElementCharge";
      // 
      // txtResourceID
      // 
      resources.ApplyResources(this.txtResourceID, "txtResourceID");
      this.txtResourceID.Name = "txtResourceID";
      this.txtResourceID.ReadOnly = true;
      // 
      // lblResourceID
      // 
      resources.ApplyResources(this.lblResourceID, "lblResourceID");
      this.lblResourceID.Name = "lblResourceID";
      // 
      // ItemEditor
      // 
      this.BackColor = System.Drawing.Color.Transparent;
      this.Controls.Add(this.grpPuppetItemInfo);
      this.Controls.Add(this.grpUsableItemInfo);
      this.Controls.Add(this.grpLogStrings);
      this.Controls.Add(this.grpEquipmentInfo);
      this.Controls.Add(this.grpEnchantmentInfo);
      this.Controls.Add(this.grpShieldInfo);
      this.Controls.Add(this.grpFurnitureInfo);
      this.Controls.Add(this.grpWeaponInfo);
      this.Controls.Add(this.grpCommonInfo);
      this.Name = "ItemEditor";
      resources.ApplyResources(this, "$this");
      this.grpEquipmentInfo.ResumeLayout(false);
      this.grpEquipmentInfo.PerformLayout();
      this.grpCommonInfo.ResumeLayout(false);
      this.grpCommonInfo.PerformLayout();
      ((System.ComponentModel.ISupportInitialize) (this.picIcon)).EndInit();
      this.grpWeaponInfo.ResumeLayout(false);
      this.grpWeaponInfo.PerformLayout();
      this.grpFurnitureInfo.ResumeLayout(false);
      this.grpFurnitureInfo.PerformLayout();
      this.grpShieldInfo.ResumeLayout(false);
      this.grpShieldInfo.PerformLayout();
      this.grpEnchantmentInfo.ResumeLayout(false);
      this.grpEnchantmentInfo.PerformLayout();
      this.grpLogStrings.ResumeLayout(false);
      this.grpLogStrings.PerformLayout();
      this.grpUsableItemInfo.ResumeLayout(false);
      this.grpUsableItemInfo.PerformLayout();
      this.grpPuppetItemInfo.ResumeLayout(false);
      this.grpPuppetItemInfo.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Label lblRaces;
    private System.Windows.Forms.TextBox txtRaces;
    private System.Windows.Forms.Label lblSlots;
    private System.Windows.Forms.TextBox txtSlots;
    private System.Windows.Forms.Label lblJobs;
    private System.Windows.Forms.TextBox txtJobs;
    private System.Windows.Forms.Label lblLevel;
    private System.Windows.Forms.TextBox txtLevel;
    private System.Windows.Forms.GroupBox grpCommonInfo;
    private System.Windows.Forms.Label lblDescription;
    private System.Windows.Forms.Label lblName;
    private System.Windows.Forms.TextBox txtName;
    private System.Windows.Forms.TextBox txtDescription;
    private System.Windows.Forms.Label lblStackSize;
    private System.Windows.Forms.Label lblFlags;
    private System.Windows.Forms.Label lblType;
    private System.Windows.Forms.Label lblID;
    private System.Windows.Forms.TextBox txtStackSize;
    private System.Windows.Forms.TextBox txtFlags;
    private System.Windows.Forms.TextBox txtType;
    private System.Windows.Forms.TextBox txtID;
    private System.Windows.Forms.PictureBox picIcon;
    private System.Windows.Forms.ToolTip ttToolTip;
    private System.Windows.Forms.GroupBox grpEquipmentInfo;
    private System.Windows.Forms.GroupBox grpWeaponInfo;
    private System.Windows.Forms.GroupBox grpFurnitureInfo;
    private System.Windows.Forms.Label lblStorage;
    private System.Windows.Forms.TextBox txtStorage;
    private System.Windows.Forms.Label lblElement;
    private System.Windows.Forms.TextBox txtElement;
    private System.Windows.Forms.Label lblShieldSize;
    private System.Windows.Forms.TextBox txtShieldSize;
    private System.Windows.Forms.Label lblSkill;
    private System.Windows.Forms.TextBox txtSkill;
    private System.Windows.Forms.Label lblDelay;
    private System.Windows.Forms.Label lblDamage;
    private System.Windows.Forms.TextBox txtDelay;
    private System.Windows.Forms.TextBox txtDamage;
    private System.Windows.Forms.Label lblDPS;
    private System.Windows.Forms.TextBox txtDPS;
    private System.Windows.Forms.GroupBox grpShieldInfo;
    private System.Windows.Forms.GroupBox grpEnchantmentInfo;
    private System.Windows.Forms.Label lblCastTime;
    private System.Windows.Forms.TextBox txtCastTime;
    private System.Windows.Forms.Label lblEquipDelay;
    private System.Windows.Forms.Label lblReuseTimer;
    private System.Windows.Forms.Label lblMaxCharges;
    private System.Windows.Forms.TextBox txtReuseTimer;
    private System.Windows.Forms.TextBox txtEquipDelay;
    private System.Windows.Forms.TextBox txtMaxCharges;
    private System.Windows.Forms.Label lblValidTargets;
    private System.Windows.Forms.TextBox txtValidTargets;
    private System.Windows.Forms.GroupBox grpLogStrings;
    private System.Windows.Forms.Label lblPlural;
    private System.Windows.Forms.Label lblSingular;
    private System.Windows.Forms.TextBox txtPlural;
    private System.Windows.Forms.TextBox txtSingular;
    private System.Windows.Forms.TextBox txtJugSize;
    private System.Windows.Forms.Label lblJugSize;
    private System.Windows.Forms.GroupBox grpUsableItemInfo;
    private System.Windows.Forms.TextBox txtActivationTime;
    private System.Windows.Forms.Label lblActivationTime;
    private System.Windows.Forms.GroupBox grpPuppetItemInfo;
    private System.Windows.Forms.TextBox txtPuppetSlot;
    private System.Windows.Forms.TextBox txtElementCharge;
    private System.Windows.Forms.Label lblPuppetSlot;
    private System.Windows.Forms.Label lblElementCharge;
    private System.Windows.Forms.TextBox txtResourceID;
    private System.Windows.Forms.Label lblResourceID;

  }

}