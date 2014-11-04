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
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using PlayOnline.Core;
using PlayOnline.FFXI.Things;

namespace PlayOnline.FFXI
{
    public partial class ItemEditor : UserControl
    {
        private Item ItemToShow_ = null;
        private int LogicalHeight_;

        #region Properties

        [Browsable(false)]
        public Item Item
        {
            get { return this.ItemToShow_; }
            set
            {
                this.ItemToShow_ = value;
                this.ShowItem();
            }
        }

        // This is currently a no-op, as actual item editing is not supported (yet)
        [Browsable(true)]
        [Category("Behavior")]
        [Description("Controls whether or not the item information can be edited.")]
        [DefaultValue(true)]
        public bool ReadOnly
        {
            get { return true; }
            set { }
        }

        #endregion

        #region Field Marks

        private Control GetFieldControl(string Field)
        {
            switch (Field)
            {
            case "activation-time":
                return this.txtActivationTime;
            case "casting-time":
                return this.txtCastTime;
            case "dps":
                return this.txtDPS;
            case "damage":
                return this.txtDamage;
            case "delay":
                return this.txtDelay;
            case "description":
                return this.txtDescription;
            case "element":
                return this.txtElement;
            case "element-charge":
                return this.txtElementCharge;
            case "name":
                return this.txtName;
            case "flags":
                return this.txtFlags;
            case "icon":
                return this.picIcon;
            case "id":
                return this.txtID;
            case "jobs":
                return this.txtJobs;
            case "jug-size":
                return this.txtJugSize;
            case "level":
                return this.txtLevel;
            case "log-name-plural":
                return this.txtPlural;
            case "log-name-singular":
                return this.txtSingular;
            case "max-charges":
                return this.txtMaxCharges;
            case "puppet-slot":
                return this.txtPuppetSlot;
            case "races":
                return this.txtRaces;
            case "resource-id":
                return this.txtResourceID;
            case "reuse-delay":
                return this.txtReuseTimer;
            case "shield-size":
                return this.txtShieldSize;
            case "skill":
                return this.txtSkill;
            case "slots":
                return this.txtSlots;
            case "stack-size":
                return this.txtStackSize;
            case "storage-slots":
                return this.txtStorage;
            case "type":
                return this.txtType;
            case "use-delay":
                return this.txtEquipDelay;
            case "valid-targets":
                return this.txtValidTargets;
            case "unknown-1":
                return null;
            case "unknown-2":
                return null;
            case "unknown-3":
                return null;
            }
            return null;
        }

        public void UnmarkAll()
        {
            foreach (string Field in this.ItemToShow_.GetAllFields())
            {
                this.MarkField(Field, false);
            }
        }

        private void MarkControl(Control C, bool Marked)
        {
            if (C == null)
            {
                return;
            }
            Color DefaultColor = SystemColors.Control;
            if (C is PictureBox)
            {
                DefaultColor = Color.Transparent;
            }
            C.BackColor = (Marked ? Color.LightGoldenrodYellow : DefaultColor);
            C.Font = new Font(C.Font, (Marked ? FontStyle.Bold : FontStyle.Regular));
        }

        public void MarkField(string Field, bool Marked) { this.MarkControl(this.GetFieldControl(Field), Marked); }

        #endregion

        #region Public Methods

        public ItemEditor()
        {
            this.InitializeComponent();
            this.ShowItem();
        }

        public bool IsFieldShown(string Field) { return (this.GetFieldControl(Field) != null); }

        #endregion

        #region Private Methods

        private void ShowItem()
        {
            this.ResetInfoGroups();
            if (this.ItemToShow_ != null)
            {
                Item I = this.ItemToShow_;
                // Common Fields
                this.picIcon.Image = I.GetIcon();
                this.ttToolTip.SetToolTip(this.picIcon, I.GetFieldText("icon"));
                this.txtID.Text = I.GetFieldText("id");
                this.txtResourceID.Text = I.GetFieldText("resource-id");
                this.txtType.Text = I.GetFieldText("type");
                this.txtStackSize.Text = I.GetFieldText("stack-size");
                this.txtName.Text = I.GetFieldText("name");
                this.txtDescription.Text = I.GetFieldText("description");
                this.txtFlags.Text = I.GetFieldText("flags");
                this.txtValidTargets.Text = I.GetFieldText("valid-targets");
                // English Fields
                if (I.HasField("log-name-singular"))
                {
                    this.txtSingular.Text = I.GetFieldText("log-name-singular");
                    this.txtPlural.Text = I.GetFieldText("log-name-plural");
                    this.ShowInfoGroup(this.grpLogStrings);
                }
                // Furniture Fields
                if (I.HasField("element"))
                {
                    this.txtElement.Text = I.GetFieldText("element");
                    this.txtStorage.Text = I.GetFieldText("storage-slots");
                    this.ShowInfoGroup(this.grpFurnitureInfo);
                }
                // Usable Item Fields
                if (I.HasField("activation-time"))
                {
                    this.txtActivationTime.Text = I.GetFieldText("activation-time");
                    this.ShowInfoGroup(this.grpUsableItemInfo);
                }
                // Equipment Fields
                if (I.HasField("level"))
                {
                    this.txtLevel.Text = I.GetFieldText("level");
                    this.txtJobs.Text = I.GetFieldText("jobs");
                    this.txtSlots.Text = I.GetFieldText("slots");
                    this.txtRaces.Text = I.GetFieldText("races");
                    this.ShowInfoGroup(this.grpEquipmentInfo);
                }
                // Armor Fields
                if (I.HasField("shield-size") && I.HasField("slots"))
                {
                    EquipmentSlot Slots = (EquipmentSlot)I.GetFieldValue("slots");
                    if ((Slots & EquipmentSlot.Sub) != 0)
                    {
                        this.txtShieldSize.Text = I.GetFieldText("shield-size");
                        this.ShowInfoGroup(this.grpShieldInfo);
                    }
                }
                // Weapon Fields
                // FIXME: Pet Food should have an alternative group (with just Amount Healed)
                if (I.HasField("damage"))
                {
                    this.txtDamage.Text = I.GetFieldText("damage");
                    this.txtDelay.Text = I.GetFieldText("delay");
                    this.txtDPS.Text = I.GetFieldText("dps");
                    this.txtSkill.Text = I.GetFieldText("skill");
                    this.txtJugSize.Text = I.GetFieldText("jug-size");
                    this.ShowInfoGroup(this.grpWeaponInfo);
                }
                // Puppet Item Fields
                if (I.HasField("puppet-slot"))
                {
                    this.txtPuppetSlot.Text = I.GetFieldText("puppet-slot");
                    this.txtElementCharge.Text = I.GetFieldText("element-charge");
                    this.ShowInfoGroup(this.grpPuppetItemInfo);
                }
                // Enchantment Fields
                if (I.HasField("max-charges"))
                {
                    byte MaxCharges = (byte)I.GetFieldValue("max-charges");
                    if (MaxCharges > 0)
                    {
                        this.txtMaxCharges.Text = I.GetFieldText("max-charges");
                        this.txtCastTime.Text = I.GetFieldText("casting-time");
                        this.txtEquipDelay.Text = I.GetFieldText("use-delay");
                        this.txtReuseTimer.Text = I.GetFieldText("reuse-delay");
                        this.ShowInfoGroup(this.grpEnchantmentInfo);
                    }
                }
            }
            else
            {
                this.ttToolTip.SetToolTip(this.picIcon, null);
                this.picIcon.Image = null;
                this.txtID.Text = this.txtResourceID.Text = String.Empty;
                this.txtType.Text = this.txtStackSize.Text = String.Empty;
                this.txtName.Text = String.Empty;
                this.txtDescription.Text = String.Empty;
                this.txtFlags.Text = String.Empty;
                this.txtValidTargets.Text = String.Empty;
            }
        }

        private void ResetInfoGroups()
        {
            this.LogicalHeight_ = this.grpCommonInfo.Top + this.grpCommonInfo.Height;
            this.Height = this.LogicalHeight_;
            this.grpEnchantmentInfo.Visible = false;
            this.grpEquipmentInfo.Visible = false;
            this.grpFurnitureInfo.Visible = false;
            this.grpUsableItemInfo.Visible = false;
            this.grpLogStrings.Visible = false;
            this.grpPuppetItemInfo.Visible = false;
            this.grpShieldInfo.Visible = false;
            this.grpWeaponInfo.Visible = false;
        }

        private void ShowInfoGroup(GroupBox GB)
        {
            if (GB.Visible)
            {
                return;
            }
            GB.Top = this.LogicalHeight_ + 4;
            this.LogicalHeight_ += 4 + GB.Height;
            GB.Visible = true;
            this.Height = this.LogicalHeight_;
        }

        #endregion
    }
}
