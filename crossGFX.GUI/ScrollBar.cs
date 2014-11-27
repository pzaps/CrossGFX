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

namespace crossGFX.GUI {
    public class ScrollBar : Control {

        public override Skinnable DefaultSkinnable {
            get{
                if(Type == ScrollBarType.Horizontal) return GUI.Instance.Skin.ScrollBarHorizontal;
                else return GUI.Instance.Skin.ScrollBarVertical;
            }
        }
        
        public override Boolean IsHoverable {get{return true;}}
        public override Boolean IsDownable {get{return true;}}

        public enum ScrollBarType { Horizontal, Vertical }

        public ScrollBarType Type { get; private set; }
        public Control Target { get; set; }

        ScrollBarThumb thumb;

        public ScrollBar(ScrollBarType type) {
            this.Type = type;
            ScrollBarThumb thumb = new ScrollBarThumb(type);
            this.Add(thumb);
            this.thumb = thumb;
        }

        public override bool HandleMouseButtonDown(Input.MouseButtonEventArgs e) {
            base.HandleMouseButtonDown(e);
            if(this.Enabled) {
                if(this.focusedActor == null) {
                    if(this.Type == ScrollBarType.Horizontal) {
                        this.Scroll((this.thumb.GlobalLocation.X > e.X ? 1 : -1) * this.Target.Bounds.Width);
                    } else {
                        this.Scroll((this.thumb.GlobalLocation.Y > e.Y ? 1 : -1) * this.Target.Bounds.Height);
                    }
                    this.ResetThumb();
                }
            }
            return false;
        }

        public override void HandleKeyPressed(Input.KeyEventArgs e) {
            bool hor = this.Type == ScrollBarType.Horizontal;
            int targetBound = hor ? this.Target.Bounds.Width : this.Target.Bounds.Height;
            int maxSize = hor ? this.Target.MaxSize.Width : this.Target.MaxSize.Height;
            if(maxSize > targetBound) {
                int offset = hor ? this.Target.Offset.X : this.Target.Offset.Y;
                switch (e.Code)
                {
                    case Input.Key.Up:
                    case Input.Key.Left:
                        {
                            offset = Math.Min(offset + this.StepSize, 0);
                        }
                        break;
                    case Input.Key.Down:
                    case Input.Key.Right:
                        {
                            offset = Math.Max(offset - this.StepSize, targetBound - maxSize);
                        }
                        break;
                    case Input.Key.PageUp:
                        {
                            offset = Math.Min(0, offset + targetBound);
                        }
                        break;
                    case Input.Key.PageDown:
                        {
                            offset = Math.Max(targetBound - maxSize, offset - targetBound);
                        }
                        break;
                    case Input.Key.Home:
                        {
                            offset = 0;
                        }
                        break;
                    case Input.Key.End:
                        {
                            offset = targetBound - maxSize;
                        }
                        break;
                }
                this.Target.Offset = new Point(hor ? offset : this.Target.Offset.X, hor ? this.Target.Offset.Y : offset);
            }
        }
        
        Point lastOffset;
        Size lastMaxSize;

        public override void Tick(IWindow window, TickEventArgs e) {
            if(this.Target == null) return;
            if(this.lastOffset != this.Target.Offset) {
                this.ResetThumb();
                this.lastOffset = this.Target.Offset;
            }
            if(this.lastMaxSize != this.Target.MaxSize) {
                this.ResetThumb();
                this.lastMaxSize = this.Target.MaxSize;
            }
        }

        public void Scroll(int offset) {
            bool hor = this.Type == ScrollBarType.Horizontal;
            int targetBound = hor ? this.Target.Bounds.Width : this.Target.Bounds.Height;
            int targetOffset = hor ? this.Target.Offset.X : this.Target.Offset.Y;
            int maxSize = hor ? this.Target.MaxSize.Width : this.Target.MaxSize.Height;
            int axis = Math.Min(0, Math.Max(targetBound - maxSize, targetOffset + offset));
            this.Target.Offset = new Point(hor ? axis : this.Target.Offset.X, hor ? this.Target.Offset.Y : axis);
            this.ResetThumb();
        }

        public void ResetThumb() {
            if(Target != null) {
                bool hor = this.Type == ScrollBarType.Horizontal;
                int bound = hor ? this.Bounds.Width : this.Bounds.Height;
                int targetBound = hor ? this.Target.Bounds.Width : this.Target.Bounds.Height;
                int offset = hor ? this.Target.Offset.X : this.Target.Offset.Y;
                int maxSize = hor ? this.Target.MaxSize.Width : this.Target.MaxSize.Height;
                int axis = (int)Math.Round((double) targetBound / maxSize * -offset);
                int size = Math.Min(bound, (int)Math.Round((double) targetBound / maxSize * bound));
                axis = Math.Max(0, Math.Min(bound - size, axis));
                
                thumb.Bounds = new Rectangle(hor ? axis : 0, hor ? 0 : axis,
                    hor ? size : this.bounds.Width, hor ? this.bounds.Height : size);
            } else thumb.Bounds = new Rectangle(0, 0, this.bounds.Width, this.bounds.Height);
        }

        public int StepSize { get; set; }

        public void ScrollBack(object sender, Input.MouseButtonEventArgs e) {
            this.Scroll(StepSize);
        }

        public void ScrollForward(object sender, Input.MouseButtonEventArgs e) {
            this.Scroll(-StepSize);
        }

        public override Rectangle Bounds {
            get { return bounds; }
            set {
                this.bounds = value;
                this.StretchRegions();
                this.ResetThumb();
            }
        }

        public override Size Size {
            get { return bounds.Size; }
            set {
                this.bounds.Width = value.Width;
                this.bounds.Height = value.Height;
                this.StretchRegions(); 
                this.ResetThumb();
            }
        }

        public override bool Enabled { 
            get{ return this.State != ControlState.Disabled; }
            set{
                if(value) {
                    this.State = ControlState.Base;
                } else {
                    this.State = ControlState.Disabled;
                    thumb.Enabled = false;
                }
            } 
        }

        public Button CreateButtonUp() {
            Button button = new Button();
            button.Bounds = new Rectangle(this.Bounds.X, this.Bounds.Y - 15, this.Bounds.Width, this.Bounds.Width);
            button.CustomSkinnable = GUI.Instance.Skin.ScrollBarButtonUp;
            button.Click += new EventHandler<crossGFX.Input.MouseButtonEventArgs>(this.ScrollBack);
            return button;
        }

        public Button CreateButtonDown() {
            Button button = new Button();
            button.Bounds = new Rectangle(this.Bounds.X, this.Bounds.Y + this.Bounds.Height, this.Bounds.Width, this.Bounds.Width);
            button.CustomSkinnable = GUI.Instance.Skin.ScrollBarButtonDown;
            button.Click += new EventHandler<crossGFX.Input.MouseButtonEventArgs>(this.ScrollForward);
            return button;
        }

        public Button CreateButtonLeft() {
            Button button = new Button();
            button.Bounds = new Rectangle(this.Bounds.X, this.Bounds.Y - 15, this.Bounds.Height, this.Bounds.Height);
            button.CustomSkinnable = GUI.Instance.Skin.ScrollBarButtonLeft;
            button.Click += new EventHandler<crossGFX.Input.MouseButtonEventArgs>(this.ScrollBack);
            return button;
        }

        public Button CreateButtonRight() {
            Button button = new Button();
            button.Bounds = new Rectangle(this.Bounds.X + this.Bounds.Width, this.Bounds.Y, this.Bounds.Height, this.Bounds.Height);
            button.CustomSkinnable = GUI.Instance.Skin.ScrollBarButtonRight;
            button.Click += new EventHandler<crossGFX.Input.MouseButtonEventArgs>(this.ScrollForward);
            return button;
        }
    }
}
