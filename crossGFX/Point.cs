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
    public struct Point
    {
        public static readonly Point Empty = new Point(0, 0);

        public int X { get; set; }
        public int Y { get; set; }

        public Point(int x, int y)
            : this() {
            X = x;
            Y = y;
        }

        public static bool operator !=(Point one, Point two) {
            return !(one == two);
        }
        public static bool operator ==(Point one, Point two) {
            return (one.X == two.X && one.Y == two.Y);
        }

        public override bool Equals(object obj) {
            if (obj is Point) {
                Point right = (Point) obj;
                if (this == right) {
                    return true;
                }
            }
            return false;
        }

        public override int GetHashCode() {
            return X * Y;
        }
        public static Point operator +(Point one, Point two) {
            return new Point(one.X + two.X, one.Y + two.Y);
        }
        public static Point operator -(Point one, Point two) {
            return new Point(one.X - two.X, one.Y - two.Y);
        }
    }
}
