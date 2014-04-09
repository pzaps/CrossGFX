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
using crossGFX.Input;

namespace crossGFX
{
    public interface IWindow : IRenderTarget
    {
        IInputHelper InputHelper { get; }
        string Title { set; }
#if PC
        System.Drawing.Icon Icon { set; }
#endif

        void Close();

        int Width { get; }
        int Height { get; }

        event EventHandler<TickEventArgs> Update;
        new event EventHandler<TickEventArgs> Draw;
        event EventHandler OnInitialize;
        event EventHandler Resized;
    }
}
