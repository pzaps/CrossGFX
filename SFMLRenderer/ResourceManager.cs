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
using SFML;
using System.IO;
using System.Net;

namespace crossGFX.SFMLRenderer
{
    public class ResourceManager : IResourceManager
    {
        public ResourceManager(string resourceDirectory) {
            this.ResourceDirectory = resourceDirectory;
        }

        public ITexture LoadTextureDirect(string fullFilePath) {
            SFML.Graphics.Texture sfTexture = new SFML.Graphics.Texture(fullFilePath);
            sfTexture.Smooth = true;

            Texture texture = new Texture(sfTexture);

            return texture;
        }

        public ITexture LoadTexture(string filePath) {
            return LoadTextureDirect(Path.Combine(ResourceDirectory, filePath));
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

        public IFont LoadFont(string filePath) {
            return LoadFontDirect(Path.Combine(ResourceDirectory, filePath));
        }

        public IRenderTexture CreateRenderTexture(int width, int height) {
            SFML.Graphics.RenderTexture sfmlRenderTexture = new SFML.Graphics.RenderTexture((uint)width, (uint)height, false);

            RenderTexture renderTexture = new RenderTexture(sfmlRenderTexture);
            return renderTexture;
        }


        public IMusic LoadMusic(string filePath, string songId) {
            return LoadMusicDirect(Path.Combine(ResourceDirectory, filePath), songId);
        }


        public string ResourceDirectory {
            get;
            private set;
        }


        public IFont LoadFontDirect(string fullFilePath) {
            global::SFML.Graphics.Font sfFont;

            //try {
            sfFont = new global::SFML.Graphics.Font(fullFilePath);
            Font font = new Font(this, fullFilePath, sfFont);

            return font;
        }


        public IMusic LoadMusicDirect(string fullFilePath, string songId) {
            return new Music(fullFilePath, songId);
        }

        public Stream GetResourceStream(string resourceName) {
            return new FileStream(Path.Combine(ResourceDirectory, resourceName), FileMode.OpenOrCreate, FileAccess.ReadWrite);
        }

        public bool ResourceExists(string resourceName) {
            return File.Exists(Path.Combine(ResourceDirectory, resourceName));
        }


        public IMusic LoadMusic(Stream musicStream, string songId) {
            return new Music(musicStream, songId);
        }

        public Stream CreateNetworkStream(Uri uri) {
            HttpWebResponse webResponse = WebRequest.Create(uri).GetResponse() as HttpWebResponse;
            return webResponse.GetResponseStream();
        }
    }
}
