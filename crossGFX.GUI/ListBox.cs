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

namespace crossGFX.GUI {
    public class ListBox<T> : Control {
        public enum SelectionMode { Disabled, Single, Multiple }

        public List<T> List { get; private set; }
        public RichString Style { get; private set; }
        public SelectionMode Mode { get; private set; }
        public List<int> SelectionIndices { get; private set; }
        
        public int CursorPosition { get; set; }
        public Color CursorBorderColor { get; set; }
        public int CursorBorderSize { get; set; }

        private Dictionary<int, string> labels;

        public ListBox(SelectionMode mode) {
            this.List = new List<T>();
            this.CursorPosition = -1;
            this.SelectionIndices = new List<int>();
            this.labels = new Dictionary<int, string>();
            this.Style = new RichString();
            this.Style.Font = GUI.Instance.Skin.Font;
            this.CursorBorderColor = Color.White;
            this.CursorBorderSize = 1;
            this.Mode = mode;
        }

        public override void DrawMiddle(IRenderTarget renderTarget) {
            Point point = Point.Empty;
            for (int i = 0; i < List.Count; i++) {
                if(point.Y + Offset.Y > Bounds.Height) break;

                Rectangle selectionRectangle = new Rectangle(point.X + Offset.X, point.Y + Offset.Y, this.Bounds.Width, Style.Size.Height);

                if(this.Mode == SelectionMode.Multiple && this.SelectionIndices.Contains(i)) {
                    renderTarget.Fill(selectionRectangle, 
                        new Color(CursorBorderColor.A / 2, CursorBorderColor.R, 
                            CursorBorderColor.G, CursorBorderColor.B));
                }

                if(this.Style.Font != null) {
                    point.X = 2;
                    renderTarget.DrawString(point + Offset, this.Style.Font, 
                        (labels.ContainsKey(i) ? labels[i] : List[i].ToString()), 
                        this.Style.TextSize, this.Style.Color, 
                        this.Style.Bold, this.Style.Italic, this.Style.Underline);
                    point.X = 0;
                }

                if(this.Mode != SelectionMode.Disabled && this.CursorPosition == i) {
                    renderTarget.DrawRectangle(selectionRectangle, CursorBorderColor, CursorBorderSize);
                }

                point.Y += Style.Size.Height;
                
            }
        }

        public override void HandleKeyPressed(Input.KeyEventArgs e) {
            if(this.Mode == SelectionMode.Disabled) return;
            if(this.List.Count == 0) return;
            switch (e.Code)
            {
                case Input.Key.Up:
                    {
                        if(this.CursorPosition > 0) {
                            this.CursorPosition--;
                            if(this.CursorPosition * this.Style.Size.Height + Offset.Y < 0) {
                                this.Offset = new Point(this.Offset.X, 
                                    -this.CursorPosition * this.Style.Size.Height);
                            }
                        }
                    }
                    break;
                case Input.Key.Down:
                    {
                        if(this.CursorPosition + 1 < this.List.Count) {
                            this.CursorPosition++;
                            if((this.CursorPosition + 1) * this.Style.Size.Height + Offset.Y > Bounds.Height) {
                                this.Offset = new Point(this.Offset.X, 
                                    (-1 - this.CursorPosition) * this.Style.Size.Height + Bounds.Height);
                            }
                        }
                    }
                    break;
                case Input.Key.PageUp:
                    {
                        int skip = this.Bounds.Height / this.Style.Size.Height;
                        if(this.CursorPosition > 0) {
                            this.CursorPosition = Math.Max(0, this.CursorPosition - skip);
                            if(this.CursorPosition * this.Style.Size.Height + Offset.Y < 0) {
                                this.Offset = new Point(this.Offset.X, 
                                    -this.CursorPosition * this.Style.Size.Height);
                            }
                        }
                    }
                    break;
                case Input.Key.PageDown:
                    {
                        int skip = this.Bounds.Height / this.Style.Size.Height;
                        if(this.CursorPosition + 1 < this.List.Count) {
                            this.CursorPosition = Math.Min(this.List.Count - 1, this.CursorPosition + skip);
                            if((this.CursorPosition + 1) * this.Style.Size.Height + Offset.Y > Bounds.Height) {
                                this.Offset = new Point(this.Offset.X, 
                                    (-1 - this.CursorPosition) * this.Style.Size.Height + Bounds.Height);
                            }
                        }
                    }
                    break;
                case Input.Key.Home:
                    {
                        this.CursorPosition = 0;
                        this.Offset = new Point(this.Offset.X, 0);
                    }
                    break;
                case Input.Key.End:
                    {
                        this.CursorPosition = this.List.Count - 1;
                        if(this.MaxSize.Height > this.Bounds.Height)
                            this.Offset = new Point(this.Offset.X, this.Bounds.Height - this.MaxSize.Height);
                    }
                    break;
                case Input.Key.Space:
                    {
                        if(Mode == SelectionMode.Multiple) {
                            this.InvertSelection(this.CursorPosition);
                        }
                    }
                    break;
                case Input.Key.Return:
                    {
                        //this.OnReturn();
                    }
                    break;
            }
        }

        public override bool HandleMouseButtonDown(Input.MouseButtonEventArgs e) {
            if(this.Enabled) {
                if(this.Mode == SelectionMode.Disabled) return false;
                if(this.List.Count == 0) return false;
                int index = (e.Y - this.Bounds.Y - this.Offset.Y) / this.Style.Size.Height;
                if(this.Mode == SelectionMode.Multiple)
                    if(this.CursorPosition == index)
                        this.InvertSelection(index);
                if(index >= 0 && index < this.List.Count) {
                    this.CursorPosition = index;
                    if(this.CursorPosition * this.Style.Size.Height + Offset.Y < 0) {
                        this.Offset = new Point(this.Offset.X, 
                            -this.CursorPosition * this.Style.Size.Height);
                    } else if(this.CursorPosition * this.Style.Size.Height + this.Style.Size.Height + Offset.Y > Bounds.Height) {
                        this.Offset = new Point(this.Offset.X, 
                            (-1 - this.CursorPosition) * this.Style.Size.Height + Bounds.Height);
                    }
                }
            }
            return false;
        }

        int lastSize;
        public override void Tick(IWindow window, TickEventArgs e) {
            if(this.lastSize != this.List.Count) {
                this.MaxSize = new Size(this.Bounds.Width, this.List.Count * this.Style.Size.Height);
                this.lastSize = this.List.Count;
            }
        }
        /// <summary>
        /// Sets a custom name to a specific position. Use with care: no changes to ListBox.List are accounted for.
        /// </summary>
        public void SetLabel(int index, string label) {
            if(label != null) labels[index] = label; 
            else labels.Remove(index);
        }

        public bool Selected(int index) {
            if(this.Mode == SelectionMode.Single) {
                return this.CursorPosition == index;
            } else if(this.Mode == SelectionMode.Multiple) {
                return this.SelectionIndices.Contains(index);
            }
            return false;
        }

        public void Select(int index) {
            if(this.Mode == SelectionMode.Single) {
                this.CursorPosition = index;
            } else if(this.Mode == SelectionMode.Multiple) {
                if(!this.SelectionIndices.Contains(index)) this.SelectionIndices.Add(index);
            }
        }

        public void Deselect(int index) {
            if(this.Mode == SelectionMode.Single) {
                if(this.CursorPosition == index)
                    this.CursorPosition = -1;
            } else if(this.Mode == SelectionMode.Multiple) {
                if(this.SelectionIndices.Contains(index)) this.SelectionIndices.Remove(index);
            }
        }

        public void DeselectAll() {
            if(this.Mode == SelectionMode.Single) {
                this.CursorPosition = -1;
            } else if(this.Mode == SelectionMode.Multiple) {
                this.SelectionIndices.Clear();
            }
        }

        public bool InvertSelection(int index) {
            if(this.Mode != SelectionMode.Disabled) {
                if(this.Selected(index)) {
                    this.Deselect(index);
                    return false;
                } else {
                    this.Select(index);
                    return true;
                }
            }
            return false;
        }
    }
}
