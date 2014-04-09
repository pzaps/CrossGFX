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

namespace crossGFX
{
    public class AnimatedTexture : IActor, ITickable, IDrawable
    {
        public List<ITexture> Frames { get; private set; }

        public int StepTime { get; set; }

        uint lastStepTime;
        int frameNumber;

        public Point Location { get; set; }

        public Scene ParentScene { get; set; }

        public bool Visible { get; set; }

        public AnimatedTexture() {
            Frames = new List<ITexture>();
        }

        public void Tick(IWindow window, TickEventArgs e) {
            if (e.Tick > lastStepTime + StepTime) {
                lastStepTime = e.Tick;
                frameNumber++;

                if (frameNumber == Frames.Count) {
                    frameNumber = 0;
                }
            }
        }

        public void Draw(IRenderTarget renderTarget) {
            renderTarget.Draw(Frames[frameNumber], Location);
        }
    }
}
