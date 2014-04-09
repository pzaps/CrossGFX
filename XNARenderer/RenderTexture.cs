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
    public class RenderTexture : RenderTarget, IRenderTexture
    {
        RenderTarget2D renderTarget2D;
        Texture texture;

        public RenderTexture(RenderTarget2D renderTarget2D)
            : base(renderTarget2D.GraphicsDevice, new SpriteBatch(renderTarget2D.GraphicsDevice)) {

                this.renderTarget2D = renderTarget2D;

                this.texture = new Texture(renderTarget2D);
        }

        public ITexture Texture {
            get { return texture; }
        }

        public void Display() {
        }

        public void SetView(global::System.Drawing.Rectangle viewRectangle) {
        }
    }
}
