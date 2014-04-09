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
    public class KeyEventArgs : EventArgs
    {
        Key code;
        bool control;
        bool shift;
        bool alt;
        bool keyDown;

        public Key Code {
            get { return code; }
        }

        public bool KeyDown {
            get { return keyDown; }
        }

        public bool Control {
            get { return control; }
        }

        public bool Shift {
            get { return shift; }
        }

        public bool Alt {
            get { return alt; }
        }

        public KeyEventArgs(Key code, bool keyDown, bool control, bool shift, bool alt) {
            this.code = code;
            this.keyDown = keyDown;
            this.control = control;
            this.shift = shift;
            this.alt = alt;
        }
    }
}
