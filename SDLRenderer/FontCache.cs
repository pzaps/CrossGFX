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

namespace crossGFX.SDLRenderer
{
    struct FontId
    {
        string identifier;
        int size;

        public FontId(string identifier, int size)
            : this() {
            this.identifier = identifier;
            this.size = size;
        }
    }

    class FontCache
    {
        static Dictionary<FontId, SdlDotNet.Graphics.Font> cache;

        public static void Initialize() {
            cache = new Dictionary<FontId, SdlDotNet.Graphics.Font>();
        }

        /// <summary>
        /// Adds a font to the cache. Does not check if it already exists
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="size"></param>
        public static SdlDotNet.Graphics.Font AddFont(string filePath, int size) {
            SdlDotNet.Graphics.Font font = new SdlDotNet.Graphics.Font(filePath, size);

            cache.Add(new FontId(filePath, size), font);

            return font;
        }

        public static SdlDotNet.Graphics.Font LookupFont(string filePath, int size) {
            SdlDotNet.Graphics.Font font = null;
            if (cache.TryGetValue(new FontId(filePath, size), out font)) {
                return font;
            } else {
                return AddFont(filePath, size);
            }
        }
    }
}
