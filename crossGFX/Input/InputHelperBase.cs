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
using crossGFX;

namespace crossGFX.Input
{
    public class InputHelperBase
    {
        protected Point mousePosition;
        protected readonly float[] lastClickTime = new float[(int)crossGFX.Input.MouseButton.ButtonCount];
        protected Point lastClickPosition;
        protected KeyData keyData;
        int keyRepeatRate;
        int keyRepeatDelay;

        public int KeyRepeatDelay {
            get { return keyRepeatDelay; }
            set { keyRepeatDelay = value; }
        }

        public int KeyRepeatRate {
            get { return keyRepeatRate; }
            set { keyRepeatRate = value; }
        }

        public Point MousePosition {
            get {
                return mousePosition;
            }
        }

        public Point LastClickPosition {
            get {
                return lastClickPosition;
            }
            set {
                lastClickPosition = value;
            }
        }

        public KeyData KeyData {
            get { return keyData; }
        }

        public float[] LastClickTime {
            get { return lastClickTime; }
        }

        public InputHelperBase() {
            keyData = new KeyData();

            keyRepeatRate = 60;
            keyRepeatDelay = 200;
        }

        /// <summary>
        /// Indicates whether the shift key is down.
        /// </summary>
        public bool IsShiftDown { get { return (IsKeyDown(Input.Key.LShift) || IsKeyDown(Input.Key.RShift)); } }

        /// <summary>
        /// Indicates whether the control key is down.
        /// </summary>
        public bool IsControlDown { get { return (IsKeyDown(Input.Key.LControl) || IsKeyDown(Input.Key.RControl)); } }

        /// <summary>
        /// Indicates whether the alt key is down.
        /// </summary>
        public bool IsAltDown { get { return (IsKeyDown(Input.Key.LAlt) || IsKeyDown(Input.Key.RAlt)); } }

        /// <summary>
        /// Checks if the given key is pressed.
        /// </summary>
        /// <param name="key">Key to check.</param>
        /// <returns>True if the key is down.</returns>
        public bool IsKeyDown(Input.Key key) {
            return KeyData.KeyState[(int)key];
        }

        protected void KeyPressedHelper(Input.Key key) {

        }

       
    }
}
