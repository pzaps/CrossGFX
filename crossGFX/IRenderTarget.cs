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
using crossGFX;

namespace crossGFX
{
    public interface IRenderTarget
    {
        void BeginDrawing();
        void EndDrawing();

        void Fill(Color color);
        void Fill(Rectangle bounds, Color color);

        void DrawLine(int x, int y, int a, int b, Color color);

        void Draw(ITexture texture, Point position);
        void Draw(ITexture texture, Point position, Rectangle sourceRectangle);

        void DrawStretched(ITexture texture, Rectangle destinationBounds);
        void DrawStretched(ITexture texture, Rectangle destinationBounds, Rectangle sourceRectangle);

        void DrawString(IFont font, string text, int textSize, Color textColor, Point position);

        void StartClip(Rectangle clipRegion, int scale);
        void EndClip();

        void BeginMoveOrigin(Point newOrigin);
        void EndMoveOrigin();

        Point Origin { get; set; }
    }
}
