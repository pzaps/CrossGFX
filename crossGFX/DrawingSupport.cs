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
    public class DrawingSupport
    {
        private static int TranslateX(Point renderOffset, int x) {
            int x1 = x + renderOffset.X;
            //return Util.Ceil(x1 * Scale);
            return (int)System.Math.Ceiling(x1);
        }

        private static int TranslateY(Point renderOffset, int y) {
            int y1 = y + renderOffset.Y;
            //return Util.Ceil(y1 * Scale);
            return (int)System.Math.Ceiling(y1);
        }

        /// <summary>
        /// Translates a panel's local drawing coordinate into view space, taking offsets into account.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public static void Translate(Point renderOffset, ref int x, ref int y) {
            x += renderOffset.X;
            y += renderOffset.Y;

            //x = Util.Ceil(x * Scale);
            //y = Util.Ceil(y * Scale);
            x = (int)System.Math.Ceiling(x);
            y = (int)System.Math.Ceiling(y);
        }

        /// <summary>
        /// Translates a panel's local drawing coordinate into view space, taking offsets into account.
        /// </summary>
        public static void Translate(Point renderOffset, ref Point p) {
            int x = p.X;
            int y = p.Y;
            Translate(renderOffset, ref x, ref y);
            p.X = x;
            p.Y = y;
        }

        /// <summary>
        /// Translates a panel's local drawing coordinate into view space, taking offsets into account.
        /// </summary>
        public static void Translate(Point renderOffset, ref Rectangle rect) {
            rect.X = TranslateX(renderOffset, rect.X);
            rect.Y = TranslateY(renderOffset, rect.Y);
            //rect.Width = Util.Ceil(rect.Width * Scale);
            //rect.Height = Util.Ceil(rect.Height * Scale);
            rect.Width = (int)System.Math.Ceiling(rect.Width);
            rect.Height = (int)System.Math.Ceiling(rect.Height);
        }
    }
}
