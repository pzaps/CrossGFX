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
using SdlDotNet.Input;

namespace crossGFX.SDLRenderer
{
    class Keyboard
    {
        public static string GetCharString(KeyboardEventArgs e) {
            switch (e.Key) {
                case Key.Space: {
                        return " ";
                    }
            }
            if (e.Mod == ModifierKeys.Caps || e.Mod == ModifierKeys.LeftShift || e.Mod == ModifierKeys.RightShift) {
                //if (SdlDotNet.Input.Keyboard.IsKeyPressed(Key.CapsLock) || SdlDotNet.Input.Keyboard.IsKeyPressed(SdlDotNet.Input.Key.LeftShift) || SdlDotNet.Input.Keyboard.IsKeyPressed(SdlDotNet.Input.Key.RightShift)) {
                switch (e.KeyboardCharacter.ToLower()) {
                    #region Alphabet
                    case "a":
                    case "b":
                    case "c":
                    case "d":
                    case "e":
                    case "f":
                    case "g":
                    case "h":
                    case "i":
                    case "j":
                    case "k":
                    case "l":
                    case "m":
                    case "n":
                    case "o":
                    case "p":
                    case "q":
                    case "r":
                    case "s":
                    case "t":
                    case "u":
                    case "v":
                    case "w":
                    case "x":
                    case "y":
                    case "z":
                        return e.KeyboardCharacter.ToUpper();
                    #endregion
                    #region Numbers
                    case "1":
                        return "!";
                    case "2":
                        return "@";
                    case "3":
                        return "#";
                    case "4":
                        return "$";
                    case "5":
                        return "%";
                    case "6":
                        return "^";
                    case "7":
                        return "&";
                    case "8":
                        return "*";
                    case "9":
                        return "(";
                    case "0":
                        return ")";
                    #endregion
                    #region Symbols
                    case "`":
                        return "~";
                    case "-":
                        return "_";
                    case "=":
                        return "+";
                    case "[":
                        return "{";
                    case "]":
                        return "}";
                    case @"\":
                        return "|";
                    case ";":
                        return ":";
                    case "'":
                        return "\"";
                    case ",":
                        return "<";
                    case ".":
                        return ">";
                    case "/":
                        return "?";
                    #endregion
                }
            } else {
                switch (e.KeyboardCharacter.ToLower()) {
                    default:
                        return e.KeyboardCharacter;
                }
            }

            return "";
        }

        public static string GetCharString(KeyboardEventArgs e, InputHelper inputHelper) {
            switch (e.Key) {
                case Key.Space: {
                        return " ";
                    }
            }
            if (e.Mod == ModifierKeys.Caps || inputHelper.IsShiftDown) {
                //if (SdlDotNet.Input.Keyboard.IsKeyPressed(Key.CapsLock) || SdlDotNet.Input.Keyboard.IsKeyPressed(SdlDotNet.Input.Key.LeftShift) || SdlDotNet.Input.Keyboard.IsKeyPressed(SdlDotNet.Input.Key.RightShift)) {
                switch (e.KeyboardCharacter.ToLower()) {
                    #region Alphabet
                    case "a":
                    case "b":
                    case "c":
                    case "d":
                    case "e":
                    case "f":
                    case "g":
                    case "h":
                    case "i":
                    case "j":
                    case "k":
                    case "l":
                    case "m":
                    case "n":
                    case "o":
                    case "p":
                    case "q":
                    case "r":
                    case "s":
                    case "t":
                    case "u":
                    case "v":
                    case "w":
                    case "x":
                    case "y":
                    case "z":
                        return e.KeyboardCharacter.ToUpper();
                    #endregion
                    #region Numbers
                    case "1":
                        return "!";
                    case "2":
                        return "@";
                    case "3":
                        return "#";
                    case "4":
                        return "$";
                    case "5":
                        return "%";
                    case "6":
                        return "^";
                    case "7":
                        return "&";
                    case "8":
                        return "*";
                    case "9":
                        return "(";
                    case "0":
                        return ")";
                    #endregion
                    #region Symbols
                    case "`":
                        return "~";
                    case "-":
                        return "_";
                    case "=":
                        return "+";
                    case "[":
                        return "{";
                    case "]":
                        return "}";
                    case @"\":
                        return "|";
                    case ";":
                        return ":";
                    case "'":
                        return "\"";
                    case ",":
                        return "<";
                    case ".":
                        return ">";
                    case "/":
                        return "?";
                    #endregion
                }
            } else {
                switch (e.KeyboardCharacter.ToLower()) {
                    default:
                        return e.KeyboardCharacter;
                }
            }

            return "";
        }

        public static bool IsModifierKey(SdlDotNet.Input.Key key) {
            switch (key) {
                case Key.RightShift:
                case Key.LeftShift:
                case Key.LeftAlt:
                case Key.RightAlt:
                    return true;
                default:
                    return false;
            }
        }
    }
}
