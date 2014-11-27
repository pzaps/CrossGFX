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
using Tao.OpenGl;

namespace crossGFX.SDLRenderer
{
    class RenderTarget : IRenderTarget
    {
        Surface surface;

        public Point Origin {
            get;
            set;
        }

        public Surface Surface {
            get { return surface; }
        }

        public RenderTarget(Surface surface) {
            this.surface = surface;
        }

        public void Fill(Color color) {
            if (color != Color.Transparent) {
                surface.Fill(ConvertColor(color));
            }
        }

        public void Fill(Rectangle bounds, Color color) {
            if (color != Color.Transparent) {
                surface.Fill(ConvertRectangle(bounds), ConvertColor(color));
            }
        }

        public void Draw(ITexture texture, Point position) {
            Draw(texture, position, new Rectangle(0, 0, texture.Width, texture.Height));
        }

        public void DrawLine(int x, int y, int a, int b, Color color) {
            SdlDotNet.Graphics.Primitives.Line line = new SdlDotNet.Graphics.Primitives.Line((short)x, (short)y, (short)a, (short)b);
            surface.Draw(line, ConvertColor(color));
        }

        public void Draw(ITexture texture, Point position, Rectangle sourceRectangle) {
            Texture mainTexture = texture as Texture;
            surface.Blit(mainTexture.Surface, ConvertPoint(position), ConvertRectangle(sourceRectangle));
        }

        public void DrawStretched(ITexture texture, Rectangle destinationBounds, Rectangle sourceRectangle) {
            Texture mainTexture = texture as Texture;
            Surface targetSurface = mainTexture.Surface.CreateSurfaceFromClipRectangle(ConvertRectangle(sourceRectangle));//.Convert();
            targetSurface.Transparent = true;
            //targetSurface.TransparentColor = Color.FromArgb(0, 255, 255, 255);

            global::System.Drawing.Rectangle destRectangleConverted = ConvertRectangle(destinationBounds);

            Surface stretchedSurface = targetSurface.CreateStretchedSurface(destRectangleConverted.Size).Convert();
            ////stretchedSurface.TransparentColor = Color.FromArgb(0, 255, 255, 255);
            stretchedSurface.Transparent = true;

            surface.Blit(stretchedSurface, destRectangleConverted.Location);

            stretchedSurface.Dispose();
            targetSurface.Dispose();
        }

        // TODO: SDL Clipping
        public void StartClip(Rectangle clipRegion, int scale) {
            //SdlDotNet.Graphics.Video.Screen.ClipRectangle = clipRegion;
            Gl.glScissor((int)(clipRegion.X * scale), (int)(clipRegion.Y * scale),
                         (int)(clipRegion.Width * scale), (int)(clipRegion.Height * scale));
            Gl.glEnable(Gl.GL_SCISSOR_TEST);
        }


        public void EndClip() {
            Gl.glDisable(Gl.GL_SCISSOR_TEST);
            // SdlDotNet.Graphics.Video.Screen.ClipRectangle = Rectangle.Empty;
        }

        public void BeginDrawing() {
        }

        public void EndDrawing() {
        }


        public void BeginMoveOrigin(Point newOrigin) {
            throw new NotImplementedException();
        }

        public void EndMoveOrigin() {
            throw new NotImplementedException();
        }


        public void DrawString(Point position, IFont font, string text, int textSize, Color textColor,
            bool bold, bool italic, bool underline) {
            font.RenderText(this, position, text, textSize, textColor, bold, italic, underline);
        }

        public void DrawString(RichString richString, Point position) {
            this.DrawString(position, richString.Font, richString.Text, richString.TextSize, richString.Color,
                richString.Bold, richString.Italic, richString.Underline);
        }


        public void DrawStretched(ITexture texture, Rectangle destinationBounds) {
            DrawStretched(texture, destinationBounds, new Rectangle(0, 0, texture.Width, texture.Height));
        }

        private global::System.Drawing.Rectangle ConvertRectangle(Rectangle rectangle) {
            return new global::System.Drawing.Rectangle(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
        }

        private global::System.Drawing.Point ConvertPoint(Point point) {
            return new global::System.Drawing.Point(point.X, point.Y);
        }

        private global::System.Drawing.Color ConvertColor(Color color) {
            return global::System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
        }
    }
}
