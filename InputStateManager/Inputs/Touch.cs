// *************************************************************************** 
// This is free and unencumbered software released into the public domain.
// 
// Anyone is free to copy, modify, publish, use, compile, sell, or
// distribute this software, either in source code form or as a compiled
// binary, for any purpose, commercial or non-commercial, and by any
// means.
// 
// In jurisdictions that recognize copyright laws, the author or authors
// of this software dedicate any and all copyright interest in the
// software to the public domain. We make this dedication for the benefit
// of the public at large and to the detriment of our heirs and
// successors. We intend this dedication to be an overt act of
// relinquishment in perpetuity of all present and future rights to this
// software under copyright law.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR
// OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
// ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
// 
// For more information, please refer to <http://unlicense.org>
// ***************************************************************************

using System;
using InputStateManager.Inputs.InputProviders.Interfaces;
using JetBrains.Annotations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

namespace InputStateManager.Inputs
{
    [PublicAPI]
    public class Touch
    {
        private ITouchInputProvider provider;

        /// <summary>
        ///     Gets information about the current state. Including calculated delta values.
        /// </summary>
        public IsSub Is { get; }

        /// <summary>
        ///     Gets information about the previous state. No delta values included, since it has no 'old' state to refer to.
        /// </summary>
        public WasSub Was { get; }

        internal Touch(ITouchInputProvider provider)
        {
            this.provider = provider;
            Is = new IsSub();
            Was = new WasSub();
        }

        public int DisplayHeight => provider.GetDisplayHeight();
        public int DisplayWidth => provider.GetDisplayWidth();
        public DisplayOrientation DisplayOrientation => provider.GetDisplayOrientation();
        public bool IsGestureAvailable => provider.GetIsGestureAvailable();
        public bool EnableMouseGestures => provider.GetEnableMouseGestures();
        public bool EnableMouseTouchPoint => provider.GetEnableMouseTouchPoint();
        public GestureType EnabledGestures => provider.GetEnabledGestures();
        public IntPtr WindowHandle => provider.GetWindowHandle();
        public TouchPanelCapabilities GetCapabilities => provider.GetCapabilities();

        public GestureSample ReadGesture() => provider.ReadGesture();

        private bool emulateWithMouse;

        public bool EmulateWithMouse
        {
            get => emulateWithMouse;
            set
            {
                if (!emulateWithMouse)
                {
                    TouchPanel.EnabledGestures = GestureType.Hold | GestureType.Tap | GestureType.DoubleTap |
                                                 GestureType.DragComplete | GestureType.Flick | GestureType.FreeDrag |
                                                 GestureType.HorizontalDrag | GestureType.VerticalDrag;
                    TouchPanel.EnableMouseGestures = true;
                    TouchPanel.EnableMouseTouchPoint = true;
                }
                emulateWithMouse = value;
            }
        }

        internal void Update()
        {
            Was.Collection = Is.Collection;
            Is.Collection = provider.GetState();
        }

        [PublicAPI]
        public class IsSub
        {
            public TouchCollection Collection { get; internal set; }
        }

        [PublicAPI]
        public class WasSub
        {
            public TouchCollection Collection { get; internal set; }
        }
    }
}