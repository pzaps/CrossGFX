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
    class RenderTexture : RenderTarget, IRenderTexture
    {
        Texture texture;

        public RenderTexture(Surface surface)
            : base(surface) {
            texture = new Texture(surface);
        }

        public ITexture Texture {
            get { return texture; }
        }

        public void Display() {
            // We don't need to do anything here
        }

        public void SetView(Rectangle viewRectangle) {
            // We don't need to do anything here
        }
    }
}
