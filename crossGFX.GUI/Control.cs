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

    public class Control : Component, IDrawable, IBoundObject, IKeyboardEventHandler, IMouseEventHandler, IFocusable
    {
        public event EventHandler<Input.MouseButtonEventArgs> Click;

        public virtual Skinnable Skinnable{get{return null;}}

        public virtual Boolean IsHoverable {get{return false;}}
        public virtual Boolean IsDownable {get{return false;}}
        public virtual Boolean IsActivable {get{return false;}}

        public enum ControlState { Base, Disabled, Hover, Down }

        public ControlState State { get; protected set; }

        
        public ITexture TextureBase { get; set; }
        public ITexture TextureBaseActive { get; set; }
        public ITexture TextureDisabled { get; set; }
        public ITexture TextureDisabledActive { get; set; }
        public ITexture TextureHover { get; set; }
        public ITexture TextureHoverActive { get; set; }
        public ITexture TextureDown { get; set; }
        public ITexture TextureDownActive { get; set; }

        public bool CanAcquireFocus { get; set; }
        public bool MouseInBounds { get; private set; }
        

        public Control() {
            this.CanAcquireFocus = true;
            this.RequiresFocus = true;
            this.Enabled = true;
            if(Skinnable != null) {
                this.subRegionsStretched = new Rectangle[Skinnable.SubRegions.Length];
                if(subRegionsStretched.Length == 9) {
                    for(int i=1;i<9;i++) {
                        if(i % 2 == 1) { // corners
                            subRegionsStretched[i].Width = Skinnable.SubRegions[i].Width;
                            subRegionsStretched[i].Height = Skinnable.SubRegions[i].Height;
                        } else if(i % 4 == 2) { // horizontals
                            subRegionsStretched[i].X = Skinnable.SubRegions[i].X;
                            subRegionsStretched[i].Height = Skinnable.SubRegions[i].Height;
                        } else if(i % 4 == 0) { // verticals
                            subRegionsStretched[i].Y = Skinnable.SubRegions[i].Y;
                            subRegionsStretched[i].Width = Skinnable.SubRegions[i].Width;
                        }
                        subRegionsStretched[0].X = subRegionsStretched[1].Width;
                        subRegionsStretched[0].Y = subRegionsStretched[1].Height;
                        subRegionsStretched[2].X = Skinnable.SubRegions[2].X;
                        subRegionsStretched[4].Y = Skinnable.SubRegions[4].Y;
                        subRegionsStretched[6].X = Skinnable.SubRegions[6].X;
                        subRegionsStretched[8].Y = Skinnable.SubRegions[1].Height;
                    }
                }
            }
        }

        /*public override void Tick(IWindow window, TickEventArgs e) {
            base.Tick(window, e);
        }*/

        public virtual void HandleMouseButtonDown(Input.MouseButtonEventArgs e) {
            if(this.Enabled)
                if (this.IsDownable) {
                    this.State = ControlState.Down;
                }
        }

        public virtual void HandleMouseButtonUp(Input.MouseButtonEventArgs e) {
            if(this.Enabled) {
                if(this.State == ControlState.Down) {
                    if (Click != null) Click(this, e);
                    this.State = ControlState.Hover;
                }
                
            }
        }

        public virtual void HandleMouseMoved(Input.MouseMovedEventArgs e) {
            if(this.Enabled)
                if (this.IsHoverable) 
                    if (this.State != ControlState.Down)
                        this.State = ControlState.Hover;
                
            MouseInBounds = true;

        }

        public virtual void HandleMouseOut(Input.MouseMovedEventArgs e) {
            if(this.Enabled) this.State = ControlState.Base;
            MouseInBounds = false;
        }

        public virtual void HandleKeyPressed(KeyEventArgs e) {
        }

        public virtual void HandleKeyReleased(KeyEventArgs e) {
        }

        Rectangle[] subRegionsStretched;

        void StretchRegions() {
            if(subRegionsStretched != null && subRegionsStretched.Length == 9) {
                // center
                subRegionsStretched[0].Width = Bounds.Width - subRegionsStretched[1].Width - subRegionsStretched[3].Width;
                subRegionsStretched[0].Height = Bounds.Height - subRegionsStretched[1].Height - subRegionsStretched[7].Height;
                // top
                subRegionsStretched[2].Width = subRegionsStretched[0].Width;
                // top-right corner
                subRegionsStretched[3].X = Bounds.Width - Skinnable.SubRegions[3].Width;
                // right
                subRegionsStretched[4].X = Bounds.Width - Skinnable.SubRegions[4].Width;
                subRegionsStretched[4].Height = subRegionsStretched[0].Height;
                // bottom-right corner
                subRegionsStretched[5].X = Bounds.Width - Skinnable.SubRegions[5].Width;
                subRegionsStretched[5].Y = Bounds.Height - Skinnable.SubRegions[5].Height;
                // bottom
                subRegionsStretched[6].Y = Bounds.Height - subRegionsStretched[6].Height;
                subRegionsStretched[6].Width = subRegionsStretched[0].Width;
                // bottom-left corner
                subRegionsStretched[7].Y = Bounds.Height - Skinnable.SubRegions[7].Height;
                // left
                subRegionsStretched[8].Height = subRegionsStretched[0].Height;
            }
        }

        public void Draw(IRenderTarget renderTarget) {
            renderTarget.Origin += Bounds.Location;
            renderTarget.StartClip(new Rectangle(0, 0, Bounds.Width, Bounds.Height), 1);

            DrawUnder(renderTarget);
            DrawMiddle(renderTarget);
            DrawOver(renderTarget);

            renderTarget.EndClip();
            renderTarget.Origin -= Bounds.Location;
        }

        public virtual void DrawUnder(IRenderTarget renderTarget) {
            if(Skinnable != null) {
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
                        if (!this.Active)  {
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
                if(texture != null) {
                    region = new Rectangle(0, 0, texture.Width, texture.Height);
                } else {
                    texture = GUI.Instance.Skin.SkinTexture.Texture;
                }
                
                if(Skinnable.SubRegions.Length != 1) {
                    for (int i = 0; i < Skinnable.SubRegions.Length; i++) {
                        renderTarget.DrawStretched(texture, subRegionsStretched[i], new Rectangle(
                                region.X + Skinnable.SubRegions[i].X, region.Y + Skinnable.SubRegions[i].Y,
                                Skinnable.SubRegions[i].Width, Skinnable.SubRegions[i].Height));
                    }
                } else {
                    renderTarget.DrawStretched(texture, new Rectangle(0, 0, Bounds.Width, Bounds.Height) , region);
                }


            }
        }

        public virtual void DrawMiddle(IRenderTarget renderTarget) {
        }

        public virtual void DrawOver(IRenderTarget renderTarget) {
        }


        public bool RequiresFocus { get; set; }

        Rectangle bounds;
        public Rectangle Bounds {
            get { return bounds; }
            set {
                bounds = value; 
                StretchRegions(); 
            }
        }

        public Point Location {
            get { return Bounds.Location; }
            set { 
                bounds.X = value.X;
                bounds.Y = value.Y;
                StretchRegions(); 
            }
        }

        public Size Size {
            get { return bounds.Size; }
            set {
                bounds.Width = value.Width;
                bounds.Height = value.Height;
                StretchRegions(); 
            }
        }

        public bool Enabled { 
            get{ return this.State != ControlState.Disabled; }
            set{
                if(value) 
                    if(MouseInBounds) this.State = ControlState.Hover; 
                    else this.State = ControlState.Base;
                else this.State = ControlState.Disabled;
            } 
        }

        bool active;

        public bool Active { 
            get{ return active; } 
            set{
                if(this.IsActivable) {
                    active = value;
                }
            } 
        }
    }
}
