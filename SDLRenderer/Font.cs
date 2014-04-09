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

namespace crossGFX.SDLRenderer
{
    class Font : IFont
    {
        SdlDotNet.Graphics.Font cachedFont = null;
        int pointSize = -1;
        ResourceManager resourceManager;
        string filePath;

        public Font(ResourceManager resourceManager, string filePath) {
            this.resourceManager = resourceManager;

            this.filePath = filePath;
        }

        public string FilePath {
            get { return filePath; }
        }

        public void RenderText(string text, int characterSize, Color color, IRenderTarget destination, Point position) {
            UpdateCachedFont(characterSize);
            Texture textTexture = new Texture(cachedFont.Render(text, global::System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B)));

            destination.Draw(textTexture, position);
        }

        public void Dispose() {
            throw new NotImplementedException();
        }

        private void UpdateCachedFont(int characterSize) {
            if (cachedFont == null || pointSize != characterSize) {
                cachedFont = FontCache.LookupFont(filePath, characterSize);
            }
        }

        public Size MeasureTextSize(string text, int characterSize) {
            UpdateCachedFont(characterSize);
            global::System.Drawing.Size size = cachedFont.SizeText(text);
            return new Size(size.Width, size.Height);
        }


        public IFont Clone() {
            return resourceManager.LoadFont(filePath);
        }
    }
}
