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
using System.Reflection;

namespace crossGFX.SFMLRenderer
{
    class Driver : IDriver
    {
        System system;
        ResourceManager textureManager;
        MusicPlayer musicPlayer;

        public void Initialize(string resourceDirectory) {
            system = new System();
            textureManager = new ResourceManager(resourceDirectory);
            musicPlayer = new MusicPlayer();

            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.AssemblyResolve += new ResolveEventHandler(LoadFromSameFolder);
        }

        Assembly LoadFromSameFolder(object sender, ResolveEventArgs args) {
            string assemblyPath = global::System.IO.Path.Combine(ResourceManager.ResourceDirectory + "/drivers/any/", new AssemblyName(args.Name).Name + ".dll");
            if (global::System.IO.File.Exists(assemblyPath) == false)
                return null;
            Assembly assembly = Assembly.LoadFrom(assemblyPath);
            return assembly;
        }

        public ISystem System {
            get { return system; }
        }

        public IResourceManager ResourceManager {
            get { return textureManager; }
        }

        public IMusicPlayer MusicPlayer {
            get {
                return musicPlayer;
            }
        }

        public string DriverName {
            get { return "SFML"; }
        }

    }
}
