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
    public class Scene : ActorContainer
    {

        public Scene() {
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
            if(e.Code == Key.Tab) { // focus switch
                int step = e.Shift ? -1 : 1;
                int index = actors.IndexOf(focusedActor);
                for(int i = index + step; i != index; i += step) {
                    if(i >= actors.Count) i = 0; else if(i < 0) i = actors.Count - 1;
                    IFocusable focusableActor = actors[i] as IFocusable;
                    if (focusableActor != null) {
                        if (focusableActor.CanAcquireFocus) {
                            if(this.focusedActor != null) (this.focusedActor as IFocusable).HasFocus = false;
                            this.focusedActor = actors[i];
                            focusableActor.HasFocus = true;
                            break;
                        }
                    }
                }
                return;
            }
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
    }
}
