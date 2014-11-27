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
using System.Drawing;

namespace crossGFX.SDLRenderer
{
    class InputHelper : crossGFX.Input.InputHelperBase, crossGFX.Input.IInputHelper
    {
        public event EventHandler<Input.KeyEventArgs> KeyPressed;

        public event EventHandler<Input.KeyEventArgs> KeyReleased;

        public event EventHandler<Input.TextEnteredEventArgs> TextEntered;

        Window window;

        public InputHelper(Window window) {
            this.window = window;
        }

        public void SubscribeToEvents() {
            SdlDotNet.Core.Events.KeyboardDown += new EventHandler<SdlDotNet.Input.KeyboardEventArgs>(Events_KeyboardDown);
            SdlDotNet.Core.Events.KeyboardUp += new EventHandler<SdlDotNet.Input.KeyboardEventArgs>(Events_KeyboardUp);

            SdlDotNet.Core.Events.MouseButtonDown += new EventHandler<SdlDotNet.Input.MouseButtonEventArgs>(Events_MouseButtonDown);
            SdlDotNet.Core.Events.MouseButtonUp += new EventHandler<SdlDotNet.Input.MouseButtonEventArgs>(Events_MouseButtonUp);
            SdlDotNet.Core.Events.MouseMotion += new EventHandler<SdlDotNet.Input.MouseMotionEventArgs>(Events_MouseMotion);
        }

        void Events_MouseMotion(object sender, SdlDotNet.Input.MouseMotionEventArgs e) {
            int dx = e.X - mousePosition.X;
            int dy = e.Y - mousePosition.Y;

            mousePosition.X = e.X;
            mousePosition.Y = e.Y;

            if (MouseMoved != null) {
                MouseMoved(this, new crossGFX.Input.MouseMovedEventArgs(mousePosition.X, mousePosition.Y, dx, dy));
            }
        }

        void Events_MouseButtonUp(object sender, SdlDotNet.Input.MouseButtonEventArgs e) {
            if (MouseButtonReleased != null) {
                MouseButtonReleased(this, new Input.MouseButtonEventArgs(TranslateButtonCode(e.Button), false, e.X, e.Y));
            }
        }

        void Events_MouseButtonDown(object sender, SdlDotNet.Input.MouseButtonEventArgs e) {
            if (MouseButtonPressed != null) {
                MouseButtonPressed(this, new Input.MouseButtonEventArgs(TranslateButtonCode(e.Button), true, e.X, e.Y));
            }
        }

        void Events_KeyboardUp(object sender, SdlDotNet.Input.KeyboardEventArgs e) {
            Input.Key key = TranslateKeyCode(e.Key);

            if (KeyData.KeyState[(int)key]) {
                KeyData.KeyState[(int)key] = false;

                if (KeyReleased != null) {
                    KeyReleased(this, new Input.KeyEventArgs(key, true, IsControlDownSDL(e), IsShiftDownSDL(e), IsAltDownSDL(e)));
                }
            }
        }

        void Events_KeyboardDown(object sender, SdlDotNet.Input.KeyboardEventArgs e) {
            Input.Key key = TranslateKeyCode(e.Key);

            if (!KeyData.KeyState[(int)key]) {
                window.InputHelper.KeyData.KeyState[(int)key] = true;
                KeyData.NextRepeat[(int)key] = DriverManager.ActiveDriver.System.GetTickCount() + KeyRepeatDelay;

                if (KeyPressed != null) {
                    KeyPressed(this, new Input.KeyEventArgs(key, true, IsControlDownSDL(e), IsShiftDownSDL(e), IsAltDownSDL(e)));
                }
            }

            if (TextEntered != null) {
                string charString = Keyboard.GetCharString(e, this);
                if (!string.IsNullOrEmpty(charString) && charString.Length == 1) {
                    TextEntered(this, new Input.TextEnteredEventArgs(charString));
                }
            }
        }

        bool IsControlDownSDL(SdlDotNet.Input.KeyboardEventArgs e) {
            return (e.Mod == SdlDotNet.Input.ModifierKeys.LeftControl || e.Mod == SdlDotNet.Input.ModifierKeys.RightControl);
        }

        bool IsShiftDownSDL(SdlDotNet.Input.KeyboardEventArgs e) {
            return (e.Mod == SdlDotNet.Input.ModifierKeys.LeftShift || e.Mod == SdlDotNet.Input.ModifierKeys.RightShift);
        }

        bool IsAltDownSDL(SdlDotNet.Input.KeyboardEventArgs e) {
            return (e.Mod == SdlDotNet.Input.ModifierKeys.LeftAlt || e.Mod == SdlDotNet.Input.ModifierKeys.RightAlt);
        }

        private static crossGFX.Input.MouseButton TranslateButtonCode(SdlDotNet.Input.MouseButton sdlMouseButton) {
            switch (sdlMouseButton) {
                case SdlDotNet.Input.MouseButton.PrimaryButton:
                    return Input.MouseButton.Left;
                case SdlDotNet.Input.MouseButton.SecondaryButton:
                    return Input.MouseButton.Right;
                case SdlDotNet.Input.MouseButton.MiddleButton:
                    return Input.MouseButton.Middle;
                default:
                    return Input.MouseButton.Left;
            }
        }

        private static SdlDotNet.Input.MouseButton TranslateButtonCode(crossGFX.Input.MouseButton mouseButton) {
            switch (mouseButton) {
                case Input.MouseButton.Left:
                    return SdlDotNet.Input.MouseButton.PrimaryButton;
                case Input.MouseButton.Right:
                    return SdlDotNet.Input.MouseButton.SecondaryButton;
                case Input.MouseButton.Middle:
                    return SdlDotNet.Input.MouseButton.MiddleButton;
                default:
                    return SdlDotNet.Input.MouseButton.None;
            }
        }

        private static crossGFX.Input.Key TranslateKeyCode(SdlDotNet.Input.Key sdlKey) {
            switch (sdlKey) {
                case SdlDotNet.Input.Key.A:
                    return Input.Key.A;
                case SdlDotNet.Input.Key.B:
                    return Input.Key.B;
                case SdlDotNet.Input.Key.C:
                    return Input.Key.C;
                case SdlDotNet.Input.Key.D:
                    return Input.Key.D;
                case SdlDotNet.Input.Key.E:
                    return Input.Key.E;
                case SdlDotNet.Input.Key.F:
                    return Input.Key.F;
                case SdlDotNet.Input.Key.G:
                    return Input.Key.G;
                case SdlDotNet.Input.Key.H:
                    return Input.Key.H;
                case SdlDotNet.Input.Key.I:
                    return Input.Key.I;
                case SdlDotNet.Input.Key.J:
                    return Input.Key.J;
                case SdlDotNet.Input.Key.K:
                    return Input.Key.K;
                case SdlDotNet.Input.Key.L:
                    return Input.Key.L;
                case SdlDotNet.Input.Key.M:
                    return Input.Key.M;
                case SdlDotNet.Input.Key.N:
                    return Input.Key.N;
                case SdlDotNet.Input.Key.O:
                    return Input.Key.O;
                case SdlDotNet.Input.Key.P:
                    return Input.Key.P;
                case SdlDotNet.Input.Key.Q:
                    return Input.Key.Q;
                case SdlDotNet.Input.Key.R:
                    return Input.Key.R;
                case SdlDotNet.Input.Key.S:
                    return Input.Key.S;
                case SdlDotNet.Input.Key.T:
                    return Input.Key.T;
                case SdlDotNet.Input.Key.U:
                    return Input.Key.U;
                case SdlDotNet.Input.Key.V:
                    return Input.Key.V;
                case SdlDotNet.Input.Key.W:
                    return Input.Key.W;
                case SdlDotNet.Input.Key.X:
                    return Input.Key.X;
                case SdlDotNet.Input.Key.Y:
                    return Input.Key.Y;
                case SdlDotNet.Input.Key.Z:
                    return Input.Key.Z;

                case SdlDotNet.Input.Key.Escape:
                    return Input.Key.Escape;
                case SdlDotNet.Input.Key.LeftControl:
                    return Input.Key.LControl;
                case SdlDotNet.Input.Key.LeftShift:
                    return Input.Key.LShift;
                case SdlDotNet.Input.Key.LeftAlt:
                    return Input.Key.LAlt;
                case SdlDotNet.Input.Key.LeftWindows:
                    return Input.Key.LSystem;
                case SdlDotNet.Input.Key.RightControl:
                    return Input.Key.RControl;
                case SdlDotNet.Input.Key.RightShift:
                    return Input.Key.RShift;
                case SdlDotNet.Input.Key.RightAlt:
                    return Input.Key.RAlt;
                case SdlDotNet.Input.Key.RightWindows:
                    return Input.Key.RSystem;
                case SdlDotNet.Input.Key.Menu:
                    return Input.Key.Menu;
                case SdlDotNet.Input.Key.LeftBracket:
                    return Input.Key.LBracket;
                case SdlDotNet.Input.Key.RightBracket:
                    return Input.Key.RBracket;
                case SdlDotNet.Input.Key.Semicolon:
                    return Input.Key.SemiColon;
                case SdlDotNet.Input.Key.Comma:
                    return Input.Key.Comma;
                case SdlDotNet.Input.Key.Period:
                    return Input.Key.Period;
                case SdlDotNet.Input.Key.Quote:
                    return Input.Key.Quote;
                case SdlDotNet.Input.Key.Slash:
                    return Input.Key.Slash;
                case SdlDotNet.Input.Key.Backslash:
                    return Input.Key.BackSlash;
                case SdlDotNet.Input.Key.Equals:
                    return Input.Key.Equal;
                case SdlDotNet.Input.Key.Minus:
                    return Input.Key.Dash;
                case SdlDotNet.Input.Key.Space:
                    return Input.Key.Space;
                case SdlDotNet.Input.Key.Return:
                    return Input.Key.Return;
                case SdlDotNet.Input.Key.Backspace:
                    return Input.Key.Backspace;
                case SdlDotNet.Input.Key.Tab:
                    return Input.Key.Tab;
                case SdlDotNet.Input.Key.PageUp:
                    return Input.Key.PageUp;
                case SdlDotNet.Input.Key.PageDown:
                    return Input.Key.PageDown;
                case SdlDotNet.Input.Key.End:
                    return Input.Key.End;
                case SdlDotNet.Input.Key.Home:
                    return Input.Key.Home;
                case SdlDotNet.Input.Key.Insert:
                    return Input.Key.Insert;
                case SdlDotNet.Input.Key.Delete:
                    return Input.Key.Delete;
                case SdlDotNet.Input.Key.KeypadPlus:
                    return Input.Key.Add;
                case SdlDotNet.Input.Key.KeypadMinus:
                    return Input.Key.Subtract;
                case SdlDotNet.Input.Key.KeypadMultiply:
                    return Input.Key.Multiply;
                case SdlDotNet.Input.Key.KeypadDivide:
                    return Input.Key.Divide;
                case SdlDotNet.Input.Key.LeftArrow:
                    return Input.Key.Left;
                case SdlDotNet.Input.Key.RightArrow:
                    return Input.Key.Right;
                case SdlDotNet.Input.Key.UpArrow:
                    return Input.Key.Up;
                case SdlDotNet.Input.Key.DownArrow:
                    return Input.Key.Down;

                case SdlDotNet.Input.Key.Zero:
                    return Input.Key.Num0;
                case SdlDotNet.Input.Key.One:
                    return Input.Key.Num1;
                case SdlDotNet.Input.Key.Two:
                    return Input.Key.Num2;
                case SdlDotNet.Input.Key.Three:
                    return Input.Key.Num3;
                case SdlDotNet.Input.Key.Four:
                    return Input.Key.Num4;
                case SdlDotNet.Input.Key.Five:
                    return Input.Key.Num5;
                case SdlDotNet.Input.Key.Six:
                    return Input.Key.Num6;
                case SdlDotNet.Input.Key.Seven:
                    return Input.Key.Num7;
                case SdlDotNet.Input.Key.Eight:
                    return Input.Key.Num8;
                case SdlDotNet.Input.Key.Nine:
                    return Input.Key.Num9;

                case SdlDotNet.Input.Key.Keypad0:
                    return Input.Key.Num0;
                case SdlDotNet.Input.Key.Keypad1:
                    return Input.Key.Num1;
                case SdlDotNet.Input.Key.Keypad2:
                    return Input.Key.Num2;
                case SdlDotNet.Input.Key.Keypad3:
                    return Input.Key.Num3;
                case SdlDotNet.Input.Key.Keypad4:
                    return Input.Key.Num4;
                case SdlDotNet.Input.Key.Keypad5:
                    return Input.Key.Num5;
                case SdlDotNet.Input.Key.Keypad6:
                    return Input.Key.Num6;
                case SdlDotNet.Input.Key.Keypad7:
                    return Input.Key.Num7;
                case SdlDotNet.Input.Key.Keypad8:
                    return Input.Key.Num8;
                case SdlDotNet.Input.Key.Keypad9:
                    return Input.Key.Num9;

                case SdlDotNet.Input.Key.F1:
                    return Input.Key.F1;
                case SdlDotNet.Input.Key.F2:
                    return Input.Key.F2;
                case SdlDotNet.Input.Key.F3:
                    return Input.Key.F3;
                case SdlDotNet.Input.Key.F4:
                    return Input.Key.F4;
                case SdlDotNet.Input.Key.F5:
                    return Input.Key.F5;
                case SdlDotNet.Input.Key.F6:
                    return Input.Key.F6;
                case SdlDotNet.Input.Key.F7:
                    return Input.Key.F7;
                case SdlDotNet.Input.Key.F8:
                    return Input.Key.F8;
                case SdlDotNet.Input.Key.F9:
                    return Input.Key.F9;
                case SdlDotNet.Input.Key.F10:
                    return Input.Key.F10;
                case SdlDotNet.Input.Key.F11:
                    return Input.Key.F11;
                case SdlDotNet.Input.Key.F12:
                    return Input.Key.F12;
                case SdlDotNet.Input.Key.F13:
                    return Input.Key.F13;
                case SdlDotNet.Input.Key.F14:
                    return Input.Key.F14;
                case SdlDotNet.Input.Key.F15:
                    return Input.Key.F15;
                case SdlDotNet.Input.Key.Pause:
                    return Input.Key.Pause;

                default:
                    return Input.Key.KeyCount;
            }
        }


        public event EventHandler<Input.MouseMovedEventArgs> MouseMoved;

        public event EventHandler<Input.MouseButtonEventArgs> MouseButtonPressed;

        public event EventHandler<Input.MouseButtonEventArgs> MouseButtonReleased;



        public IWindow Window {
            get { return window; }
        }

        public void Update() {
            ThinkAboutKeyRepeat();
        }

        protected void ThinkAboutKeyRepeat() {
            float time = DriverManager.ActiveDriver.System.GetTickCount();

            for (int i = 0; i < (int)Input.Key.KeyCount; i++) {
                //if (KeyData.KeyState[i] && KeyData.Target != window.GUI.KeyboardFocus) {
                //    KeyData.KeyState[i] = false;
                //    continue;
                //}

                if (KeyData.KeyState[i] && time > KeyData.NextRepeat[i]) {
                    KeyData.NextRepeat[i] = time + KeyRepeatRate;

                    //if (window.GUI.KeyboardFocus != null) {
                    //    window.GUI.KeyboardFocus.InputKeyPressed((Input.Key)i);
                    //}
                    if (KeyPressed != null) {
                        KeyPressed(this, new Input.KeyEventArgs((crossGFX.Input.Key)i, true, IsControlDown, IsShiftDown, IsAltDown));
                    }
                }
            }
        }

        public bool IsMouseButtonDown(Input.MouseButton mouseButton) {
            return SdlDotNet.Input.Mouse.IsButtonPressed(TranslateButtonCode(mouseButton));
        }


        public void PrepareKeyboard(string defaultText) {
        }
    }
}