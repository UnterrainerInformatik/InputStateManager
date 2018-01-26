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

using Microsoft.Xna.Framework.Input;
using NUnit.Framework;

namespace NUnitTests.Tests.Pad
{
    [TestFixture]
    [Category("InputStateManager.Pad.DPad.PressRelease")]
    public partial class PadTests
    {
        [Test]
        public void DPadPressTriggers()
        {
            providerMock.SetupSequence(o => o.GetState(0))
                .Returns(GetDpadS(ButtonState.Pressed));
            input.Update();
            Assert.IsTrue(input.Pad().Is.DPad.Press(InputStateManager.Inputs.Pad.DPadDirection.UP));
        }

        [Test]
        public void DPadPressIsResetWhenKeyIsReleased()
        {
            providerMock.SetupSequence(o => o.GetState(0))
                .Returns(GetDpadS(ButtonState.Pressed))
                .Returns(IdleState);
            input.Update();
            Assert.IsTrue(input.Pad().Is.DPad.Press(InputStateManager.Inputs.Pad.DPadDirection.UP));
            input.Update();
            Assert.IsFalse(input.Pad().Is.DPad.Press(InputStateManager.Inputs.Pad.DPadDirection.UP));
        }

        [Test]
        public void DPadPressIsResetWhenKeyIsStillDown()
        {
            providerMock.SetupSequence(o => o.GetState(0))
                .Returns(GetDpadS(ButtonState.Pressed))
                .Returns(GetDpadS(ButtonState.Pressed));
            input.Update();
            Assert.IsTrue(input.Pad().Is.DPad.Press(InputStateManager.Inputs.Pad.DPadDirection.UP));
            input.Update();
            Assert.IsFalse(input.Pad().Is.DPad.Press(InputStateManager.Inputs.Pad.DPadDirection.UP));
        }

        [Test]
        public void DPadReleaseTriggers()
        {
            providerMock.SetupSequence(o => o.GetState(0))
                .Returns(GetDpadS(ButtonState.Pressed))
                .Returns(GetDpadS(ButtonState.Pressed))
                .Returns(IdleState);
            input.Update();
            Assert.IsFalse(input.Pad().Is.DPad.Release(InputStateManager.Inputs.Pad.DPadDirection.UP));
            input.Update();
            Assert.IsFalse(input.Pad().Is.DPad.Release(InputStateManager.Inputs.Pad.DPadDirection.UP));
            input.Update();
            Assert.IsTrue(input.Pad().Is.DPad.Release(InputStateManager.Inputs.Pad.DPadDirection.UP));
        }

        [Test]
        public void DPadReleaseIsResetProperly()
        {
            providerMock.SetupSequence(o => o.GetState(0))
                .Returns(GetDpadS(ButtonState.Pressed))
                .Returns(IdleState)
                .Returns(IdleState);
            input.Update();
            Assert.IsFalse(input.Pad().Is.DPad.Release(InputStateManager.Inputs.Pad.DPadDirection.UP));
            input.Update();
            Assert.IsTrue(input.Pad().Is.DPad.Release(InputStateManager.Inputs.Pad.DPadDirection.UP));
            input.Update();
            Assert.IsFalse(input.Pad().Is.DPad.Press(InputStateManager.Inputs.Pad.DPadDirection.UP));
        }

        [Test]
        public void DPadPressAndReleaseWorksWithSeveralKeys()
        {
            providerMock.SetupSequence(o => o.GetState(0))
                .Returns(GetDpadM(ButtonState.Pressed, ButtonState.Pressed))
                .Returns(GetDpadS(ButtonState.Pressed));
            input.Update();
            Assert.IsTrue(input.Pad().Is.DPad.Press(InputStateManager.Inputs.Pad.DPadDirection.UP,
                InputStateManager.Inputs.Pad.DPadDirection.DOWN));
            Assert.IsFalse(input.Pad().Is.DPad.Release(InputStateManager.Inputs.Pad.DPadDirection.UP,
                InputStateManager.Inputs.Pad.DPadDirection.DOWN));
            input.Update();
            Assert.IsFalse(input.Pad().Is.DPad.Press(InputStateManager.Inputs.Pad.DPadDirection.UP,
                InputStateManager.Inputs.Pad.DPadDirection.DOWN));
            Assert.IsFalse(input.Pad().Is.DPad.Release(InputStateManager.Inputs.Pad.DPadDirection.UP,
                InputStateManager.Inputs.Pad.DPadDirection.DOWN));
            input.Update();
            Assert.IsFalse(input.Pad().Is.DPad.Press(InputStateManager.Inputs.Pad.DPadDirection.UP,
                InputStateManager.Inputs.Pad.DPadDirection.DOWN));
            // A and B were not released at once.
            Assert.IsFalse(input.Pad().Is.DPad.Release(InputStateManager.Inputs.Pad.DPadDirection.UP,
                InputStateManager.Inputs.Pad.DPadDirection.DOWN));
        }

        [Test]
        public void DPadOnePressAndOneReleaseWorksWithSeveralKeys()
        {
            providerMock.SetupSequence(o => o.GetState(0))
                .Returns(GetDpadS(ButtonState.Pressed))
                .Returns(GetDpadM(ButtonState.Pressed, ButtonState.Pressed))
                .Returns(GetDpadS(ButtonState.Pressed));
            input.Update();
            Assert.IsTrue(input.Pad().Is.DPad.OnePress(InputStateManager.Inputs.Pad.DPadDirection.UP,
                InputStateManager.Inputs.Pad.DPadDirection.DOWN));
            Assert.IsFalse(input.Pad().Is.DPad.OneRelease(InputStateManager.Inputs.Pad.DPadDirection.UP,
                InputStateManager.Inputs.Pad.DPadDirection.DOWN));
            input.Update();
            Assert.IsTrue(input.Pad().Is.DPad.OnePress(InputStateManager.Inputs.Pad.DPadDirection.UP,
                InputStateManager.Inputs.Pad.DPadDirection.DOWN));
            Assert.IsFalse(input.Pad().Is.DPad.OneRelease(InputStateManager.Inputs.Pad.DPadDirection.UP,
                InputStateManager.Inputs.Pad.DPadDirection.DOWN));
            input.Update();
            Assert.IsFalse(input.Pad().Is.DPad.OnePress(InputStateManager.Inputs.Pad.DPadDirection.UP,
                InputStateManager.Inputs.Pad.DPadDirection.DOWN));
            Assert.IsTrue(input.Pad().Is.DPad.OneRelease(InputStateManager.Inputs.Pad.DPadDirection.UP,
                InputStateManager.Inputs.Pad.DPadDirection.DOWN));
            input.Update();
            Assert.IsFalse(input.Pad().Is.DPad.OnePress(InputStateManager.Inputs.Pad.DPadDirection.UP,
                InputStateManager.Inputs.Pad.DPadDirection.DOWN));
            Assert.IsTrue(input.Pad().Is.DPad.OneRelease(InputStateManager.Inputs.Pad.DPadDirection.UP,
                InputStateManager.Inputs.Pad.DPadDirection.DOWN));
        }
    }
}