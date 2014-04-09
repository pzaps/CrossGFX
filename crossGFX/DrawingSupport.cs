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
