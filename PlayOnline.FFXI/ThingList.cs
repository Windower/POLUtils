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
using System.Xml;
using PlayOnline.FFXI.Things;

namespace PlayOnline.FFXI
{
    /// <summary>
    ///     Heterogenic list of IThing objects.
    /// </summary>
    public class ThingList : List<IThing>
    {
        public bool Load(string FileName)
        {
            try
            {
                this.Clear();
                XmlDocument D = new XmlDocument();
                D.Load(FileName);
                if (D.DocumentElement == null || D.DocumentElement.Name != "thing-list")
                {
                    return false;
                }
                foreach (XmlNode N in D.DocumentElement.ChildNodes)
                {
                    if (N is XmlElement && N.Name == "thing")
                    {
                        XmlElement E = N as XmlElement;
                        IThing Element = Thing.Create(E.GetAttribute("type"));
                        if (Element != null)
                        {
                            Element.Load(E);
                            this.Add(Element);
                        }
                    }
                }
                return true;
            }
            catch
            {
                this.Clear();
                return false;
            }
        }

        public bool Save(string FileName)
        {
            try
            {
                XmlDocument D = new XmlDocument();
                D.AppendChild(D.CreateXmlDeclaration("1.0", "utf-8", null));
                D.AppendChild(D.CreateElement("thing-list"));
                foreach (IThing T in this)
                {
                    D.DocumentElement.AppendChild(T.Save(D));
                }
                D.Save(FileName);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

    /// <summary>
    ///     List of a specific type of IThing.
    /// </summary>
    /// <typeparam name="T">The type of IThing this list can contain.</typeparam>
    public class ThingList<T> : List<T> where T : IThing, new()
    {
        public bool Load(string FileName)
        {
            try
            {
                this.Clear();
                XmlDocument D = new XmlDocument();
                D.Load(FileName);
                if (D.DocumentElement == null || D.DocumentElement.Name != "thing-list")
                {
                    return false;
                }
                foreach (XmlNode N in D.DocumentElement.ChildNodes)
                {
                    if (N is XmlElement && N.Name == "thing")
                    {
                        XmlElement E = N as XmlElement;
                        T Element = new T();
                        Element.Load(E);
                        this.Add(Element);
                    }
                }
                return true;
            }
            catch
            {
                this.Clear();
                return false;
            }
        }

        public bool Save(string FileName)
        {
            try
            {
                XmlDocument D = new XmlDocument();
                D.AppendChild(D.CreateElement("thing-list"));
                foreach (T Element in this)
                {
                    D.DocumentElement.AppendChild(Element.Save(D));
                }
                D.Save(FileName);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
