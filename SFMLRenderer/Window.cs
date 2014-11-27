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

        public new event EventHandler<TickEventArgs> Draw;

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
