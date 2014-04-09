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
