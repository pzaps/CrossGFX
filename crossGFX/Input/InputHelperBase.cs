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
using crossGFX;

namespace crossGFX.Input
{
    public class InputHelperBase
    {
        protected Point mousePosition;
        protected readonly float[] lastClickTime = new float[(int)crossGFX.Input.MouseButton.ButtonCount];
        protected Point lastClickPosition;
        protected KeyData keyData;
        int keyRepeatRate;
        int keyRepeatDelay;

        public int KeyRepeatDelay {
            get { return keyRepeatDelay; }
            set { keyRepeatDelay = value; }
        }

        public int KeyRepeatRate {
            get { return keyRepeatRate; }
            set { keyRepeatRate = value; }
        }

        public Point MousePosition {
            get {
                return mousePosition;
            }
        }

        public Point LastClickPosition {
            get {
                return lastClickPosition;
            }
            set {
                lastClickPosition = value;
            }
        }

        public KeyData KeyData {
            get { return keyData; }
        }

        public float[] LastClickTime {
            get { return lastClickTime; }
        }

        public InputHelperBase() {
            keyData = new KeyData();

            keyRepeatRate = 60;
            keyRepeatDelay = 200;
        }

        

        protected void KeyPressedHelper(Input.Key key) {

        }


    }
}
