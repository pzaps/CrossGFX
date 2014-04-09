using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace crossGFX.XNARenderer
{
    public class Driver : IDriver
    {
        System system;
        ResourceManager textureManager;

        public void Initialize(string resourceDirectory) {
            system = new System();
            textureManager = new ResourceManager();
        }

        public ISystem System {
            get { return system; }
        }

        public IResourceManager ResourceManager {
            get { return textureManager; }
        }

        public IMusicPlayer MusicPlayer {
            get { throw new NotImplementedException(); }
        }

        public string DriverName {
            get { return "XNA"; }
        }
    }
}
