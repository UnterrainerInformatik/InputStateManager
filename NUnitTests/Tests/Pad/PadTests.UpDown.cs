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

using InputStateManager;
using InputStateManager.Inputs.InputProviders.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Moq;
using NUnit.Framework;

namespace NUnitTests.Tests.Pad
{
    [TestFixture]
    [Category("InputStateManager.Pad.UpDown")]
    public partial class PadTests
    {
        private InputManager input;
        private Mock<IPadInputProvider> providerMock;

        [SetUp]
        public void Setup()
        {
            providerMock = new Mock<IPadInputProvider>();
            input = new InputManager(null, null, providerMock.Object, null);
        }

        private static GamePadState IdleState => new GamePadState(new GamePadThumbSticks(Vector2.Zero, Vector2.Zero),
            new GamePadTriggers(0f, 0f), new GamePadButtons(0),
            new GamePadDPad(ButtonState.Released, ButtonState.Released, ButtonState.Released, ButtonState.Released));

        private static GamePadState GetStateB(Buttons buttons) => new GamePadState(
            new GamePadThumbSticks(Vector2.Zero, Vector2.Zero),
            new GamePadTriggers(0f, 0f), new GamePadButtons(buttons),
            new GamePadDPad(ButtonState.Released, ButtonState.Released, ButtonState.Released, ButtonState.Released));

        [Test]
        public void ButtonDownTriggers()
        {
            providerMock.SetupSequence(o => o.GetState(0))
                .Returns(GetStateB(Buttons.A));
            input.Update();
            Assert.IsTrue(input.Pad().Is.Down(Buttons.A));
        }

        [Test]
        public void ButtonDownMultipleInputsTriggers()
        {
            providerMock.SetupSequence(o => o.GetState(0))
                .Returns(GetStateB(Buttons.A | Buttons.B));
            input.Update();
            Assert.IsTrue(input.Pad().Is.Down(Buttons.A, Buttons.B));
        }

        [Test]
        public void ButtonOneDownTriggers()
        {
            providerMock.SetupSequence(o => o.GetState(0))
                .Returns(GetStateB(Buttons.A));
            input.Update();
            Assert.IsTrue(input.Pad().Is.OneDown(Buttons.A, Buttons.B));
        }

        [Test]
        public void ButtonDownIsResetProperly()
        {
            providerMock.SetupSequence(o => o.GetState(0))
                .Returns(GetStateB(Buttons.A))
                .Returns(GetStateB(Buttons.A))
                .Returns(IdleState);
            input.Update();
            Assert.IsTrue(input.Pad().Is.Down(Buttons.A));
            input.Update();
            Assert.IsTrue(input.Pad().Is.Down(Buttons.A));
            input.Update();
            Assert.IsFalse(input.Pad().Is.Down(Buttons.A));
        }

        [Test]
        public void ButtonUpTriggers()
        {
            providerMock.SetupSequence(o => o.GetState(0))
                .Returns(GetStateB(Buttons.A))
                .Returns(IdleState);
            input.Update();
            Assert.IsFalse(input.Pad().Is.Up(Buttons.A));
            input.Update();
            Assert.IsTrue(input.Pad().Is.Up(Buttons.A));
        }

        [Test]
        public void ButtonUpMultipleInputsTriggers()
        {
            providerMock.SetupSequence(o => o.GetState(0))
                .Returns(GetStateB(Buttons.A | Buttons.B))
                .Returns(IdleState);
            input.Update();
            Assert.IsFalse(input.Pad().Is.Up(Buttons.A, Buttons.B));
            input.Update();
            Assert.IsTrue(input.Pad().Is.Up(Buttons.A, Buttons.B));
        }

        [Test]
        public void ButtonOneUpTriggers()
        {
            providerMock.SetupSequence(o => o.GetState(0))
                .Returns(GetStateB(Buttons.A))
                .Returns(IdleState);
            input.Update();
            Assert.IsTrue(input.Pad().Is.OneUp(Buttons.A, Buttons.B));
            input.Update();
            Assert.IsTrue(input.Pad().Is.OneUp(Buttons.A, Buttons.B));
        }

        [Test]
        public void ButtonUpIsResetProperly()
        {
            providerMock.SetupSequence(o => o.GetState(0))
                .Returns(GetStateB(Buttons.A))
                .Returns(IdleState)
                .Returns(GetStateB(Buttons.A));
            input.Update();
            Assert.IsFalse(input.Pad().Is.Up(Buttons.A));
            input.Update();
            Assert.IsTrue(input.Pad().Is.Up(Buttons.A));
            input.Update();
            Assert.IsFalse(input.Pad().Is.Up(Buttons.A));
        }
    }
}