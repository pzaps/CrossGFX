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

namespace crossGFX
{
    public struct Size
    {
        public static readonly Size Empty = new Size(0, 0);

        public int Width { get; set; }
        public int Height { get; set; }

        public Size(int width, int height)
            : this() {
            this.Width = width;
            this.Height = height;
        }

        public static bool operator !=(Size one, Size two) {
            return !(one == two);
        }
        public static bool operator ==(Size one, Size two) {
            return (one.Width == two.Width && one.Height == two.Height);
        }

        public override bool Equals(object obj) {
            if (obj is Size) {
                Size right = (Size) obj;
                if (this == right) {
                    return true;
                }
            }
            return false;
        }

        public override int GetHashCode() {
            return Width * Height;
        }
        public static Size operator +(Size one, Size two) {
            return new Size(one.Width + two.Width, one.Height + two.Height);
        }
        public static Size operator -(Size one, Size two) {
            return new Size(one.Width - two.Width, one.Height - two.Height);
        }
    }
}
