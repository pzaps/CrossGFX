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
using Audio = SFML.Audio;

namespace crossGFX.SFMLRenderer
{
    class MusicPlayer : IMusicPlayer
    {
        Music currentSong;
        Music crossfadeTargetSong;
        int crossfadeStepSize;
        uint lastCrossfadeStep;

        public IMusic CurrentSong {
            get { return currentSong; }
        }

        public void Play(IMusic music) {
            currentSong = music as Music;

            currentSong.SFMusic.Play();
        }

        public void Stop() {
            if (currentSong != null) {
                currentSong.SFMusic.Stop();
                currentSong = null;
            }
        }

        public void FadeOut() {
            if (currentSong != null) {
                //currentSong.SFMusic.
            }
        }

        public void Pause() {
            if (currentSong != null) {
                currentSong.SFMusic.Pause();
            }
        }

        public void Resume() {
            if (currentSong != null) {
                currentSong.SFMusic.Play();
            }
        }

        public void Crossfade(IMusic music, int length) {
            if (currentSong == null) {
                // TODO: Fade in music
                Play(music);
            } else {
                crossfadeTargetSong = music as Music;
                crossfadeStepSize = length / 100;

                crossfadeTargetSong.SFMusic.Volume = 0f;
                crossfadeTargetSong.SFMusic.Play();
            }
        }

        public void Update(uint tick) {
            // Process cross-fade
            if (currentSong != null && crossfadeTargetSong != null) {
                if (tick >= lastCrossfadeStep + crossfadeStepSize) {
                    lastCrossfadeStep = tick;
                    currentSong.SFMusic.Volume -= 1f;
                    crossfadeTargetSong.SFMusic.Volume += 1f;
                }
                  
                if (crossfadeTargetSong.SFMusic.Volume >= 100f) {
                    crossfadeTargetSong.SFMusic.Volume = 100f;
                    currentSong.SFMusic.Stop();
                    currentSong.SFMusic.Dispose();

                    currentSong = crossfadeTargetSong;
                    crossfadeTargetSong = null;
                }
            }

        }
    }
}
