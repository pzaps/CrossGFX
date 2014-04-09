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
using crossGFX.Input;
using System.Drawing;

namespace crossGFX.SFMLRenderer
{
    public class InputHelper : crossGFX.Input.InputHelperBase, crossGFX.Input.IInputHelper
    {
        Window window;

        public event EventHandler<TextEnteredEventArgs> TextEntered;

        public IWindow Window {
            get { return window; }
        }

        public InputHelper(Window window) {
            this.window = window;
        }

        public void SubscribeToEvents(RenderWindow renderWindow) {
            renderWindow.KeyPressed += new EventHandler<SFML.Window.KeyEventArgs>(renderWindow_KeyPressed);
            renderWindow.KeyReleased += new EventHandler<SFML.Window.KeyEventArgs>(renderWindow_KeyReleased);

            renderWindow.MouseMoved += new EventHandler<MouseMoveEventArgs>(renderWindow_MouseMoved);
            renderWindow.MouseButtonPressed += new EventHandler<SFML.Window.MouseButtonEventArgs>(renderWindow_MouseButtonPressed);
            renderWindow.MouseButtonReleased += new EventHandler<SFML.Window.MouseButtonEventArgs>(renderWindow_MouseButtonReleased);

            //renderWindow.TextEntered += new EventHandler<TextEventArgs>(renderWindow_TextEntered);
        }

        void renderWindow_TextEntered(object sender, TextEventArgs e) {
            if (TextEntered != null) {
                Console.WriteLine(e.Unicode);
                TextEntered(this, new TextEnteredEventArgs(e.Unicode));
            }
        }

        void renderWindow_MouseButtonReleased(object sender, SFML.Window.MouseButtonEventArgs e) {
            if (MouseButtonReleased != null) {
                MouseButtonReleased(this, new Input.MouseButtonEventArgs(TranslateButtonCode(e.Button), false, e.X, e.Y));
            }
        }

        void renderWindow_MouseButtonPressed(object sender, SFML.Window.MouseButtonEventArgs e) {
            if (MouseButtonPressed != null) {
                MouseButtonPressed(this, new Input.MouseButtonEventArgs(TranslateButtonCode(e.Button), true, e.X, e.Y));
            }
        }

        void renderWindow_MouseMoved(object sender, MouseMoveEventArgs e) {
            int lx = mousePosition.X;
            int ly = mousePosition.Y;

            mousePosition.X = e.X;
            mousePosition.Y = e.Y;

            if (MouseMoved != null) {
                MouseMoved(this, new crossGFX.Input.MouseMovedEventArgs(e.X, e.Y, lx, ly));
            }
        }

        void renderWindow_KeyPressed(object sender, SFML.Window.KeyEventArgs e) {
            Input.Key key = TranslateKeyCode(e.Code);

            if (!KeyData.KeyState[(int)key]) {
                window.InputHelper.KeyData.KeyState[(int)key] = true;
                KeyData.NextRepeat[(int)key] = DriverManager.ActiveDriver.System.GetTickCount() + KeyRepeatDelay;

                if (KeyPressed != null) {
                    KeyPressed(this, new Input.KeyEventArgs(key, true, e.Control, e.Shift, e.Alt));
                }
            }
        }

        void renderWindow_KeyReleased(object sender, SFML.Window.KeyEventArgs e) {
            Input.Key key = TranslateKeyCode(e.Code);
            int keyint = (int) key;
            if (keyint != -1 && KeyData.KeyState[keyint]) {
                KeyData.KeyState[keyint] = false;

                if (KeyReleased != null) {
                    KeyReleased(this, new Input.KeyEventArgs(key, false, e.Control, e.Shift, e.Alt));
                }
            }
        }

        //bool ProcessKeyEvent(SFML.Window.KeyEventArgs e, bool keyDown) {
        //    if (e.Control && e.Alt && e.Code == Keyboard.Key.LControl)
        //        return false; // sfml bug: this is right alt

        //    char ch = TranslateChar(e.Code);
        //    if (keyDown && InputHandler.DoSpecialKeys(m_Canvas, ch))
        //        return false;

        //    Key key = TranslateKeyCode(e.Code);
        //    if (key == Key.Invalid && !keyDown) // it's not special char and it's been released
        //        return InputHandler.HandleAccelerator(m_Canvas, ch);
        //    //return m_Canvas.Input_Character(ch);)

        //    return m_Canvas.Input_Key(key, ev.Down);
        //}

       

        /// <summary>
        /// Translates SFML key code to crossGFX key code. Since crossGFX key codes are based on SFML, the conversion is really easy.
        /// </summary>
        /// <param name="sfmlKey"></param>
        /// <returns></returns>
        private static crossGFX.Input.Key TranslateKeyCode(Keyboard.Key sfmlKey) {
            return (crossGFX.Input.Key)sfmlKey;
        }

        /// <summary>
        /// Translates SFML mouse button code to crossGFX mouse button code. Since crossGFX mouse button codes are based on SFML, the conversion is really easy.
        /// </summary>
        /// <param name="sfmlKey"></param>
        /// <returns></returns>
        private static crossGFX.Input.MouseButton TranslateButtonCode(Mouse.Button sfmlMouseButton) {
            return (crossGFX.Input.MouseButton)sfmlMouseButton;
        }

        private static Mouse.Button TranslateButtonCode(crossGFX.Input.MouseButton mouseButton) {
            return (Mouse.Button)mouseButton;
        }

        ///// <summary>
        ///// Main entrypoint for processing input events. Call from your RenderWindow's event handlers.
        ///// </summary>
        ///// <param name="args">SFML input event args: can be MouseMoveEventArgs, SFMLMouseButtonEventArgs, MouseWheelEventArgs, TextEventArgs, SFMLKeyEventArgs.</param>
        ///// <returns>True if the event was handled.</returns>
        //public bool ProcessMessage(EventArgs args) {

        //    if (args is SFMLMouseButtonEventArgs) {
        //        SFMLMouseButtonEventArgs ev = args as SFMLMouseButtonEventArgs;
        //        return m_Canvas.Input_MouseButton((int)ev.Args.Button, ev.Down);
        //    }

        //    if (args is MouseWheelEventArgs) {
        //        MouseWheelEventArgs ev = args as MouseWheelEventArgs;
        //        return m_Canvas.Input_MouseWheel(ev.Delta * 60);
        //    }

        //    if (args is TextEventArgs) {
        //        TextEventArgs ev = args as TextEventArgs;
        //        // [omeg] following may not fit in 1 char in theory
        //        return m_Canvas.Input_Character(ev.Unicode[0]);
        //    }

        //    if (args is SFMLKeyEventArgs) {
        //        SFMLKeyEventArgs ev = args as SFMLKeyEventArgs;


        //    }

        //    throw new ArgumentException("Invalid event args", "args");
        //    return false;
        //}

        public event EventHandler<crossGFX.Input.KeyEventArgs> KeyPressed;

        public event EventHandler<crossGFX.Input.KeyEventArgs> KeyReleased;

        public event EventHandler<crossGFX.Input.MouseMovedEventArgs> MouseMoved;


        public event EventHandler<Input.MouseButtonEventArgs> MouseButtonPressed;

        public event EventHandler<Input.MouseButtonEventArgs> MouseButtonReleased;

        public void HandleUpdate(uint tickCount) {
            HandleKeyRepeat(tickCount);
        }

        protected void HandleKeyRepeat(uint tickCount) {
            for (int i = 0; i < (int)Input.Key.KeyCount; i++) {
                //if (KeyData.KeyState[i] && KeyData.Target != window.GUI.KeyboardFocus) {
                //    KeyData.KeyState[i] = false;
                //    continue;
                //}

                if (KeyData.KeyState[i] && tickCount > KeyData.NextRepeat[i]) {
                    KeyData.NextRepeat[i] = tickCount + KeyRepeatRate;

                    //if (window.GUI.KeyboardFocus != null) {
                    //    window.GUI.KeyboardFocus.InputKeyPressed((Input.Key)i);
                    //}
                    if (KeyPressed != null) {
                        KeyPressed(this, new Input.KeyEventArgs((Key)i, true, IsControlDown, IsShiftDown, IsAltDown));
                    }
                }
            }
        }

        public bool IsMouseButtonDown(MouseButton mouseButton) {
            return SFML.Window.Mouse.IsButtonPressed(TranslateButtonCode(mouseButton));
        }


        public void PrepareKeyboard(string defaultText) {
        }
    }
}
