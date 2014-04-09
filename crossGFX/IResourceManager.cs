﻿// Copyright (c) 2014 CrossGFX Team

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
using System.IO;
using System.Linq;
using System.Text;

namespace crossGFX
{
    public interface IResourceManager
    {
        string ResourceDirectory { get; }

        ITexture LoadTextureDirect(string fullFilePath);
        ITexture LoadTexture(string filePath);
        ITexture LoadTexture(byte[] data);
        IRenderTexture CreateRenderTexture(int width, int height);

        IFont LoadFont(string filePath);
        IFont LoadFontDirect(string fullFilePath);

        IMusic LoadMusic(string filePath, string songId);
        IMusic LoadMusicDirect(string fullFilePath, string songId);
        IMusic LoadMusic(Stream musicStream, string songId); 

        Stream GetResourceStream(string resourceName);
        bool ResourceExists(string resourceName);
        Stream CreateNetworkStream(Uri uri);
    }
}
