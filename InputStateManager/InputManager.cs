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
using InputStateManager.Inputs;
using InputStateManager.Inputs.InputProviders.Implementations;
using InputStateManager.Inputs.InputProviders.Interfaces;
using JetBrains.Annotations;
using Microsoft.Xna.Framework;

namespace InputStateManager
{
    [PublicAPI]
    public class InputManager
    {
        public Key Key { get; }
        public Mouse Mouse { get; }
        private Pad[] pads;
        public Pad Pad(PlayerIndex playerIndex = PlayerIndex.One) => pads[(int)playerIndex];
        public Pad Pad(int playerIndex) => pads[playerIndex];
        public Touch Touch { get; }

        public InputManager()
        {
            Key = new Key(new XnaKeyInputProvider());
            Mouse = new Mouse(new XnaMouseInputProvider());
            SetProviderForPads(new XnaPadInputProvider());
            Touch = new Touch(new XnaTouchInputProvider());
        }

        /// <summary>
        ///     Constructor that lets you inject input-providers for testing purposes.
        /// </summary>
        public InputManager(IKeyInputProvider keyInputProvider, IMouseInputProvider mouseInputProvider,
            IPadInputProvider padInputProvider, ITouchInputProvider touchInputProvider)
        {
            Key = new Key(keyInputProvider);
            Mouse = new Mouse(mouseInputProvider);
            SetProviderForPads(padInputProvider);
            Touch = new Touch(touchInputProvider);
        }

        private void SetProviderForPads(IPadInputProvider provider)
        {
            var players = Enum.GetNames(typeof(PlayerIndex));
            pads = new Pad[players.Length];
            for (var i = 0; i < players.Length; i++)
            {
                pads[i] = new Pad((PlayerIndex)i,  provider);
            }
        }

        public void Update()
        {
            Mouse.Update();
            Key.Update();
            foreach (var pad in pads)
            {
                pad.Update();
            }
            Touch.Update();
        }
    }
}