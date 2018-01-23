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
using Microsoft.Xna.Framework.Input;

namespace InputStateManager.Inputs
{
    [PublicAPI]
    public class Key
    {
        private IKeyInputProvider provider;

        public KeyboardState OldState { get; set; }
        public KeyboardState State { get; set; }

        /// <summary>
        ///     Gets information about the current state. Including calculated delta values.
        /// </summary>
        public IsSub Is { get; }

        /// <summary>
        ///     Gets information about the previous state. No delta values included, since it has no 'old' state to refer to.
        /// </summary>
        public WasSub Was { get; }

        internal Key(IKeyInputProvider provider)
        {
            this.provider = provider;
            Is = new IsSub(GetState, GetOldState);
            Was = new WasSub(GetOldState);
        }

        internal KeyboardState GetState() => State;
        internal KeyboardState GetOldState() => OldState;

        internal void Update()
        {
            if (provider == null)
                return;

            OldState = State;
            State = provider.GetState();
        }

        [PublicAPI]
        public class IsSub : WasSub
        {
            private Func<KeyboardState> State { get; set; }
            private Func<KeyboardState> OldState { get; set; }

            internal IsSub(Func<KeyboardState> mapping, Func<KeyboardState> oldMapping) : base(mapping)
            {
                State = mapping;
                OldState = oldMapping;
            }

            public bool OnePress(params Keys[] keys)
            {
                foreach (var key in keys)
                    if (State().IsKeyDown(key) && OldState().IsKeyUp(key))
                        return true;
                return false;
            }

            public bool Press(params Keys[] keys)
            {
                foreach (var key in keys)
                    if (State().IsKeyUp(key) || OldState().IsKeyDown(key))
                        return false;
                return true;
            }

            public bool OneRelease(params Keys[] keys)
            {
                foreach (var key in keys)
                    if (OldState().IsKeyDown(key) && State().IsKeyUp(key))
                        return true;
                return false;
            }

            public bool Release(params Keys[] keys)
            {
                foreach (var key in keys)
                    if (OldState().IsKeyUp(key) || State().IsKeyDown(key))
                        return false;
                return true;
            }

            public bool ShiftPress => OnePress(Keys.LeftShift, Keys.RightShift);
            public bool CtrlPress => OnePress(Keys.LeftControl, Keys.RightControl);
            public bool AltPress => OnePress(Keys.LeftAlt, Keys.RightAlt);
            public bool WindowsPress => OnePress(Keys.LeftWindows, Keys.RightWindows);
            public bool NumLockStateEnter => State().NumLock && !OldState().NumLock;
            public bool CapsLockStateEnter => State().CapsLock && !OldState().CapsLock;

            public bool ShiftRelease => Release(Keys.LeftShift, Keys.RightShift);
            public bool CtrlRelease => Release(Keys.LeftControl, Keys.RightControl);
            public bool AltRelease => Release(Keys.LeftAlt, Keys.RightAlt);
            public bool WindowsRelease => Release(Keys.LeftWindows, Keys.RightWindows);
            public bool NumLockStateExit => !State().NumLock && OldState().NumLock;
            public bool CapsLockStateExit => !State().CapsLock && OldState().CapsLock;
        }

        [PublicAPI]
        public class WasSub
        {
            private Func<KeyboardState> State { get; set; }

            internal WasSub(Func<KeyboardState> mapping)
            {
                State = mapping;
            }

            public bool OneDown(params Keys[] keys)
            {
                foreach (var key in keys)
                    if (State().IsKeyDown(key))
                        return true;
                return false;
            }

            public bool Down(params Keys[] keys)
            {
                foreach (var key in keys)
                    if (State().IsKeyUp(key))
                        return false;
                return true;
            }

            public bool OneUp(params Keys[] keys)
            {
                foreach (var key in keys)
                    if (State().IsKeyUp(key))
                        return true;
                return false;
            }

            public bool Up(params Keys[] keys)
            {
                foreach (var key in keys)
                    if (State().IsKeyDown(key))
                        return false;
                return true;
            }

            public bool ShiftDown => OneDown(Keys.LeftShift, Keys.RightShift);
            public bool CtrlDown => OneDown(Keys.LeftControl, Keys.RightControl);
            public bool AltDown => OneDown(Keys.LeftAlt, Keys.RightAlt);
            public bool WindowsDown => OneDown(Keys.LeftWindows, Keys.RightWindows);
            
            public bool ShiftUp => Up(Keys.LeftShift, Keys.RightShift);
            public bool CtrlUp => Up(Keys.LeftControl, Keys.RightControl);
            public bool AltUp => Up(Keys.LeftAlt, Keys.RightAlt);
            public bool WindowsUp => Up(Keys.LeftWindows, Keys.RightWindows);

            public bool NumLockStateOn => State().NumLock;
            public bool CapsLockStateOn => State().CapsLock;
            public bool NumLockStateOff => !State().NumLock;
            public bool CapsLockStateOff => !State().CapsLock;

            public Keys[] GetPressedKeys => State().GetPressedKeys();
        }
    }
}