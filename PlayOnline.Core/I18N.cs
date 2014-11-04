// Copyright © 2004-2014 Tim Van Holder, Windower Team
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS"
// BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using System;
using System.Globalization;
using System.Reflection;
using System.Resources;

namespace PlayOnline.Core
{
    public class I18N
    {
        public static string GetText(string Name) { return I18N.GetText(Name, Assembly.GetCallingAssembly()); }

        public static string GetText(string Name, Assembly A)
        {
            try
            {
                ResourceManager Resources = new ResourceManager("Messages", A);
                string ResourceString = Resources.GetObject(Name, CultureInfo.CurrentUICulture) as string;
                if (ResourceString == null)
                {
                    ResourceString = Resources.GetObject(Name, CultureInfo.InvariantCulture) as string;
                }
                if (ResourceString != null)
                {
                    return ResourceString;
                }
            }
            catch {}
            return Name;
        }
    }
}
