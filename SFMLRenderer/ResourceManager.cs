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
using SFML;
using System.IO;
using System.Net;

namespace crossGFX.SFMLRenderer
{
    public class ResourceManager : IResourceManager
    {
        public string ResourceDirectory { get; private set; }

        public ITexture LoadTextureDirect(string fullFilePath) {
            SFML.Graphics.Texture sfTexture = new SFML.Graphics.Texture(fullFilePath);
            sfTexture.Smooth = true;

            Texture texture = new Texture(sfTexture);

            return texture;
        }

        private void VerifyResourceDirectory() {
            if (string.IsNullOrEmpty(ResourceDirectory)) {
                throw new ArgumentNullException("ResourceDirectory", "No resource directory has been loaded.");
            }
        }

        public ITexture LoadTexture(string resourceName) {
            VerifyResourceDirectory();
            return LoadTextureDirect(Path.Combine(ResourceDirectory, resourceName));
        }

        public ITexture LoadTexture(byte[] data) {
            using (MemoryStream stream = new MemoryStream()) {
                stream.Write(data, 0, data.Length);

                SFML.Graphics.Texture sfTexture = new SFML.Graphics.Texture(stream);
                sfTexture.Smooth = true;

                Texture texture = new Texture(sfTexture);

                return texture;
            }
        }

        public IFont LoadFont(string resourceName) {
            VerifyResourceDirectory();
            return LoadFontDirect(Path.Combine(ResourceDirectory, resourceName));
        }

        public IFont LoadFont(byte[] data) {
            // Note: This memory stream needs to be kept open for the font to be usable in SFML...
            MemoryStream stream = new MemoryStream();
            stream.Write(data, 0, data.Length);

            global::SFML.Graphics.Font sfFont;

            sfFont = new global::SFML.Graphics.Font(stream);

            Font font = new Font(sfFont);

            return font;
        }

        public IRenderTexture CreateRenderTexture(int width, int height) {
            SFML.Graphics.RenderTexture sfmlRenderTexture = new SFML.Graphics.RenderTexture((uint)width, (uint)height, false);

            RenderTexture renderTexture = new RenderTexture(sfmlRenderTexture);
            return renderTexture;
        }

        public IMusic LoadMusic(string resourceName, string songId) {
            VerifyResourceDirectory();
            return LoadMusicDirect(Path.Combine(ResourceDirectory, resourceName), songId);
        }

        public IFont LoadFontDirect(string fullFilePath) {
            global::SFML.Graphics.Font sfFont;

            //try {
            sfFont = new global::SFML.Graphics.Font(fullFilePath);
            Font font = new Font(sfFont);

            return font;
        }

        public IMusic LoadMusicDirect(string fullFilePath, string songId) {
            return new Music(fullFilePath, songId);
        }

        public Stream GetResourceStream(string resourceName) {
            VerifyResourceDirectory();
            return new FileStream(Path.Combine(ResourceDirectory, resourceName), FileMode.OpenOrCreate, FileAccess.Read);
        }

        public bool ResourceExists(string resourceName) {
            VerifyResourceDirectory();
            return File.Exists(Path.Combine(ResourceDirectory, resourceName));
        }

        public IMusic LoadMusic(Stream musicStream, string songId) {
            return new Music(musicStream, songId);
        }

        public Stream CreateNetworkStream(Uri uri) {
            HttpWebResponse webResponse = WebRequest.Create(uri).GetResponse() as HttpWebResponse;
            return webResponse.GetResponseStream();
        }

        public void PrepareResourceDirectory(string resourceDirectory) {
            this.ResourceDirectory = resourceDirectory;
        }
    }
}
