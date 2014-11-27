﻿// Copyright (c) 2014 CrossGFX Team

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

namespace crossGFX.SFMLRenderer
{
    class RenderTexture : RenderTarget, IRenderTexture
    {
        Texture texture;
        SFML.Graphics.RenderTexture renderTexture;

        public RenderTexture(SFML.Graphics.RenderTexture renderTexture)
            : base(renderTexture) {
                this.renderTexture = renderTexture;
            texture = new Texture(renderTexture.Texture);
        }

        public ITexture Texture {
            get { return texture; }
        }

        public void Display() {
            this.renderTexture.Display();
        }

        public void SetView(Rectangle viewRectangle) {
            View view = new View(new FloatRect(0, 0, viewRectangle.Width, viewRectangle.Height));
            //view.Viewport = new FloatRect(0, control.Height, control.Width, control.Height);
            renderTexture.SetView(view);
        }
    }
}
