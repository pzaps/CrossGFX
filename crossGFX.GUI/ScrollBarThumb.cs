// Copyright (c) 2014 CrossGFX Team

// This is free and unencumbered software released into the public domain.

// Anyone is free to copy, modify, publish, use, compile, sell, or
// distribute this software, either in source code form or as a compiled
// binary, for any purpose, commercial or non-commercial, and by any
// means.

// In jurisdictions that recognize copyright laws, the author or authors
// of this software dedicate any and all copyright interest in the
// software to the public domain. We make this dedication for the benefit
// of the public at large and to the detriment of our heirs and
// successors. We intend this dedication to be an overt act of
// relinquishment in perpetuity of all present and future rights to this
// software under copyright law.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR
// OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
// ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.

// For more information, please refer to <http://unlicense.org/>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using crossGFX.GUI.Skins;

namespace crossGFX.GUI {
    internal class ScrollBarThumb : Control {

        public override Skinnable DefaultSkinnable {
            get{
                if(Type == ScrollBar.ScrollBarType.Horizontal) return GUI.Instance.Skin.ScrollBarHorizontalThumb;
                else return GUI.Instance.Skin.ScrollBarVerticalThumb;
            }
        }
        
        public override Boolean IsHoverable {get{return true;}}
        public override Boolean IsDownable {get{return true;}}

        internal ScrollBar.ScrollBarType Type { get; private set; }

        internal ScrollBarThumb(ScrollBar.ScrollBarType type) {
            this.Type = type;
        }

        public override bool HandleMouseMoved(Input.MouseMovedEventArgs e) {
            base.HandleMouseMoved(e);
            if(this.Enabled)
                if(this.State == ControlState.Down) {
                    ScrollBar parent = this.Parent as ScrollBar;
                    int x = 0;
                    int y = 0;
                    if(this.Type == ScrollBar.ScrollBarType.Horizontal) {
                        parent.Scroll((int)Math.Round((double)-e.dX * parent.Target.MaxSize.Width / parent.Target.Bounds.Width));
                    } else {
                        parent.Scroll((int)Math.Round((double)-e.dY * parent.Target.MaxSize.Height / parent.Target.Bounds.Height));
                    }
                    this.Location = new Point(x, y);
                    parent.ResetThumb();
                }
                
            this.MouseInBounds = true;
            return false;
        }
    }
}
