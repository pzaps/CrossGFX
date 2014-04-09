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
using crossGFX.Input;

namespace crossGFX
{
    public class Scene : IDrawable, IBoundObject, IFocusable, IMouseEventHandler, IKeyboardEventHandler
    {
        public ActorCollection Actors { get; private set; }
        IActor focusedActor;

        public bool Visible { get; set; }
        public bool MouseInBounds { get {return true;} }

        public Scene ParentScene { get; set; }

        public Scene() {
            this.Actors = new ActorCollection(this);

            Visible = true;
        }

        public void SubscribeToEvents(IInputHelper inputHelper) {
            inputHelper.MouseButtonPressed += inputHelper_MouseButtonPressed;
            inputHelper.MouseButtonReleased += inputHelper_MouseButtonReleased;
            inputHelper.MouseMoved += inputHelper_MouseMoved;

            inputHelper.KeyPressed += inputHelper_KeyPressed;
            inputHelper.KeyReleased += inputHelper_KeyReleased;
        }

        void inputHelper_KeyReleased(object sender, KeyEventArgs e) {
            HandleKeyReleased(e);
        }

        void inputHelper_KeyPressed(object sender, KeyEventArgs e) {
            HandleKeyPressed(e);
        }

        void inputHelper_MouseMoved(object sender, MouseMovedEventArgs e) {
            HandleMouseMoved(e);
        }

        void inputHelper_MouseButtonReleased(object sender, MouseButtonEventArgs e) {
            HandleMouseButtonUp(e);
        }

        void inputHelper_MouseButtonPressed(object sender, MouseButtonEventArgs e) {
            HandleMouseButtonDown(e);
        }

        public void Tick(IWindow window, TickEventArgs e) {
            foreach (ITickable actor in GetActors<ITickable>(0, Actors.Count)) {
                if (actor.Visible) {
                    actor.Tick(window, e);
                }
            }
        }

        public void Draw(IRenderTarget renderTarget) {
            foreach (IDrawable actor in GetActors<IDrawable>(0, Actors.Count)) {
                if (actor.Visible) {
                    actor.Draw(renderTarget);
                }
            }
        }

        private IEnumerable<T> GetActors<T>(int startIndex, int length) where T : class {
            for (int i = startIndex; i < startIndex + length; i++) {
                if (i >= Actors.Count) {
                    break;
                }
                T actor = Actors[i] as T;
                if (actor != null) {
                    yield return actor;
                }
            }
        }

        public void HandleMouseButtonDown(MouseButtonEventArgs e) {
            bool focusFound = false;
            for (int i = Actors.Count - 1; i >= 0; i--) {
                if (Actors[i].Visible) {
                    IBoundObject boundActor = Actors[i] as IBoundObject;
                    if (boundActor != null) {
                        if (boundActor.Bounds.ContainsPoint(e.X, e.Y)) {
                            boundActor.HandleMouseButtonDown(e);

                            IFocusable focusableActor = Actors[i] as IFocusable;
                            if (focusableActor != null) {
                                if (focusableActor.CanAcquireFocus) {
                                    this.focusedActor = Actors[i];
                                    focusFound = true;
                                }
                            }
                            break;
                        }
                    }
                }
            }

            if (focusFound == false) {
                this.focusedActor = null;
            }
        }

        public void HandleMouseButtonUp(MouseButtonEventArgs e) {
            for (int i = Actors.Count - 1; i >= 0; i--) {
                if (Actors[i].Visible) {
                    IBoundObject boundActor = Actors[i] as IBoundObject;
                    if (boundActor != null) {
                        if (boundActor.Bounds.ContainsPoint(e.X, e.Y)) {
                            boundActor.HandleMouseButtonUp(e);
                            break;
                        }
                    }
                }
            }
        }

        public void HandleMouseMoved(MouseMovedEventArgs e) {
            foreach (IBoundObject actor in GetActors<IBoundObject>(0, Actors.Count)) {
                if (actor.Visible) {
                    if (actor.Bounds.ContainsPoint(e.X, e.Y)) actor.HandleMouseMoved(e);
                    else if (actor.MouseInBounds) actor.HandleMouseOut(e);
                }
                
            }
        }

        public void HandleMouseOut(MouseMovedEventArgs e) {

        }

        public void HandleKeyPressed(KeyEventArgs e) {
            if (this.focusedActor != null && this.focusedActor.Visible) {
                IKeyboardEventHandler keyboardHandler = this.focusedActor as IKeyboardEventHandler;
                if (keyboardHandler != null) {
                    keyboardHandler.HandleKeyPressed(e);
                }
            } else {
                foreach (IKeyboardEventHandler keyboardHandler in GetActors<IKeyboardEventHandler>(0, Actors.Count)) {
                    if (keyboardHandler.Visible && !keyboardHandler.RequiresFocus) {
                        keyboardHandler.HandleKeyPressed(e);
                    }
                }
            }
        }

        public void HandleKeyReleased(KeyEventArgs e) {
            if (this.focusedActor != null && this.focusedActor.Visible) {
                IKeyboardEventHandler keyboardHandler = this.focusedActor as IKeyboardEventHandler;
                if (keyboardHandler != null) {
                    keyboardHandler.HandleKeyReleased(e);
                }
            } else {
                foreach (IKeyboardEventHandler keyboardHandler in GetActors<IKeyboardEventHandler>(0, Actors.Count)) {
                    if (keyboardHandler.Visible && !keyboardHandler.RequiresFocus) {
                        keyboardHandler.HandleKeyReleased(e);
                    }
                }
            }
        }

        public Rectangle Bounds {
            get {
                return new Rectangle(0, 0, 99999, 99999);
            }
            set { }
        }


        public bool CanAcquireFocus {
            get {
                return false;
            }
            set { }
        }

        public bool RequiresFocus {
            get { return false; }
        }
    }
}
