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
using Microsoft.Xna.Framework.Input;

namespace InputStateManager.Inputs
{
    /// <summary>
    ///     Relies on the PlayerIndex enumeration to get the number of valid inputs and their respecitve indexes.
    /// </summary>
    [PublicAPI]
    public class Pad
    {
        public enum DPadDirection
        {
            LEFT,
            RIGHT,
            DOWN,
            UP
        }

        private IPadInputProvider provider;

        public PlayerIndex PlayerIndex;
        public GamePadState OldState { get; private set; }
        public GamePadState State { get; private set; }

        /// <summary>
        ///     Gets information about the current state. Including calculated delta values.
        /// </summary>
        public IsSub Is { get; }

        /// <summary>
        ///     Gets information about the previous state. No delta values included, since there are no 'old' state to refer to.
        /// </summary>
        public WasSub Was { get; }

        internal Pad(PlayerIndex playerIndex, IPadInputProvider provider)
        {
            PlayerIndex = playerIndex;
            this.provider = provider;
            Is = new IsSub(GetState, GetOldState);
            Was = new WasSub(GetOldState);
        }

        internal GamePadState GetState() => State;
        internal GamePadState GetOldState() => OldState;

        internal void Update()
        {
            if (provider == null)
                return;
            OldState = State;
            State = provider.GetState((int) PlayerIndex);
        }

        [PublicAPI]
        public class IsSub : WasSub
        {
            private Func<GamePadState> State { get; set; }
            private Func<GamePadState> OldState { get; set; }

            internal IsSub(Func<GamePadState> mapping, Func<GamePadState> oldMapping) : base(mapping)
            {
                State = mapping;
                OldState = oldMapping;
                DPad = new DPadSub(State, OldState);
                ThumbSticks = new ThumbSticksSub(State, OldState);
                Triggers = new TriggersSub(State, OldState);
            }

            public bool Press(params Buttons[] buttons)
            {
                foreach (var button in buttons)
                    if (State().IsButtonUp(button) || OldState().IsButtonDown(button))
                        return false;
                return true;
            }

            public bool OnePress(params Buttons[] buttons)
            {
                foreach (var button in buttons)
                    if (State().IsButtonDown(button) && OldState().IsButtonUp(button))
                        return true;
                return false;
            }

            public bool Release(params Buttons[] buttons)
            {
                foreach (var button in buttons)
                    if (OldState().IsButtonUp(button) || State().IsButtonDown(button))
                        return false;
                return true;
            }

            public bool OneRelease(params Buttons[] buttons)
            {
                foreach (var button in buttons)
                    if (OldState().IsButtonDown(button) && State().IsButtonUp(button))
                        return true;
                return false;
            }

            public new DPadSub DPad { get; }
            public new ThumbSticksSub ThumbSticks;
            public new TriggersSub Triggers;

            public bool JustConnected
                => !OldState().IsConnected && State().IsConnected;
        }

        [PublicAPI]
        public class WasSub
        {
            private Func<GamePadState> State { get; set; }

            internal WasSub(Func<GamePadState> mapping)
            {
                State = mapping;
                DPad = new DPadOldSub(State);
                ThumbSticks = new ThumbSticksOldSub(State);
                Triggers = new TriggersOldSub(State);
            }

            public bool Connected => State().IsConnected;

            public GamePadButtons Buttons => State().Buttons;

            public bool Down(params Buttons[] buttons)
            {
                foreach (var button in buttons)
                {
                    if (State().IsButtonUp(button))
                        return false;
                }

                return true;
            }

            public bool OneDown(params Buttons[] buttons)
            {
                foreach (var button in buttons)
                {
                    if (State().IsButtonDown(button))
                        return true;
                }

                return false;
            }

            public bool Up(params Buttons[] buttons)
            {
                foreach (var button in buttons)
                {
                    if (State().IsButtonDown(button))
                        return false;
                }

                return true;
            }

            public bool OneUp(params Buttons[] buttons)
            {
                foreach (var button in buttons)
                {
                    if (State().IsButtonUp(button))
                        return true;
                }

                return false;
            }

            public GamePadDPad DPadValues => State().DPad;
            public DPadOldSub DPad { get; }

            public GamePadThumbSticks ThumbSticksValues => State().ThumbSticks;
            public ThumbSticksOldSub ThumbSticks;

            public GamePadTriggers TriggersValues => State().Triggers;
            public TriggersOldSub Triggers;

            public int PacketNumber => State().PacketNumber;
        }

        [PublicAPI]
        public class DPadSub : DPadOldSub
        {
            private Func<GamePadState> OldState { get; set; }

            public DPadSub(Func<GamePadState> mapping, Func<GamePadState> oldMapping)
                : base(mapping)
            {
                OldState = oldMapping;
            }

            public bool Press(DPadDirection direction)
                => Down(State, direction) && Up(OldState, direction);

            public bool Release(DPadDirection direction)
                => Down(OldState, direction) && Up(State, direction);
        }

        [PublicAPI]
        public class DPadOldSub
        {
            protected Func<GamePadState> State { get; set; }

            internal DPadOldSub(Func<GamePadState> mapping)
            {
                State = mapping;
            }

            public bool Down(DPadDirection direction) => Down(State, direction);
            public bool Up(DPadDirection direction) => Up(State, direction);

            protected bool Down(Func<GamePadState> mapping, DPadDirection direction,
                PlayerIndex p = PlayerIndex.One)
            {
                switch (direction)
                {
                    case DPadDirection.DOWN:
                        return mapping().DPad.Down == ButtonState.Pressed;
                    case DPadDirection.LEFT:
                        return mapping().DPad.Left == ButtonState.Pressed;
                    case DPadDirection.RIGHT:
                        return mapping().DPad.Right == ButtonState.Pressed;
                    case DPadDirection.UP:
                        return mapping().DPad.Up == ButtonState.Pressed;
                }

                return false;
            }

            protected bool Up(Func<GamePadState> mapping, DPadDirection direction)
            {
                switch (direction)
                {
                    case DPadDirection.DOWN:
                        return mapping().DPad.Down == ButtonState.Released;
                    case DPadDirection.LEFT:
                        return mapping().DPad.Left == ButtonState.Released;
                    case DPadDirection.RIGHT:
                        return mapping().DPad.Right == ButtonState.Released;
                    case DPadDirection.UP:
                        return mapping().DPad.Up == ButtonState.Released;
                }

                return false;
            }
        }

        [PublicAPI]
        public class ThumbSticksSub : ThumbSticksOldSub
        {
            private Func<GamePadState> OldState { get; set; }

            public ThumbSticksSub(Func<GamePadState> mapping, Func<GamePadState> oldMapping)
                : base(mapping)
            {
                OldState = oldMapping;
            }

            public Vector2 LeftDelta => Left - OldState().ThumbSticks.Left;
            public Vector2 RightDelta => Right - OldState().ThumbSticks.Right;
        }

        [PublicAPI]
        public class ThumbSticksOldSub
        {
            protected Func<GamePadState> State { get; set; }

            internal ThumbSticksOldSub(Func<GamePadState> mapping)
            {
                State = mapping;
            }

            public Vector2 Left => State().ThumbSticks.Left;
            public Vector2 Right => State().ThumbSticks.Right;
        }

        [PublicAPI]
        public class TriggersSub : TriggersOldSub
        {
            private Func<GamePadState> OldState { get; set; }

            public TriggersSub(Func<GamePadState> mapping, Func<GamePadState> oldMapping)
                : base(mapping)
            {
                OldState = oldMapping;
            }

            public float LeftDelta => Left - OldState().Triggers.Left;
            public float RightDelta => Right - OldState().Triggers.Right;
        }

        [PublicAPI]
        public class TriggersOldSub
        {
            protected Func<GamePadState> State { get; set; }

            internal TriggersOldSub(Func<GamePadState> mapping)
            {
                State = mapping;
            }

            public float Left => State().Triggers.Left;
            public float Right => State().Triggers.Right;
        }
    }
}