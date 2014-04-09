using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using crossGFX;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;

namespace crossGFX.XNARenderer
{
    public class ResourceManager : IResourceManager
    {
        public ITexture LoadTexture(string filePath) {
            System system = DriverManager.ActiveDriver.System as System;
            return new Texture(system.GameWindow.Content.Load<Texture2D>(filePath));

            //using (FileStream fileStream = new FileStream(filePath, FileMode.Open)) {
            //    Texture2D loadedTexture = Texture2D.FromStream(system.GameWindow.GraphicsDevice, fileStream);

            //    Texture texture = new Texture(loadedTexture);
            //    return texture;
            //}

        }

        public ITexture LoadTexture(byte[] data) {
            System system = DriverManager.ActiveDriver.System as System;

            using (MemoryStream dataStream = new MemoryStream()) {
                dataStream.Write(data, 0, data.Length);
                Texture2D loadedTexture = Texture2D.FromStream(system.GameWindow.GraphicsDevice, dataStream);

                Texture texture = new Texture(loadedTexture);
                return texture;
            }
        }

        public IRenderTexture CreateRenderTexture(int width, int height) {
            System system = DriverManager.ActiveDriver.System as System;

            RenderTarget2D renderTarget = new RenderTarget2D(system.GameWindow.GraphicsDevice, width, height);

            return new RenderTexture(renderTarget);
        }

        public IFont LoadFont(string filePath) {
            System system = DriverManager.ActiveDriver.System as System;

            return new Font(system.GameWindow.Content.Load<SpriteFont>(filePath), filePath);
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
    }
}
