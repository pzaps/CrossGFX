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

namespace crossGFX.SDLRenderer
{
    class System : ISystem
    {
        Window mainWindow = null;
        public IClipboard Clipboard { get; private set; }

        public System(IClipboard clipboard) {
            this.Clipboard = clipboard;
        }

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
