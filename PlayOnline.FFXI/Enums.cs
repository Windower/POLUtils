// Copyright © 2004-2014 Tim Van Holder, Nevin Stepan, Windower Team
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS"
// BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using System;

namespace PlayOnline.FFXI
{
    public enum AbilityType : byte
    {
        General = 0x00,
        Job = 0x01,
        Pet = 0x02,
        Weapon = 0x03,
        Trait = 0x04,
        BloodPactRage = 0x06,
        Corsair = 0x08,
        CorsairShot = 0x09,
        BloodPactWard = 0x0a,
        Samba = 0x0b,
        Waltz = 0x0c,
        Step = 0x0d,
        Flourish1 = 0x0e,
        Scholar = 0x0f,
        Jig = 0x10,
        Flourish2 = 0x11,
        Monster = 0x12,
        Flourish3 = 0x13,
        Weaponskill = 0x14,
        Rune = 0x15,
        Ward = 0x16,
        Effusion = 0x17,
    }

    public enum Element : byte
    {
        Fire = 0x00,
        Ice = 0x01,
        Air = 0x02,
        Earth = 0x03,
        Thunder = 0x04,
        Water = 0x05,
        Light = 0x06,
        Dark = 0x07,
        Special = 0x0f, // this is the element set on the Meteor spell
        Undecided = 0xff, // this is the element set on inactive furnishing items in the item data
    }

    public enum ElementColor : byte
    {
        Red = 0x00,
        Clear = 0x01,
        Green = 0x02,
        Yellow = 0x03,
        Purple = 0x04,
        Blue = 0x05,
        White = 0x06,
        Black = 0x07,
    }

    [Flags]
    public enum EquipmentSlot : ushort
    {
        // Specific Slots
        None = 0x0000,
        Main = 0x0001,
        Sub = 0x0002,
        Range = 0x0004,
        Ammo = 0x0008,
        Head = 0x0010,
        Body = 0x0020,
        Hands = 0x0040,
        Legs = 0x0080,
        Feet = 0x0100,
        Neck = 0x0200,
        Waist = 0x0400,
        LEar = 0x0800,
        REar = 0x1000,
        LRing = 0x2000,
        RRing = 0x4000,
        Back = 0x8000,
        // Slot Groups
        Ears = 0x1800,
        Rings = 0x6000,
        All = 0xFFFF,
    }

    [Flags]
    public enum ItemFlags : ushort
    {
        None = 0x0000,
        // Simple Flags - mostly assumed meanings
        WallHanging = 0x0001, // Used by furnishing like paintings.
        Flag01 = 0x0002,
        MysteryBox = 0x0004,  // Can be gained from Gobbie Mystery Box
        MogGarden = 0x0008,   // Can use in Mog Garden
        CanSendPOL = 0x0010,
        Inscribable = 0x0020,
        NoAuction = 0x0040,
        Scroll = 0x0080,
        Linkshell = 0x0100,
        CanUse = 0x0200,
        CanTradeNPC = 0x0400,
        CanEquip = 0x0800,
        NoSale = 0x1000,
        NoDelivery = 0x2000,
        NoTradePC = 0x4000,
        Rare = 0x8000,
        // Combined Flags
        Ex = 0x6040, // NoAuction + NoDelivery + NoTrade
    }

    public enum ItemType : ushort
    {
        Nothing = 0x0000,
        Item = 0x0001,
        QuestItem = 0x0002,
        Fish = 0x0003,
        Weapon = 0x0004,
        Armor = 0x0005,
        Linkshell = 0x0006,
        UsableItem = 0x0007,
        Crystal = 0x0008,
        Currency = 0x0009,
        Furnishing = 0x000A,
        Plant = 0x000B,
        Flowerpot = 0x000C,
        PuppetItem = 0x000D,
        Mannequin = 0x000E,
        Book = 0x000F,
        RacingForm = 0x0010,
        BettingSlip = 0x0011,
        SoulPlate = 0x0012,
        Reflector = 0x0013,
        ItemType20 = 0x0014,
        LotteryTicket = 0x0015,
        MazeTabula_M = 0x0016,
        MazeTabula_R = 0x0017,
        MazeVoucher = 0x0018,
        MazeRune = 0x0019,
        ItemType_26 = 0x001A,
        StorageSlip = 0x001B,
    }

    [Flags]
    public enum Job : uint
    {
        None = 0x00000000,
        All = 0x007FFFFE, // Masks valid jobs
        // Specific
        WAR = 0x00000002,
        MNK = 0x00000004,
        WHM = 0x00000008,
        BLM = 0x00000010,
        RDM = 0x00000020,
        THF = 0x00000040,
        PLD = 0x00000080,
        DRK = 0x00000100,
        BST = 0x00000200,
        BRD = 0x00000400,
        RNG = 0x00000800,
        SAM = 0x00001000,
        NIN = 0x00002000,
        DRG = 0x00004000,
        SMN = 0x00008000,
        BLU = 0x00010000,
        COR = 0x00020000,
        PUP = 0x00040000,
        DNC = 0x00080000,
        SCH = 0x00100000,
        GEO = 0x00200000,
        RUN = 0x00400000,
        MON = 0x00800000,
        JOB24 = 0x01000000,
        JOB25 = 0x02000000,
        JOB26 = 0x04000000,
        JOB27 = 0x08000000,
        JOB28 = 0x10000000,
        JOB29 = 0x20000000,
        JOB30 = 0x40000000,
        JOB31 = 0x80000000,
    }

    public enum MagicType : byte
    {
        None = 0x00,
        WhiteMagic = 0x01,
        BlackMagic = 0x02,
        SummonerPact = 0x03,
        Ninjutsu = 0x04,
        BardSong = 0x05,
        BlueMagic = 0x06,
        Geomancy = 0x07,
    }

    public enum PuppetSlot : byte
    {
        None = 0x00,
        Head = 0x01,
        Body = 0x02,
        Attachment = 0x03,
    }

    [Flags]
    public enum Race : ushort
    {
        None = 0x0000,
        All = 0x01FE,
        // Specific
        HumeMale = 0x0002,
        HumeFemale = 0x0004,
        ElvaanMale = 0x0008,
        ElvaanFemale = 0x0010,
        TarutaruMale = 0x0020,
        TarutaruFemale = 0x0040,
        Mithra = 0x0080,
        Galka = 0x0100,
        // Race Groups
        Hume = 0x0006,
        Elvaan = 0x0018,
        Tarutaru = 0x0060,
        // Gender Groups (with Mithra = female, and Galka = male)
        Male = 0x012A,
        Female = 0x00D4,
    }

    public enum Skill : byte
    {
        None = 0x00,
        HandToHand = 0x01,
        Dagger = 0x02,
        Sword = 0x03,
        GreatSword = 0x04,
        Axe = 0x05,
        GreatAxe = 0x06,
        Scythe = 0x07,
        PoleArm = 0x08,
        Katana = 0x09,
        GreatKatana = 0x0a,
        Club = 0x0b,
        Staff = 0x0c,
        AutomatonMelee = 0x16,
        AutomatonRange = 0x17,
        AutomatonMagic = 0x18,
        Ranged = 0x19,
        Marksmanship = 0x1a,
        Thrown = 0x1b,
        DivineMagic = 0x20,
        HealingMagic = 0x21,
        EnhancingMagic = 0x22,
        EnfeeblingMagic = 0x23,
        ElementalMagic = 0x24,
        DarkMagic = 0x25,
        SummoningMagic = 0x26,
        Ninjutsu = 0x27,
        Singing = 0x28,
        StringInstrument = 0x29,
        WindInstrument = 0x2a,
        BlueMagic = 0x2b,
        Geomancy = 0x2c,
        Handbell = 0x2d,
        Fishing = 0x30,
        // These are assumed values, no known data actually uses them
        Woodworking = 0x31,
        Smithing = 0x32,
        Goldsmithing = 0x33,
        Clothcraft = 0x34,
        Leathercraft = 0x35,
        Bonecraft = 0x36,
        Alchemy = 0x37,
        Cooking = 0x38,
        // Set on pet food
        Special = 0xff
    }

    [Flags]
    public enum ValidTarget : ushort
    {
        None = 0x00,
        Self = 0x01,
        Player = 0x02,
        PartyMember = 0x04,
        Ally = 0x08,
        NPC = 0x10,
        Enemy = 0x20,
        Unknown = 0x40,
        Object = 0x60,
        CorpseOnly = 0x80,
        Corpse = 0x9D // CorpseOnly + NPC + Ally + Partymember + Self
    }
}
