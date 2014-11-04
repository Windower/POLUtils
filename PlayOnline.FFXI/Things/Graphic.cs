// Copyright © 2004-2014 Tim Van Holder, Windower Team
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS"
// BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

#define EnableTransparency

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using PlayOnline.Core;

namespace PlayOnline.FFXI.Things
{
    public class Graphic : Thing
    {
        public Graphic()
        {
            // Fill Thing helpers
            this.IconField_ = "image";
            // Clear fields
            this.Clear();
        }

        public override string ToString()
        {
            return String.Format("[{0}] {1} ({2}, {3}x{4})", this.Category_, this.ID_, this.Format_,
                this.Width_.GetValueOrDefault(0), this.Height_.GetValueOrDefault(0));
        }

        public static List<string> AllFields
        {
            get
            {
                return
                    new List<string>(new string[]
                    {
                        "format", "flag", "category", "id", "width", "height", "planes", "bits", "compression", "size",
                        "horizontal-resolution", "vertical-resolution", "used-colors", "important-colors", "image",
                    });
            }
        }

        public override List<string> GetAllFields() { return Graphic.AllFields; }

        public bool Read(BinaryReader BR)
        {
            try
            {
                this.Flag_ = BR.ReadByte();
                if (this.Flag_ != 0x91 && this.Flag_ != 0xA1 && this.Flag_ != 0xB1)
                {
                    // Accuracy unknown
                    this.Clear();
                    return false;
                }
                // Assumes that BR is set to ASCII encoding
                this.Category_ = new string(BR.ReadChars(8)).TrimEnd(' ');
                this.ID_ = new string(BR.ReadChars(8)).TrimEnd(' ');
                // Read BITMAPINFO structure length
                if (BR.ReadUInt32() != 40)
                {
                    this.Clear();
                    return false;
                }
                // Read BITMAPINFO structure
                this.Width_ = BR.ReadInt32();
                this.Height_ = BR.ReadInt32();
                this.Planes_ = BR.ReadUInt16();
                this.BitCount_ = BR.ReadUInt16();
                this.Compression_ = BR.ReadUInt32();
                this.ImageSize_ = BR.ReadUInt32();
                this.HorizontalResolution_ = BR.ReadUInt32();
                this.VerticalResolution_ = BR.ReadUInt32();
                this.UsedColors_ = BR.ReadUInt32();
                this.ImportantColors_ = BR.ReadUInt32();
                // Sanity check on the values in the structure
                if (this.Width_ < 0 || this.Width_ > 16 * 1024 || this.Height_ < 0 || this.Height_ > 16 * 1024 || this.Planes_ != 1)
                {
                    this.Clear();
                    return false;
                }
                if (this.Flag_ == 0xA1) // Assume DirectX texture
                {
                    this.ReadDXT(BR);
                }
                else if (this.Flag_ == 0x91 || this.Flag_ == 0xB1) // Bitmap
                {
                    this.ReadBitmap(BR);
                }
            }
            catch
            {
                this.Clear();
            }
            return (this.Image_ != null);
        }

        #region Data Fields

        private String Format_;
        private byte? Flag_;
        private String Category_;
        private String ID_;
        private int? Width_;
        private int? Height_;
        private ushort? Planes_;
        private ushort? BitCount_;
        private uint? Compression_;
        private uint? ImageSize_;
        private uint? HorizontalResolution_;
        private uint? VerticalResolution_;
        private uint? UsedColors_;
        private uint? ImportantColors_;
        private Image Image_;

        #endregion

        #region Thing Members

        public override bool HasField(string Field)
        {
            switch (Field)
            {
                // Objects
            case "category":
                return (this.Category_ != null);
            case "format":
                return (this.Format_ != null);
            case "id":
                return (this.ID_ != null);
            case "image":
                return (this.Image_ != null);
                // Nullables
            case "bits":
                return this.BitCount_.HasValue;
            case "compression":
                return this.Compression_.HasValue;
            case "flag":
                return this.Flag_.HasValue;
            case "height":
                return this.Height_.HasValue;
            case "horizontal-resolution":
                return this.HorizontalResolution_.HasValue;
            case "important-colors":
                return this.ImportantColors_.HasValue;
            case "planes":
                return this.Planes_.HasValue;
            case "size":
                return this.ImageSize_.HasValue;
            case "used-colors":
                return this.UsedColors_.HasValue;
            case "vertical-resolution":
                return this.VerticalResolution_.HasValue;
            case "width":
                return this.Width_.HasValue;
            default:
                return false;
            }
        }

        public override string GetFieldText(string Field)
        {
            switch (Field)
            {
                // Objects
            case "category":
                return this.Category_;
            case "format":
                return this.Format_;
            case "id":
                return this.ID_;
            case "image":
                return I18N.GetText("ImageText");
                // Nullables
            case "bits":
                return (!this.BitCount_.HasValue ? String.Empty : this.BitCount_.Value.ToString());
            case "compression":
                return (!this.Compression_.HasValue ? String.Empty : this.Compression_.Value.ToString());
            case "flag":
                return (!this.Flag_.HasValue ? String.Empty : String.Format("{0:X2}", this.Flag_.Value));
            case "height":
                return (!this.Height_.HasValue ? String.Empty : this.Height_.Value.ToString());
            case "horizontal-resolution":
                return (!this.HorizontalResolution_.HasValue ? String.Empty : this.HorizontalResolution_.Value.ToString());
            case "important-colors":
                return (!this.ImportantColors_.HasValue ? String.Empty : this.ImportantColors_.Value.ToString());
            case "planes":
                return (!this.Planes_.HasValue ? String.Empty : this.Planes_.Value.ToString());
            case "size":
                return (!this.ImageSize_.HasValue ? String.Empty : this.ImageSize_.Value.ToString());
            case "used-colors":
                return (!this.UsedColors_.HasValue ? String.Empty : this.UsedColors_.Value.ToString());
            case "vertical-resolution":
                return (!this.VerticalResolution_.HasValue ? String.Empty : this.VerticalResolution_.Value.ToString());
            case "width":
                return (!this.Width_.HasValue ? String.Empty : this.Width_.Value.ToString());
            default:
                return null;
            }
        }

        public override object GetFieldValue(string Field)
        {
            switch (Field)
            {
                // Objects
            case "category":
                return this.Category_;
            case "format":
                return this.Format_;
            case "id":
                return this.ID_;
            case "image":
                return this.Image_;
                // Nullables
            case "bits":
                return (!this.BitCount_.HasValue ? null : (object)this.BitCount_.Value);
            case "compression":
                return (!this.Compression_.HasValue ? null : (object)this.Compression_.Value);
            case "flag":
                return (!this.Flag_.HasValue ? null : (object)this.Flag_.Value);
            case "height":
                return (!this.Height_.HasValue ? null : (object)this.Height_.Value);
            case "horizontal-resolution":
                return (!this.HorizontalResolution_.HasValue ? null : (object)this.HorizontalResolution_.Value);
            case "important-colors":
                return (!this.ImportantColors_.HasValue ? null : (object)this.ImportantColors_.Value);
            case "planes":
                return (!this.Planes_.HasValue ? null : (object)this.Planes_.Value);
            case "size":
                return (!this.ImageSize_.HasValue ? null : (object)this.ImageSize_.Value);
            case "used-colors":
                return (!this.UsedColors_.HasValue ? null : (object)this.UsedColors_.Value);
            case "vertical-resolution":
                return (!this.VerticalResolution_.HasValue ? null : (object)this.VerticalResolution_.Value);
            case "width":
                return (!this.Width_.HasValue ? null : (object)this.Width_.Value);
            default:
                return null;
            }
        }

        protected override void LoadField(string Field, System.Xml.XmlElement Node)
        {
            switch (Field)
            {
            case "bits":
                this.BitCount_ = (ushort)this.LoadUnsignedIntegerField(Node);
                break;
            case "category":
                this.Category_ = this.LoadTextField(Node);
                break;
            case "compression":
                this.Compression_ = (uint)this.LoadUnsignedIntegerField(Node);
                break;
            case "flag":
                this.Flag_ = (byte)this.LoadUnsignedIntegerField(Node);
                break;
            case "format":
                this.Format_ = this.LoadTextField(Node);
                break;
            case "height":
                this.Height_ = (int)this.LoadSignedIntegerField(Node);
                break;
            case "horizontal-resolution":
                this.HorizontalResolution_ = (uint)this.LoadUnsignedIntegerField(Node);
                break;
            case "id":
                this.ID_ = this.LoadTextField(Node);
                break;
            case "important-colors":
                this.ImportantColors_ = (uint)this.LoadUnsignedIntegerField(Node);
                break;
            case "planes":
                this.Planes_ = (ushort)this.LoadUnsignedIntegerField(Node);
                break;
            case "size":
                this.ImageSize_ = (uint)this.LoadUnsignedIntegerField(Node);
                break;
            case "used-colors":
                this.UsedColors_ = (uint)this.LoadUnsignedIntegerField(Node);
                break;
            case "vertical-resolution":
                this.VerticalResolution_ = (uint)this.LoadUnsignedIntegerField(Node);
                break;
            case "width":
                this.Width_ = (int)this.LoadSignedIntegerField(Node);
                break;
            case "image":
                this.Image_ = this.LoadImageField(Node);
                if (this.Image_ != null)
                {
                    // Fill in a few other fields if needed
                    if (!this.Width_.HasValue)
                    {
                        this.Width_ = this.Image_.Width;
                    }
                    if (!this.Height_.HasValue)
                    {
                        this.Height_ = this.Image_.Height;
                    }
                    if (!this.VerticalResolution_.HasValue)
                    {
                        this.VerticalResolution_ = (uint)this.Image_.VerticalResolution;
                    }
                    if (!this.HorizontalResolution_.HasValue)
                    {
                        this.HorizontalResolution_ = (uint)this.Image_.HorizontalResolution;
                    }
                }
                break;
            }
        }

        public override void Clear()
        {
            // Dispose of the fields if needed
            if (this.Image_ != null)
            {
                this.Image_.Dispose();
            }
            // Null them
            this.BitCount_ = null;
            this.Category_ = null;
            this.Compression_ = null;
            this.Flag_ = null;
            this.Format_ = null;
            this.Height_ = null;
            this.HorizontalResolution_ = null;
            this.ID_ = null;
            this.ImageSize_ = null;
            this.Image_ = null;
            this.ImportantColors_ = null;
            this.Planes_ = null;
            this.UsedColors_ = null;
            this.VerticalResolution_ = null;
            this.Width_ = null;
        }

        public override List<PropertyPages.IThing> GetPropertyPages()
        {
            List<PropertyPages.IThing> Pages = base.GetPropertyPages();
            Pages.Add(new PropertyPages.Graphic(this));
            return Pages;
        }

        #endregion

        #region Image Reading Subroutines

        private void ReadDXT(BinaryReader BR)
        {
            char[] FourCCArray = BR.ReadChars(4);
            Array.Reverse(FourCCArray);
            string FourCC = new string(FourCCArray).TrimEnd('\0');
            // Currently, only the DirectX texture format is (partially) supported
            if (!FourCC.StartsWith("DXT") || this.Height_ % 4 != 0 || this.Width_ % 4 != 0)
            {
                this.Clear();
                return;
            }
            this.Format_ = String.Format(I18N.GetText("GraphicTypeDXT"), FourCC);
            int TexelBlockCount = (this.Height_.Value / 4) * (this.Width_.Value / 4); // 4x4 blocks
            BR.ReadUInt64(); // Unknown
            int x = 0;
            int y = 0;
            PixelFormat PF;
            if (FourCC == "DXT2" || FourCC == "DXT4") // These have premultiplied RGB values
            {
                PF = PixelFormat.Format32bppPArgb;
            }
            else
            {
                PF = PixelFormat.Format32bppArgb;
            }
            Bitmap BM = new Bitmap(this.Width_.Value, this.Height_.Value, PF);
            for (int i = 0; i < TexelBlockCount; ++i)
            {
                Color[] TexelBlock = this.ReadTexelBlock(BR, FourCC);
                for (int j = 0; j < 16; ++j)
                {
                    BM.SetPixel(x + (j % 4), y + (j - (j % 4)) / 4, TexelBlock[j]);
                }
                x += 4;
                if (x >= this.Width_)
                {
                    y += 4;
                    x = 0;
                }
            }
            this.Image_ = BM;
        }

        private void ReadBitmap(BinaryReader BR)
        {
            if (this.Flag_ == 0xB1)
            {
                BR.ReadInt32(); // Unknown - always seems to be 10 (0x0000000A)
            }
            Bitmap BM = new Bitmap(this.Width_.Value, this.Height_.Value, PixelFormat.Format32bppArgb);
            this.Format_ = String.Format(I18N.GetText("GraphicTypeBitmap"), this.BitCount_.Value);
            int PixelCount = this.Height_.Value * this.Width_.Value;
            if (this.BitCount_ == 8)
            {
                // 8-bit, with palette
                Color[] Palette = new Color[256];
                for (int i = 0; i < 256; ++i)
                {
                    Palette[i] = Graphic.ReadColor(BR, 32);
                }
                byte[] BitFields = BR.ReadBytes(PixelCount);
                for (int i = 0; i < PixelCount; ++i)
                {
                    int x = (i % this.Width_.Value);
                    int y = this.Height_.Value - 1 - ((i - x) / this.Width_.Value);
                    BM.SetPixel(x, y, Palette[BitFields[i]]);
                }
            }
            else
            {
                for (int i = 0; i < PixelCount; ++i)
                {
                    int x = (i % this.Width_.Value);
                    int y = this.Height_.Value - 1 - ((i - x) / this.Width_.Value);
                    BM.SetPixel(x, y, Graphic.ReadColor(BR, this.BitCount_.Value));
                }
            }
            this.Image_ = BM;
        }

        private Color[] ReadTexelBlock(BinaryReader BR, string FourCC)
        {
            ulong AlphaBlock = 0;
            if (FourCC == "DXT2" || FourCC == "DXT3" || FourCC == "DXT4" || FourCC == "DXT5")
            {
                AlphaBlock = BR.ReadUInt64();
            }
            else if (FourCC != "DXT1")
            {
                return null;
            }
            ushort C0 = BR.ReadUInt16();
            ushort C1 = BR.ReadUInt16();
            Color[] Colors = new Color[4];
            Colors[0] = Graphic.DecodeRGB565(C0);
            Colors[1] = Graphic.DecodeRGB565(C1);
            if (C0 > C1 || FourCC != "DXT1")
            {
                // opaque, 4-color
                Colors[2] = Color.FromArgb((2 * Colors[0].R + Colors[1].R + 1) / 3, (2 * Colors[0].G + Colors[1].G + 1) / 3,
                    (2 * Colors[0].B + Colors[1].B + 1) / 3);
                Colors[3] = Color.FromArgb((2 * Colors[1].R + Colors[0].R + 1) / 3, (2 * Colors[1].G + Colors[0].G + 1) / 3,
                    (2 * Colors[1].B + Colors[0].B + 1) / 3);
            }
            else
            {
                // 1-bit alpha, 3-color
                Colors[2] = Color.FromArgb((Colors[0].R + Colors[1].R) / 2, (Colors[0].G + Colors[1].G) / 2,
                    (Colors[0].B + Colors[1].B) / 2);
                Colors[3] = Color.Transparent;
            }
            uint CompressedColor = BR.ReadUInt32();
            Color[] DecodedColors = new Color[16];
            for (int i = 0; i < 16; ++i)
            {
                if (FourCC == "DXT2" || FourCC == "DXT3" || FourCC == "DXT4" || FourCC == "DXT5")
                {
                    int A = 255;
#if EnableTransparency
                    if (FourCC == "DXT2" || FourCC == "DXT3")
                    {
                        // Seems to be 8 maximum; so treat 8 as 255 and all other values as 3-bit alpha
                        A = (int)((AlphaBlock >> (4 * i)) & 0xf);
                        if (A >= 8)
                        {
                            A = 255;
                        }
                        else
                        {
                            A <<= 5;
                        }
                    }
                    else
                    {
                        // Interpolated alpha
                        int[] Alphas = new int[8];
                        Alphas[0] = (byte)((AlphaBlock >> 0) & 0xff);
                        Alphas[1] = (byte)((AlphaBlock >> 8) & 0xff);
                        if (Alphas[0] > Alphas[1])
                        {
                            Alphas[2] = (Alphas[0] * 6 + Alphas[1] * 1 + 3) / 7;
                            Alphas[3] = (Alphas[0] * 5 + Alphas[1] * 2 + 3) / 7;
                            Alphas[4] = (Alphas[0] * 4 + Alphas[1] * 3 + 3) / 7;
                            Alphas[5] = (Alphas[0] * 3 + Alphas[1] * 4 + 3) / 7;
                            Alphas[6] = (Alphas[0] * 2 + Alphas[1] * 5 + 3) / 7;
                            Alphas[7] = (Alphas[0] * 1 + Alphas[1] * 6 + 3) / 7;
                        }
                        else
                        {
                            Alphas[2] = (Alphas[0] * 4 + Alphas[1] * 1 + 2) / 5;
                            Alphas[3] = (Alphas[0] * 3 + Alphas[1] * 2 + 2) / 5;
                            Alphas[4] = (Alphas[0] * 2 + Alphas[1] * 3 + 2) / 5;
                            Alphas[5] = (Alphas[0] * 1 + Alphas[1] * 4 + 2) / 5;
                            Alphas[6] = 0;
                            Alphas[7] = 255;
                        }
                        ulong AlphaMatrix = (AlphaBlock >> 16) & 0xffffffffffffL;
                        A = Alphas[(AlphaMatrix >> (3 * i)) & 0x7];
                    }
#endif
                    DecodedColors[i] = Color.FromArgb(A, Colors[CompressedColor & 0x3]);
                }
                else
                {
                    DecodedColors[i] = Colors[CompressedColor & 0x3];
                }
                CompressedColor >>= 2;
            }
            return DecodedColors;
        }

        #endregion

        #region Reading/Writing Color Values

        public static Color ReadColor(BinaryReader BR, int BitDepth)
        {
            switch (BitDepth)
            {
            case 16:
                return Graphic.DecodeRGB565(BR.ReadUInt16());
            case 32:
            case 24:
            {
                int B = BR.ReadByte();
                int G = BR.ReadByte();
                int R = BR.ReadByte();
                int A = 255;
                if (BitDepth == 32)
                {
                    byte SemiAlpha = BR.ReadByte();
                    if (SemiAlpha < 0x80)
                    {
                        A = 2 * SemiAlpha;
                    }
                }
                return Color.FromArgb(A, R, G, B);
            }
            case 8:
            {
                int GrayScale = BR.ReadByte();
                return Color.FromArgb(GrayScale, GrayScale, GrayScale);
            }
            default:
                return Color.HotPink;
            }
        }

        private static Color DecodeRGB565(ushort C)
        {
            int R = ((C & 0xf800) >> 11) * 255 / 31;
            int G = ((C & 0x07e0) >> 5) * 255 / 63;
            int B = (C & 0x001f) * 255 / 31;
            return Color.FromArgb(R, G, B);
        }

        public static void WriteColor(BinaryWriter BW, Color C, int BitDepth)
        {
            switch (BitDepth)
            {
            case 32:
            case 24:
                BW.Write(C.B);
                BW.Write(C.G);
                BW.Write(C.R);
                if (BitDepth == 32)
                {
                    BW.Write((byte)(((int)C.A + 1) / 2));
                }
                break;
            case 16:
                BW.Write(Graphic.EncodeRGB565(C));
                break;
            case 8:
                BW.Write((byte)((C.R + C.G + C.B) / 3));
                break;
            }
        }

        private static ushort EncodeRGB565(Color C)
        {
            ushort ColorValue = 0;
            ColorValue |= (ushort)((C.R >> 3) & 0x1f);
            ColorValue <<= 6;
            ColorValue |= (ushort)((C.G >> 2) & 0x3f);
            ColorValue <<= 5;
            ColorValue |= (ushort)((C.B >> 3) & 0x1f);
            return ColorValue;
        }

        #endregion
    }
}
