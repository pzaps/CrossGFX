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
using SFML.Graphics;
using SFML.Window;

namespace crossGFX.SFMLRenderer
{
    class Font : IFont
    {
        SFML.Graphics.Font font;
        string filePath;
        ResourceManager resourceManager;

        public string FilePath {
            get { return filePath; }
            set { filePath = value; }
        }

        public SFML.Graphics.Font BaseFont {
            get { return font; }
        }

        public Font(ResourceManager resourceManager, string filePath, SFML.Graphics.Font font) {
            this.resourceManager = resourceManager;

            this.filePath = filePath;
            this.font = font;
        }

        public void RenderText(string text, int characterSize, Color color, IRenderTarget destination, Point position) {
            // If the font doesn't exist, or the font size should be changed
            //if (sfFont == null || Math.Abs(font.RealSize - font.Size * Scale) > 2) {
            //    FreeFont(font);
            //    LoadFont(font);
            //}

            //if (sfFont == null)
            //    sfFont = global::SFML.Graphics.Font.DefaultFont;

            // todo: this is workaround for SFML.Net bug under mono
            if (Environment.OSVersion.Platform != PlatformID.Win32NT) {
                if (text[text.Length - 1] != '\0')
                    text += '\0';
            }

            Text sfText = new Text(text, font);
            sfText.Position = new Vector2f(position.X, position.Y);
            sfText.CharacterSize = (uint)characterSize;//(uint)font.RealSize; // [omeg] round?
            sfText.Color = new SFML.Graphics.Color(color.R, color.G, color.B, color.A);

            RenderTarget targetTexture = destination as RenderTarget;
            targetTexture.FlushCache();
            targetTexture.m_RenderState.Texture = null;
            targetTexture.Draw(sfText);

            sfText.Dispose();
        }

        public void Dispose() {
            throw new NotImplementedException();
        }


        public Size MeasureTextSize(string text, int characterSize) {
            // If the font doesn't exist, or the font size should be changed
            //if (Math.Abs(font.RealSize - font.Size * Scale) > 2) {
            //    FreeFont(font);
            //    LoadFont(font);
            //}

            //// todo: this is workaround for SFML.Net bug under mono
            //if (Environment.OSVersion.Platform != PlatformID.Win32NT) {
            //    if (text[text.Length - 1] != '\0')
            //        text += '\0';
            //}

            //Text sfText = new Text(text);
            //sfText.Font = font;
            //sfText.CharacterSize = (uint)pointSize; // [omeg] round?

            //FloatRect fr = sfText.GetRect();
            //sfText.Dispose();
            //return new Size((int)Math.Round(fr.Width), (int)Math.Round(fr.Height));

            // todo: this is workaround for SFML.Net bug under mono
            if (Environment.OSVersion.Platform != PlatformID.Win32NT) {
                if (text[text.Length - 1] != '\0')
                    text += '\0';
            }

            Size extents = new Size(0, font.GetLineSpacing((uint)characterSize));
            char prev = '\0';

            for (int i = 0; i < text.Length; i++) {
                char cur = text[i];
                font.GetKerning(prev, cur, (uint)characterSize);
                prev = cur;
                if (cur == '\n' || cur == '\v')
                    continue;
                extents.Width += font.GetGlyph(cur, (uint)characterSize, false).Advance;
            }

            return extents;

        }

       public IFont Clone() {
           return resourceManager.LoadFont(filePath);
        }
    }
}
