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
using crossGFX.GUI.Skins;
using crossGFX.Input;

namespace crossGFX.GUI
{

    /// <summary>
    /// The base class for all controls. 
    /// </summary>
    /// <remarks>
    /// Inheritance for attributes is avoided to keep everything simple.
    /// While you can use inappropriate attributes, that won't do anything.
    /// (i.e. TextBox.TextureBaseActive = ITexture or TextBox.Active = true)
    /// </remarks>

    public abstract class Control : Component, IDrawable, IBoundObject, IKeyboardEventHandler, IMouseEventHandler, IFocusable
    {
        public delegate void ReturnHandler(string s);

        public event EventHandler<Input.MouseButtonEventArgs> Click;
        public event ReturnHandler Return;

        public Skinnable Skinnable {
            get {
                return this.CustomSkinnable != null ? this.CustomSkinnable : this.DefaultSkinnable;
            }
        }
        public virtual Skinnable DefaultSkinnable {
            get { return null; }
        }
        Skinnable customSkinnable;
        public Skinnable CustomSkinnable {
            get { return customSkinnable; }
            set {
                customSkinnable = value;
                this.ResetRegionsStretched();
            }
        }

        public enum ControlState { Base, Disabled, Hover, Down }

        public ControlState State { get; protected set; }

        public virtual Boolean IsHoverable { get { return false; } }
        public virtual Boolean IsDownable { get { return false; } }
        public virtual Boolean IsActivable { get { return false; } }

        public ITexture TextureBase { get; set; }
        public ITexture TextureBaseActive { get; set; }
        public ITexture TextureDisabled { get; set; }
        public ITexture TextureDisabledActive { get; set; }
        public ITexture TextureHover { get; set; }
        public ITexture TextureHoverActive { get; set; }
        public ITexture TextureDown { get; set; }
        public ITexture TextureDownActive { get; set; }


        public Control() {
            this.CanAcquireFocus = true;
            this.RequiresFocus = true;
            this.Enabled = true;
            this.ResetRegionsStretched();

            KeyRepeatRate = 60;
            KeyRepeatDelay = 200;
        }

        public override bool HandleMouseButtonDown(Input.MouseButtonEventArgs e) {
            bool passed = false;
            if (this.Enabled) {
                passed = base.HandleMouseButtonDown(e);
                if (!passed) {
                    if (this.IsDownable) {
                        this.State = ControlState.Down;
                    }
                }
            }
            return passed;
        }

        public override bool HandleMouseButtonUp(Input.MouseButtonEventArgs e) {
            bool passed = false;
            if (this.Enabled) {
                passed = base.HandleMouseButtonUp(e);
                if (this.IsActivable) this.Active = !this.Active;
                if (!passed) {
                    if (Click != null) Click(this, e);
                    if (this.State == ControlState.Down) {
                        this.State = ControlState.Hover;
                    }
                }
            }
            return passed;
        }

        public override bool HandleMouseMoved(Input.MouseMovedEventArgs e) {
            bool passed = false;
            if (this.Enabled) {
                passed = base.HandleMouseMoved(e);
                if (!passed) {
                    if (this.IsHoverable)
                        if (this.State != ControlState.Down)
                            this.State = ControlState.Hover;
                } else {
                    this.State = ControlState.Base;
                }
            }
            return passed;
        }

        public override bool HandleMouseOut(Input.MouseMovedEventArgs e) {
            if (this.Enabled) {
                this.State = ControlState.Base;
                return base.HandleMouseOut(e);
            }
            return false;
        }

        Rectangle[] subRegionsStretched;

        protected void ResetRegionsStretched() {
            if (Skinnable != null) {
                this.subRegionsStretched = new Rectangle[Skinnable.SubRegions.Length];
                if (subRegionsStretched.Length == 9) {
                    for (int i = 1; i < 9; i++) {
                        if (i % 2 == 1) { // corners
                            subRegionsStretched[i].Width = Skinnable.SubRegions[i].Width;
                            subRegionsStretched[i].Height = Skinnable.SubRegions[i].Height;
                        } else if (i % 4 == 2) { // horizontals
                            subRegionsStretched[i].X = Skinnable.SubRegions[i].X;
                            subRegionsStretched[i].Height = Skinnable.SubRegions[i].Height;
                        } else if (i % 4 == 0) { // verticals
                            subRegionsStretched[i].Y = Skinnable.SubRegions[i].Y;
                            subRegionsStretched[i].Width = Skinnable.SubRegions[i].Width;
                        }
                    }
                    subRegionsStretched[0].X = subRegionsStretched[1].Width;
                    subRegionsStretched[0].Y = subRegionsStretched[1].Height;
                    subRegionsStretched[2].X = Skinnable.SubRegions[2].X;
                    subRegionsStretched[4].Y = Skinnable.SubRegions[4].Y;
                    subRegionsStretched[6].X = Skinnable.SubRegions[6].X;
                    subRegionsStretched[8].Y = Skinnable.SubRegions[1].Height;
                }
            }

            this.StretchRegions();
        }

        protected void StretchRegions() {
            if (subRegionsStretched != null && subRegionsStretched.Length == 9) {
                // center
                subRegionsStretched[0].Width = Bounds.Width - subRegionsStretched[1].Width - subRegionsStretched[3].Width;
                subRegionsStretched[0].Height = Bounds.Height - subRegionsStretched[1].Height - subRegionsStretched[7].Height;
                // top
                subRegionsStretched[2].Width = subRegionsStretched[0].Width;
                // top-right corner
                subRegionsStretched[3].X = Bounds.Width - subRegionsStretched[3].Width;
                // right
                subRegionsStretched[4].X = Bounds.Width - subRegionsStretched[4].Width;
                subRegionsStretched[4].Height = subRegionsStretched[0].Height;
                // bottom-right corner
                subRegionsStretched[5].X = Bounds.Width - subRegionsStretched[5].Width;
                subRegionsStretched[5].Y = Bounds.Height - subRegionsStretched[5].Height;
                // bottom
                subRegionsStretched[6].Y = Bounds.Height - subRegionsStretched[6].Height;
                subRegionsStretched[6].Width = subRegionsStretched[0].Width;
                // bottom-left corner
                subRegionsStretched[7].Y = Bounds.Height - subRegionsStretched[7].Height;
                // left
                subRegionsStretched[8].Height = subRegionsStretched[0].Height;
            }
        }

        public override void Tick(IWindow window, TickEventArgs e) {
            base.Tick(window, e);
        }

        public override void HandleKeyPressed(KeyEventArgs e) {
            base.HandleKeyPressed(e);
        }

        public override void Draw(IRenderTarget renderTarget) {
            renderTarget.Origin += this.Bounds.Location;
            renderTarget.StartClip(new Rectangle(0, 0, this.Bounds.Width, this.Bounds.Height), 1);

            this.DrawUnder(renderTarget);
            this.DrawMiddle(renderTarget);
            this.DrawOver(renderTarget);

            renderTarget.EndClip();
            renderTarget.Origin -= this.Bounds.Location;

            base.Draw(renderTarget);
        }

        public virtual void DrawUnder(IRenderTarget renderTarget) {
            if (this.BackgroundColor != null)
                renderTarget.Fill(new Rectangle(0, 0, this.Bounds.Width, this.Bounds.Height), this.BackgroundColor.Value);
            if (Skinnable != null) {
                Rectangle region;
                ITexture texture;
                switch (State) {
                    case ControlState.Base:
                        if (!this.Active) {
                            region = Skinnable.TextureBaseRegion;
                            texture = this.TextureBase;
                        } else {
                            region = Skinnable.TextureBaseActiveRegion;
                            texture = this.TextureBaseActive;
                        } break;
                    case ControlState.Disabled:
                        if (!this.Active) {
                            region = Skinnable.TextureDisabledRegion;
                            texture = this.TextureDisabled;
                        } else {
                            region = Skinnable.TextureDisabledActiveRegion;
                            texture = this.TextureDisabledActive;
                        } break;
                    case ControlState.Hover:
                        if (!this.Active) {
                            region = Skinnable.TextureHoverRegion;
                            texture = this.TextureHover;
                        } else {
                            region = Skinnable.TextureHoverActiveRegion;
                            texture = this.TextureHoverActive;
                        } break;
                    case ControlState.Down:
                        if (!this.Active) {
                            region = Skinnable.TextureDownRegion;
                            texture = this.TextureDown;
                        } else {
                            region = Skinnable.TextureDownActiveRegion;
                            texture = this.TextureDownActive;
                        } break;
                    default:
                        region = new Rectangle(0, 0, 0, 0);
                        texture = null;
                        break;
                }
                if (texture != null) {
                    region = new Rectangle(0, 0, texture.Width, texture.Height);
                } else {
                    texture = GUI.Instance.Skin.SkinTexture.Texture;
                }

                if (Skinnable.SubRegions.Length != 1) {
                    for (int i = 0; i < Skinnable.SubRegions.Length; i++) {
                        renderTarget.DrawStretched(texture, subRegionsStretched[i], new Rectangle(
                                region.X + Skinnable.SubRegions[i].X, region.Y + Skinnable.SubRegions[i].Y,
                                Skinnable.SubRegions[i].Width, Skinnable.SubRegions[i].Height));
                    }
                } else {
                    renderTarget.DrawStretched(texture, new Rectangle(0, 0, Bounds.Width, Bounds.Height), region);
                }


            }
        }

        public virtual void DrawMiddle(IRenderTarget renderTarget) {
        }

        public virtual void DrawOver(IRenderTarget renderTarget) {
        }

        protected Rectangle bounds;
        public override Rectangle Bounds {
            get { return bounds; }
            set {
                this.bounds = value;
                this.StretchRegions();
            }
        }

        public virtual Point Location {
            get { return bounds.Location; }
            set {
                this.bounds.X = value.X;
                this.bounds.Y = value.Y;
                this.StretchRegions();
            }
        }

        public virtual Size Size {
            get { return bounds.Size; }
            set {
                this.bounds.Width = value.Width;
                this.bounds.Height = value.Height;
                this.StretchRegions();
            }
        }

        public Color? BackgroundColor { get; set; }

        /// <summary>
        /// Offset is only used in specialized controls.
        /// </summary>
        public Point Offset { get; set; }
        /// <summary>
        /// MaxSize is only used when contents of the control can take more space than the bounds.
        /// Otherwise it's never changed or used anywhere.
        /// </summary>
        public Size MaxSize { get; set; }

        public virtual bool Enabled {
            get { return this.State != ControlState.Disabled; }
            set {
                if (value)
                    if (MouseInBounds) this.State = ControlState.Hover;
                    else this.State = ControlState.Base;
                else this.State = ControlState.Disabled;
            }
        }


        bool active;

        public bool Active {
            get { return active; }
            set {
                if (this.IsActivable) {
                    active = value;
                }
            }
        }

        public void MoveUp() {
            if (this.Parent == null) return;
            int index = this.Parent.IndexOf(this);
            if (index < this.Parent.Count - 1) this.Parent.Move(index, index + 1);
        }

        public void MoveDown() {
            if (this.Parent == null) return;
            int index = this.Parent.IndexOf(this);
            if (index > 0) this.Parent.Move(index, index - 1);
        }

        public void MoveTop() {
            if (this.Parent == null) return;
            int index = this.Parent.IndexOf(this);
            this.Parent.Move(index, this.Parent.Count - 1);
        }

        public void MoveBottom() {
            if (this.Parent == null) return;
            int index = this.Parent.IndexOf(this);
            this.Parent.Move(index, 0);
        }

        protected bool OnClick(object sender, Input.MouseButtonEventArgs e) {
            if (this.Click != null) {
                this.Click(sender, e);
                return true;
            }
            return false;
        }

        protected bool OnReturn(string s) {
            if (this.Return != null) {
                this.Return(s);
                return true;
            }
            return false;
        }
    }
}
