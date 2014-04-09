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

namespace crossGFX.GUI.Skins {

    public class Skinnable {

        public Rectangle[] SubRegions{get; private set;}

        readonly IRenderTexture renderTexture;
        
        /// <summary>
        /// A class that contains default textures and regions of a skinnable control type.
        /// </summary>
        /// <remarks>
        /// subRegions.Length must be either 1 or 9.
        /// 1: The whole texture. 
        /// 9: 0 is center. 1 is topleft corner. 1-8 are regions that are situated clockwise.
        /// </remarks>
        public Skinnable(Rectangle[] subRegions, IRenderTexture renderTexture) {
            this.SubRegions = subRegions;
            this.renderTexture = renderTexture;
        }

        public ITexture TextureBase { set {
            renderTexture.DrawStretched(value, this.TextureBaseRegion);
            renderTexture.Display();
        } }

        public Rectangle TextureBaseRegion {get; set;}

        public ITexture TextureBaseActive { set { 
            renderTexture.DrawStretched(value, this.TextureBaseActiveRegion);
            renderTexture.Display();
        } }

        public Rectangle TextureBaseActiveRegion {get; set;}

        public ITexture TextureDisabled { set {
            renderTexture.DrawStretched(value, this.TextureDisabledRegion);
            renderTexture.Display();
        } }

        public Rectangle TextureDisabledRegion {get; set;}

        public ITexture TextureDisabledActive { set { 
            renderTexture.DrawStretched(value, this.TextureDisabledActiveRegion);
            renderTexture.Display();
        } }

        public Rectangle TextureDisabledActiveRegion {get; set;}

        public ITexture TextureHover { set { 
            renderTexture.DrawStretched(value, this.TextureHoverRegion);
            renderTexture.Display();
        } }

        public Rectangle TextureHoverRegion {get; set;}

        public ITexture TextureHoverActive { set { 
            renderTexture.DrawStretched(value, this.TextureHoverActiveRegion);
            renderTexture.Display();
        } }

        public Rectangle TextureHoverActiveRegion {get; set;}

        public ITexture TextureDown { set { 
            renderTexture.DrawStretched(value, this.TextureDownRegion);
            renderTexture.Display();
        } }

        public Rectangle TextureDownRegion {get; set;}

        public ITexture TextureDownActive { set { 
            renderTexture.DrawStretched(value, this.TextureDownActiveRegion);
            renderTexture.Display();
        } }

        public Rectangle TextureDownActiveRegion {get; set;}

        public static Rectangle[] CreateRectangleSubregions(int width, int height, int borderSizeTop, int borderSizeRight, int borderSizeBottom, int borderSizeLeft) {
            int middleWidth = width - borderSizeLeft - borderSizeRight;
            int middleHeight = height - borderSizeTop - borderSizeBottom;
            int rightX = width - borderSizeRight;
            int bottomY = height - borderSizeBottom;
            return new Rectangle[]{
                new Rectangle(borderSizeLeft, borderSizeTop, middleWidth, middleHeight), // center
                new Rectangle(0, 0, borderSizeLeft, borderSizeTop), // top-left corner
                new Rectangle(borderSizeLeft, 0, middleWidth, borderSizeTop), // top
                new Rectangle(rightX, 0, borderSizeTop, borderSizeRight), // top-right corner
                new Rectangle(rightX, borderSizeTop, borderSizeRight, middleHeight), // right
                new Rectangle(rightX, bottomY, borderSizeRight, borderSizeBottom), // bottom-right corner
                new Rectangle(borderSizeLeft, bottomY, middleWidth, borderSizeBottom), // bottom
                new Rectangle(0, bottomY, borderSizeLeft, borderSizeBottom), // bottom-left corner
                new Rectangle(0, borderSizeTop, borderSizeLeft, middleHeight) // left
            };
        }


        public static Rectangle[] CreateRectangleSubregions(int width, int height, int borderSize) {
            return CreateRectangleSubregions(width, height, borderSize, borderSize, borderSize, borderSize);
        }
    }
}
