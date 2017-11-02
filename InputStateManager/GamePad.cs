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

using Inputs.Helpers;
using JetBrains.Annotations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Inputs
{
    [PublicAPI]
    public class GamePad
    {
        GamePadStates GamePadStates { get; set; } = new GamePadStates();

        public GamePadState OldState(PlayerIndex p = PlayerIndex.One) => GamePadStates.GetOld(p);
        public GamePadState State(PlayerIndex p = PlayerIndex.One) => GamePadStates.Get(p);

        public bool IsConnected(PlayerIndex p = PlayerIndex.One) => State(p).IsConnected;

        public bool IsDown(Buttons button, PlayerIndex p = PlayerIndex.One)
            => State(p).IsButtonDown(button);

        public bool IsUp(Buttons button, PlayerIndex p = PlayerIndex.One)
            => State(p).IsButtonUp(button);

        public bool IsPress(Buttons button, PlayerIndex p = PlayerIndex.One)
            => State(p).IsButtonDown(button) && OldState(p).IsButtonUp(button);

        public bool IsRelease(Buttons button, PlayerIndex p = PlayerIndex.One)
            => OldState(p).IsButtonDown(button) && State(p).IsButtonUp(button);

        public GamePadButtons Buttons(PlayerIndex p = PlayerIndex.One) => State(p).Buttons;
        public GamePadDPad DPad(PlayerIndex p = PlayerIndex.One) => State(p).DPad;
        public int PacketNumber(PlayerIndex p = PlayerIndex.One) => State(p).PacketNumber;
        public GamePadThumbSticks ThumbSticks(PlayerIndex p = PlayerIndex.One) => State(p).ThumbSticks;
        public GamePadTriggers Triggers(PlayerIndex p = PlayerIndex.One) => State(p).Triggers;
        public bool Connected(PlayerIndex p = PlayerIndex.One) => !OldState(p).IsConnected && State(p).IsConnected;

        // Old states.
        public bool IsOldConnected(PlayerIndex p = PlayerIndex.One) => OldState(p).IsConnected;

        public bool IsOldDown(Buttons button, PlayerIndex p = PlayerIndex.One)
            => OldState(p).IsButtonDown(button);

        public bool IsOldUp(Buttons button, PlayerIndex p = PlayerIndex.One)
            => OldState(p).IsButtonUp(button);

        public GamePadButtons OldButtons(PlayerIndex p = PlayerIndex.One) => OldState(p).Buttons;
        public GamePadDPad OldDPad(PlayerIndex p = PlayerIndex.One) => OldState(p).DPad;
        public int OldPacketNumber(PlayerIndex p = PlayerIndex.One) => OldState(p).PacketNumber;
        public GamePadThumbSticks OldThumbSticks(PlayerIndex p = PlayerIndex.One) => OldState(p).ThumbSticks;
        public GamePadTriggers OldTriggers(PlayerIndex p = PlayerIndex.One) => OldState(p).Triggers;

        internal void Update()
        {
            GamePadStates.Update();
        }
    }
}