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
using System.Globalization;
using System.Reflection;
using System.Resources;

namespace PlayOnline.Core
{
    public class NamedEnum
    {
        private object EnumValue_;
        private string EnumValueName_;

        public NamedEnum(object EnumValue)
        {
            this.EnumValue_ = EnumValue;
            string MessageName = String.Format("E:{0}.{1}", EnumValue.GetType().Name, EnumValue);
            this.EnumValueName_ = I18N.GetText(MessageName, EnumValue.GetType().Assembly);
            if (this.EnumValueName_ == MessageName)
            {
                this.EnumValueName_ = I18N.GetText(MessageName, Assembly.GetCallingAssembly());
            }
            if (this.EnumValueName_ == MessageName)
            {
                this.EnumValueName_ = EnumValue.ToString();
            }
        }

        public string Name
        {
            get { return this.EnumValueName_; }
        }

        public object Value
        {
            get { return this.EnumValue_; }
        }

        public override string ToString() { return this.Name; }

        public static NamedEnum[] GetAll(Type T)
        {
            if (!T.IsEnum)
            {
                return null;
            }
            ArrayList Values = new ArrayList();
            foreach (FieldInfo FI in T.GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                Values.Add(new NamedEnum(FI.GetValue(null)));
            }
            return (NamedEnum[])Values.ToArray(typeof (NamedEnum));
        }
    }
}
