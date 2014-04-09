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
using crossGFX.Windows;

namespace crossGFX.SDLRenderer
{
    class Driver : IDriver
    {
        System system;
        ResourceManager textureManager;
        MusicPlayer musicPlayer;

        public void Initialize(string resourceDirectory) {
            system = new System();
            textureManager = new ResourceManager();
            musicPlayer = new MusicPlayer();

            DriverLoader.AddEnvironmentPaths(new string[] { global::System.IO.Path.GetFullPath(resourceDirectory + "/drivers/x86/sdl") });
        }

       

        public ISystem System {
            get { return system; }
        }

        public IMusicPlayer MusicPlayer {
            get { return musicPlayer; }
        }


        public IResourceManager ResourceManager {
            get { return textureManager; }
        }

        public string DriverName {
            get { return "SDL"; }
        }
    }
}
