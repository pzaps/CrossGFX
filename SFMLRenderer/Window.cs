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
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;

namespace crossGFX.SFMLRenderer
{
    public class Window : RenderTarget, crossGFX.IWindow
    {
        RenderWindow renderWindow;
        InputHelper inputHelper;

        public event EventHandler<TickEventArgs> Draw;

        public event EventHandler<TickEventArgs> Update;

        public event EventHandler OnInitialize;

        public event EventHandler Resized;

        internal void RaiseOnInitialize() {
            if (OnInitialize != null) {
                OnInitialize(this, EventArgs.Empty);
            }
        }

        public crossGFX.Input.IInputHelper InputHelper {
            get { return inputHelper; }
        }

        internal void HandleUpdate(uint tickCount) {
            // Update components used by the window
            inputHelper.HandleUpdate(tickCount);

            // All for user updates
            if (Update != null) {
                Update(this, new TickEventArgs(tickCount));
            }
        }

        internal void RaiseOnDraw(uint tickCount) {
            if (Draw != null) {
                Draw(this, new TickEventArgs(tickCount));
            }
        }

        internal void RaiseOnResized() {
            if (Resized != null) {
                Resized(this, EventArgs.Empty);
            }
        }

        public RenderWindow RenderWindow {
            get {
                return renderWindow;
            }
        }

        public int Width {
            get { return (int)renderWindow.Size.X; }
        }

        public int Height {
            get { return (int)renderWindow.Size.Y; }
        }

        public Window(RenderWindow renderWindow)
            : base(renderWindow) {
            this.renderWindow = renderWindow;
            this.renderWindow.SetKeyRepeatEnabled(true);
            this.renderWindow.Closed += new EventHandler(renderWindow_Closed);
            this.renderWindow.Resized += new EventHandler<SFML.Window.SizeEventArgs>(renderWindow_Resized);

            inputHelper = new InputHelper(this);
            inputHelper.SubscribeToEvents(renderWindow);
        }

        void renderWindow_Resized(object sender, SFML.Window.SizeEventArgs e) {
            this.renderWindow.SetView(new View(new FloatRect(0f, 0f, e.Width, e.Height)));
            RaiseOnResized();
        }

        void renderWindow_Closed(object sender, EventArgs e) {
            renderWindow.Close();
        }

        public string Title {
            set {
                this.renderWindow.SetTitle(value);
            }
        }

        public Icon Icon {
            set {
                using (Bitmap bitmap = value.ToBitmap()) {
                    using (Bitmap bitmap2 = SwapRedAndBlueChannels(bitmap)) {
                        BitmapData data = bitmap2.LockBits(new global::System.Drawing.Rectangle(0, 0, value.Width, value.Height), ImageLockMode.ReadOnly, bitmap2.PixelFormat);
                        int bytes = data.Stride * data.Height;
                        byte[] rgbValues = new byte[bytes];
                        // Copy the RGB values into the array.
                        global::System.Runtime.InteropServices.Marshal.Copy(data.Scan0, rgbValues, 0, bytes);
                        //SFML.Graphics.Image icon = new SFML.Graphics.Image(ms);

                        bitmap2.UnlockBits(data);

                        renderWindow.SetIcon((uint)value.Width, (uint)value.Height, rgbValues);
                    }
                }
            }
        }

        private Bitmap SwapRedAndBlueChannels(Bitmap bitmap) {
            var imageAttr = new ImageAttributes();
            imageAttr.SetColorMatrix(new ColorMatrix(
                                         new[]
                                             {
                                                 new[] {0.0F, 0.0F, 1.0F, 0.0F, 0.0F},
                                                 new[] {0.0F, 1.0F, 0.0F, 0.0F, 0.0F},
                                                 new[] {1.0F, 0.0F, 0.0F, 0.0F, 0.0F},
                                                 new[] {0.0F, 0.0F, 0.0F, 1.0F, 0.0F},
                                                 new[] {0.0F, 0.0F, 0.0F, 0.0F, 1.0F}
                                             }
                                         ));
            var temp = new Bitmap(bitmap.Width, bitmap.Height);
            GraphicsUnit pixel = GraphicsUnit.Pixel;
            using (Graphics g = Graphics.FromImage(temp)) {
                g.DrawImage(bitmap, global::System.Drawing.Rectangle.Round(bitmap.GetBounds(ref pixel)), 0, 0, bitmap.Width, bitmap.Height,
                            GraphicsUnit.Pixel, imageAttr);
            }

            return temp;
        }

        public void Close() {
            renderWindow.Close();
        }
    }
}
