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

namespace crossGFX.SDLRenderer
{
    class System : ISystem
    {
        Window mainWindow = null;

        public IWindow CreateWindow(int width, int height, bool fullScreen) {
            Window window = new Window(SdlDotNet.Graphics.Video.SetVideoMode(width, height, 32, false, false, fullScreen));
            window.Title = "[CrossGFX] SDL_App";

            mainWindow = window;

            return window;
        }

        public IWindow DisplayWindow {
            get { return mainWindow; }
        }

        public void BeginProcessingEventsOnce(IWindow window) {
            SdlDotNet.Core.Events.RunThreadTickerOnce();
        }

        public void EndProcessingEventsOnce(IWindow window) {
            if (mainWindow != null) {
                mainWindow.Surface.Update();
            }
        }

        public bool ShouldProcessEvents(IWindow window) {
            Window sdlWindow = window as Window;
            return !sdlWindow.quitFlag;
        }

        public void PrepareSystem() {
            // Initialize the font cache
            FontCache.Initialize();
            mainWindow.RaiseOnInitialize();
        }

        public void Run() {
            while (ShouldProcessEvents(mainWindow)) {
                RunOnce();
            }
        }

        public void RunOnce() {
            uint currentTickCount = GetTickCount();

            BeginProcessingEventsOnce(mainWindow);

            DriverManager.ActiveDriver.MusicPlayer.Update(currentTickCount);
            mainWindow.RaiseOnTick(currentTickCount);
            mainWindow.RaiseOnDraw(currentTickCount);

            EndProcessingEventsOnce(mainWindow);
        }

        public void Cleanup() {
            SdlDotNet.Core.Events.QuitApplication();
        }

        public int TargetFPS {
            set { SdlDotNet.Core.Events.Fps = value; }
        }


        public uint GetTickCount() {
            return (uint)SdlDotNet.Core.Timer.TicksElapsed;
        }
    }
}
