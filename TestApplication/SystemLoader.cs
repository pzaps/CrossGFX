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
using crossGFX.Windows;

namespace TestApplication
{
    class SystemLoader
    {
        public static void Load(ref string basePath) {
#if DEBUG
            basePath = AppDomain.CurrentDomain.BaseDirectory + "../../../";
#else
            basePath = AppDomain.CurrentDomain.BaseDirectory + "/";
#endif
            string test = System.IO.Path.GetFullPath(basePath + "dependancies/");
            DriverLoader.AddEnvironmentPaths(new string[] { System.IO.Path.GetFullPath(basePath + "dependancies/") });
            if (System.Environment.Is64BitProcess) {
                DriverLoader.AddEnvironmentPaths(new string[] { System.IO.Path.GetFullPath(basePath + "dependancies/x64/") });
            } else {
                DriverLoader.AddEnvironmentPaths(new string[] { System.IO.Path.GetFullPath(basePath + "dependancies/x86/") });
                DriverLoader.AddEnvironmentPaths(new string[] { System.IO.Path.GetFullPath(basePath + "dependancies/x86/sdl") });

            }
            string baseDriverPath;
#if DEBUG
            baseDriverPath = "../../../SFMLRenderer/bin/Debug/";
#else
            baseDriverPath = "drivers/";
#endif
            string driverPath = null;

            driverPath = baseDriverPath + "crossGFX.SFMLRenderer.dll";

            DriverLoader.LoadDriverDirect(System.IO.Path.GetFullPath(basePath + "resources"), driverPath);
        }
    }
}
