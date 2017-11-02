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
using Inputs.Helpers;
using JetBrains.Annotations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Inputs
{
    [PublicAPI]
    public class GamePad
    {
        public enum DPadDirection
        {
            LEFT,
            RIGHT,
            DOWN,
            UP
        }

        private GamePadStates GamePadStates { get; set; } = new GamePadStates();
        public GamePadState OldState(PlayerIndex p = PlayerIndex.One) => GamePadStates.GetOld(p);
        public GamePadState State(PlayerIndex p = PlayerIndex.One) => GamePadStates.Get(p);

        /// <summary>
        ///     Gets information about the current state. Including calculated delta values.
        /// </summary>
        public IsSub Is { get; }

        /// <summary>
        ///     Gets information about the previous state. No delta values included, since it has no 'old' state to refer to.
        /// </summary>
        public WasSub Was { get; }

        internal GamePad()
        {
            Is = new IsSub(GamePadStates.Get, GamePadStates.GetOld);
            Was = new WasSub(GamePadStates.GetOld);
        }

        internal void Update()
        {
            GamePadStates.Update();
        }

        [PublicAPI]
        public class IsSub
        {
            private Func<PlayerIndex, GamePadState> State { get; set; }
            private Func<PlayerIndex, GamePadState> OldState { get; set; }

            internal IsSub(Func<PlayerIndex, GamePadState> mapping, Func<PlayerIndex, GamePadState> oldMapping)
            {
                State = mapping;
                OldState = oldMapping;
                DPad = new DPadSub(State, OldState);
                ThumbSticks = new ThumbSticksSub(State, OldState);
                Triggers = new TriggersSub(State, OldState);
            }

            public bool Connected(PlayerIndex p = PlayerIndex.One) => State(p).IsConnected;

            public GamePadButtons Buttons(PlayerIndex p = PlayerIndex.One) => State(p).Buttons;

            public bool Down(Buttons button, PlayerIndex p = PlayerIndex.One)
                => State(p).IsButtonDown(button);

            public bool Up(Buttons button, PlayerIndex p = PlayerIndex.One)
                => State(p).IsButtonUp(button);

            public bool Press(Buttons button, PlayerIndex p = PlayerIndex.One)
                => State(p).IsButtonDown(button) && OldState(p).IsButtonUp(button);

            public bool Release(Buttons button, PlayerIndex p = PlayerIndex.One)
                => OldState(p).IsButtonDown(button) && State(p).IsButtonUp(button);

            public GamePadDPad DPadValues(PlayerIndex p = PlayerIndex.One) => State(p).DPad;
            public DPadSub DPad { get; }

            public GamePadThumbSticks ThumbSticksValues(PlayerIndex p = PlayerIndex.One) => State(p).ThumbSticks;
            public ThumbSticksSub ThumbSticks;

            public GamePadTriggers TriggersValues(PlayerIndex p = PlayerIndex.One) => State(p).Triggers;
            public TriggersSub Triggers;

            public int PacketNumber(PlayerIndex p = PlayerIndex.One) => State(p).PacketNumber;

            public bool WasConnected(PlayerIndex p = PlayerIndex.One)
                => !OldState(p).IsConnected && State(p).IsConnected;
        }

        [PublicAPI]
        public class WasSub
        {
            private Func<PlayerIndex, GamePadState> State { get; set; }

            internal WasSub(Func<PlayerIndex, GamePadState> mapping)
            {
                State = mapping;
                DPad = new DPadOldSub(State);
                ThumbSticks = new ThumbSticksOldSub(State);
                Triggers = new TriggersOldSub(State);
            }

            public bool Connected(PlayerIndex p = PlayerIndex.One) => State(p).IsConnected;

            public GamePadButtons Buttons(PlayerIndex p = PlayerIndex.One) => State(p).Buttons;

            public bool Down(Buttons button, PlayerIndex p = PlayerIndex.One)
                => State(p).IsButtonDown(button);

            public bool Up(Buttons button, PlayerIndex p = PlayerIndex.One)
                => State(p).IsButtonUp(button);

            public GamePadDPad DPadValues(PlayerIndex p = PlayerIndex.One) => State(p).DPad;
            public DPadOldSub DPad { get; }

            public GamePadThumbSticks ThumbSticksValues(PlayerIndex p = PlayerIndex.One) => State(p).ThumbSticks;
            public ThumbSticksOldSub ThumbSticks;

            public GamePadTriggers TriggersValues(PlayerIndex p = PlayerIndex.One) => State(p).Triggers;
            public TriggersOldSub Triggers;

            public int PacketNumber(PlayerIndex p = PlayerIndex.One) => State(p).PacketNumber;
        }

        [PublicAPI]
        public class DPadSub : DPadOldSub
        {
            private Func<PlayerIndex, GamePadState> OldState { get; set; }

            public DPadSub(Func<PlayerIndex, GamePadState> mapping, Func<PlayerIndex, GamePadState> oldMapping)
                : base(mapping)
            {
                OldState = oldMapping;
            }

            public bool Press(DPadDirection direction, PlayerIndex p = PlayerIndex.One)
                => Down(State, direction, p) && Up(OldState, direction, p);

            public bool Release(DPadDirection direction, PlayerIndex p = PlayerIndex.One)
                => Down(OldState, direction, p) && Up(State, direction, p);
        }

        [PublicAPI]
        public class DPadOldSub
        {
            protected Func<PlayerIndex, GamePadState> State { get; set; }

            internal DPadOldSub(Func<PlayerIndex, GamePadState> mapping)
            {
                State = mapping;
            }

            public bool Down(DPadDirection direction, PlayerIndex p = PlayerIndex.One) => Down(State, direction, p);
            public bool Up(DPadDirection direction, PlayerIndex p = PlayerIndex.One) => Up(State, direction, p);

            protected bool Down(Func<PlayerIndex, GamePadState> mapping, DPadDirection direction,
                PlayerIndex p = PlayerIndex.One)
            {
                switch (direction)
                {
                    case DPadDirection.DOWN:
                        return mapping(p).DPad.Down == ButtonState.Pressed;
                    case DPadDirection.LEFT:
                        return mapping(p).DPad.Left == ButtonState.Pressed;
                    case DPadDirection.RIGHT:
                        return mapping(p).DPad.Right == ButtonState.Pressed;
                    case DPadDirection.UP:
                        return mapping(p).DPad.Up == ButtonState.Pressed;
                }
                return false;
            }

            protected bool Up(Func<PlayerIndex, GamePadState> mapping, DPadDirection direction,
                PlayerIndex p = PlayerIndex.One)
            {
                switch (direction)
                {
                    case DPadDirection.DOWN:
                        return mapping(p).DPad.Down == ButtonState.Released;
                    case DPadDirection.LEFT:
                        return mapping(p).DPad.Left == ButtonState.Released;
                    case DPadDirection.RIGHT:
                        return mapping(p).DPad.Right == ButtonState.Released;
                    case DPadDirection.UP:
                        return mapping(p).DPad.Up == ButtonState.Released;
                }
                return false;
            }
        }

        [PublicAPI]
        public class ThumbSticksSub : ThumbSticksOldSub
        {
            private Func<PlayerIndex, GamePadState> OldState { get; set; }

            public ThumbSticksSub(Func<PlayerIndex, GamePadState> mapping, Func<PlayerIndex, GamePadState> oldMapping)
                : base(mapping)
            {
                OldState = oldMapping;
            }

            public Vector2 LeftDelta(PlayerIndex p = PlayerIndex.One) => Left(p) - OldState(p).ThumbSticks.Left;
            public Vector2 RightDelta(PlayerIndex p = PlayerIndex.One) => Right(p) - OldState(p).ThumbSticks.Right;
        }

        [PublicAPI]
        public class ThumbSticksOldSub
        {
            protected Func<PlayerIndex, GamePadState> State { get; set; }

            internal ThumbSticksOldSub(Func<PlayerIndex, GamePadState> mapping)
            {
                State = mapping;
            }

            public Vector2 Left(PlayerIndex p = PlayerIndex.One) => State(p).ThumbSticks.Left;
            public Vector2 Right(PlayerIndex p = PlayerIndex.One) => State(p).ThumbSticks.Right;
        }

        [PublicAPI]
        public class TriggersSub : TriggersOldSub
        {
            private Func<PlayerIndex, GamePadState> OldState { get; set; }

            public TriggersSub(Func<PlayerIndex, GamePadState> mapping, Func<PlayerIndex, GamePadState> oldMapping)
                : base(mapping)
            {
                OldState = oldMapping;
            }

            public float LeftDelta(PlayerIndex p = PlayerIndex.One) => Left(p) - OldState(p).Triggers.Left;
            public float RightDelta(PlayerIndex p = PlayerIndex.One) => Right(p) - OldState(p).Triggers.Right;
        }

        [PublicAPI]
        public class TriggersOldSub
        {
            protected Func<PlayerIndex, GamePadState> State { get; set; }

            internal TriggersOldSub(Func<PlayerIndex, GamePadState> mapping)
            {
                State = mapping;
            }

            public float Left(PlayerIndex p = PlayerIndex.One) => State(p).Triggers.Left;
            public float Right(PlayerIndex p = PlayerIndex.One) => State(p).Triggers.Right;
        }
    }
}