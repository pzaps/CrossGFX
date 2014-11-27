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

namespace crossGFX.Input
{
    public static class InputExtensions
    {
        /// <summary>
        /// Indicates whether the shift key is down.
        /// </summary>
        public static bool IsShiftDown(this KeyData keyData) {
            return (keyData.KeyState[(int)Input.Key.LShift] || keyData.KeyState[(int)Input.Key.RShift]);
        }

        /// <summary>
        /// Indicates whether the control key is down.
        /// </summary>
        public static bool IsControlDown(this KeyData keyData) {
            return (keyData.KeyState[(int)Input.Key.LControl] || keyData.KeyState[(int)Input.Key.RControl]);
        }

        /// <summary>
        /// Indicates whether the alt key is down.
        /// </summary>
        public static bool IsAltDown(this KeyData keyData) {
            return (keyData.KeyState[(int)Input.Key.LAlt] || keyData.KeyState[(int)Input.Key.RAlt]);
        }

        /// <summary>
        /// Checks if the given key is pressed.
        /// </summary>
        /// <param name="key">Key to check.</param>
        /// <returns>True if the key is down.</returns>
        public static bool IsKeyDown(this KeyData keyData, Input.Key key) {
            return keyData.KeyState[(int)key];
        }
    }
}
