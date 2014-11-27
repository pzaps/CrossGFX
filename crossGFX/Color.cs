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

namespace crossGFX
{
    public struct Color
    {
        private const int ARGBAlphaShift = 24;
        private const int ARGBRedShift = 16;
        private const int ARGBGreenShift = 8;
        private const int ARGBBlueShift = 0;

        public static readonly Color Empty = new Color();

        public static Color White {
            get {
                return new Color(unchecked((int)0xFFFFFFFF));
            }
        }

        public static Color Transparent {
            get {
                return new Color((int)0x00FFFFFF);
            }
        }

        public static Color Black {
            get {
                return new Color(unchecked((int)0xFF000000));
            }
        }

        public static Color Yellow {
            get {
                return new Color(unchecked((int)0xFFFFFF00));
            }
        }

        public static Color Red {
            get {
                return new Color(unchecked((int)0xFFFF0000));
            }
        }

        public static Color WhiteSmoke {
            get {
                return new Color(unchecked((int)0xFFF5F5F5));
            }
        }

        public static Color LightGray {
            get {
                return new Color(unchecked((int)0xFFD3D3D3));
            }
        }

        public static Color Cyan {
            get {
                return new Color(unchecked((int)0xFF00FFFF));
            }
        }

        public static Color Gray {
            get {
                return new Color(unchecked((int)0xFF808080));
            }
        }

        public static Color DarkGray {
            get {
                return new Color(unchecked((int)0xFFA9A9A9));
            }
        }

        public static Color Green {
            get {
                return new Color(unchecked((int)0xFF008000));
            }
        }

        public static Color LawnGreen {
            get {
                return new Color(unchecked((int)0xFF7CFC00));
            }
        }

        public static Color LightGreen {
            get {
                return new Color(unchecked((int)0xFF90EE90));
            }
        }

        public static Color LightSkyBlue {
            get {
                return new Color(unchecked((int)0xFF87CEFA));
            }
        }

        public static Color SkyBlue {
            get {
                return new Color(unchecked((int)0xFF87CEEB));
            }
        }

        long value;

        public Color(int a, int r, int g, int b)
            : this() {
            CheckByte(a, "alpha");
            CheckByte(r, "red");
            CheckByte(g, "green");
            CheckByte(b, "blue");
            this.value = MakeArgb((byte)a, (byte)r, (byte)g, (byte)b);
        }

        private Color(long value) {
            this.value = value;
        }

        private static long MakeArgb(byte alpha, byte red, byte green, byte blue) {
            return (long)((uint)(red << ARGBRedShift |
                         green << ARGBGreenShift |
                         blue << ARGBBlueShift |
                         alpha << ARGBAlphaShift)) & 0xffffffff;
        }

        public byte R {
            get {
                return (byte)((value >> ARGBRedShift) & 0xFF);
            }
        }

        public byte G {
            get {
                return (byte)((value >> ARGBGreenShift) & 0xFF);
            }
        }

        public byte B {
            get {
                return (byte)((value >> ARGBBlueShift) & 0xFF);
            }
        }

        public byte A {
            get {
                return (byte)((value >> ARGBAlphaShift) & 0xFF);
            }
        }

        private static void CheckByte(int value, string name) {
            if (value < 0 || value > 255)
                throw new ArgumentException();
        }

        public int ToArgb() {
            return (int)value;
        }

        public static Color FromArgb(int argb) {
            return new Color((long)argb & 0xffffffff);
        }

        public static Color FromArgb(int alpha, int red, int green, int blue) {
            CheckByte(alpha, "alpha");
            CheckByte(red, "red");
            CheckByte(green, "green");
            CheckByte(blue, "blue");
            return new Color(MakeArgb((byte)alpha, (byte)red, (byte)green, (byte)blue));
        }

        public static Color FromArgb(int red, int green, int blue) {
            return FromArgb(255, red, green, blue);
        }

        public Single GetHue() {
            if (R == G && G == B)
                return 0; // 0 makes as good an UNDEFINED value as any

            float r = (float)R / 255.0f;
            float g = (float)G / 255.0f;
            float b = (float)B / 255.0f;

            float max, min;
            float delta;
            float hue = 0.0f;

            max = r; min = r;

            if (g > max) max = g;
            if (b > max) max = b;

            if (g < min) min = g;
            if (b < min) min = b;

            delta = max - min;

            if (r == max) {
                hue = (g - b) / delta;
            } else if (g == max) {
                hue = 2 + (b - r) / delta;
            } else if (b == max) {
                hue = 4 + (r - g) / delta;
            }
            hue *= 60;

            if (hue < 0.0f) {
                hue += 360.0f;
            }
            return hue;
        }

        public Color Invert() {
            return Color.FromArgb(this.A, 255 - this.R, 255 - this.G, 255 - this.B);
        }

        public static bool operator ==(Color left, Color right) {
            if (left.value == right.value) {
                return true;
            } else {
                return false;
            }
        }

        public static bool operator !=(Color left, Color right) {
            return !(left == right);
        }

        public override bool Equals(object obj) {
            if (obj is Color) {
                Color right = (Color)obj;
                if (value == right.value) {
                    return true;
                }
            }
            return false;
        }

        public override int GetHashCode() {
            return (int) value;
        }

    }
}
