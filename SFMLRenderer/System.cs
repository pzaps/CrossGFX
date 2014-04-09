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

using SFML.Window;
using SFML.Graphics;
using Tao.OpenGl;
using System.Diagnostics;

namespace crossGFX.SFMLRenderer
{
    class System : ISystem
    {
        Stopwatch stopwatch;

        public System() {
            stopwatch = new Stopwatch();
        }

        Window displayWindow;

        public IWindow DisplayWindow {
            get { return displayWindow; }
        }

        public int TargetFPS {
            set {
                displayWindow.RenderWindow.SetFramerateLimit((uint)value);
            }
        }

        public IWindow CreateWindow(int width, int height, bool fullScreen) {
            Styles style;

            if (fullScreen) {
                style = Styles.Fullscreen;
            } else {
                style = Styles.Titlebar | Styles.Close | Styles.Resize;
            }

            Window window = new Window(new RenderWindow(new VideoMode((uint)width, (uint)height), "[CrossGFX] SFML_App", style));

            displayWindow = window;

            return window;
        }

        public void PrepareSystem() {
            stopwatch.Start();
            displayWindow.RaiseOnInitialize();
            displayWindow.RenderWindow.SetActive();
        }

        public bool ShouldProcessEvents(IWindow window) {
            Window displayWindow = window as Window;

            return displayWindow.RenderWindow.IsOpen();
        }

        public void Run() {
            PrepareSystem();
            while (ShouldProcessEvents(displayWindow)) {
                RunOnce();
            }
        }

        public void RunOnce() {
            uint currentTickCount = GetTickCount();

            // Update
            displayWindow.RenderWindow.DispatchEvents();
            DriverManager.ActiveDriver.MusicPlayer.Update(currentTickCount);
            displayWindow.HandleUpdate(currentTickCount);

            // Draw
            displayWindow.RenderWindow.Clear();
            displayWindow.RaiseOnDraw(currentTickCount);
            displayWindow.RenderWindow.Display();
        }

        public void Cleanup() {
        }

        public uint GetTickCount() {
            return (uint)stopwatch.ElapsedMilliseconds;
        }
    }
}
