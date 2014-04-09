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
    public class Texture : ITexture
    {
        Texture2D texture2D;
        byte alpha = 255;

        public Texture2D Texture2D {
            get { return texture2D; }
        }

        public Texture(Texture2D texture2D) {
            this.texture2D = texture2D;
        }

        public byte Alpha {
            get {
                return alpha;
            }
            set {
                alpha = value;
            }
        }

        public int Width {
            get { return texture2D.Width; }
        }

        public int Height {
            get { return texture2D.Height; }
        }

        public global::System.Drawing.Color GetPixelColor(int x, int y) {
            Color[] pixels = new Color[texture2D.Width * texture2D.Height];
            texture2D.GetData<Color>(pixels);
            Color color = pixels[(y * texture2D.Width) + x];
#if WINDOWS
            return global::System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
#elif WINDOWS_PHONE
            return new global::System.Drawing.Color(color.A, color.R, color.G, color.B);
#endif
        }

        public global::System.Drawing.Color GetPixelColor(int x, int y, global::System.Drawing.Color defaultColor) {
            return GetPixelColor(x, y);
        }

        public void Dispose() {
            texture2D.Dispose();
        }
    }
}
