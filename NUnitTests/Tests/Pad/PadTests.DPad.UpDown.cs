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

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using NUnit.Framework;

namespace NUnitTests.Tests.Pad
{
    [TestFixture]
    [Category("InputStateManager.Pad.DPad.UpDown")]
    public partial class PadTests
    {
        private static GamePadState GetDpadS(ButtonState up) => new GamePadState(
            new GamePadThumbSticks(Vector2.Zero, Vector2.Zero), new GamePadTriggers(0f, 0f), new GamePadButtons(0),
            new GamePadDPad(up, ButtonState.Released, ButtonState.Released, ButtonState.Released));

        private static GamePadState GetDpadM(ButtonState up, ButtonState down) => new GamePadState(
            new GamePadThumbSticks(Vector2.Zero, Vector2.Zero), new GamePadTriggers(0f, 0f), new GamePadButtons(0),
            new GamePadDPad(up, down, ButtonState.Released, ButtonState.Released));

        [Test]
        public void DPadDownTriggers()
        {
            providerMock.SetupSequence(o => o.GetState(0))
                .Returns(GetDpadS(ButtonState.Pressed));
            input.Update();
            Assert.IsTrue(input.Pad().Is.DPad.Down(InputStateManager.Inputs.Pad.DPadDirection.UP));
        }

        [Test]
        public void DPadDownMultipleInputsTriggers()
        {
            providerMock.SetupSequence(o => o.GetState(0))
                .Returns(GetDpadM(ButtonState.Pressed, ButtonState.Pressed));
            input.Update();
            Assert.IsTrue(input.Pad().Is.DPad.Down(InputStateManager.Inputs.Pad.DPadDirection.UP,
                InputStateManager.Inputs.Pad.DPadDirection.DOWN));
        }

        [Test]
        public void DPadOneDownTriggers()
        {
            providerMock.SetupSequence(o => o.GetState(0))
                .Returns(GetDpadS(ButtonState.Pressed));
            input.Update();
            Assert.IsTrue(input.Pad().Is.DPad.OneDown(InputStateManager.Inputs.Pad.DPadDirection.UP,
                InputStateManager.Inputs.Pad.DPadDirection.DOWN));
        }

        [Test]
        public void DPadDownIsResetProperly()
        {
            providerMock.SetupSequence(o => o.GetState(0))
                .Returns(GetDpadS(ButtonState.Pressed))
                .Returns(GetDpadS(ButtonState.Pressed))
                .Returns(IdleState);
            input.Update();
            Assert.IsTrue(input.Pad().Is.DPad.Down(InputStateManager.Inputs.Pad.DPadDirection.UP));
            input.Update();
            Assert.IsTrue(input.Pad().Is.DPad.Down(InputStateManager.Inputs.Pad.DPadDirection.UP));
            input.Update();
            Assert.IsFalse(input.Pad().Is.DPad.Down(InputStateManager.Inputs.Pad.DPadDirection.UP));
        }

        [Test]
        public void DPadUpTriggers()
        {
            providerMock.SetupSequence(o => o.GetState(0))
                .Returns(GetDpadS(ButtonState.Pressed))
                .Returns(IdleState);
            input.Update();
            Assert.IsFalse(input.Pad().Is.DPad.Up(InputStateManager.Inputs.Pad.DPadDirection.UP));
            input.Update();
            Assert.IsTrue(input.Pad().Is.DPad.Up(InputStateManager.Inputs.Pad.DPadDirection.UP));
        }

        [Test]
        public void DPadUpMultipleInputsTriggers()
        {
            providerMock.SetupSequence(o => o.GetState(0))
                .Returns(GetDpadM(ButtonState.Pressed, ButtonState.Pressed))
                .Returns(IdleState);
            input.Update();
            Assert.IsFalse(input.Pad().Is.DPad.Up(InputStateManager.Inputs.Pad.DPadDirection.UP,
                InputStateManager.Inputs.Pad.DPadDirection.DOWN));
            input.Update();
            Assert.IsTrue(input.Pad().Is.DPad.Up(InputStateManager.Inputs.Pad.DPadDirection.UP,
                InputStateManager.Inputs.Pad.DPadDirection.DOWN));
        }

        [Test]
        public void DPadOneUpTriggers()
        {
            providerMock.SetupSequence(o => o.GetState(0))
                .Returns(GetDpadS(ButtonState.Pressed))
                .Returns(IdleState);
            input.Update();
            Assert.IsTrue(input.Pad().Is.DPad.OneUp(InputStateManager.Inputs.Pad.DPadDirection.UP,
                InputStateManager.Inputs.Pad.DPadDirection.DOWN));
            input.Update();
            Assert.IsTrue(input.Pad().Is.DPad.OneUp(InputStateManager.Inputs.Pad.DPadDirection.UP,
                InputStateManager.Inputs.Pad.DPadDirection.DOWN));
        }

        [Test]
        public void DPadUpIsResetProperly()
        {
            providerMock.SetupSequence(o => o.GetState(0))
                .Returns(GetDpadS(ButtonState.Pressed))
                .Returns(IdleState)
                .Returns(GetDpadS(ButtonState.Pressed));
            input.Update();
            Assert.IsFalse(input.Pad().Is.DPad.Up(InputStateManager.Inputs.Pad.DPadDirection.UP));
            input.Update();
            Assert.IsTrue(input.Pad().Is.DPad.Up(InputStateManager.Inputs.Pad.DPadDirection.UP));
            input.Update();
            Assert.IsFalse(input.Pad().Is.DPad.Up(InputStateManager.Inputs.Pad.DPadDirection.UP));
        }
    }
}