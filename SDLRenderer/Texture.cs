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
using SdlDotNet.Graphics;

namespace crossGFX.SDLRenderer
{
    public class Texture : ITexture
    {
        Surface surface;

        public Surface Surface {
            get { return surface; }
        }

        public Texture(Surface surface) {
            this.surface = surface;

            //this.surface.AlphaBlending = true;
            //this.Alpha = 255;
        }

        public void Dispose() {
        }

        public byte Alpha {
            get {
                return surface.Alpha;
            }
            set {
                surface.Alpha = value;
                surface.AlphaBlending = true;
            }
        }

        public int Width {
            get { return surface.Width; }
        }

        public int Height {
            get { return surface.Height; }
        }


        public Color GetPixelColor(int x, int y) {
            return GetPixelColor(x, y);
        }

        public Color GetPixelColor(int x, int y, Color defaultColor) {
            global::System.Drawing.Color color = surface.GetPixel(new global::System.Drawing.Point(x, y));
            return new Color(color.A, color.R, color.G, color.B);
        }
    }
}
