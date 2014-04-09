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
