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
