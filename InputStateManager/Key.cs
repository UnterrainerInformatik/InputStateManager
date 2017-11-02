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
using JetBrains.Annotations;
using Microsoft.Xna.Framework.Input;

namespace Inputs
{
    [PublicAPI]
    public class Key
    {
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

        internal Key()
        {
            Is = new IsSub(GetState, GetOldState);
            Was = new WasSub(GetOldState);
        }

        internal KeyboardState GetState() => State;
        internal KeyboardState GetOldState() => OldState;

        internal void Update()
        {
            OldState = State;
            State = Keyboard.GetState();
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

            public bool Press(Keys key) => State().IsKeyDown(key) && OldState().IsKeyUp(key);
            public bool Release(Keys key) => OldState().IsKeyDown(key) && State().IsKeyUp(key);

            public bool ShiftPress => Press(Keys.LeftShift) || Press(Keys.RightShift);
            public bool CtrlPress => Press(Keys.LeftControl) || Press(Keys.RightControl);
            public bool AltPress => Press(Keys.LeftAlt) || Press(Keys.RightAlt);

            public bool ShiftRelease => Release(Keys.LeftShift) && Release(Keys.RightShift);
            public bool CtrlRelease => Release(Keys.LeftControl) && Release(Keys.RightControl);
            public bool AltRelease => Release(Keys.LeftAlt) && Release(Keys.RightAlt);

            public bool NumLockPress => State().NumLock && !OldState().NumLock;
            public bool NumLockRelease => !State().NumLock && OldState().NumLock;

            public bool CapsLockPress => State().CapsLock && !OldState().CapsLock;
            public bool CapsLockRelease => !State().CapsLock && OldState().CapsLock;
        }

        [PublicAPI]
        public class WasSub
        {
            private Func<KeyboardState> State { get; set; }

            internal WasSub(Func<KeyboardState> mapping)
            {
                State = mapping;
            }

            public bool Down(Keys key) => State().IsKeyDown(key);
            public bool Up(Keys key) => State().IsKeyUp(key);

            public bool ShiftDown => Down(Keys.LeftShift) || Down(Keys.RightShift);
            public bool CtrlDown => Down(Keys.LeftControl) || Down(Keys.RightControl);
            public bool AltDown => Down(Keys.LeftAlt) || Down(Keys.RightAlt);
            public bool ShiftUp => Up(Keys.LeftShift) && Up(Keys.RightShift);
            public bool CtrlUp => Up(Keys.LeftControl) && Up(Keys.RightControl);
            public bool AltUp => Up(Keys.LeftAlt) && Up(Keys.RightAlt);
            public bool NumLock => State().NumLock;
            public bool CapsLock => State().CapsLock;
            public Keys[] GetPressedKeys => State().GetPressedKeys();
        }
    }
}