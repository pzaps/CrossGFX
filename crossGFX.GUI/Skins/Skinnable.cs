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
