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
    public class ActorCollection
    {
        List<IActor> actors;
        Scene parentScene;

        public int Count {
            get { return actors.Count; }
        }

        public ActorCollection(Scene parentScene) {
            this.parentScene = parentScene;
            
            actors = new List<IActor>();
        }

        public IActor this[int index] {
            get { return actors[index]; }
        }

        public void Add(IActor actor) {
            actors.Add(actor);
        }

        public bool Contains(IActor actor) {
            return actors.Contains(actor);
        }

        public void Remove(IActor actor) {
            actors.Remove(actor);
        }

        public void Insert(int position, IActor actor) {
            actors.Insert(position, actor);
        }
    }
}
