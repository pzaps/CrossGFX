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
using System.IO;
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

        static string driverPath;

        public static void LoadDriver(string driverPath) {
            DriverLoader.driverPath = driverPath;

            // Add environment paths to find driver 
            if (System.Environment.Is64BitProcess) {
                AddEnvironmentPaths(new string[] { Path.GetFullPath(Path.Combine(Path.GetDirectoryName(driverPath), "x64")) });
            } else {
                AddEnvironmentPaths(new string[] { Path.GetFullPath(Path.Combine(Path.GetDirectoryName(driverPath), "x86")) });
            }

            // Actually load the driver
            activeDriverAssembly = System.Reflection.Assembly.LoadFrom(driverPath);
            DriverManager.AssignDriverInstance(CreateDriverInstance(System.IO.Path.GetFileNameWithoutExtension(driverPath), activeDriverAssembly));

            DriverManager.ActiveDriver.Initialize(new crossGFX.Windows.Clipboard());
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
