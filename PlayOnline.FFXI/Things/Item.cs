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
using System.Windows.Forms;
using PlayOnline.Core;

namespace PlayOnline.FFXI.Things
{
    public class Item : Thing
    {
        public Item()
        {
            // Fill Thing helpers
            this.IconField_ = "icon";
            // Clear fields
            this.Clear();
        }

        public override string ToString() { return String.Format("[{0:00000000}] {1}", this.ID_, this.Name_); }

        public override List<PropertyPages.IThing> GetPropertyPages()
        {
            List<PropertyPages.IThing> Pages = base.GetPropertyPages();
            Pages.Add(new PropertyPages.Item(this));
            return Pages;
        }

        #region Fields

        public static List<string> AllFields
        {
            get
            {
                return new List<string>(new string[]
                {
                    // General
                    "id", "flags", "stack-size", "type", "resource-id", "valid-targets", "name", "description",
                    // English-Specific
                    "log-name-singular", "log-name-plural",
                    // Furniture-Specific
                    "element", "storage-slots",
                    // UsableItem-Specific
                    "activation-time",
                    // Equipment-Specific
                    "level", "iLevel", "slots", "races", "jobs", "superior-level",
                    // Armor-Specific
                    "shield-size",
                    // Weapon-Specific
                    "damage", "delay", "dps", "skill", "jug-size",
                    // Enchantment Info
                    "max-charges", "casting-time", "use-delay", "reuse-delay",
                    // Puppet Item Info
                    "puppet-slot", "element-charge",
                    //Monstrosity Related
                    "instinct-cost",
                    // Special Stuff
                    "icon", "unknown-1", "unknown-2", "unknown-3", "unknown-4",
                });
            }
        }

        public override List<string> GetAllFields() { return Item.AllFields; }

        #region Data Fields

        // General
        private uint? ID_;
        private ItemFlags? Flags_;
        private ushort? StackSize_;
        private ItemType? Type_;
        private ushort? ResourceID_;
        private ValidTarget? ValidTargets_;
        private string Name_;
        private string Description_;
        // English-Specific
        private string LogNameSingular_;
        private string LogNamePlural_;
        // Furniture-Specific
        private Element? Element_;
        private int? StorageSlots_;
        // UsableItem-Specific
        private ushort? ActivationTime_;
        // Equipment-Specific
        private ushort? iLevel_;
        private ushort? Level_;
        private EquipmentSlot? Slots_;
        private Race? Races_;
        private Job? Jobs_;
        private ushort? SuperiorLevel_;
        // Armor-Specific
        private ushort? ShieldSize_;
        // Weapon-Specific
        private ushort? Damage_;
        private short? Delay_;
        private ushort? DPS_;
        private Skill? Skill_;
        private byte? JugSize_;
        // Enchantment Info
        private byte? MaxCharges_;
        private byte? CastingTime_;
        private ushort? UseDelay_;
        private uint? ReuseDelay_;
        // Puppet Item Info
        private PuppetSlot? PuppetSlot_;
        private uint? ElementCharge_;
        //Monstrosity Related
        private ushort? InstinctCost_;
        // Special
        private Graphic Icon_;
        private uint? Unknown1_;
        private ushort? Unknown2_;
        private uint? Unknown3_;
        private uint? Unknown4_;
        private uint? Unknown5_;

        #endregion

        public override void Clear()
        {
            if (this.Icon_ != null)
            {
                this.Icon_.Clear();
            }
            this.ID_ = null;
            this.Flags_ = null;
            this.StackSize_ = null;
            this.Type_ = null;
            this.ResourceID_ = null;
            this.ValidTargets_ = null;
            this.Name_ = null;
            this.Description_ = null;
            this.LogNameSingular_ = null;
            this.LogNamePlural_ = null;
            this.Element_ = null;
            this.StorageSlots_ = null;
            this.ActivationTime_ = null;
            this.Level_ = null;
            this.Slots_ = null;
            this.Races_ = null;
            this.Jobs_ = null;
            this.SuperiorLevel_ = null;
            this.ShieldSize_ = null;
            this.Damage_ = null;
            this.Delay_ = null;
            this.DPS_ = null;
            this.Skill_ = null;
            this.JugSize_ = null;
            this.iLevel_ = null;
            this.MaxCharges_ = null;
            this.CastingTime_ = null;
            this.UseDelay_ = null;
            this.ReuseDelay_ = null;
            this.PuppetSlot_ = null;
            this.ElementCharge_ = null;
            this.InstinctCost_ = null;
            this.Icon_ = null;
            this.Unknown1_ = null;
            this.Unknown2_ = null;
            this.Unknown3_ = null;
            this.Unknown4_ = null;
        }

        #endregion

        #region Field Access

        public override bool HasField(string Field)
        {
            switch (Field)
            {
                // Objects
                case "description":
                    return (this.Description_ != null);
                case "icon":
                    return (this.Icon_ != null);
                case "log-name-plural":
                    return (this.LogNamePlural_ != null);
                case "log-name-singular":
                    return (this.LogNameSingular_ != null);
                case "name":
                    return (this.Name_ != null);
                // Nullables
                case "activation-time":
                    return this.ActivationTime_.HasValue;
                case "casting-time":
                    return this.CastingTime_.HasValue;
                case "damage":
                    return this.Damage_.HasValue;
                case "delay":
                    return this.Delay_.HasValue;
                case "dps":
                    return this.DPS_.HasValue;
                case "element":
                    return this.Element_.HasValue;
                case "element-charge":
                    return this.ElementCharge_.HasValue;
                case "flags":
                    return this.Flags_.HasValue;
                case "id":
                    return this.ID_.HasValue;
                case "iLevel":
                    return this.iLevel_.HasValue;
                case "instinct-cost":
                    return this.InstinctCost_.HasValue;
                case "jobs":
                    return this.Jobs_.HasValue;
                case "jug-size":
                    return this.JugSize_.HasValue;
                case "level":
                    return this.Level_.HasValue;
                case "max-charges":
                    return this.MaxCharges_.HasValue;
                case "puppet-slot":
                    return this.PuppetSlot_.HasValue;
                case "races":
                    return this.Races_.HasValue;
                case "resource-id":
                    return this.ResourceID_.HasValue;
                case "reuse-delay":
                    return this.ReuseDelay_.HasValue;
                case "superior-level":
                    return this.SuperiorLevel_.HasValue;
                case "shield-size":
                    return this.ShieldSize_.HasValue;
                case "skill":
                    return this.Skill_.HasValue;
                case "slots":
                    return this.Slots_.HasValue;
                case "stack-size":
                    return this.StackSize_.HasValue;
                case "storage-slots":
                    return this.StorageSlots_.HasValue;
                case "type":
                    return this.Type_.HasValue;
                case "unknown-1":
                    return this.Unknown1_.HasValue;
                case "unknown-2":
                    return this.Unknown2_.HasValue;
                case "unknown-3":
                    return this.Unknown3_.HasValue;
                case "unknown-4":
                    return this.Unknown4_.HasValue;
                case "use-delay":
                    return this.UseDelay_.HasValue;
                case "valid-targets":
                    return this.ValidTargets_.HasValue;
                default:
                    return false;
            }
        }

        public override string GetFieldText(string Field)
        {
            switch (Field)
            {
                // Objects
                case "description":
                    return this.Description_;
                case "icon":
                    return this.Icon_.ToString();
                case "log-name-plural":
                    return this.LogNamePlural_;
                case "log-name-singular":
                    return this.LogNameSingular_;
                case "name":
                    return this.Name_;
                // Objects - Special Formatting
                case "element-charge":
                    {
                        string Text = String.Empty;
                        if (this.ElementCharge_.HasValue)
                        {
                            for (short i = 0; i < 8; ++i)
                            {
                                byte Charge = (byte)((this.ElementCharge_ >> (4 * i)) & 0xf);
                                if (Charge == 0)
                                {
                                    continue;
                                }
                                if (Text != String.Empty)
                                {
                                    Text += ' ';
                                }
                                Text += String.Format("{0}<{1}>", (Element)i, Charge);
                            }
                        }
                        return Text;
                    }
                // Nullables - Simple Values
                case "damage":
                    return (!this.Damage_.HasValue ? String.Empty : String.Format("{0}", this.Damage_.Value));
                case "dps":
                    return (!this.DPS_.HasValue ? String.Empty : String.Format("{0}", this.DPS_.Value / 100.0));
                case "element":
                    return (!this.Element_.HasValue ? String.Empty : String.Format("{0}", this.Element_.Value));
                case "flags":
                    return (!this.Flags_.HasValue ? String.Empty : String.Format("{0}", this.Flags_.Value));
                case "iLevel":
                    return (!this.iLevel_.HasValue ? String.Empty : String.Format("{0}", this.iLevel_.Value));
                case "instinct-cost":
                    return (!this.InstinctCost_.HasValue ? String.Empty : String.Format("{0}", this.InstinctCost_.Value));
                case "jobs":
                    return (!this.Jobs_.HasValue ? String.Empty : String.Format("{0}", this.Jobs_.Value));
                case "jug-size":
                    return (!this.JugSize_.HasValue ? String.Empty : String.Format("{0}", this.JugSize_.Value));
                case "level":
                    return (!this.Level_.HasValue ? String.Empty : String.Format("{0}", this.Level_.Value));
                case "max-charges":
                    return (!this.MaxCharges_.HasValue ? String.Empty : String.Format("{0}", this.MaxCharges_.Value));
                case "puppet-slot":
                    return (!this.PuppetSlot_.HasValue ? String.Empty : String.Format("{0}", this.PuppetSlot_.Value));
                case "races":
                    return (!this.Races_.HasValue ? String.Empty : String.Format("{0}", this.Races_.Value));
                case "superior-level":
                    return (!this.SuperiorLevel_.HasValue ? String.Empty : String.Format("{0}", this.SuperiorLevel_.Value));
                case "shield-size":
                    return (!this.ShieldSize_.HasValue ? String.Empty : String.Format("{0}", this.ShieldSize_.Value));
                case "skill":
                    return (!this.Skill_.HasValue ? String.Empty : String.Format("{0}", this.Skill_.Value));
                case "slots":
                    return (!this.Slots_.HasValue ? String.Empty : String.Format("{0}", this.Slots_.Value));
                case "stack-size":
                    return (!this.StackSize_.HasValue ? String.Empty : String.Format("{0}", this.StackSize_.Value));
                case "storage-slots":
                    return (!this.StorageSlots_.HasValue ? String.Empty : String.Format("{0}", this.StorageSlots_.Value));
                case "type":
                    return (!this.Type_.HasValue ? String.Empty : String.Format("{0}", this.Type_.Value));
                case "valid-targets":
                    return (!this.ValidTargets_.HasValue ? String.Empty : String.Format("{0}", this.ValidTargets_.Value));
                // Nullables - Hex Values
                case "id":
                    return (!this.ID_.HasValue ? String.Empty : String.Format("{0:X8} ({0})", this.ID_.Value));
                case "resource-id":
                    return (!this.ResourceID_.HasValue ? String.Empty : String.Format("{0:X4} ({0})", this.ResourceID_.Value));
                case "unknown-1":
                    return (!this.Unknown1_.HasValue ? String.Empty : String.Format("{0:X8} ({0})", this.Unknown1_.Value));
                case "unknown-2":
                    return (!this.Unknown2_.HasValue ? String.Empty : String.Format("{0:X4} ({0})", this.Unknown2_.Value));
                case "unknown-3":
                    return (!this.Unknown3_.HasValue ? String.Empty : String.Format("{0:X8} ({0})", this.Unknown3_.Value));
                case "unknown-4":
                    return (!this.Unknown4_.HasValue ? String.Empty : String.Format("{0}", this.Unknown4_.Value));
                // Nullables - Time Values
                case "activation-time":
                    return (!this.ActivationTime_.HasValue ? String.Empty : this.FormatTime(this.ActivationTime_.Value / 4.0));
                case "casting-time":
                    return (!this.CastingTime_.HasValue ? String.Empty : this.FormatTime(this.CastingTime_.Value / 4.0));
                case "reuse-delay":
                    return (!this.ReuseDelay_.HasValue ? String.Empty : this.FormatTime(this.ReuseDelay_.Value));
                case "use-delay":
                    return (!this.UseDelay_.HasValue ? String.Empty : this.FormatTime(this.UseDelay_.Value));
                // Nullables - Special/Complex Values
                case "delay":
                    return (!this.Delay_.HasValue
                        ? String.Empty
                        : String.Format("{0} ({1:+###0;-###0})", this.Delay_.Value, this.Delay_.Value - 240));
                default:
                    return null;
            }
        }

        public override object GetFieldValue(string Field)
        {
            switch (Field)
            {
                // Objects
                case "description":
                    return this.Description_;
                case "icon":
                    return this.Icon_;
                case "log-name-plural":
                    return this.LogNamePlural_;
                case "log-name-singular":
                    return this.LogNameSingular_;
                case "name":
                    return this.Name_;
                // Nullables
                case "activation-time":
                    return (!this.ActivationTime_.HasValue ? null : (object)this.ActivationTime_.Value);
                case "casting-time":
                    return (!this.CastingTime_.HasValue ? null : (object)this.CastingTime_.Value);
                case "damage":
                    return (!this.Damage_.HasValue ? null : (object)this.Damage_.Value);
                case "delay":
                    return (!this.Delay_.HasValue ? null : (object)this.Delay_.Value);
                case "dps":
                    return (!this.DPS_.HasValue ? null : (object)this.DPS_.Value);
                case "element":
                    return (!this.Element_.HasValue ? null : (object)this.Element_.Value);
                case "element-charge":
                    return (!this.ElementCharge_.HasValue ? null : (object)this.ElementCharge_.Value);
                case "flags":
                    return (!this.Flags_.HasValue ? null : (object)this.Flags_.Value);
                case "id":
                    return (!this.ID_.HasValue ? null : (object)this.ID_.Value);
                case "iLevel":
                    return (!this.iLevel_.HasValue ? null : (object)this.iLevel_.Value);
                case "instinct-cost":
                    return (!this.InstinctCost_.HasValue ? null : (object)this.InstinctCost_.Value);
                case "jobs":
                    return (!this.Jobs_.HasValue ? null : (object)this.Jobs_.Value);
                case "jug-size":
                    return (!this.JugSize_.HasValue ? null : (object)this.JugSize_.Value);
                case "level":
                    return (!this.Level_.HasValue ? null : (object)this.Level_.Value);
                case "max-charges":
                    return (!this.MaxCharges_.HasValue ? null : (object)this.MaxCharges_.Value);
                case "puppet-slot":
                    return (!this.PuppetSlot_.HasValue ? null : (object)this.PuppetSlot_.Value);
                case "races":
                    return (!this.Races_.HasValue ? null : (object)this.Races_.Value);
                case "resource-id":
                    return (!this.ResourceID_.HasValue ? null : (object)this.ResourceID_.Value);
                case "reuse-delay":
                    return (!this.ReuseDelay_.HasValue ? null : (object)this.ReuseDelay_.Value);
                case "superior-level":
                    return (!this.SuperiorLevel_.HasValue ? null : (object)this.SuperiorLevel_.Value);
                case "shield-size":
                    return (!this.ShieldSize_.HasValue ? null : (object)this.ShieldSize_.Value);
                case "skill":
                    return (!this.Skill_.HasValue ? null : (object)this.Skill_.Value);
                case "slots":
                    return (!this.Slots_.HasValue ? null : (object)this.Slots_.Value);
                case "stack-size":
                    return (!this.StackSize_.HasValue ? null : (object)this.StackSize_.Value);
                case "storage-slots":
                    return (!this.StorageSlots_.HasValue ? null : (object)this.StorageSlots_.Value);
                case "type":
                    return (!this.Type_.HasValue ? null : (object)this.Type_.Value);
                case "unknown-1":
                    return (!this.Unknown1_.HasValue ? null : (object)this.Unknown1_.Value);
                case "unknown-2":
                    return (!this.Unknown2_.HasValue ? null : (object)this.Unknown2_.Value);
                case "unknown-3":
                    return (!this.Unknown3_.HasValue ? null : (object)this.Unknown3_.Value);
                case "unknown-4":
                    return (!this.Unknown4_.HasValue ? null : (object)this.Unknown4_.Value);
                case "use-delay":
                    return (!this.UseDelay_.HasValue ? null : (object)this.UseDelay_.Value);
                case "valid-targets":
                    return (!this.ValidTargets_.HasValue ? null : (object)this.ValidTargets_.Value);
                default:
                    return null;
            }
        }

        protected override void LoadField(string Field, System.Xml.XmlElement Node)
        {
            switch (Field)
            {
                // "Simple" Fields
                case "activation-time":
                    this.ActivationTime_ = (ushort)this.LoadUnsignedIntegerField(Node);
                    break;
                case "casting-time":
                    this.CastingTime_ = (byte)this.LoadUnsignedIntegerField(Node);
                    break;
                case "damage":
                    this.Damage_ = (ushort)this.LoadUnsignedIntegerField(Node);
                    break;
                case "delay":
                    this.Delay_ = (short)this.LoadSignedIntegerField(Node);
                    break;
                case "description":
                    this.Description_ = this.LoadTextField(Node);
                    break;
                case "dps":
                    this.DPS_ = (ushort)this.LoadUnsignedIntegerField(Node);
                    break;
                case "element":
                    this.Element_ = (Element)this.LoadHexField(Node);
                    break;
                case "element-charge":
                    this.ElementCharge_ = (uint)this.LoadUnsignedIntegerField(Node);
                    break;
                case "flags":
                    this.Flags_ = (ItemFlags)this.LoadHexField(Node);
                    break;
                case "id":
                    this.ID_ = (uint)this.LoadUnsignedIntegerField(Node);
                    break;
                case "iLevel:":
                    this.iLevel_ = (ushort)this.LoadUnsignedIntegerField(Node);
                    break;
                case "instinct-cost:":
                    this.iLevel_ = (ushort)this.LoadUnsignedIntegerField(Node);
                    break;
                case "jobs":
                    this.Jobs_ = (Job)this.LoadHexField(Node);
                    break;
                case "jug-size":
                    this.JugSize_ = (byte)this.LoadUnsignedIntegerField(Node);
                    break;
                case "level":
                    this.Level_ = (ushort)this.LoadUnsignedIntegerField(Node);
                    break;
                case "log-name-plural":
                    this.LogNamePlural_ = this.LoadTextField(Node);
                    break;
                case "log-name-singular":
                    this.LogNameSingular_ = this.LoadTextField(Node);
                    break;
                case "max-charges":
                    this.MaxCharges_ = (byte)this.LoadUnsignedIntegerField(Node);
                    break;
                case "name":
                    this.Name_ = this.LoadTextField(Node);
                    break;
                case "puppet-slot":
                    this.PuppetSlot_ = (PuppetSlot)this.LoadHexField(Node);
                    break;
                case "races":
                    this.Races_ = (Race)this.LoadHexField(Node);
                    break;
                case "resource-id":
                    this.ResourceID_ = (ushort)this.LoadUnsignedIntegerField(Node);
                    break;
                case "reuse-delay":
                    this.ReuseDelay_ = (uint)this.LoadUnsignedIntegerField(Node);
                    break;
                case "superior-level":
                    this.SuperiorLevel_ = (ushort)this.LoadUnsignedIntegerField(Node);
                    break;
                case "shield-size":
                    this.ShieldSize_ = (ushort)this.LoadUnsignedIntegerField(Node);
                    break;
                case "skill":
                    this.Skill_ = (Skill)this.LoadHexField(Node);
                    break;
                case "slots":
                    this.Slots_ = (EquipmentSlot)this.LoadHexField(Node);
                    break;
                case "stack-size":
                    this.StackSize_ = (ushort)this.LoadUnsignedIntegerField(Node);
                    break;
                case "storage-slots":
                    this.StorageSlots_ = (int)this.LoadSignedIntegerField(Node);
                    break;
                case "type":
                    this.Type_ = (ItemType)this.LoadHexField(Node);
                    break;
                case "unknown-1":
                    this.Unknown1_ = (uint)this.LoadUnsignedIntegerField(Node);
                    break;
                case "unknown-2":
                    this.Unknown2_ = (ushort)this.LoadUnsignedIntegerField(Node);
                    break;
                case "unknown-3":
                    this.Unknown3_ = (uint)this.LoadUnsignedIntegerField(Node);
                    break;
                case "unknown-4":
                    this.Unknown4_ = (uint)this.LoadUnsignedIntegerField(Node);
                    break;
                case "use-delay":
                    this.UseDelay_ = (ushort)this.LoadUnsignedIntegerField(Node);
                    break;
                case "valid-targets":
                    this.ValidTargets_ = (ValidTarget)this.LoadHexField(Node);
                    break;
                // Sub-Things
                case "icon":
                    if (this.Icon_ == null)
                    {
                        this.Icon_ = new Graphic();
                    }
                    this.LoadThingField(Node, this.Icon_);
                    break;
            }
        }

        #endregion

        #region ROM File Reading

        public enum Type
        {
            Unknown,
            Armor,
            Currency,
            Item,
            PuppetItem,
            UsableItem,
            Weapon,
            Slip,
            Instinct,
            Monipulator
        };

        public static void DeduceType(BinaryReader BR, out Type T)
        {
            T = Type.Unknown;
            byte[] FirstItem = null;
            long Position = BR.BaseStream.Position;
            BR.BaseStream.Position = 0;
            try
            {
                while (BR.BaseStream.Position != BR.BaseStream.Length)
                {
                    FirstItem = BR.ReadBytes(0x4);
                    BR.BaseStream.Position += (0xc00 - 0x4);
                    FFXIEncryption.Rotate(FirstItem, 5);
                    {
                        // Type -> Based on ID
                        uint ID = 0;
                        for (int i = 0; i < 4; ++i)
                        {
                            ID <<= 8;
                            ID += FirstItem[3 - i];
                        }
                        if (ID == 0xffff)
                        {
                            T = Type.Currency;
                        }
                        else if (ID < 0x1000)
                        {
                            T = Type.Item;
                        }
                        else if (ID < 0x2000)
                        {
                            T = Type.UsableItem;
                        }
                        else if (ID < 0x2200)
                        {
                            T = Type.PuppetItem;
                        }
                        else if (ID < 0x2800)
                        {
                            T = Type.Item;
                        }
                        else if (ID < 0x4000)
                        {
                            T = Type.Armor;
                        }
                        else if (ID < 0x6000)
                        {
                            T = Type.Weapon;
                        }
                        else if (ID < 0x7000)
                        {
                            T = Type.Armor;
                        }
                        else if (ID < 0x7400)
                        {
                            T = Type.Slip;
                        }
                        else if (ID < 0x7800)
                        {
                            T = Type.Instinct;
                        }
                        else if (ID < 0xF200)
                        {
                            T = Type.Monipulator;
                        }
                    }
                    if (T != Type.Unknown)
                    {
                        break;
                    }
                }
            }
            catch { }
            BR.BaseStream.Position = Position;
        }

        public bool Read(BinaryReader BR, Type T)
        {
            this.Clear();
            try
            {
                byte[] ItemBytes = BR.ReadBytes(0xC00);
                FFXIEncryption.Rotate(ItemBytes, 5);
                BR = new BinaryReader(new MemoryStream(ItemBytes, false));
                BR.BaseStream.Seek(0x280, SeekOrigin.Begin);
                Graphic G = new Graphic();
                int GraphicSize = BR.ReadInt32();
                if (GraphicSize < 0 || !G.Read(BR) || BR.BaseStream.Position != 0x280 + 4 + GraphicSize)
                {
                    BR.Close();
                    return false;
                }
                this.Icon_ = G;
                BR.BaseStream.Seek(0, SeekOrigin.Begin);
            }
            catch
            {
                return false;
            }
            // Common Fields (14 bytes)
            this.ID_ = BR.ReadUInt32();
            this.Flags_ = (ItemFlags)BR.ReadUInt16();
            this.StackSize_ = BR.ReadUInt16(); // 0xe0ff for Currency, which kinda suggests this is really 2 separate bytes
            this.Type_ = (ItemType)BR.ReadUInt16();
            this.ResourceID_ = BR.ReadUInt16();
            this.ValidTargets_ = (ValidTarget)BR.ReadUInt16();
            // Extra Fields (22/30/10/6/2 bytes for Armor/Weapon/Puppet/Item/UsableItem)

            if (T == Type.Armor || T == Type.Weapon)
            {
                this.Level_ = BR.ReadUInt16();
                this.Slots_ = (EquipmentSlot)BR.ReadUInt16();
                this.Races_ = (Race)BR.ReadUInt16();
                this.Jobs_ = (Job)BR.ReadUInt32();
                this.SuperiorLevel_ = BR.ReadUInt16();
                if (T == Type.Armor)
                {
                    this.ShieldSize_ = BR.ReadUInt16();
                }
                else
                {
                    // Weapon
                    this.Unknown4_ = BR.ReadUInt16();
                    this.Damage_ = BR.ReadUInt16();
                    this.Delay_ = BR.ReadInt16();
                    this.DPS_ = BR.ReadUInt16();
                    this.Skill_ = (Skill)BR.ReadByte();
                    this.JugSize_ = BR.ReadByte();
                    this.Unknown1_ = BR.ReadUInt32();
                }
                this.MaxCharges_ = BR.ReadByte();
                this.CastingTime_ = BR.ReadByte();
                this.UseDelay_ = BR.ReadUInt16();
                this.ReuseDelay_ = BR.ReadUInt32();
                this.Unknown2_ = BR.ReadUInt16();
                this.iLevel_ = BR.ReadUInt16();
                this.Unknown3_ = BR.ReadUInt32();
            }
            else if (T == Type.PuppetItem)
            {
                this.PuppetSlot_ = (PuppetSlot)BR.ReadUInt16();
                this.ElementCharge_ = BR.ReadUInt32();
                this.Unknown3_ = BR.ReadUInt32();
            }
            else if (T == Type.Instinct)
            {
                BR.ReadUInt32();
                BR.ReadUInt32();
                BR.ReadUInt16();
                this.InstinctCost_ = BR.ReadUInt16();
                BR.ReadUInt16();
                BR.ReadUInt32();
                BR.ReadUInt32();
                BR.ReadUInt32();
            }
            else if (T == Type.Item)
            {
                switch (this.Type_.Value)
                {
                    case ItemType.Flowerpot:
                    case ItemType.Furnishing:
                    case ItemType.Mannequin:
                        this.Element_ = (Element)BR.ReadUInt16();
                        this.StorageSlots_ = BR.ReadInt32();
                        this.Unknown3_ = BR.ReadUInt32();
                        break;
                    default:
                        this.Unknown2_ = BR.ReadUInt16();
                        this.Unknown3_ = BR.ReadUInt32();
                        this.Unknown3_ = BR.ReadUInt32();
                        break;
                }
            }
            else if (T == Type.UsableItem)
            {
                this.ActivationTime_ = BR.ReadUInt16();
                this.Unknown1_ = BR.ReadUInt32();
                this.Unknown3_ = BR.ReadUInt32();
                this.Unknown4_ = BR.ReadUInt32();
            }
            else if (T == Type.Currency)
            {
                this.Unknown2_ = BR.ReadUInt16();
            }
            else if (T == Type.Slip)
            {
                this.Unknown1_ = BR.ReadUInt16();
                for (int counter = 0; counter < 17; counter++)
                {
                    BR.ReadUInt32();
                }
            }
            else if (T == Type.Monipulator)
            {
                this.Unknown1_ = BR.ReadUInt16();
                for (int counter = 0; counter < 24; counter++)
                {
                    BR.ReadInt32();
                }
            }
            // Next Up: Strings (variable size)
            long StringBase = BR.BaseStream.Position;
            uint StringCount = BR.ReadUInt32();
            if (StringCount > 9)
            {
                // Sanity check, for safety - 0 strings is fine for now
                this.Clear();
                return false;
            }
            FFXIEncoding E = new FFXIEncoding();
            string[] Strings = new string[StringCount];
            for (byte i = 0; i < StringCount; ++i)
            {
                long Offset = StringBase + BR.ReadUInt32();
                uint Flag = BR.ReadUInt32();
                if (Offset < 0 || Offset + 0x20 > 0x280 || (Flag != 0 && Flag != 1))
                {
                    this.Clear();
                    return false;
                }
                // Flag seems to be 1 if the offset is not actually an offset. Could just be padding to make StringCount unique per language, or it could be an indication
                // of the pronoun to use (a/an/the/...). The latter makes sense because of the increased number of such flags for french and german.
                if (Flag == 0)
                {
                    BR.BaseStream.Position = Offset;
                    Strings[i] = this.ReadString(BR, E);
                    if (Strings[i] == null)
                    {
                        this.Clear();
                        return false;
                    }
                    BR.BaseStream.Position = StringBase + 4 + 8 * (i + 1);
                }
            }
            // Assign the strings to the proper fields
            switch (StringCount)
            {
                case 1:
                    this.Name_ = Strings[0];
                    break;
                case 2: // Japanese
                    this.Name_ = Strings[0];
                    this.Description_ = Strings[1];
                    break;
                case 5: // English
                    this.Name_ = Strings[0];
                    // unused:              Strings[1]
                    this.LogNameSingular_ = Strings[2];
                    this.LogNamePlural_ = Strings[3];
                    this.Description_ = Strings[4];
                    break;
                case 6: // French
                    this.Name_ = Strings[0];
                    // unused:              Strings[1]
                    // unused:              Strings[2]
                    this.LogNameSingular_ = Strings[3];
                    this.LogNamePlural_ = Strings[4];
                    this.Description_ = Strings[5];
                    break;
                case 9: // German
                    this.Name_ = Strings[0];
                    // unused:              Strings[1]
                    // unused:              Strings[2]
                    // unused:              Strings[3]
                    this.LogNameSingular_ = Strings[4];
                    // unused:              Strings[5]
                    // unused:              Strings[6]
                    this.LogNamePlural_ = Strings[7];
                    this.Description_ = Strings[8];
                    break;
            }
            BR.Close();
            return true;
        }

        private string ReadString(BinaryReader BR, Encoding E)
        {
            // Read past "padding"
            if (BR.ReadUInt32() != 1)
            {
                return null;
            }
            for (byte i = 0; i < 6; ++i)
            {
                if (BR.ReadUInt32() != 0)
                {
                    return null;
                }
            }
            List<byte> TextBytes = new List<byte>();
            while (BR.BaseStream.Position < 0x280)
            {
                byte[] Next4 = BR.ReadBytes(4);
                byte UsableBytes = (byte)Next4.Length;
                while (UsableBytes > 0 && Next4[UsableBytes - 1] == 0)
                {
                    --UsableBytes;
                }
                if (UsableBytes != 4)
                {
                    byte i = 0;
                    while (UsableBytes-- > 0)
                    {
                        TextBytes.Add(Next4[i++]);
                    }
                    return E.GetString(TextBytes.ToArray()).Replace("\n", Environment.NewLine);
                }
                else
                {
                    TextBytes.AddRange(Next4);
                }
            }
            return null;
        }

        #endregion
    }
}
