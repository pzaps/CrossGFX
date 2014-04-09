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
using System.IO;
using System.Linq;
using System.Text;

namespace crossGFX.SFMLRenderer
{
    public class Music : IMusic
    {
        SFML.Audio.Music sfMusic;
        string songId;

        public string SongId {
            get { return songId; }
        }

        public SFML.Audio.Music SFMusic {
            get { return sfMusic; }
        }

        public Music(string filePath, string songId) {
            sfMusic = new SFML.Audio.Music(filePath);
            this.songId = songId;
        }

        public Music(Stream musicStream, string songId) {
            sfMusic = new SFML.Audio.Music(musicStream);
            this.songId = songId;
        }

        public void Play() {
            DriverManager.ActiveDriver.MusicPlayer.Play(this);
        }
    }
}
