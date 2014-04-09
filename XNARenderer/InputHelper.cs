using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using crossGFX.Input;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace crossGFX.XNARenderer
{
    class InputHelper : crossGFX.Input.InputHelperBase, crossGFX.Input.IInputHelper
    {
        Window window;
        KeyboardState currentKeyboardState;
        KeyboardState oldKeyboardState;
        MouseState currentMouseState;
        MouseState oldMouseState;

        public event EventHandler<TextEnteredEventArgs> TextEntered;

        public IWindow Window {
            get { return window; }
        }

        public InputHelper(Window window) {
            this.window = window;
        }

        public void PrepareKeyboard(string defaultText) {
#if WINDOWS_PHONE
            if (!Guide.IsVisible) {
                // display the guide

                Guide.BeginShowKeyboardInput(PlayerIndex.One,
                    "",       // title for the page
                    "Enter your text",  // question for user
                    defaultText,             // default text
                    new AsyncCallback(GetKeyboardInputString),  // callback method 
                    this);                       // object reference
            }
#endif
        }

        private void GetKeyboardInputString(IAsyncResult result) {
            if (result.IsCompleted) {
                string text = Guide.EndShowKeyboardInput(result);
                if (text == null) {
                    // user pressed cancel
                } else {
                    if (TextEntered != null) {
                        foreach (char character in text) {
                            TextEntered(this, new TextEnteredEventArgs(character.ToString()));
                        }
                    }
                }
            }
        }

        public void UpdateInput() {
            currentKeyboardState = Keyboard.GetState();
            currentMouseState = Mouse.GetState();

            // Do keyboard processing
            foreach (Keys key in currentKeyboardState.GetPressedKeys()) {
                if (oldKeyboardState.IsKeyDown(key)) {
                    // This key has been pressed!
                    keyPressed(key);
                }
            }
            foreach (Keys key in oldKeyboardState.GetPressedKeys()) {
                if (currentKeyboardState.IsKeyUp(key)) {
                    // This key has been released!
                    keyReleased(key);
                }
            }
            // Do mouse processing
            int oldMouseX = mousePosition.X;
            int oldMouseY = mousePosition.Y;
            int newMouseX = -1;
            int newMouseY = -1;
#if WINDOWS_PHONE
            if (currentMouseState.LeftButton == ButtonState.Pressed) {
                newMouseX = currentMouseState.X;
                newMouseY = currentMouseState.Y;
            }
#else
            newMouseX = currentMouseState.X;
            newMouseY = currentMouseState.Y;
#endif
            if ((newMouseX != -1 && newMouseY != -1) && (newMouseX != mousePosition.X || newMouseY != mousePosition.Y)) {
                // The mouse positions are different, they moved!
                mouseMoved(newMouseX, newMouseY);
            }

            if (currentMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released) {
                mouseButtonPressed(MouseButton.Left, currentMouseState.X, currentMouseState.Y);
            }
            if (currentMouseState.LeftButton == ButtonState.Released && oldMouseState.LeftButton == ButtonState.Pressed) {
                mouseButtonReleased(MouseButton.Left, oldMouseX, oldMouseY);
#if WINDOWS_PHONE
                // The mouse cursor position is only updated when the button is held down, so pretend it went off-screen
                mouseMoved(0, 0);
#endif
            }

            oldKeyboardState = currentKeyboardState;
            oldMouseState = currentMouseState;
        }

        //void renderWindow_TextEntered(object sender, TextEventArgs e) {
        //    if (TextEntered != null) {
        //        Console.WriteLine(e.Unicode);
        //        TextEntered(this, new TextEnteredEventArgs(e.Unicode));
        //    }
        //}

        void mouseButtonReleased(crossGFX.Input.MouseButton mouseButton, int x, int y) {
            if (MouseButtonReleased != null) {
                MouseButtonReleased(this, new Input.MouseButtonEventArgs(mouseButton, false, x, y));
            }
        }

        void mouseButtonPressed(crossGFX.Input.MouseButton mouseButton, int x, int y) {
            if (MouseButtonPressed != null) {
                MouseButtonPressed(this, new Input.MouseButtonEventArgs(mouseButton, true, x, y));
            }
        }

        void mouseMoved(int x, int y) {
            int dx = x - mousePosition.X;
            int dy = y - mousePosition.Y;

            mousePosition.X = x;
            mousePosition.Y = y;

            if (MouseMoved != null) {
                MouseMoved(this, new crossGFX.Input.MouseMovedEventArgs(mousePosition.X, mousePosition.Y, dx, dy));
            }
        }

        void keyPressed(Keys xnaKey) {
            Input.Key key = TranslateKeyCode(xnaKey);

            if (!KeyData.KeyState[(int)key]) {
                window.InputHelper.KeyData.KeyState[(int)key] = true;
                KeyData.NextRepeat[(int)key] = DriverManager.ActiveDriver.System.GetTickCount() + KeyRepeatDelay;

                if (KeyPressed != null) {
                    KeyPressed(this, new Input.KeyEventArgs(key, true, IsControlDown, IsShiftDown, IsAltDown));
                }
            }
        }

        void keyReleased(Keys xnaKey) {
            Input.Key key = TranslateKeyCode(xnaKey);

            if (KeyData.KeyState[(int)key]) {
                KeyData.KeyState[(int)key] = false;

                if (KeyReleased != null) {
                    KeyReleased(this, new Input.KeyEventArgs(key, false, IsControlDown, IsShiftDown, IsAltDown));
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

        private static crossGFX.Input.Key TranslateKeyCode(Microsoft.Xna.Framework.Input.Keys xnaKey) {
            switch (xnaKey) {
                case Keys.A:
                    return Input.Key.A;
                case Keys.B:
                    return Input.Key.B;
                case Keys.C:
                    return Input.Key.C;
                case Keys.D:
                    return Input.Key.D;
                case Keys.E:
                    return Input.Key.E;
                case Keys.F:
                    return Input.Key.F;
                case Keys.G:
                    return Input.Key.G;
                case Keys.H:
                    return Input.Key.H;
                case Keys.I:
                    return Input.Key.I;
                case Keys.J:
                    return Input.Key.J;
                case Keys.K:
                    return Input.Key.K;
                case Keys.L:
                    return Input.Key.L;
                case Keys.M:
                    return Input.Key.M;
                case Keys.N:
                    return Input.Key.N;
                case Keys.O:
                    return Input.Key.O;
                case Keys.P:
                    return Input.Key.P;
                case Keys.Q:
                    return Input.Key.Q;
                case Keys.R:
                    return Input.Key.R;
                case Keys.S:
                    return Input.Key.S;
                case Keys.T:
                    return Input.Key.T;
                case Keys.U:
                    return Input.Key.U;
                case Keys.V:
                    return Input.Key.V;
                case Keys.W:
                    return Input.Key.W;
                case Keys.X:
                    return Input.Key.X;
                case Keys.Y:
                    return Input.Key.Y;
                case Keys.Z:
                    return Input.Key.Z;

                case Keys.Escape:
                    return Input.Key.Escape;
                case Keys.LeftControl:
                    return Input.Key.LControl;
                case Keys.LeftShift:
                    return Input.Key.LShift;
                case Keys.LeftAlt:
                    return Input.Key.LAlt;
                case Keys.LeftWindows:
                    return Input.Key.LSystem;
                case Keys.RightControl:
                    return Input.Key.RControl;
                case Keys.RightShift:
                    return Input.Key.RShift;
                case Keys.RightAlt:
                    return Input.Key.RAlt;
                case Keys.RightWindows:
                    return Input.Key.RSystem;
                //case Keys.Menu:
                //    return Input.Key.Menu;
                //case Keys.LeftBracket:
                //    return Input.Key.LBracket;
                //case Keys.RightBracket:
                //    return Input.Key.RBracket;
                //case Keys.Semicolon:
                //    return Input.Key.SemiColon;
                //case Keys.Comma:
                //    return Input.Key.Comma;
                //case Keys.Period:
                //    return Input.Key.Period;
                //case Keys.Quote:
                //    return Input.Key.Quote;
                //case Keys.Slash:
                //    return Input.Key.Slash;
                //case Keys.Backslash:
                //    return Input.Key.BackSlash;
                //case Keys.Equals:
                //    return Input.Key.Equal;
                //case Keys.Minus:
                //    return Input.Key.Dash;
                case Keys.Space:
                    return Input.Key.Space;
                case Keys.Enter:
                    return Input.Key.Return;
                case Keys.Back:
                    return Input.Key.Backspace;
                case Keys.Tab:
                    return Input.Key.Tab;
                case Keys.PageUp:
                    return Input.Key.PageUp;
                case Keys.PageDown:
                    return Input.Key.PageDown;
                case Keys.End:
                    return Input.Key.End;
                case Keys.Home:
                    return Input.Key.Home;
                case Keys.Insert:
                    return Input.Key.Insert;
                case Keys.Delete:
                    return Input.Key.Delete;
                //case Keys.KeypadPlus:
                //    return Input.Key.Add;
                //case Keys.KeypadMinus:
                //    return Input.Key.Subtract;
                //case Keys.KeypadMultiply:
                //    return Input.Key.Multiply;
                //case Keys.KeypadDivide:
                //    return Input.Key.Divide;
                //case Keys.LeftArrow:
                //    return Input.Key.Left;
                //case Keys.RightArrow:
                //    return Input.Key.Right;
                //case Keys.UpArrow:
                //    return Input.Key.Up;
                //case Keys.DownArrow:
                //    return Input.Key.Down;

                //case Keys.Zero:
                //    return Input.Key.Num0;
                //case Keys.One:
                //    return Input.Key.Num1;
                //case Keys.Two:
                //    return Input.Key.Num2;
                //case Keys.Three:
                //    return Input.Key.Num3;
                //case Keys.Four:
                //    return Input.Key.Num4;
                //case Keys.Five:
                //    return Input.Key.Num5;
                //case Keys.Six:
                //    return Input.Key.Num6;
                //case Keys.Seven:
                //    return Input.Key.Num7;
                //case Keys.Eight:
                //    return Input.Key.Num8;
                //case Keys.Nine:
                //    return Input.Key.Num9;

                //case Keys.Keypad0:
                //    return Input.Key.Num0;
                //case Keys.Keypad1:
                //    return Input.Key.Num1;
                //case Keys.Keypad2:
                //    return Input.Key.Num2;
                //case Keys.Keypad3:
                //    return Input.Key.Num3;
                //case Keys.Keypad4:
                //    return Input.Key.Num4;
                //case Keys.Keypad5:
                //    return Input.Key.Num5;
                //case Keys.Keypad6:
                //    return Input.Key.Num6;
                //case Keys.Keypad7:
                //    return Input.Key.Num7;
                //case Keys.Keypad8:
                //    return Input.Key.Num8;
                //case Keys.Keypad9:
                //    return Input.Key.Num9;

                case Keys.F1:
                    return Input.Key.F1;
                case Keys.F2:
                    return Input.Key.F2;
                case Keys.F3:
                    return Input.Key.F3;
                case Keys.F4:
                    return Input.Key.F4;
                case Keys.F5:
                    return Input.Key.F5;
                case Keys.F6:
                    return Input.Key.F6;
                case Keys.F7:
                    return Input.Key.F7;
                case Keys.F8:
                    return Input.Key.F8;
                case Keys.F9:
                    return Input.Key.F9;
                case Keys.F10:
                    return Input.Key.F10;
                case Keys.F11:
                    return Input.Key.F11;
                case Keys.F12:
                    return Input.Key.F12;
                case Keys.F13:
                    return Input.Key.F13;
                case Keys.F14:
                    return Input.Key.F14;
                case Keys.F15:
                    return Input.Key.F15;
                case Keys.Pause:
                    return Input.Key.Pause;

                default:
                    return Input.Key.KeyCount;
            }
        }


        ///// <summary>
        ///// Translates SFML mouse button code to crossGFX mouse button code. Since crossGFX mouse button codes are based on SFML, the conversion is really easy.
        ///// </summary>
        ///// <param name="sfmlKey"></param>
        ///// <returns></returns>
        //private static crossGFX.Input.MouseButton TranslateButtonCode(Microsoft.Xna.Framework.Input.MouseState sfmlMouseButton) {
        //    return (crossGFX.Input.MouseButton)sfmlMouseButton;
        //}

        //private static Mouse.Button TranslateButtonCode(crossGFX.Input.MouseButton mouseButton) {
        //    return (Mouse.Button)mouseButton;
        //}

        public event EventHandler<crossGFX.Input.KeyEventArgs> KeyPressed;

        public event EventHandler<crossGFX.Input.KeyEventArgs> KeyReleased;

        public event EventHandler<crossGFX.Input.MouseMovedEventArgs> MouseMoved;


        public event EventHandler<Input.MouseButtonEventArgs> MouseButtonPressed;

        public event EventHandler<Input.MouseButtonEventArgs> MouseButtonReleased;

        public void Think() {
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
                        KeyPressed(this, new Input.KeyEventArgs((Key)i, true, IsControlDown, IsShiftDown, IsAltDown));
                    }
                }
            }
        }

        public bool IsMouseButtonDown(MouseButton mouseButton) {
            if (mouseButton == MouseButton.Left) {
                return (currentMouseState.LeftButton == ButtonState.Pressed);
            }
            //return SFML.Window.Mouse.IsButtonPressed(TranslateButtonCode(mouseButton));
            return false;
        }
    }
}
