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
using crossGFX.Input;

namespace crossGFX
{
    public class ActorContainer : IDrawable, IBoundObject, IKeyboardEventHandler, IMouseEventHandler, IFocusable
    {

        protected List<IActor> actors { get; set; }
        protected IActor focusedActor;
        public ActorContainer Parent { get; set; }

        public KeyData KeyData { get; private set; }

        public int KeyRepeatRate { get; set; }
        public int KeyRepeatDelay { get; set; }

        public ActorContainer() {
            this.actors = new List<IActor>();
            this.KeyData = new KeyData();

            this.Visible = true;
        }

        public virtual void Tick(IWindow window, TickEventArgs e) {
            for (int i = 0; i < actors.Count; i++) {
                if (actors[i].Visible) {
                    ITickable tickableActor = actors[i] as ITickable;
                    if (tickableActor != null) {
                        tickableActor.Tick(window, e);
                    }

                    IKeyboardEventHandler keyActor = actors[i] as IKeyboardEventHandler;
                    if (keyActor != null) {
                        HandleKeyRepeat(keyActor, e.Tick);
                    }
                }
            }
        }

        private void HandleKeyRepeat(IKeyboardEventHandler keyEventActor, uint tickCount) {
            for (int i = 0; i < (int)Input.Key.KeyCount; i++) {
                if (keyEventActor.KeyData.KeyState[i] && tickCount > keyEventActor.KeyData.NextRepeat[i]) {
                    keyEventActor.KeyData.NextRepeat[i] = tickCount + keyEventActor.KeyRepeatRate;
                    keyEventActor.HandleKeyPressed(new KeyEventArgs((Key)i, true, keyEventActor.KeyData.IsControlDown(), keyEventActor.KeyData.IsShiftDown(), keyEventActor.KeyData.IsAltDown()));
                }
            }
        }

        public virtual void Draw(IRenderTarget renderTarget) {
            Point point = new Point(this.Bounds.X, this.Bounds.Y);
            renderTarget.Origin += point;
            foreach (IDrawable actor in this.GetActors<IDrawable>(0, this.actors.Count)) {
                if (actor.Visible) {
                    actor.Draw(renderTarget);
                }
            }
            renderTarget.Origin -= point;
        }

        public virtual bool HandleMouseButtonDown(MouseButtonEventArgs e) {
            this.LoseFocus();

            foreach (IBoundObject actor in this.GetActors<IBoundObject>(this.actors.Count - 1, -this.actors.Count)) {
                if (actor.Visible) {
                    if (actor.Bounds.ContainsPoint(e.X - this.GlobalLocation.X, e.Y - this.GlobalLocation.Y)) {
                        actor.HandleMouseButtonDown(e);

                        IFocusable focusableActor = actor as IFocusable;
                        if (focusableActor != null) {
                            if (focusableActor.CanAcquireFocus) {
                                this.focusedActor = actor;
                                focusableActor.HasFocus = true;
                            }
                        }
                        return true;
                    }
                }
            }
            return false;
        }

        public virtual bool HandleMouseButtonUp(MouseButtonEventArgs e) {
            foreach (IBoundObject actor in this.GetActors<IBoundObject>(this.actors.Count - 1, -this.actors.Count)) {
                if (actor.Visible) {
                    if (actor.Bounds.ContainsPoint(e.X - this.GlobalLocation.X, e.Y - this.GlobalLocation.Y)) {
                        actor.HandleMouseButtonUp(e);
                        return true;
                    }
                }
            }
            return false;
        }

        public virtual bool HandleMouseMoved(MouseMovedEventArgs e) {
            this.MouseInBounds = true;
            bool passed = false;
            foreach (IBoundObject actor in this.GetActors<IBoundObject>(this.actors.Count - 1, -this.actors.Count)) {
                if (actor.Visible) {
                    if (!passed && actor.Bounds.ContainsPoint(e.X - this.GlobalLocation.X, e.Y - this.GlobalLocation.Y)) {
                        actor.HandleMouseMoved(e);
                        passed = true;
                    } else if (actor.MouseInBounds) actor.HandleMouseOut(e);
                }

            }
            return passed;
        }

        public virtual bool HandleMouseOut(MouseMovedEventArgs e) {
            this.MouseInBounds = false;
            foreach (IBoundObject actor in this.GetActors<IBoundObject>(this.actors.Count - 1, -this.actors.Count)) {
                if (actor.Visible && actor.MouseInBounds) {
                    actor.HandleMouseOut(e);
                    return true;
                }
            }
            return false;
        }

        public virtual void HandleKeyPressed(KeyEventArgs e) {
            if (this.focusedActor != null && this.focusedActor.Visible) {
                IKeyboardEventHandler keyboardHandler = this.focusedActor as IKeyboardEventHandler;
                if (keyboardHandler != null) {
                    if (keyboardHandler.KeyData.KeyState[(int)e.Code] == false) {
                        keyboardHandler.KeyData.KeyState[(int)e.Code] = true;
                        keyboardHandler.KeyData.NextRepeat[(int)e.Code] = DriverManager.ActiveDriver.System.GetTickCount() + keyboardHandler.KeyRepeatDelay;
                        keyboardHandler.HandleKeyPressed(e);
                    }
                }
            } else {
                foreach (IKeyboardEventHandler keyboardHandler in this.GetActors<IKeyboardEventHandler>(0, this.actors.Count)) {
                    if (keyboardHandler.Visible && !keyboardHandler.RequiresFocus) {
                        if (keyboardHandler.KeyData.KeyState[(int)e.Code] == false) {
                            keyboardHandler.KeyData.KeyState[(int)e.Code] = true;
                            keyboardHandler.KeyData.NextRepeat[(int)e.Code] = DriverManager.ActiveDriver.System.GetTickCount() + keyboardHandler.KeyRepeatDelay;

                            keyboardHandler.HandleKeyPressed(e);
                        }
                    }
                }
            }
        }

        public virtual void HandleKeyReleased(KeyEventArgs e) {
            if (this.focusedActor != null && this.focusedActor.Visible) {
                IKeyboardEventHandler keyboardHandler = this.focusedActor as IKeyboardEventHandler;
                if (keyboardHandler != null) {
                    keyboardHandler.KeyData.KeyState[(int)e.Code] = false;

                    keyboardHandler.HandleKeyReleased(e);
                }
            } else {
                foreach (IKeyboardEventHandler keyboardHandler in this.GetActors<IKeyboardEventHandler>(0, this.actors.Count)) {
                    if (keyboardHandler.Visible && !keyboardHandler.RequiresFocus) {
                        keyboardHandler.KeyData.KeyState[(int)e.Code] = false;

                        keyboardHandler.HandleKeyReleased(e);
                    }
                }
            }
        }

        protected IEnumerable<T> GetActors<T>(int startIndex, int length) where T : class {
            for (int i = startIndex; i != startIndex + length; i += (length >= 0 ? 1 : -1)) {
                if (i >= this.actors.Count || i < 0) break;
                T actor = this.actors[i] as T;
                if (actor != null) {
                    yield return actor;
                }
            }
        }

        public virtual void Add(IActor actor) {
            actor.Parent = this;
            this.actors.Add(actor);
        }

        public virtual void Remove(IActor actor) {
            actor.Parent = null;
            this.actors.Remove(actor);
        }

        public IActor this[int index] {
            get { return this.actors[index]; }
        }

        public int Count {
            get { return this.actors.Count; }
        }

        public int IndexOf(IActor actor) {
            return this.actors.IndexOf(actor);
        }

        public void Move(int oldIndex, int newIndex) {
            IActor actor = this.actors[oldIndex];
            this.actors.RemoveAt(oldIndex);
            //if (newIndex > oldIndex) newIndex--; 
            this.actors.Insert(newIndex, actor);
        }

        protected void LoseFocus() {
            IFocusable focusableActor = focusedActor as IFocusable;
            if (focusableActor != null) {
                focusableActor.HasFocus = false;
                ActorContainer actorContainer = focusedActor as ActorContainer;
                if (actorContainer != null) {
                    actorContainer.LoseFocus();
                }
                this.focusedActor = null;
            }
        }

        public virtual Rectangle Bounds { get; set; }

        public Point GlobalLocation {
            get {
                Point location = new Point(this.Bounds.X, this.Bounds.Y);
                if (this.Parent != null)
                    location += this.Parent.GlobalLocation;
                return location;
            }
        }

        public bool Visible { get; set; }

        public bool MouseInBounds { get; protected set; }

        public bool HasFocus { get; set; }

        public bool CanAcquireFocus { get; protected set; }

        public bool RequiresFocus { get; protected set; }



    }
}
