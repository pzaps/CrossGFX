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

namespace crossGFX.Input
{
    public class MouseMovedEventArgs : EventArgs
    {

        public MouseMovedEventArgs(int x, int y, int lx, int ly) {
            this.X = x;
            this.Y = y;
            this.lX = lx;
            this.lY = ly;
        }

        public int X { get; private set; }

        public int Y { get; private set; }

        public int lX { get; private set; }

        public int lY { get; private set; }

        public int dX { get { return X - lX; } }

        public int dY { get { return Y - lY; } }
    }
}
