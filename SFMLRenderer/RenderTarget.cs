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
using SFML.Graphics;
using SFML.Window;
using System.Drawing;
using Tao.OpenGl;

namespace crossGFX.SFMLRenderer
{
    public class RenderTarget : IRenderTarget
    {
        public enum CacheMode
        {
            Quads,
            Lines
        }

        SFML.Graphics.RenderTarget renderTarget;

        internal RenderStates m_RenderState;
        uint m_CacheSize;
        readonly Vertex[] m_VertexCache;
        CacheMode currentCacheMode = CacheMode.Quads;
        private Vector2f m_ViewScale;
        private int Scale = 1;
        bool inDrawingMode = false;

        public Point Origin {
            get { return newOrigin; }
            set { newOrigin = value; }
        }

        Point newOrigin = Point.Empty;

        public const int CacheSize = 1024;

        public RenderTarget(SFML.Graphics.RenderTarget renderTarget) {
            this.renderTarget = renderTarget;
            m_VertexCache = new Vertex[CacheSize];

            m_RenderState = RenderStates.Default;

        }

        public void BeginDrawing() {
            //base.Begin();

            using (var view = renderTarget.GetView()) {
                var port = renderTarget.GetViewport(view);
                var scaled = renderTarget.MapPixelToCoords(new Vector2i(port.Width, port.Height));
                m_ViewScale.X = (port.Width / scaled.X) * Scale;
                m_ViewScale.Y = (port.Height / scaled.Y) * Scale;
                inDrawingMode = true;
            }
        }

        public void EndDrawing() {
            FlushCache();
            inDrawingMode = false;
            //base.End();
        }


        public void Fill(Rectangle bounds, Color color) {
            bool localDraw = false;
            if (inDrawingMode == false) {
                BeginDrawing();
                localDraw = true;
            }

            HandleCacheFlushing(CacheMode.Quads, 4, null);

            if (newOrigin != Point.Empty) {
                DrawingSupport.Translate(newOrigin, ref bounds);
            }

            int right = bounds.X + bounds.Width;
            int bottom = bounds.Y + bounds.Height;
            SFML.Graphics.Color sfColor = new SFML.Graphics.Color(color.R, color.G, color.B, color.A);

            m_VertexCache[m_CacheSize++] = new Vertex(new Vector2f(bounds.X, bounds.Y), sfColor);
            m_VertexCache[m_CacheSize++] = new Vertex(new Vector2f(right, bounds.Y), sfColor);
            m_VertexCache[m_CacheSize++] = new Vertex(new Vector2f(right, bottom), sfColor);
            m_VertexCache[m_CacheSize++] = new Vertex(new Vector2f(bounds.X, bottom), sfColor);

            if (localDraw) {
                EndDrawing();
            }
        }

        public void Fill(Color color) {
            Fill(new Rectangle(0, 0, (int)this.renderTarget.Size.X, (int)renderTarget.Size.Y), color);
        }


        public void Draw(ITexture texture, Point position) {
            Draw(texture, position, new Rectangle(0, 0, texture.Width, texture.Height));
        }


        public void DrawLine(int x, int y, int a, int b, Color color) {
            bool localDraw = false;
            if (inDrawingMode == false) {
                BeginDrawing();
                localDraw = true;
            }

            HandleCacheFlushing(CacheMode.Lines, 2, null);

            if (newOrigin != Point.Empty) {
                DrawingSupport.Translate(newOrigin, ref x, ref y);
                DrawingSupport.Translate(newOrigin, ref a, ref b);
            }

            SFML.Graphics.Color sfColor = new SFML.Graphics.Color(color.R, color.G, color.B, color.A);

            m_VertexCache[m_CacheSize++] = new Vertex(new Vector2f(x, y), sfColor);
            m_VertexCache[m_CacheSize++] = new Vertex(new Vector2f(a, b), sfColor);

            //Vertex[] line = { new Vertex(new Vector2f(x, y), sfColor), new Vertex(new Vector2f(a, b), sfColor) };

            //renderTarget.Draw(line, PrimitiveType.Lines);

            if (localDraw) {
                EndDrawing();
            }
        }

        public void Draw(ITexture texture, Point position, Rectangle sourceRectangle) {
            Texture mainTexture = texture as Texture;

            bool localDraw = false;
            if (inDrawingMode == false) {
                BeginDrawing();
                localDraw = true;
            }

            HandleCacheFlushing(CacheMode.Quads, 4, mainTexture);

            if (newOrigin != Point.Empty) {
                DrawingSupport.Translate(newOrigin, ref position);
            }

            m_VertexCache[m_CacheSize++] = new Vertex(new Vector2f(position.X, position.Y), mainTexture.DrawColor, new Vector2f(sourceRectangle.X, sourceRectangle.Y));
            m_VertexCache[m_CacheSize++] = new Vertex(new Vector2f(position.X + sourceRectangle.Width, position.Y), mainTexture.DrawColor, new Vector2f(sourceRectangle.Right, sourceRectangle.Y));
            m_VertexCache[m_CacheSize++] = new Vertex(new Vector2f(position.X + sourceRectangle.Width, position.Y + sourceRectangle.Height), mainTexture.DrawColor, new Vector2f(sourceRectangle.Right, sourceRectangle.Bottom));
            m_VertexCache[m_CacheSize++] = new Vertex(new Vector2f(position.X, position.Y + sourceRectangle.Height), mainTexture.DrawColor, new Vector2f(sourceRectangle.X, sourceRectangle.Bottom));

            if (localDraw) {
                EndDrawing();
            }
        }

        public void DrawStretched(ITexture texture, Rectangle targetBounds, Rectangle sourceRectangle) {
            Texture mainTexture = texture as Texture;

            bool localDraw = false;
            if (inDrawingMode == false) {
                BeginDrawing();
                localDraw = true;
            }

            HandleCacheFlushing(CacheMode.Quads, 4, mainTexture);

            if (newOrigin != Point.Empty) {
                DrawingSupport.Translate(newOrigin, ref targetBounds);
            }

            m_VertexCache[m_CacheSize++] = new Vertex(new Vector2f(targetBounds.X, targetBounds.Y), mainTexture.DrawColor, new Vector2f(sourceRectangle.X, sourceRectangle.Y));
            m_VertexCache[m_CacheSize++] = new Vertex(new Vector2f(targetBounds.Right, targetBounds.Y), mainTexture.DrawColor, new Vector2f(sourceRectangle.Right, sourceRectangle.Y));
            m_VertexCache[m_CacheSize++] = new Vertex(new Vector2f(targetBounds.Right, targetBounds.Bottom), mainTexture.DrawColor, new Vector2f(sourceRectangle.Right, sourceRectangle.Bottom));
            m_VertexCache[m_CacheSize++] = new Vertex(new Vector2f(targetBounds.X, targetBounds.Bottom), mainTexture.DrawColor, new Vector2f(sourceRectangle.X, sourceRectangle.Bottom));

            if (localDraw) {
                EndDrawing();
            }
        }

        public void Draw(SFML.Graphics.Drawable objectToDraw) {
            renderTarget.Draw(objectToDraw);
        }

        public void StartClip(Rectangle clipRegion, int scale) {
            FlushCache();

            Rectangle rect = clipRegion;
            if (newOrigin != Point.Empty) {
                DrawingSupport.Translate(newOrigin, ref rect);
            }
            // OpenGL's coords are from the bottom left
            // so we need to translate them here.
            using (var view = renderTarget.GetView()) {
                var v = renderTarget.GetViewport(view);
                rect.Y = v.Height - (rect.Y + rect.Height);
            }

            Gl.glScissor((int)(rect.X * scale), (int)(rect.Y * scale),
                         (int)(rect.Width * scale), (int)(rect.Height * scale));
            Gl.glEnable(Gl.GL_SCISSOR_TEST);
        }

        public void EndClip() {
            FlushCache();
            Gl.glDisable(Gl.GL_SCISSOR_TEST);
        }

        internal void FlushCache() {
            if (m_CacheSize > 0) {
                if (currentCacheMode == CacheMode.Quads) {
                    renderTarget.Draw(m_VertexCache, 0, m_CacheSize, PrimitiveType.Quads, m_RenderState);
                    m_CacheSize = 0;

                } else if (currentCacheMode == CacheMode.Lines) {
                    renderTarget.Draw(m_VertexCache, 0, m_CacheSize, PrimitiveType.Lines);
                    m_CacheSize = 0;
                }
            }
        }

        void HandleCacheFlushing(CacheMode newCacheMode, int cacheDelta, Texture newTexture) {
            if (currentCacheMode != newCacheMode || m_RenderState.Texture != (newTexture == null ? null : newTexture.sfTexture) || m_CacheSize + cacheDelta >= CacheSize) {
                FlushCache();
                m_RenderState.Texture = (newTexture == null ? null : newTexture.sfTexture);
                currentCacheMode = newCacheMode;
            }
        }

        public void BeginMoveOrigin(Point newOrigin) {
            this.newOrigin = newOrigin;
        }

        public void EndMoveOrigin() {
            this.newOrigin = Point.Empty;
        }

        public void DrawString(IFont font, string text, int textSize, Color textColor, Point position) {
            if (newOrigin != Point.Empty) {
                DrawingSupport.Translate(newOrigin, ref position);
            }

            font.RenderText(text, textSize, textColor, this, position);
        }

        public void DrawStretched(ITexture texture, Rectangle destinationBounds) {
            DrawStretched(texture, destinationBounds, new Rectangle(0, 0, texture.Width, texture.Height));
        }
    }
}
