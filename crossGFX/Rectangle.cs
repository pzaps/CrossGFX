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
