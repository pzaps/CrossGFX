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
using System.IO;
using System.Linq;
using System.Text;

namespace crossGFX.SFMLRenderer
{
    public class Texture : ITexture
    {
        SFML.Graphics.Texture texture;
        SFML.Graphics.Color drawColor;

        public SFML.Graphics.Texture sfTexture {
            get { return texture; }
        }
        public SFML.Graphics.Color DrawColor {
            get { return drawColor; }
        }

        public Texture(SFML.Graphics.Texture texture) {
            this.texture = texture;
            this.drawColor = SFML.Graphics.Color.White;
        }

        public void Dispose() {
        }

        public byte Alpha {
            get {
                return this.drawColor.A;
            }
            set {
                this.drawColor.A = value;
            }
        }

        public int Width {
            get { return (int)texture.Size.X; }
        }

        public int Height {
            get { return (int)texture.Size.Y; }
        }

        public void Save(string fileName) {
            SFML.Graphics.Image image = texture.CopyToImage();
            image.SaveToFile(fileName);
        }

        public Color GetPixelColor(int x, int y) {
            return GetPixelColor(x, y, Color.Red);
        }

        public Color GetPixelColor(int x, int y, Color defaultColor) {
            SFML.Graphics.Image image = texture.CopyToImage();
            SFML.Graphics.Color color = image.GetPixel((uint)x, (uint)y);

            return Color.FromArgb(color.A, color.R, color.G, color.B);
        }
    }
}
