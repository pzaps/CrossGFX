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
    class ResourceManager : IResourceManager
    {
        public ITexture LoadTexture(string filePath) {

            SdlDotNet.Graphics.Surface surface = new SdlDotNet.Graphics.Surface(filePath);//.Convert(SdlDotNet.Graphics.Video.Screen);
            surface.Alpha = 255;
            surface.AlphaBlending = true;
            surface.TransparentColor = global::System.Drawing.Color.FromArgb(0, 255, 255, 255);
            surface.Transparent = true;
            //surface.Transparent = true;

            Texture texture = new Texture(surface);

            return texture;
        }

        public ITexture LoadTexture(byte[] data) {
            SdlDotNet.Graphics.Surface surface = new Surface(data).Convert();

            surface.Alpha = 255;
            surface.AlphaBlending = true;

            Texture texture = new Texture(surface);

            return texture;
        }


        public IFont LoadFont(string filePath) {
            Font font = new Font(this, filePath);
            return font;
        }


        public IRenderTexture CreateRenderTexture(int width, int height) {
            Surface surface = new Surface(width, height);

            RenderTexture renderTexture = new RenderTexture(surface);
            return renderTexture;
        }


        public IMusic LoadMusic(string filePath, string songId) {
            throw new NotImplementedException();
        }

        public string ResourceDirectory {
            get { throw new NotImplementedException(); }
        }


        public ITexture LoadTextureDirect(string fullFilePath) {
            throw new NotImplementedException();
        }


        public IFont LoadFontDirect(string fullFilePath) {
            throw new NotImplementedException();
        }


        public IMusic LoadMusicDirect(string fullFilePath, string songId) {
            throw new NotImplementedException();
        }


        public global::System.IO.Stream GetResourceStream(string resourceName) {
            throw new NotImplementedException();
        }


        public bool ResourceExists(string resourceName) {
            throw new NotImplementedException();
        }


        public IMusic LoadMusic(global::System.IO.Stream musicStream, string songId) {
            throw new NotImplementedException();
        }


        public global::System.IO.Stream CreateNetworkStream(Uri uri) {
            throw new NotImplementedException();
        }
    }
}
