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
using System.Text;
using PlayOnline.Core;

namespace PlayOnline.FFXI
{
    public enum MoonPhase
    {
        NewMoon,
        WaxingCrescent,
        FirstQuarterMoon,
        WaxingGibbous,
        FullMoon,
        WaningGibbous,
        LastQuarterMoon,
        WaningCrescent,
    }

    public class MoonState
    {
        private sbyte Percentage_ = 0;

        public MoonPhase Phase
        {
            get
            {
                if (this.Percentage_ >= -10 && this.Percentage_ <= 5)
                {
                    return MoonPhase.NewMoon;
                }
                if (this.Percentage_ > 5 && this.Percentage_ < 40)
                {
                    return MoonPhase.WaxingCrescent;
                }
                if (this.Percentage_ >= 40 && this.Percentage_ <= 55)
                {
                    return MoonPhase.FirstQuarterMoon;
                }
                if (this.Percentage_ > 55 && this.Percentage_ < 90)
                {
                    return MoonPhase.WaxingGibbous;
                }
                if (this.Percentage_ >= 90 || this.Percentage_ <= -95)
                {
                    return MoonPhase.FullMoon;
                }
                if (this.Percentage_ > -95 && this.Percentage_ <= -60)
                {
                    return MoonPhase.WaningGibbous;
                }
                if (this.Percentage_ >= 60 && this.Percentage_ <= -45)
                {
                    return MoonPhase.LastQuarterMoon;
                }
                if (this.Percentage_ > -45 && this.Percentage_ < -10)
                {
                    return MoonPhase.WaningCrescent;
                }
                return MoonPhase.NewMoon;
            }
        }

        public byte Percentage
        {
            get { return (byte)Math.Abs(this.Percentage_); }
        }

        public override string ToString() { return String.Format("{0}% ({1})", this.Percentage, this.Phase); }

        internal MoonState(long Days)
        {
            byte DayInPhase = (byte)((Days + 26) % 84);
            this.Percentage_ = (sbyte)-Math.Round(((42 - DayInPhase) / 42.0 * 100));
        }
    }

    public class RSEInfo
    {
        private ushort ZoneID_ = 0;
        private Race Race_ = Race.None;

        internal RSEInfo(long Week)
        {
            this.Race_ = (Race)(2 << (byte)((Week + 2) % 8));
            switch (Week % 3)
            {
            case 0:
                this.ZoneID_ = 193;
                break; // Ordelle's Caves
            case 1:
                this.ZoneID_ = 196;
                break; // Gusgen Mines
            case 2:
                this.ZoneID_ = 198;
                break; // Maze of Shakrami
            }
        }

        public Race Race
        {
            get { return this.Race_; }
        }

        public ushort ZoneID
        {
            get { return this.ZoneID_; }
        }

        public string ZoneName
        {
            get { return FFXIResourceManager.GetAreaName(this.ZoneID_); }
        }

        public override string ToString()
        {
            return String.Format(I18N.GetText("{0} ({1})"), new NamedEnum(this.Race_), this.ZoneName);
        }
    }

    public enum DayOfWeek
    {
        Firesday = 0,
        Earthsday = 1,
        Watersday = 2,
        Windsday = 3,
        Iceday = 4,
        Lightningday = 5,
        Lightsday = 6,
        Darksday = 7,
    }

    public class VanadielDate
    {
        private DateTime VanadielEpoch = new DateTime(2002, 6, 23, 15, 00, 00, DateTimeKind.Utc);

        public VanadielDate()
            : this(DateTime.Now) {}

        public VanadielDate(DateTime RealWorldDate)
        {
            double SecondsSinceEpoch = (RealWorldDate.ToUniversalTime() - VanadielEpoch).TotalMilliseconds / 40;
            // 25 times faster, so 40 milliseconds = 1 Vanadiel second
            // Vanadiel "life" began on 0898/02/01 00:00:00 => add 898 years and 1 month
            SecondsSinceEpoch += (898L * 12 + 1) * 30 * 24 * 60 * 60;
            this.Second_ = (SecondsSinceEpoch % 60);
            // Switch to a long so we don't have to worry about rounding
            long Remainder = (long)((SecondsSinceEpoch - this.Second_) / 60);
            this.Minute_ = (byte)(Remainder % 60);
            Remainder /= 60;
            this.Hour_ = (byte)(Remainder % 24);
            Remainder /= 24;
            this.Day_ = (byte)(Remainder % 30);
            Remainder /= 30;
            this.Day_ += 1;
            this.Month_ = (byte)(Remainder % 12);
            Remainder /= 12;
            this.Month_ += 1;
            this.Year_ = (int)Remainder;
        }

        public VanadielDate(int Year, byte Month, byte Day)
        {
            this.Year_ = Year;
            this.Month_ = Month;
            this.Day_ = Day;
        }

        public VanadielDate(int Year, byte Month, byte Day, byte Hour, byte Minute, double Second)
        {
            this.Year_ = Year;
            this.Month_ = Month;
            this.Day_ = Day;
            this.Hour_ = Hour;
            this.Minute_ = Minute;
            this.Second_ = Second;
        }

        private int Year_;

        public int Year
        {
            get { return this.Year_; }
            set { this.Year_ = value; }
        }

        private byte Month_;

        public byte Month
        {
            get { return this.Month_; }
            set { this.Month_ = value; }
        }

        private byte Day_;

        public byte Day
        {
            get { return this.Day_; }
            set { this.Day_ = value; }
        }

        private byte Hour_;

        public byte Hour
        {
            get { return this.Hour_; }
            set { this.Hour_ = value; }
        }

        private byte Minute_;

        public byte Minute
        {
            get { return this.Minute_; }
            set { this.Minute_ = value; }
        }

        private double Second_;

        public double Second
        {
            get { return this.Second_; }
            set { this.Second_ = value; }
        }

        public override string ToString()
        {
            return String.Format("{6} {0:0000}/{1:00}/{2:00} {3:00}:{4:00}:{5:00}", this.Year_, this.Month_, this.Day_, this.Hour_,
                this.Minute_, this.Second_, this.DayOfWeek);
        }

        public DayOfWeek DayOfWeek
        {
            get { return (DayOfWeek)(((this.Year_ * 12L + (this.Month_ - 1)) * 30 + (this.Day_ - 1)) % 8); }
        }

        public RSEInfo RSEInfo
        {
            get { return new RSEInfo(((this.Year_ * 12L + (this.Month_ - 1)) * 30 + (this.Day_ - 1)) / 8); }
        }

        public MoonState MoonState
        {
            get { return new MoonState((this.Year_ * 12L + (this.Month_ - 1)) * 30 + (this.Day_ - 1)); }
        }
    }
}
