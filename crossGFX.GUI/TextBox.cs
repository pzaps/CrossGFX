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
using crossGFX.GUI.Skins;
using crossGFX.Input;

namespace crossGFX.GUI
{
    public class TextBox : Label
    {
        public override Skinnable Skinnable{get{return GUI.Instance.Skin.TextField;}}

        int cursorPos;
        int cursorEnd;

        //bool caretVisible;
        //Rectangle caretBounds;

        public TextBox() {
        }

        private void InsertText(string text) {
            if (!string.IsNullOrEmpty(Text)) {
                Text = Text.Insert(cursorPos, text);
            } else {
                Text = text;
            }

            cursorPos += text.Length;
            cursorEnd = cursorPos;

            MakeCaretVisible();
        }

        private enum DeleteRange { LeftChar, LeftWord, RightChar, RightWord }

        private void RemoveText(DeleteRange range) {
            Boolean isLeft = (range == DeleteRange.LeftChar || range == DeleteRange.LeftWord),
                isChar = (range == DeleteRange.LeftChar || range == DeleteRange.RightChar),
                inBounds = false;

            int removeFrom, removeTo;
            if (isChar) {
                removeFrom = (isLeft) ? cursorPos - 1 : cursorPos; removeTo = (isLeft) ? cursorPos : cursorPos + 1;
            } else {
                removeFrom = (isLeft) ? 0 : cursorPos; removeTo = (isLeft) ? cursorPos : Text.Length; // Range boundary
                int index = (isLeft) ? Text.LastIndexOf(' ', (cursorPos > 0) ? cursorPos - 1 : 0) // Index of the space
                    : Text.IndexOf(' ', cursorPos);                                               // past the word
                if (index != -1) if (isLeft) removeFrom = index; else removeTo = index + 1; // Range boundary 2
            }
            inBounds = (removeFrom >= 0 && removeTo <= Text.Length);
            if (!string.IsNullOrEmpty(Text) && inBounds) {
                Text = Text.Remove(removeFrom, removeTo - removeFrom);
                if(isLeft) {MoveCaret(removeFrom); MakeCaretVisible();} // Move the caret if deleted characters to its left
            }            
        }

        void MakeCaretVisible() {

            int caretPos = GetCharacterPosition(cursorPos).X; // was m_Text.Get...

            // If the caret is already in a semi-good position, leave it.
            {
                int realCaretPos = caretPos;
                if (realCaretPos > Bounds.Width * 0.1f && realCaretPos < Bounds.Width * 0.9f)
                    return;
            }

            // The ideal position is for the caret to be right in the middle
            int idealx = (int)(-caretPos + Bounds.Width * 0.5f);

            Size textSize = Font.MeasureTextSize(Text, TextSize);

            int idealY = 0;

            switch(VAlign) {
                    case VerticalAlignment.Center: idealY = Bounds.Height / 2 - textSize.Height / 2; break;
                    case VerticalAlignment.Bottom: idealY = Bounds.Height - textSize.Height; break;
            }

            // Don't show too much whitespace to the right
            if (idealx + textSize.Width < Bounds.Width)
                idealx = -textSize.Width + (Bounds.Width);

            // Or the left
            if (idealx > 1)
                idealx = 1;

            SetTextPosition(idealx, idealY);
        }

        public override void DrawOver(IRenderTarget renderTarget) {
            base.DrawOver(renderTarget);

            if (!string.IsNullOrEmpty(Text)) {
                Point pA = GetCharacterPosition(cursorPos);

                Rectangle caretRectangle = new Rectangle(pA.X + base.textPosition.X, base.textPosition.Y - 2, 1, TextSize + 4);
                renderTarget.Fill(caretRectangle, Color.Black);
            }
        }

        /*public override void Tick(IWindow window, TickEventArgs e) {
            base.Tick(window, e);

        }*/

        private void MoveCaret(int destination) {
            if (Text != null && destination >= 0 && Text.Length >= destination) {
                cursorPos = destination;

                MakeCaretVisible();
            }
        }


        public override void HandleKeyPressed(Input.KeyEventArgs e) {
            //base.HandleKeyPressed(e);

            switch (e.Code)
            {
                case Input.Key.Left:
                    {
                        if(e.Control) {
                            int index = Text.LastIndexOf(' ', (cursorPos > 0) ? cursorPos - 1 : 0);
                            MoveCaret((index != -1) ? index : 0);
                        } else MoveCaret(cursorPos - 1);
                        cursorEnd = cursorPos;
                    }
                    break;
                case Input.Key.Right:
                    {
                        if(e.Control) {
                            int index = Text.IndexOf(' ', cursorPos);
                            MoveCaret((index != -1) ? index + 1 : Text.Length);
                        } else MoveCaret(cursorPos + 1);
                        cursorEnd = cursorPos;
                    }
                    break;
                case Input.Key.Home:
                    {
                        MoveCaret(0);
                        cursorEnd = cursorPos;
                    }
                    break;
                case Input.Key.End:
                    {
                        MoveCaret(Text.Length);
                        cursorEnd = cursorPos;
                    }
                    break;
                case Input.Key.Backspace:
                    {
                        if (e.Control)
                            RemoveText(DeleteRange.LeftWord);
                        else RemoveText(DeleteRange.LeftChar);
                    }
                    break;
                case Input.Key.Delete:
                    {
                        if (e.Control)
                            RemoveText(DeleteRange.RightWord);
                        else RemoveText(DeleteRange.RightChar);
                    }
                    break;
                default: {
                        InsertText(GetCharString(e, DriverManager.ActiveDriver.System.DisplayWindow.InputHelper));
                    }
                    break;
            }
        }

        public static string GetCharString(Input.KeyEventArgs e, IInputHelper inputHelper) {
            switch (e.Code) {
                case Key.Space: {
                        return " ";
                    }
            }
            if (inputHelper.IsShiftDown) {

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
    }
}
