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
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using crossGFX;

namespace crossGFX.Windows
{
    public class DriverLoader
    {
        static Assembly activeDriverAssembly;

        public static void LoadDriver(string resourceDirectory, string driverName) {
            LoadDriverDirect(resourceDirectory, resourceDirectory + "/drivers/" + driverName);
        }

        public static void LoadDriverDirect(string resourceDirectory, string driverPath) {
            // Add environment paths to find driver 
            //AddEnvironmentPaths(new string[] { System.IO.Path.GetFullPath(resourceDirectory + "/drivers/any/") });
            if (System.Environment.Is64BitProcess) {
                AddEnvironmentPaths(new string[] { System.IO.Path.GetFullPath(resourceDirectory + "/drivers/x64/") });
            } else {
                AddEnvironmentPaths(new string[] { System.IO.Path.GetFullPath(resourceDirectory + "/drivers/x86/") });
            }

            // Actually load the driver
            activeDriverAssembly = System.Reflection.Assembly.LoadFrom(driverPath);
            DriverManager.AssignDriverInstance(CreateDriverInstance(System.IO.Path.GetFileNameWithoutExtension(driverPath), activeDriverAssembly));

            DriverManager.ActiveDriver.Initialize(resourceDirectory);
        }

        private static IDriver CreateDriverInstance(string assemblyName, Assembly assembly) {
            IDriver driver = null;
            if (assembly != null) {
                driver = assembly.CreateInstance(assemblyName + ".Driver") as IDriver;
            }
            return driver;
        }

        public static void AddEnvironmentPaths(string[] paths) {
            string path = Environment.GetEnvironmentVariable("PATH") ?? string.Empty;
            path += ";" + string.Join(";", paths);

            Environment.SetEnvironmentVariable("PATH", path);
        }
    }
}
