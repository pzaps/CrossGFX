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

namespace crossGFX
{
    public interface IRenderTarget
    {
        void BeginDrawing();
        void EndDrawing();

        void Fill(Color color);
        void Fill(Rectangle bounds, Color color);

        void DrawLine(int x, int y, int a, int b, Color color);

        void DrawRectangle(Rectangle bounds, Color color,
            int borderSizeTop, int borderSizeRight, int borderSizeBottom, int borderSizeLeft);
        void DrawRectangle(Rectangle bounds, Color color, int borderSize);

        void Draw(ITexture texture, Point position);
        void Draw(ITexture texture, Point position, Rectangle sourceRectangle);

        void DrawStretched(ITexture texture, Rectangle destinationBounds);
        void DrawStretched(ITexture texture, Rectangle destinationBounds, Rectangle sourceRectangle);

        void DrawString(Point position, IFont font, string text, int textSize, Color textColor,
            bool bold, bool italic, bool underline);
        void DrawString(Point position, RichString richString);

        void StartClip(Rectangle clipRegion, int scale);
        void EndClip();

        void BeginMoveOrigin(Point newOrigin);
        void EndMoveOrigin();

        Point Origin { get; set; }
    }
}
