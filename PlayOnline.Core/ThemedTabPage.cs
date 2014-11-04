// Copyright © 2004-2014 Tim Van Holder, Windower Team
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS"
// BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using System.Windows.Forms.VisualStyles;

namespace System.Windows.Forms
{
    public class ThemedTabPage : TabPage
    {
        public ThemedTabPage()
            : base() { }

        public ThemedTabPage(string text)
            : base(text) { }

        // The UseVisualStyleBackColor only uses a plain background color, not the background rendering for
        // the current visual style.  This corrects that error.
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            if (!VisualStyleRenderer.IsSupported || !this.UseVisualStyleBackColor)
            {
                base.OnPaintBackground(e);
                return;
            }
            TabControl P = this.Parent as TabControl;
            if (P == null || P.Appearance != TabAppearance.Normal)
            {
                base.OnPaintBackground(e);
                return;
            }
            VisualStyleRenderer VSR = new VisualStyleRenderer(VisualStyleElement.Tab.Body.Normal);
            VSR.DrawBackground(e.Graphics, this.ClientRectangle, e.ClipRectangle);
        }
    }
}
