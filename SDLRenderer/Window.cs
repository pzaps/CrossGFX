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

namespace crossGFX.SDLRenderer
{
    class Window : RenderTarget, IWindow
    {
        InputHelper inputHelper;
        internal bool quitFlag;

        public event EventHandler<TickEventArgs> Draw;

        internal void RaiseOnDraw(uint tickCount) {
            if (Draw != null) {
                Draw(this, new TickEventArgs(tickCount));
            }
        }

        public event EventHandler OnInitialize;

        internal void RaiseOnInitialize() {
            if (OnInitialize != null) {
                OnInitialize(this, EventArgs.Empty);
            }
        }

        public event EventHandler<TickEventArgs> Update;

        internal void RaiseOnTick(uint tickCount) {
            inputHelper.Update();

            if (Update != null) {
                Update(this, new TickEventArgs(tickCount));
            }
        }

        internal void RaiseOnResized() {
            if (Resized != null) {
                Resized(this, EventArgs.Empty);
            }
        }

        public string Title {
            set { SdlDotNet.Graphics.Video.WindowCaption = value; }
        }

        public Window(Surface surface)
            : base(surface) {

            inputHelper = new InputHelper(this);
            inputHelper.SubscribeToEvents();

            SdlDotNet.Core.Events.Quit += new EventHandler<SdlDotNet.Core.QuitEventArgs>(Events_Quit);
        }

        void Events_Quit(object sender, SdlDotNet.Core.QuitEventArgs e) {
            SdlDotNet.Core.Events.QuitApplication();
        }

        public Input.IInputHelper InputHelper {
            get { return inputHelper; }
        }


        public int Width {
            get { return SdlDotNet.Graphics.Video.Screen.Width; }
        }

        public int Height {
            get { return SdlDotNet.Graphics.Video.Screen.Height; }
        }

        public void Close() {
            quitFlag = true;
        }

        public global::System.Drawing.Icon Icon {
            set {
                SdlDotNet.Graphics.Video.WindowIcon(value);
            }
        }

        public event EventHandler Resized;
    }
}
