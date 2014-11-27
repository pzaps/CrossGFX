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
using SFML.Graphics;
using SFML.Window;

namespace crossGFX.SFMLRenderer
{
    class Font : IFont
    {
        SFML.Graphics.Font font;

        public SFML.Graphics.Font BaseFont {
            get { return font; }
        }

        public Font(SFML.Graphics.Font font) {
            this.font = font;
        }

        public void RenderText(IRenderTarget destination, Point position, string text, int characterSize, Color color,
            bool bold, bool italic, bool underline) {

            // todo: this is workaround for SFML.Net bug under mono
            if (Environment.OSVersion.Platform != PlatformID.Win32NT) {
                if (text[text.Length - 1] != '\0')
                    text += '\0';
            }

            Text sfText = new Text(text, font);
            sfText.Position = new Vector2f(position.X, position.Y);
            sfText.CharacterSize = (uint)characterSize;//(uint)font.RealSize; // [omeg] round?
            sfText.Color = new SFML.Graphics.Color(color.R, color.G, color.B, color.A);
            if (bold) sfText.Style |= Text.Styles.Bold;
            if (italic) sfText.Style |= Text.Styles.Italic;
            if (underline) sfText.Style |= Text.Styles.Underlined;

            RenderTarget targetTexture = destination as RenderTarget;
            targetTexture.FlushCache();
            targetTexture.m_RenderState.Texture = null;
            targetTexture.Draw(sfText);

            sfText.Dispose();
        }

        public void Dispose() {
            throw new NotImplementedException();
        }


        public Size MeasureTextSize(string text, int characterSize, bool bold) {
            // todo: this is workaround for SFML.Net bug under mono
            if (Environment.OSVersion.Platform != PlatformID.Win32NT) {
                if (text[text.Length - 1] != '\0')
                    text += '\0';
            }

            Size extents = new Size(0, font.GetLineSpacing((uint)characterSize));
            char prev = '\0';

            for (int i = 0; i < text.Length; i++) {
                char cur = text[i];
                font.GetKerning(prev, cur, (uint)characterSize);
                prev = cur;
                if (cur == '\n' || cur == '\v')
                    continue;
                extents.Width += font.GetGlyph(cur, (uint)characterSize, bold).Advance;
            }

            return extents;

        }

        public Size MeasureTextSize(RichString richString) {
            return this.MeasureTextSize(richString.Text, richString.TextSize, richString.Bold);
        }
    }
}
