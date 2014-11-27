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

namespace crossGFX.GUI.Skins
{
    public class Skin
    {
        public IRenderTexture SkinTexture {get; private set;}

        public IFont Font { get; set; }
        public Skinnable TextField { get; private set; }
        public Skinnable Button { get; private set; }
        public Skinnable CheckBox { get; private set; }
        public Skinnable ScrollBarHorizontal { get; private set; }
        public Skinnable ScrollBarVertical { get; private set; }
        public Skinnable ScrollBarHorizontalThumb { get; private set; }
        public Skinnable ScrollBarVerticalThumb { get; private set; }
        public Skinnable ScrollBarButtonUp { get; private set; }
        public Skinnable ScrollBarButtonDown { get; private set; }
        public Skinnable ScrollBarButtonLeft { get; private set; }
        public Skinnable ScrollBarButtonRight { get; private set; }

        public Skin(IRenderTexture skinTexture) {
            this.SkinTexture = skinTexture;

            TextField = new Skinnable(Skinnable.CreateRectangleSubregions(127, 21, 1), SkinTexture);
            TextField.TextureBaseRegion = new Rectangle(0, 150, 127, 21);
            TextField.TextureDisabledRegion = new Rectangle(0, 194, 127, 21);

            Button = new Skinnable(Skinnable.CreateRectangleSubregions(31, 31, 2), SkinTexture);
            Button.TextureBaseRegion = new Rectangle(480, 0, 31, 31);
            Button.TextureHoverRegion = new Rectangle(480, 32, 31, 31);
            Button.TextureDisabledRegion = new Rectangle(480, 64, 31, 31);
            Button.TextureDownRegion = new Rectangle(480, 96, 31, 31);

            CheckBox = new Skinnable(Skinnable.CreateRectangleSubregions(15, 15, 1), SkinTexture);
            CheckBox.TextureBaseRegion = new Rectangle(464, 32, 15, 15);
            CheckBox.TextureBaseActiveRegion = new Rectangle(448, 32, 15, 15);
            CheckBox.TextureDisabledRegion = new Rectangle(464, 48, 15, 15);
            CheckBox.TextureDisabledActiveRegion = new Rectangle(464, 32, 15, 15);

            ScrollBarHorizontal = new Skinnable(Skinnable.CreateRectangleSubregions(15, 127, 2), SkinTexture);
            ScrollBarHorizontal.TextureBaseRegion = new Rectangle(400, 208, 15, 127);
            ScrollBarHorizontal.TextureHoverRegion = new Rectangle(416, 208, 15, 127);
            ScrollBarHorizontal.TextureDownRegion = new Rectangle(432, 208, 15, 127);
            ScrollBarHorizontal.TextureDisabledRegion = new Rectangle(448, 208, 15, 127);

            ScrollBarVertical = new Skinnable(Skinnable.CreateRectangleSubregions(15, 127, 2), SkinTexture);
            ScrollBarVertical.TextureBaseRegion = new Rectangle(400, 208, 15, 127);
            ScrollBarVertical.TextureHoverRegion = new Rectangle(416, 208, 15, 127);
            ScrollBarVertical.TextureDownRegion = new Rectangle(432, 208, 15, 127);
            ScrollBarVertical.TextureDisabledRegion = new Rectangle(448, 208, 15, 127);

            ScrollBarHorizontalThumb = new Skinnable(Skinnable.CreateRectangleSubregions(15, 127, 2), SkinTexture);
            ScrollBarHorizontalThumb.TextureBaseRegion = new Rectangle(400, 208, 15, 127);
            ScrollBarHorizontalThumb.TextureHoverRegion = new Rectangle(416, 208, 15, 127);
            ScrollBarHorizontalThumb.TextureDownRegion = new Rectangle(432, 208, 15, 127);
            ScrollBarHorizontalThumb.TextureDisabledRegion = new Rectangle(448, 208, 15, 127);

            ScrollBarVerticalThumb = new Skinnable(Skinnable.CreateRectangleSubregions(15, 127, 2), SkinTexture);
            ScrollBarVerticalThumb.TextureBaseRegion = new Rectangle(400, 208, 15, 127);
            ScrollBarVerticalThumb.TextureHoverRegion = new Rectangle(416, 208, 15, 127);
            ScrollBarVerticalThumb.TextureDownRegion = new Rectangle(432, 208, 15, 127);
            ScrollBarVerticalThumb.TextureDisabledRegion = new Rectangle(448, 208, 15, 127);

            ScrollBarButtonUp = new Skinnable(Skinnable.CreateRectangleSubregions(15, 15, 2), SkinTexture);
            ScrollBarButtonUp.TextureBaseRegion = new Rectangle(464, 224, 15, 15);
            ScrollBarButtonUp.TextureHoverRegion = new Rectangle(480, 224, 15, 15);
            ScrollBarButtonUp.TextureDisabledRegion = new Rectangle(480, 288, 15, 15);
            ScrollBarButtonUp.TextureDownRegion = new Rectangle(464, 288, 15, 15);

            ScrollBarButtonDown = new Skinnable(Skinnable.CreateRectangleSubregions(15, 15, 2), SkinTexture);
            ScrollBarButtonDown.TextureBaseRegion = new Rectangle(464, 256, 15, 15);
            ScrollBarButtonDown.TextureHoverRegion = new Rectangle(480, 256, 15, 15);
            ScrollBarButtonDown.TextureDisabledRegion = new Rectangle(480, 320, 15, 15);
            ScrollBarButtonDown.TextureDownRegion = new Rectangle(464, 320, 15, 15);

            ScrollBarButtonLeft = new Skinnable(Skinnable.CreateRectangleSubregions(15, 15, 2), SkinTexture);
            ScrollBarButtonLeft.TextureBaseRegion = new Rectangle(464, 208, 15, 15);
            ScrollBarButtonLeft.TextureHoverRegion = new Rectangle(480, 208, 15, 15);
            ScrollBarButtonLeft.TextureDisabledRegion = new Rectangle(480, 272, 15, 15);
            ScrollBarButtonLeft.TextureDownRegion = new Rectangle(464, 272, 15, 15);

            ScrollBarButtonRight = new Skinnable(Skinnable.CreateRectangleSubregions(15, 15, 2), SkinTexture);
            ScrollBarButtonRight.TextureBaseRegion = new Rectangle(464, 240, 15, 15);
            ScrollBarButtonRight.TextureHoverRegion = new Rectangle(480, 240, 15, 15);
            ScrollBarButtonRight.TextureDisabledRegion = new Rectangle(480, 304, 15, 15);
            ScrollBarButtonRight.TextureDownRegion = new Rectangle(464, 304, 15, 15);
        }

        /*public void Draw(IRenderTarget renderTarget, Rectangle destinationRegion, Skinnable skinnable, Rectangle skinRegion) {

            double xRatio = (double)destinationRegion.Width / skinRegion.Width;
            double yRatio = (double)destinationRegion.Height / skinRegion.Height;
            
            for (int i = 0; i < skinnable.SubRegions.Length; i++) {
                renderTarget.DrawStretched(SkinTexture.Texture, new Rectangle((int)System.Math.Ceiling(destinationRegion.X + skinnable.SubRegions[i].X * xRatio), (int)System.Math.Ceiling(destinationRegion.Y + skinnable.SubRegions[i].Y * yRatio), (int)System.Math.Ceiling(skinnable.SubRegions[i].Width * xRatio), (int)System.Math.Ceiling(skinnable.SubRegions[i].Height * yRatio)),
                    new Rectangle(skinRegion.X + skinnable.SubRegions[i].X, skinRegion.Y + skinnable.SubRegions[i].Y, skinnable.SubRegions[i].Width, skinnable.SubRegions[i].Height));
            }

        }*/

        public void SetTextureDirect (ITexture texture, Rectangle destinationRegion) {
            this.SetTextureDirect(texture, destinationRegion, new Rectangle(0, 0, texture.Width, texture.Height)); // texture.Bounds
        }
        
        public void SetTextureDirect (ITexture texture, Rectangle destinationRegion, Rectangle cropRegion) {
            this.SkinTexture.DrawStretched(texture, destinationRegion, cropRegion);
            SkinTexture.Display();
        }

    }
}
