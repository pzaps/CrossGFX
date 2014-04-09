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

namespace crossGFX.GUI.Skins
{
    public class Skin
    {
        public IRenderTexture SkinTexture {get; private set;}

        public IFont Font { get; set; }
        public Skinnable TextField { get; private set; }
        public Skinnable Button { get; private set; }
        public Skinnable CheckBox { get; private set; }

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
