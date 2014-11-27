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
