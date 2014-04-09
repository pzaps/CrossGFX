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
    public class RenderTarget : IRenderTarget
    {
        GraphicsDevice graphicsDevice;
        SpriteBatch spriteBatch;
        RasterizerState _rasterizerState = new RasterizerState() { ScissorTestEnable = true };
        Rectangle originalClipRectangle;

        // Make a 1x1 texture named pixel.  
        Texture2D pixel;

        public RenderTarget(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch) {
            this.graphicsDevice = graphicsDevice;
            this.spriteBatch = spriteBatch;

            // Prepare 1x1 texture that is used for some of the drawing methods
            pixel = new Texture2D(graphicsDevice, 1, 1);
            // Create a 1D array of color data to fill the pixel texture with.  
            Color[] colorData = { Color.White };
            // Set the texture data with our color information.  
            pixel.SetData<Color>(colorData);

            originalClipRectangle = new Rectangle(0, 0, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height);
        }

        public void Fill(global::System.Drawing.Color color) {
            graphicsDevice.Clear(ConvertToXnaColor(color));
        }

        private Microsoft.Xna.Framework.Color ConvertToXnaColor(global::System.Drawing.Color color) {
            return new Microsoft.Xna.Framework.Color(color.R, color.G, color.B, color.A);
        }

        private Microsoft.Xna.Framework.Rectangle ConvertToXnaRectangle(global::System.Drawing.Rectangle rectangle) {
            return new Microsoft.Xna.Framework.Rectangle(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
        }

        public void Fill(global::System.Drawing.Rectangle bounds, global::System.Drawing.Color color) {
            // Draw a stretched pixel, shaded in with the appropriate color
            spriteBatch.Draw(pixel, ConvertToXnaRectangle(bounds), ConvertToXnaColor(color));
        }

        public void DrawLine(int x, int y, int a, int b, global::System.Drawing.Color color) {
            // Draw a stretched pixel, shaded in with the appropriate color
            spriteBatch.Draw(pixel, new Microsoft.Xna.Framework.Rectangle(x, y, (a - x) + 1, (b - y) + 1), ConvertToXnaColor(color));
        }

        public void Draw(ITexture texture, global::System.Drawing.Point position) {
            Texture mainTexture = texture as Texture;

            graphicsDevice.BlendState = BlendState.Additive;

            spriteBatch.Draw(mainTexture.Texture2D, new Rectangle(position.X, position.Y, texture.Width, texture.Height), new Color(255, 255, 255, texture.Alpha));
        }

        public void Draw(ITexture texture, global::System.Drawing.Point position, global::System.Drawing.Rectangle sourceRectangle) {
            Texture mainTexture = texture as Texture;
            spriteBatch.Draw(mainTexture.Texture2D, new Rectangle(position.X, position.Y, texture.Width, texture.Height), ConvertToXnaRectangle(sourceRectangle), new Color(255, 255, 255, texture.Alpha));
        }

        public void DrawStretched(ITexture texture, global::System.Drawing.Rectangle destinationBounds, global::System.Drawing.Rectangle sourceRectangle) {
            Texture mainTexture = texture as Texture;
            spriteBatch.Draw(mainTexture.Texture2D, ConvertToXnaRectangle(destinationBounds), ConvertToXnaRectangle(sourceRectangle), new Color(255, 255, 255, texture.Alpha));
        }

        public void StartClip(global::System.Drawing.Rectangle clipRegion, int scale) {
            //throw new NotImplementedException();
            // TODO: XNA: StartClip
            if (CreateBoundedClipRegion(clipRegion) != graphicsDevice.ScissorRectangle) {
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.BackToFront, null, null, null, _rasterizerState);
                graphicsDevice.ScissorRectangle = CreateBoundedClipRegion(clipRegion);
                //} else {
                //Console.WriteLine("hi");
            }
        }

        public void EndClip() {
            //throw new NotImplementedException();
            // TODO: XNA: EndClip
            //global::System.Diagnostics.Debug.WriteLine("Width: " + graphicsDevice.Viewport.Width + ", Height: " + graphicsDevice.Viewport.Height);
            //originalClipRectangle = new Rectangle(0, 0, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height);
            ////if (originalClipRectangle != graphicsDevice.ScissorRectangle) {
            //spriteBatch.End();
            //spriteBatch.Begin();
            //graphicsDevice.ScissorRectangle = originalClipRectangle;
            //}
        }

        private Microsoft.Xna.Framework.Rectangle CreateBoundedClipRegion(global::System.Drawing.Rectangle clipRegion) {
            Rectangle rect = ConvertToXnaRectangle(clipRegion);

            if (rect.Right > graphicsDevice.Viewport.Width) {
                rect.Width = graphicsDevice.Viewport.Width - rect.X;
            }
            if (rect.Bottom > graphicsDevice.Viewport.Height) {
                rect.Height = graphicsDevice.Viewport.Height - rect.Y;
            }

            return rect;
        }

        public void BeginDrawing() {
            //spriteBatch.Begin();
            //spriteBatch.End();
            //spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, _rasterizerState);
        }

        public void EndDrawing() {
            //spriteBatch.End();
            //spriteBatch.End();
            //spriteBatch.Begin();
        }


        public void BeginMoveOrigin(global::System.Drawing.Point newOrigin) {
            throw new NotImplementedException();
        }

        public void EndMoveOrigin() {
            throw new NotImplementedException();
        }


        public void DrawString(IFont font, string text, int textSize, global::System.Drawing.Color textColor, global::System.Drawing.Point position) {
            font.RenderText(text, textSize, textColor, this, position);
        }

        public void DrawStretched(ITexture texture, global::System.Drawing.Rectangle destinationBounds) {
            DrawStretched(texture, destinationBounds, new global::System.Drawing.Rectangle(0, 0, texture.Width, texture.Height));
        }
    }
}
