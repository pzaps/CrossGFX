using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace crossGFX.XNARenderer
{
    public class System : ISystem
    {
        Window gameWindow;

        public Window GameWindow {
            get { return gameWindow; }
            set { gameWindow = value; }
        }

        public IWindow CreateWindow(int width, int height, bool fullScreen) {
#if WINDOWS
            Window window = new Window(width, height, fullScreen);
#elif WINDOWS_PHONE
            Window window = new Window();
#endif
            window.Title = "[CrossGFX] XNA_App";

            this.gameWindow = window;

            return window;
        }

        public void BeginProcessingEventsOnce(IWindow window) {
            Window displayWindow = window as Window;

            displayWindow.SpriteBatch.Begin();
        }

        public void EndProcessingEventsOnce(IWindow window) {
            Window displayWindow = window as Window;

            displayWindow.SpriteBatch.End();
        }

        public bool ShouldProcessEvents(IWindow window) {
            return false;
        }

        public void Run() {
            this.gameWindow.Run();
        }

        public void RunOnce() {
            throw new NotImplementedException();
        }

        public void Cleanup() {
        }

        public int TargetFPS {
            set {
                // TODO: XNA: SetTargetFPS
            }
        }

        public uint GetTickCount() {
            return 0;
            // TODO: XNA: GetTickCount()
        }

    }
}
