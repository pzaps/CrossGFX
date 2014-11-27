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

        void Save(string filePath);
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
