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
using System.IO;
using crossGFX;

namespace crossGFX
{
    /// <summary>
    /// Represents a texture.
    /// </summary>
    public interface ITexture : IDisposable
    {
        byte Alpha { get; set; }

        int Width { get; }
        int Height { get; }

        Color GetPixelColor(int x, int y);
        Color GetPixelColor(int x, int y, Color defaultColor);
        /////// <summary>
        /////// Texture name. Usually file name, but exact meaning depends on renderer.
        /////// </summary>
        ////String Name { get; set; }

        /////// <summary>
        /////// Texture width.
        /////// </summary>
        ////public int Width { get; set; }

        /////// <summary>
        /////// Texture height.
        /////// </summary>
        ////public int Height { get; set; }

        ////private readonly Renderer.Base m_Renderer;

        ///// <summary>
        ///// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        ///// </summary>
        //public void Dispose()
        //{
        //    //m_Renderer.FreeTexture(this);
        //    GC.SuppressFinalize(this);
        //}

   }
}
