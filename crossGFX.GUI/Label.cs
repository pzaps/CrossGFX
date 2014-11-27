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
using System.Text.RegularExpressions;

namespace crossGFX.GUI
{
    public class Label : Control
    {

        public enum VerticalAlignment { Top, Center, Bottom }
        public enum HorizontalAlignment { Left, Center, Right }

        public Label() {
            this.Value = new RichString();
            this.Value.Font = GUI.Instance.Skin.Font;
            this.VAlign = VerticalAlignment.Center;
            this.ResetTextPosition();
        }

        public override void DrawMiddle(IRenderTarget renderTarget) {
            if(string.IsNullOrEmpty(this.Value.Text) || this.Value.Font == null) return;
            renderTarget.DrawString(textPosition + Offset, this.Value.Font, this.VisibleText,
                this.Value.TextSize, this.Value.Color, this.Value.Bold, this.Value.Italic, this.Value.Underline);
            
        }
        
        public override void Tick(IWindow window, TickEventArgs e) {
            if(this.Value.Modified) {
                this.ResetTextPosition();
                this.MaxSize = Value.Size;
                this.Value.Modified = false;
            }
        }
        
        protected Point textPosition;

        protected virtual void ResetTextPosition() {
            if(this.Value.Font == null) return;
            Size size = this.Value.Font.MeasureTextSize(this.VisibleText, this.Value.TextSize, this.Value.Bold);
            Point point = Point.Empty;

            switch(Align) {
                //case HorizontalAlignment.Left: point.X = 0; break;
                case HorizontalAlignment.Center: point.X = Bounds.Width / 2 - size.Width / 2; break;
                case HorizontalAlignment.Right: point.X = Bounds.Width - size.Width; break;
            }

            switch(VAlign) {
                //case VerticalAlignment.Top: point.Y = 0; break;
                case VerticalAlignment.Center: point.Y = Bounds.Height / 2 - size.Height / 2; break;
                case VerticalAlignment.Bottom: point.Y = Bounds.Height - size.Height; break;
            }

            this.textPosition = point;
        }

        /// <summary>
        /// Gets the coordinates of specified character in the text.
        /// </summary>
        /// <param name="index">Character index.</param>
        /// <returns>Character position in local coordinates.</returns>
        public Point GetCharacterPosition(int index) {
            if (String.IsNullOrEmpty(this.Value.Text) || index == 0) {
                return textPosition;
            }

            String sub = this.VisibleText.Substring(0, index);
            Size p = this.Value.Font.MeasureTextSize(sub, this.Value.TextSize, this.Value.Bold);

            /*if (p.Height >= this.Value.TextSize)
                p = new Size(p.Width, p.Height - this.Value.TextSize);*/

            return new Point(p.Width, this.textPosition.Y);
        }
        
        RichString value;
        public RichString Value {
            get { return value; }
            set { 
                this.value = (value != null) ? value : new RichString();
                this.ResetTextPosition();
            }
        }

        public string VisibleText {
            get { return ((Mask == null) ? this.Value.Text : Regex.Replace(this.Value.Text, ".", Mask.ToString())); }
        }

        HorizontalAlignment align;
        public HorizontalAlignment Align { 
            get { return align; } 
            set {
                this.align = value;
                this.ResetTextPosition();
            }
        }

        VerticalAlignment valign;
        public VerticalAlignment VAlign { 
            get { return valign; } 
            set {
                this.valign = value;
                this.ResetTextPosition();
            }
        }

        public char? Mask { get; set; }

        public virtual void Append(string str) {
            this.Value.Text += str;
        }

        public virtual void Clear() {
            this.Value.Text = "";
        }
    }
}
