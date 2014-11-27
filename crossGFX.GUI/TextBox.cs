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
using System.Text.RegularExpressions;
using crossGFX.GUI.Skins;
using crossGFX.Input;

namespace crossGFX.GUI
{
    public class TextBox : Label
    {
        public override Skinnable DefaultSkinnable { get { return GUI.Instance.Skin.TextField; } }

        int cursorPos;
        int selectionStart;
        int selectionEnd;
        bool caretVisible;

        public TextBox() {
        }

        public override void DrawMiddle(IRenderTarget renderTarget) {
            if (string.IsNullOrEmpty(this.Value.Text) || this.Value.Font == null) return;
            base.DrawMiddle(renderTarget);
            string text = this.VisibleText;
            if (selectionEnd - selectionStart > 0) {
                string selection = text.Substring(selectionStart, selectionEnd - selectionStart);
                Size size = this.Value.Font.MeasureTextSize(selection, this.Value.TextSize, this.Value.Bold);
                Size offset = this.Value.Font.MeasureTextSize(text.Substring(0, selectionStart), this.Value.TextSize, this.Value.Bold);
                Rectangle highlightRectangle = new Rectangle(
                    offset.Width + base.textPosition.X, base.textPosition.Y - 2,
                    size.Width, this.Value.TextSize + 4
                    );
                renderTarget.Fill(highlightRectangle, this.Value.Color);
                renderTarget.DrawString(new Point(this.textPosition.X + offset.Width, this.textPosition.Y), this.Value.Font, selection,
                    this.Value.TextSize, this.Value.Color.Invert(), this.Value.Bold, this.Value.Italic, this.Value.Underline);
            }
        }

        public override void DrawOver(IRenderTarget renderTarget) {
            if (this.HasFocus && this.caretVisible) {
                Point pA = GetCharacterPosition(cursorPos);

                Rectangle caretRectangle = new Rectangle(pA.X + base.textPosition.X, base.textPosition.Y - 2, 1, this.Value.TextSize + 4);
                renderTarget.Fill(caretRectangle,
                    ((selectionEnd - selectionStart > 0) ? this.Value.Color.Invert() : this.Value.Color));
            }
        }

        uint lastTickCheck;
        public override void Tick(IWindow window, TickEventArgs e) {
            base.Tick(window, e);
            if (this.HasFocus)
                if (e.Tick > this.lastTickCheck + 500) {
                    this.caretVisible = !this.caretVisible;
                    this.lastTickCheck = e.Tick;
                }
        }

        public override void HandleKeyPressed(Input.KeyEventArgs e) {
            switch (e.Code) {
                case Input.Key.Left: {
                        bool moved;
                        if (e.Control) { // skips the word to the left
                            int index = this.Value.Text.LastIndexOf(' ', (cursorPos > 0) ? cursorPos - 1 : 0);
                            moved = this.MoveCaret((index != -1) ? index : 0);
                        } else { // skips the character to the left
                            moved = this.MoveCaret(cursorPos - 1);
                        }
                        // updates selection
                        if (e.Shift) {
                            if (moved)
                                if (this.cursorPos >= this.selectionStart) {
                                    this.selectionEnd = this.cursorPos;
                                } else {
                                    this.selectionStart = this.cursorPos;
                                }
                        } else {
                            this.selectionStart = this.selectionEnd = cursorPos;
                        }
                    }
                    break;
                case Input.Key.Right: {
                        bool moved;
                        if (e.Control) { // skips the word to the right
                            int index = this.Value.Text.IndexOf(' ', cursorPos);
                            moved = this.MoveCaret((index != -1) ? index + 1 : this.Value.Text.Length);
                        } else { // skips the character to the right
                            moved = this.MoveCaret(cursorPos + 1);
                        }
                        // updates selection
                        if (e.Shift) {
                            if (moved)
                                if (this.cursorPos > this.selectionEnd) {
                                    this.selectionEnd = this.cursorPos;
                                } else {
                                    this.selectionStart = this.cursorPos;
                                }
                        } else {
                            this.selectionStart = this.selectionEnd = cursorPos;
                        }
                    }
                    break;
                case Input.Key.Home: {
                        this.MoveCaret(0); // move caret to the beginning
                        // updates selection
                        if (e.Shift) {
                            if (this.cursorPos >= this.selectionEnd) {
                                this.selectionEnd = this.selectionStart;
                            }
                            this.selectionStart = 0;
                        } else {
                            this.selectionStart = this.selectionEnd = 0;
                        }
                    }
                    break;
                case Input.Key.End: {
                        this.MoveCaret(this.Value.Text.Length); // move caret to the end
                        // updates selection
                        if (e.Shift) {
                            if (this.cursorPos < this.selectionEnd) {
                                this.selectionStart = this.selectionEnd;
                            }
                            this.selectionEnd = this.Value.Text.Length;
                        } else {
                            this.selectionStart = this.selectionEnd = this.Value.Text.Length;
                        }
                    }
                    break;
                case Input.Key.Backspace: {
                        if (this.selectionEnd - this.selectionStart > 0) { // deletes selection
                            this.Value.Text = this.Value.Text.Remove(this.selectionStart, this.selectionEnd - this.selectionStart);
                            this.selectionEnd = this.selectionStart;
                            this.MoveCaret(selectionStart);
                            break;
                        }
                        if (e.Control) { // deletes the word to the left
                            int index = this.Value.Text.LastIndexOf(' ', (cursorPos > 0) ? cursorPos - 1 : 0);
                            int removeFrom = index != -1 ? index : 0;
                            if (removeFrom >= 0 && this.cursorPos <= this.Value.Text.Length) {
                                this.Value.Text = this.Value.Text.Remove(removeFrom, cursorPos - removeFrom);
                                this.MoveCaret(removeFrom);
                            }
                        } else { // deletes the character to the left
                            int removeFrom = this.cursorPos - 1;
                            if (removeFrom >= 0) {
                                this.Value.Text = this.Value.Text.Remove(removeFrom, 1);
                                this.MoveCaret(removeFrom);
                            }
                        }
                        this.ResetTextPosition();
                    }
                    break;
                case Input.Key.Delete: {
                        if (this.selectionEnd - this.selectionStart > 0) { // deletes selection
                            this.Value.Text = this.Value.Text.Remove(this.selectionStart, this.selectionEnd - this.selectionStart);
                            this.selectionEnd = this.selectionStart;
                            this.MoveCaret(selectionStart);
                            break;
                        }
                        if (e.Control) { // deletes the word to the right
                            int index = this.Value.Text.IndexOf(' ', cursorPos);
                            int removeTo = (index != -1) ? index + 1 : this.Value.Text.Length;
                            if (this.cursorPos >= 0 && removeTo <= this.Value.Text.Length) {
                                this.Value.Text = this.Value.Text.Remove(cursorPos, removeTo - cursorPos);
                            }
                        } else { // deletes the character to the right
                            int removeFrom = this.cursorPos;
                            if (removeFrom < this.Value.Text.Length) {
                                this.Value.Text = this.Value.Text.Remove(removeFrom, 1);
                            }
                        }
                        this.ResetTextPosition();
                    }
                    break;
                case Input.Key.A: {
                        if (e.Control) { // selects all
                            this.selectionStart = 0;
                            this.selectionEnd = this.Value.Text.Length;
                        } else InsertText(GetCharString(e, DriverManager.ActiveDriver.System.DisplayWindow.InputHelper)); // inserts the character
                    }
                    break;
                case Input.Key.X: {
                        if (e.Control) { // cuts the selection to the clipboard
                            if (this.selectionEnd - this.selectionStart > 0) {
                                DriverManager.ActiveDriver.System.Clipboard.SetText(this.Value.Text.Substring(this.selectionStart, this.selectionEnd - this.selectionStart));
                                this.Value.Text = this.Value.Text.Remove(this.selectionStart, this.selectionEnd - this.selectionStart);
                                this.selectionEnd = this.selectionStart;
                                this.MoveCaret(selectionStart);
                            }
                        } else InsertText(GetCharString(e, DriverManager.ActiveDriver.System.DisplayWindow.InputHelper)); // inserts the character
                    }
                    break;
                case Input.Key.C: {
                        if (e.Control) { // copies the selection to the clipboard
                            if (this.selectionEnd - this.selectionStart > 0)
                                DriverManager.ActiveDriver.System.Clipboard.SetText(this.Value.Text.Substring(this.selectionStart, this.selectionEnd - this.selectionStart));
                        } else InsertText(GetCharString(e, DriverManager.ActiveDriver.System.DisplayWindow.InputHelper)); // inserts the character
                    }
                    break;
                case Input.Key.V: {
                        if (e.Control) // pastes from the clipboard
                            this.InsertText(DriverManager.ActiveDriver.System.Clipboard.GetText());
                        else InsertText(GetCharString(e, DriverManager.ActiveDriver.System.DisplayWindow.InputHelper)); // inserts the character
                    }
                    break;
                case Input.Key.Return: {
                        if (this.OnReturn(this.Value.Text))
                            this.Clear();
                    }
                    break;
                default: {
                        // inserts the character
                        InsertText(GetCharString(e, DriverManager.ActiveDriver.System.DisplayWindow.InputHelper));
                    }
                    break;
            }
        }

        private void InsertText(string text) {
            if (String.IsNullOrEmpty(text)) return;
            if (this.selectionEnd - this.selectionStart > 0) {
                this.Value.Text = this.Value.Text.Remove(this.selectionStart, this.selectionEnd - this.selectionStart);
                this.MoveCaret(selectionStart);
            }
            if (!string.IsNullOrEmpty(this.Value.Text)) {
                this.Value.Text = this.Value.Text.Insert(this.cursorPos, text);
            } else {
                this.Value.Text = text;
            }

            this.MoveCaret(this.cursorPos + text.Length);
            this.selectionStart = this.selectionEnd = this.cursorPos;
        }

        protected override void ResetTextPosition() {

            Point point = Point.Empty;

            if (cursorPos > this.Value.Text.Length) {
                cursorPos = this.Value.Text.Length;
            }

            int height = this.Value.TextSize;
            if (this.Value.Font != null) {
                int caretPos = GetCharacterPosition(cursorPos).X;
                Size size = this.Value.Font.MeasureTextSize(this.Value.Text, this.Value.TextSize, this.Value.Bold);
                point.X = (int)(-caretPos + Bounds.Width * 0.5f);
                if (point.X + size.Width < Bounds.Width) // Don't show too much whitespace to the right
                    point.X = -size.Width + (Bounds.Width);
                if (point.X > 1) // Or the left
                    point.X = 1;
                height = size.Height;
            }

            switch (VAlign) {
                //case VerticalAlignment.Top: point.Y = 0; break;
                case VerticalAlignment.Center: point.Y = Bounds.Height / 2 - height / 2; break;
                case VerticalAlignment.Bottom: point.Y = Bounds.Height - height; break;
            }

            this.textPosition = point;
        }

        private bool MoveCaret(int destination) {
            if (destination >= 0 && destination <= this.Value.Text.Length) {
                this.cursorPos = destination;
                this.ResetTextPosition();
                return true;
            }
            return false;
        }

        public static string GetCharString(Input.KeyEventArgs e, IInputHelper inputHelper) {
            switch (e.Code) {
                case Key.Space: {
                        return " ";
                    }
            }
            if (inputHelper.KeyData.IsShiftDown()) {

                if ((int)e.Code >= 0 && (int)e.Code <= 25) {
                    return e.Code.ToString().ToUpper();
                }

                switch (e.Code) {
                    #region Numbers
                    case Key.Num1:
                        return "!";
                    case Key.Num2:
                        return "@";
                    case Key.Num3:
                        return "#";
                    case Key.Num4:
                        return "$";
                    case Key.Num5:
                        return "%";
                    case Key.Num6:
                        return "^";
                    case Key.Num7:
                        return "&";
                    case Key.Num8:
                        return "*";
                    case Key.Num9:
                        return "(";
                    case Key.Num0:
                        return ")";
                    #endregion
                    #region Symbols
                    case Key.Tilde:
                        return "~";
                    case Key.Dash:
                        return "_";
                    case Key.Equal:
                        return "+";
                    case Key.LBracket:
                        return "{";
                    case Key.RBracket:
                        return "}";
                    case Key.BackSlash:
                        return "|";
                    case Key.SemiColon:
                        return ":";
                    case Key.Quote:
                        return "\"";
                    case Key.Comma:
                        return "<";
                    case Key.Period:
                        return ">";
                    case Key.Slash:
                        return "?";
                    #endregion
                }
            } else {

                if ((int)e.Code >= 0 && (int)e.Code <= 25) {
                    return e.Code.ToString().ToLower();
                }

                switch (e.Code) {
                    #region Numbers
                    case Key.Num1:
                        return "1";
                    case Key.Num2:
                        return "2";
                    case Key.Num3:
                        return "3";
                    case Key.Num4:
                        return "4";
                    case Key.Num5:
                        return "5";
                    case Key.Num6:
                        return "6";
                    case Key.Num7:
                        return "7";
                    case Key.Num8:
                        return "8";
                    case Key.Num9:
                        return "9";
                    case Key.Num0:
                        return "0";
                    #endregion
                    #region Symbols
                    case Key.Tilde:
                        return "`";
                    case Key.Dash:
                        return "-";
                    case Key.Equal:
                        return "+";
                    case Key.LBracket:
                        return "[";
                    case Key.RBracket:
                        return "]";
                    case Key.BackSlash:
                        return "\\";
                    case Key.SemiColon:
                        return ";";
                    case Key.Quote:
                        return "\'";
                    case Key.Comma:
                        return ",";
                    case Key.Period:
                        return ".";
                    case Key.Slash:
                        return "/";
                    #endregion
                }
            }

            return "";
        }

        public override void Clear() {
            base.Clear();
            this.MoveCaret(0);
        }
    }
}
