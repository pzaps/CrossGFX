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
    public struct Rectangle
    {
        public static readonly Rectangle Empty = new Rectangle(); 

        public int X { get; set; }

        public int Y { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public Point Location {
            get { return new Point(this.X, this.Y); }
            set { this.X=value.X; this.Y=value.Y; }
        }

        public Size Size {
            get { return new Size(this.Width, this.Height); }
            set { this.Width=value.Width; this.Height=value.Height; }
        }

        public int Left {
            get {
                return this.X;
            }
        }

        public int Top {
            get {
                return this.Y;
            }
        }

        public int Right {
            get {
                return this.X + this.Width;
            }
        }

        public int Bottom {
            get {
                return this.Y + this.Height;
            }
        }

        public Rectangle(int x, int y, int width, int height) :this() {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
        }

        public bool ContainsPoint(int x, int y) {
            if (x >= this.X && y >= this.Y && x - this.X <= this.Width && y - this.Y <= this.Height) {
                return true;
            } else {
                return false;
            }
        }
    }
}
