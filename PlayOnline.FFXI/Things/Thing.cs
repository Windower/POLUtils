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
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using PlayOnline.Core;

namespace PlayOnline.FFXI.Things
{
    public abstract class Thing : IThing
    {
        /// <summary>
        ///     Helper field; if set, it will be used by GetIcon to find and return an icon image.
        /// </summary>
        protected string IconField_;

        #region Implemented IThing Members

        public virtual string TypeName
        {
            get
            {
                string MessageID = String.Format("T:{0}", this.GetType().Name);
                string Result = MessageID;
                try
                {
                    Result = I18N.GetText(MessageID, this.GetType().Assembly);
                }
                catch {}
                if (Result == MessageID)
                {
                    Result = this.GetType().Name;
                }
                return Result;
            }
        }

        public string GetFieldName(string Field)
        {
            string MessageID = String.Format("F:{0}:{1}", this.GetType().Name, Field);
            string Name = MessageID;
            try
            {
                Name = I18N.GetText(MessageID, this.GetType().Assembly);
            }
            catch {}
            if (Name == MessageID)
            {
                MessageID = String.Format("F::{0}", Field);
                Name = MessageID;
                try
                {
                    Name = I18N.GetText(MessageID, this.GetType().Assembly);
                }
                catch {}
                if (Name == MessageID)
                {
                    Name = Field;
                }
            }
            return Name;
        }

        public virtual Image GetIcon()
        {
            if (this.IconField_ == null)
            {
                return null;
            }
            object IconValue = this.GetFieldValue(this.IconField_);
            if (IconValue == null || IconValue is Image)
            {
                return IconValue as Image;
            }
            else if (IconValue is IThing)
            {
                return ((IThing)IconValue).GetIcon();
            }
            return null;
        }

        public virtual List<string> GetFields()
        {
            List<String> Fields = new List<string>();
            foreach (string Field in this.GetAllFields())
            {
                if (this.HasField(Field))
                {
                    Fields.Add(Field);
                }
            }
            return Fields;
        }

        public virtual List<PropertyPages.IThing> GetPropertyPages()
        {
            List<PropertyPages.IThing> Pages = new List<PropertyPages.IThing>();
            Pages.Add(new PropertyPages.Thing(this));
            return Pages;
        }

        public virtual void Load(XmlElement Element)
        {
            this.Clear();
            if (Element == null)
            {
                throw new ArgumentNullException();
            }
            if (Element.Name != "thing" || !Element.HasAttribute("type"))
            {
                throw new ArgumentException(String.Format(I18N.GetText("InvalidThingToLoad"), this.TypeName));
            }
            if (Element.GetAttribute("type") != this.GetType().Name)
            {
                throw new ArgumentException(String.Format(I18N.GetText("WrongThingToLoad"), this.TypeName));
            }
            foreach (string FieldName in this.GetAllFields())
            {
                try
                {
                    XmlElement FieldElement =
                        Element.SelectSingleNode(String.Format("./child::field[@name = '{0}']", FieldName)) as XmlElement;
                    if (FieldElement != null)
                    {
                        this.LoadField(FieldName, FieldElement);
                    }
                }
                catch {}
            }
        }

        public virtual XmlElement Save(XmlDocument Document)
        {
            XmlElement E = Document.CreateElement("thing");
            {
                XmlAttribute A = Document.CreateAttribute("type");
                A.InnerText = this.GetType().Name;
                E.Attributes.Append(A);
            }
            foreach (string FieldName in this.GetFields())
            {
                XmlElement F = Document.CreateElement("field");
                {
                    XmlAttribute A = Document.CreateAttribute("name");
                    A.InnerText = FieldName;
                    F.Attributes.Append(A);
                }
                this.SaveField(FieldName, Document, F);
                E.AppendChild(F);
            }
            return E;
        }

        #endregion

        #region Helper Routines

        protected string FormatTime(double time)
        {
            double seconds = time % 60;
            long minutes = (long)(time - seconds) / 60;
            long hours = minutes / 60;
            minutes %= 60;
            long days = hours / 24;
            hours %= 24;
            string Result = String.Empty;
            if (days > 0)
            {
                Result += String.Format("{0}d", days);
            }
            if (hours > 0)
            {
                Result += String.Format("{0}h", hours);
            }
            if (minutes > 0)
            {
                Result += String.Format("{0}m", minutes);
            }
            if (seconds > 0 || Result == String.Empty)
            {
                Result += String.Format("{0}s", seconds);
            }
            return Result;
        }

        #endregion

        #region Load()/Save() Subroutines

        protected abstract void LoadField(string Field, XmlElement Element);

        // Generics don't allow specifying value types as constraints, nor does it allow casting to
        // an unbounded type parameter.  As a result, a separate function is needed for each array type.

        protected T[] LoadIntegerArray<T>(XmlElement Node)
        {
            int ArraySize = 0;
            try
            {
                ArraySize = int.Parse(Node.GetAttribute("array-size"), NumberStyles.Integer);
            }
            catch
            {
                return null;
            }
            if (ArraySize < 0)
            {
                return null;
            }
            T[] Result = new T[ArraySize];
            for (int i = 0; i < ArraySize; ++i)
            {
                try
                {
                    XmlNode ElementNode = Node.SelectSingleNode(String.Format("./element[@index = '{0}']", i));
                    if (ElementNode != null && ElementNode is System.Xml.XmlElement)
                    {
                        Result[i] = (T)(object)ulong.Parse(ElementNode.InnerText, NumberStyles.Integer);
                    }
                }
                catch
                {
                    return null;
                }
            }
            return Result;
        }

        protected string[] LoadTextArray(XmlElement Node)
        {
            int ArraySize = 0;
            try
            {
                ArraySize = int.Parse(Node.GetAttribute("array-size"), NumberStyles.Integer);
            }
            catch
            {
                return null;
            }
            if (ArraySize < 0)
            {
                return null;
            }
            string[] Result = new string[ArraySize];
            for (int i = 0; i < ArraySize; ++i)
            {
                try
                {
                    XmlNode ElementNode = Node.SelectSingleNode(String.Format("./element[@index = '{0}']", i));
                    if (ElementNode != null && ElementNode is System.Xml.XmlElement)
                    {
                        Result[i] = ElementNode.InnerText;
                    }
                }
                catch
                {
                    return null;
                }
            }
            return Result;
        }

        protected ulong? LoadHexField(XmlElement Node)
        {
            try
            {
                return ulong.Parse(Node.InnerText, NumberStyles.HexNumber);
            }
            catch
            {
                return null;
            }
        }

        protected Image LoadImageField(XmlElement Node)
        {
            if (!Node.HasAttribute("format") || Node.GetAttribute("format") != "image/png")
            {
                return null;
            }
            if (!Node.HasAttribute("encoding") || Node.GetAttribute("encoding") != "base64")
            {
                return null;
            }
            byte[] ImageData = Convert.FromBase64String(Node.InnerText);
            MemoryStream MS = new MemoryStream(ImageData, false);
            Image Result = new Bitmap(MS);
            MS.Close();
            MS.Dispose();
            return Result;
        }

        protected long? LoadSignedIntegerField(XmlElement Node)
        {
            try
            {
                return long.Parse(Node.InnerText, NumberStyles.Integer);
            }
            catch
            {
                return null;
            }
        }

        protected void LoadThingField(XmlElement Node, IThing T)
        {
            XmlElement ThingRoot =
                Node.SelectSingleNode(String.Format("./child::thing[@type = '{0}']", T.GetType().Name)) as XmlElement;
            if (ThingRoot != null)
            {
                T.Load(ThingRoot);
            }
            else
            {
                throw new ArgumentException(String.Format(I18N.GetText("InvalidThingField"), T.TypeName));
            }
        }

        protected string LoadTextField(XmlElement Node) { return Node.InnerText; }

        protected ulong? LoadUnsignedIntegerField(XmlElement Node)
        {
            try
            {
                return ulong.Parse(Node.InnerText, NumberStyles.Integer);
            }
            catch
            {
                return null;
            }
        }

        protected virtual void SaveField(object Value, XmlDocument Document, XmlElement Element)
        {
            // Default Implementation:
            // - Array           -> recurse
            // - IThing          -> Save()
            // - Image           -> save as PNG/base64
            // - Enum            -> save as hex number
            // - Everything Else -> Value.ToString()
            if (Value != null)
            {
                if (Value is IThing)
                {
                    Element.AppendChild(((IThing)Value).Save(Document));
                }
                else if (Value is Array)
                {
                    Array Values = Value as Array;
                    {
                        XmlAttribute A = Document.CreateAttribute("array-size");
                        A.InnerText = Values.Length.ToString();
                        Element.Attributes.Append(A);
                    }
                    for (int i = 0; i < Values.Length; ++i)
                    {
                        XmlElement E = Document.CreateElement("element");
                        {
                            XmlAttribute A = Document.CreateAttribute("index");
                            A.InnerText = i.ToString();
                            E.Attributes.Append(A);
                        }
                        this.SaveField(Values.GetValue(i), Document, E);
                        Element.AppendChild(E);
                    }
                }
                else if (Value is Image)
                {
                    {
                        XmlAttribute A = Document.CreateAttribute("format");
                        A.InnerText = "image/png";
                        Element.Attributes.Append(A);
                    }
                    {
                        XmlAttribute A = Document.CreateAttribute("encoding");
                        A.InnerText = "base64";
                        Element.Attributes.Append(A);
                    }
                    MemoryStream MS = new MemoryStream();
                    ((Image)Value).Save(MS, ImageFormat.Png);
                    Element.InnerText = Convert.ToBase64String(MS.GetBuffer());
                    MS.Close();
                    MS.Dispose();
                }
                else if (Value is Enum) // Store enums as hex numbers
                {
                    Element.InnerText = ((Enum)Value).ToString("X");
                }
                else
                {
                    Element.InnerText = Value.ToString();
                }
            }
        }

        protected virtual void SaveField(string Field, XmlDocument Document, XmlElement Element)
        {
            this.SaveField(this.GetFieldValue(Field), Document, Element);
        }

        #endregion

        #region Abstract IThing Members

        public abstract void Clear();
        public abstract bool HasField(string Field);
        public abstract List<string> GetAllFields();
        public abstract string GetFieldText(string Field);
        public abstract object GetFieldValue(string Field);

        #endregion

        #region Thing Instantiation

        public static IThing Create(string TypeName)
        {
            try
            {
                string FullName = typeof (Thing).Namespace + "." + TypeName;
                return Assembly.GetExecutingAssembly().CreateInstance(FullName, false) as IThing;
            }
            catch {}
            return null;
        }

        #endregion
    }
}
