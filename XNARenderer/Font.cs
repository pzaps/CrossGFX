using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace crossGFX.XNARenderer
{
    public class Font : IFont
    {
        string filePath;
        SpriteFont font;
        int size;

        public Font(SpriteFont font, string filePath) {
            this.font = font;
            this.filePath = filePath;

            this.size = font.LineSpacing;
        }

        public int Size {
            get {
                return size;
            }
            set {
                size = value;
            }
        }

        public string FilePath {
            get { return filePath; }
        }

        public void RenderText(string text, int characterSize, global::System.Drawing.Color color, IRenderTarget destination, global::System.Drawing.Point position) {
            System sys = DriverManager.ActiveDriver.System as System;
            sys.GameWindow.SpriteBatch.DrawString(font, text, new Vector2(position.X, position.Y), new Color(color.R, color.G, color.B, color.A));
        }

        public global::System.Drawing.Size MeasureTextSize(string text, int characterSize) {
            Vector2 size = font.MeasureString(text);
            return new global::System.Drawing.Size((int)size.X, (int)size.Y);
        }

        public void Dispose() {
        }


        public IFont Clone() {
            return DriverManager.ActiveDriver.ResourceManager.LoadFont(filePath);
        }
    }
}
