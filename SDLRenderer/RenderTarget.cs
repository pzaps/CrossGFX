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


        public void DrawString(IFont font, string text, int textSize, Color textColor, Point position) {
            font.RenderText(text, textSize, textColor, this, position);
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
