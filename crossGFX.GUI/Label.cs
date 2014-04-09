// Copyright (c) 2014 CrossGFX Team

// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation;
// version 3.0.

// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details.

// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, visit
// https://www.gnu.org/licenses/lgpl.html.

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

        protected Point textPosition;

        public Label() {
            this.textSize = 12;
            this.foreColor = Color.Black;
            this.font = GUI.Instance.Skin.Font;
            this.VAlign = VerticalAlignment.Center;
        }

        /// <summary>
        /// Gets the coordinates of specified character in the text.
        /// </summary>
        /// <param name="index">Character index.</param>
        /// <returns>Character position in local coordinates.</returns>
        public Point GetCharacterPosition(int index) {
            if (text.Length == 0 || index == 0) {
                return new Point(1, 0);
            }

            String sub = ((maskedText == null) ? text : maskedText).Substring(0, index);
            Size p = font.MeasureTextSize(sub, textSize);

            if (p.Height >= textSize)
                p = new Size(p.Width, p.Height - textSize);

            return new Point(p.Width, p.Height);
        }

        public override void DrawMiddle(IRenderTarget renderTarget) {
            base.DrawMiddle(renderTarget);

            if (!string.IsNullOrEmpty(text) && font != null) {
                renderTarget.DrawString(font, ((maskedText == null) ? text : maskedText), textSize, foreColor, textPosition);
            }
        }

        protected void SetTextPosition(int x, int y) {
            this.textPosition = new Point(x, y);
        }

        void ResetTextPosition() {
            if(!string.IsNullOrEmpty(Text)) {
                Size size = Font.MeasureTextSize(Text, textSize);
                Point point = new Point(0, 0);
                switch(Align) {
                    case HorizontalAlignment.Left: point.X = 0; break;
                    case HorizontalAlignment.Center: point.X = Bounds.Width / 2 - size.Width / 2; break;
                    case HorizontalAlignment.Right: point.X = Bounds.Width - size.Width; break;
                }
                switch(VAlign) {
                    case VerticalAlignment.Top: point.Y = 0; break;
                    case VerticalAlignment.Center: point.Y = Bounds.Height / 2 - size.Height / 2; break;
                    case VerticalAlignment.Bottom: point.Y = Bounds.Height - size.Height; break;
                }
                this.textPosition = point;
            }
        }
        
        IFont font;
        public IFont Font {
            get { return this.font; }
            set {
                this.font = value;
                this.ResetTextPosition();
            }
        }
        
        string text;
        public string Text {
            get { return text; }
            set { 
                this.text = value;
                if(this.Mask != null)
                    this.maskedText = Regex.Replace(text, ".", Mask.ToString());
                this.ResetTextPosition();
            }
        }

        int textSize;
        public int TextSize { 
            get { return this.textSize; } 
            set { 
                this.textSize = value; 
                this.ResetTextPosition();
            } 
        }
        
        Color foreColor;
        public Color ForeColor {
            get { return foreColor; }
            set { this.foreColor = value; }
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

        String maskedText;
        char? mask;
        public char? Mask { 
            get{ return mask; } 
            set{
                this.mask = value;
                if(value != null) this.Text = this.Text;
                else this.maskedText = null;
            } 
        }
    }
}
