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
