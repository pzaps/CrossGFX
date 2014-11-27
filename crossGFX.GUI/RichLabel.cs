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
using System.Text.RegularExpressions;

namespace crossGFX.GUI
{
    public class RichLabel : Control
    {
        List<RichLabelString> text;
        public List<IFont> Fonts { get; private set; }
        public List<Color> Colors { get; private set; }

        public RichLabel() {
            text = new List<RichLabelString>();
            Fonts = new List<IFont>();
            Fonts.Add(GUI.Instance.Skin.Font);
            activeFont = Fonts[0];
            Colors = new List<Color>();
            Colors.Add(Color.Black);
            activeColor = Colors[0];
        }

        public override void HandleKeyPressed(Input.KeyEventArgs e) {
            switch (e.Code)
            {
                case Input.Key.Up:
                    {
                        if(this.MaxSize.Height > this.Bounds.Height) {
                            this.Offset = new Point(this.Offset.X, Math.Min(this.Offset.Y + this.text.Last().RichString.Size.Height, 0));
                        }
                    }
                    break;
                case Input.Key.Down:
                    {
                        if(this.MaxSize.Height > this.Bounds.Height) {
                            this.Offset = new Point(this.Offset.X, Math.Max(this.Offset.Y - this.text.Last().RichString.Size.Height, this.Bounds.Height - this.MaxSize.Height));
                        }
                    }
                    break;
                case Input.Key.PageUp:
                    {
                        if(this.MaxSize.Height > this.Bounds.Height) {
                            this.Offset = new Point(this.Offset.X, Math.Min(0, this.Offset.Y + this.Bounds.Height));
                        }
                    }
                    break;
                case Input.Key.PageDown:
                    {
                        if(this.MaxSize.Height > this.Bounds.Height) {
                            this.Offset = new Point(this.Offset.X, Math.Max(this.Bounds.Height - this.MaxSize.Height, this.Offset.Y - this.Bounds.Height));
                        }
                    }
                    break;
                case Input.Key.Home:
                    {
                        this.Offset = new Point(this.Offset.X, 0);
                    }
                    break;
                case Input.Key.End:
                    {
                        this.Offset = new Point(this.Offset.X, (this.MaxSize.Height > this.Bounds.Height ? 
                            this.Bounds.Height - this.MaxSize.Height : 0));
                    }
                    break;
            }
        }

        public override void DrawMiddle(IRenderTarget renderTarget) {
            for (int i = 0; i < text.Count; i++) {
                if(text[i].StringPoints[0].Y < Offset.Y) continue;
                if(text[i].StringPoints[0].Y + Offset.Y > Bounds.Height) break;
                if(text[i].StringFragments != null)
                    for(int k = 0; k < text[i].StringFragments.Length; k++) {
                        if(text[i].StringPoints[k].Y + Offset.Y > Bounds.Height) break;
                        renderTarget.DrawString(text[i].StringPoints[k] + Offset, text[i].RichString.Font, text[i].StringFragments[k],
                        text[i].RichString.TextSize, text[i].RichString.Color, 
                        text[i].RichString.Bold, text[i].RichString.Italic, text[i].RichString.Underline);
                    }
                else renderTarget.DrawString(text[i].StringPoints[0] + Offset, text[i].RichString);
            }
        }

        IFont activeFont;
        int activeTextSize;
        Color activeColor;
        bool activeBold, activeItalic, activeUnderline, activeNewLine;

        public void Append(string str) {
            Regex regex = new Regex("<(.*?)>");
            Match match = regex.Match(str);
            if(match.Index > 0) {
                this.Append(this.CreateRichString(str.Substring(0, match.Index)));
            }
            if(!match.Success && str.Length > 0) {
                this.Append(this.CreateRichString(str));
            }
            while(match.Success) {
                string s = match.Groups[1].ToString();
                if(s.StartsWith("line")) {
                    activeNewLine = true;
                } else if(s.StartsWith("b")) {
                    activeBold = !s.EndsWith("0");
                } else if(s.StartsWith("i")) {
                    activeItalic = !s.EndsWith("0");
                } else if(s.StartsWith("u")) {
                    activeUnderline = !s.EndsWith("0");
                } else if(s.StartsWith("fc")) {
                    if (s.Length > 2) {
                        int hexcolor = Convert.ToInt32(s.Substring(2), 16);
                        for(int i=s.Length - 2; i < 6; i++) hexcolor <<= 8;
                        hexcolor += unchecked((int) 0xFF000000);
                        activeColor = Color.FromArgb(hexcolor);
                    }
                } else if(s.StartsWith("cf")) {
                    if (s.Length > 2) {
                        int index = Convert.ToInt32(s.Substring(2));
                        if (index < Colors.Count) activeColor = Colors[index];
                    } 
                } else if(s.StartsWith("fs")) {
                    if (s.Length > 2) activeTextSize = Convert.ToInt32(s.Substring(2)) / 2;
                } else if(s.StartsWith("f")) {
                    if (s.Length > 1) {
                        int index = Convert.ToInt32(s.Substring(1));
                        if (index < Fonts.Count) activeFont = Fonts[index];
                    } 
                }

                int textIndex = match.Index + match.Length;
                string stringText = str.Substring(
                    textIndex, 
                    (match = match.NextMatch()).Success ? match.Index - textIndex : str.Length - textIndex
                    );
                if(stringText.Length > 0 || activeNewLine) {
                    this.Append(CreateRichString(stringText));
                }
            }
        }

        Point nextStringPoint;
        int lineSize;

        public void Append(RichString richString) {
            if(richString.NewLine) {
                nextStringPoint.X = 0;
                nextStringPoint.Y += lineSize;
                lineSize = 0;
            }
            lineSize = Math.Max(lineSize, richString.Size.Height);
            
            RichLabelString richLabelString = new RichLabelString(richString);

            if(nextStringPoint.X + richLabelString.RichString.Size.Width > Bounds.Width) { // if the text will not fit on this line
                string str = richLabelString.RichString.Text;
                if(str.Contains(" ")) { // if there are multiple words

                    List<string> fragments = new List<string>();
                    List<Point> points = new List<Point>();

                    while(nextStringPoint.X + richLabelString.RichString.Font.MeasureTextSize(str, richLabelString.RichString.TextSize, 
                        richLabelString.RichString.Bold).Width > Bounds.Width) { // while there's remaining text that can't fit
                        int bestMatch = 0;
                        while(true) { // it matches for the best point of splitting
                            int nextMatch = str.IndexOf(' ', bestMatch + 1);
                            if(nextMatch != -1) { // if there is another space after the best match
                                if(nextStringPoint.X + richLabelString.RichString.Font.MeasureTextSize(
                                    str.Substring(0, nextMatch), richLabelString.RichString.TextSize, richLabelString.RichString.Bold
                                    ).Width > Bounds.Width) 
                                    break; // if it doesn't fit, abort
                                else bestMatch = nextMatch; // otherwise the line can hold this much
                            } else break;
                        }
                        if(bestMatch == 0) bestMatch = str.IndexOf(" ");
                        if(bestMatch == -1) bestMatch = str.Length - 1;
                        fragments.Add(str.Substring(0, bestMatch));
                        points.Add(nextStringPoint);
                        nextStringPoint.X = 0;
                        nextStringPoint.Y += lineSize;
                        str = str.Substring(bestMatch + 1);
                    }
                    if(str.Length > 0) { // put the remnants
                        fragments.Add(str);
                        points.Add(nextStringPoint);
                        nextStringPoint.X += richLabelString.RichString.Font.MeasureTextSize(str, richLabelString.RichString.TextSize, 
                            richLabelString.RichString.Bold).Width;
                    }
                    richLabelString.StringPoints = points.ToArray();
                    richLabelString.StringFragments = fragments.ToArray();
                } else { // there is only one word, put it on the next line
                    nextStringPoint.X = 0;
                    nextStringPoint.Y += lineSize;

                    richLabelString.StringPoints = new Point[]{ nextStringPoint };

                    nextStringPoint.X += richLabelString.RichString.Size.Width;
                    lineSize = richLabelString.RichString.Size.Height;
                }
            } else {
                richLabelString.StringPoints = new Point[]{ nextStringPoint };
                nextStringPoint.X += richLabelString.RichString.Size.Width;
            }
            this.MaxSize = new Size(this.Bounds.Width,
                richLabelString.StringPoints.Last().Y + richLabelString.RichString.Size.Height);
            text.Add(richLabelString);
        }

        public RichString CreateRichString(string fragment) {
            RichString richString = new RichString(fragment);
            richString.Font = activeFont;
            richString.TextSize = activeTextSize;
            richString.Color = activeColor;
            richString.Bold = activeBold;
            richString.Italic = activeItalic;
            richString.Underline = activeUnderline;
            richString.NewLine = activeNewLine;
            richString.Modified = false;

            activeNewLine = false;

            return richString;
        }

        public override Rectangle Bounds {
            get { return bounds; }
            set {
                int oldWidth = bounds.Width;
                this.bounds = value; 
                if(value.Width != oldWidth) {
                    List<RichLabelString> list = new List<RichLabelString>(text);
                    this.Clear();
                    for(int i = 0; i < list.Count; i++)
                        this.Append(list[i].RichString);
                }
                this.StretchRegions();
            }
        }

        public void Clear() {
            this.text.Clear();
            this.nextStringPoint = Point.Empty;
            this.Offset = new Point(0, 0);
            this.MaxSize = new Size(0, 0);
        }
    }
}