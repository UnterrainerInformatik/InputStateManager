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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

namespace InputStateManager.Inputs.InputProviders.Implementations
{
    internal class XnaTouchInputProvider : ITouchInputProvider
    {
        public TouchCollection GetState()
        {
            return TouchPanel.GetState();
        }

        public int GetDisplayHeight()
        {
            return TouchPanel.DisplayHeight;
        }

        public int GetDisplayWidth()
        {
            return TouchPanel.DisplayWidth;
        }

        public DisplayOrientation GetDisplayOrientation()
        {
            return TouchPanel.DisplayOrientation;
        }

        public bool GetIsGestureAvailable()
        {
            return TouchPanel.IsGestureAvailable;
        }

        public bool GetEnableMouseGestures()
        {
            return TouchPanel.EnableMouseGestures;
        }

        public bool GetEnableMouseTouchPoint()
        {
            return TouchPanel.EnableMouseTouchPoint;
        }

        public GestureType GetEnabledGestures()
        {
            return TouchPanel.EnabledGestures;
        }

        public IntPtr GetWindowHandle()
        {
            return TouchPanel.WindowHandle;
        }

        public TouchPanelCapabilities GetCapabilities()
        {
            return TouchPanel.GetCapabilities();
        }

        public GestureSample ReadGesture()
        {
            return TouchPanel.ReadGesture();
        }
    }
}