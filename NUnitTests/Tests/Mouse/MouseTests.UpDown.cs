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
using Microsoft.Xna.Framework.Input;
using Moq;
using NUnit.Framework;

namespace NUnitTests.Tests.Mouse
{
    [TestFixture]
    [Category("InputStateManager.Mouse.UpDown")]
    public partial class MouseTests
    {
        private InputManager input;
        private Mock<IMouseInputProvider> providerMock;

        [SetUp]
        public void Setup()
        {
            providerMock = new Mock<IMouseInputProvider>();
            input = new InputManager(null, providerMock.Object, null, null);
        }

        private static MouseState IdleState => new MouseState(0, 0, 0, ButtonState.Released, ButtonState.Released,
            ButtonState.Released, ButtonState.Released, ButtonState.Released);

        private static MouseState GetStateS(ButtonState left) => new MouseState(0, 0, 0, left, ButtonState.Released,
            ButtonState.Released, ButtonState.Released, ButtonState.Released);

        private static MouseState GetStateM(ButtonState left, ButtonState right) => new MouseState(0, 0, 0, left,
            ButtonState.Released, right, ButtonState.Released, ButtonState.Released);

        [Test]
        public void ButtonDownTriggers()
        {
            providerMock.SetupSequence(o => o.GetState())
                .Returns(GetStateS(ButtonState.Pressed));
            input.Update();
            Assert.IsTrue(input.Mouse.Is.Down(InputStateManager.Inputs.Mouse.Button.LEFT));
        }

        [Test]
        public void ButtonDownMultipleInputsTriggers()
        {
            providerMock.SetupSequence(o => o.GetState())
                .Returns(GetStateM(ButtonState.Pressed, ButtonState.Pressed));
            input.Update();
            Assert.IsTrue(input.Mouse.Is.Down(InputStateManager.Inputs.Mouse.Button.LEFT,
                InputStateManager.Inputs.Mouse.Button.RIGHT));
        }

        [Test]
        public void ButtonOneDownTriggers()
        {
            providerMock.SetupSequence(o => o.GetState())
                .Returns(GetStateS(ButtonState.Pressed));
            input.Update();
            Assert.IsTrue(input.Mouse.Is.OneDown(InputStateManager.Inputs.Mouse.Button.LEFT,
                InputStateManager.Inputs.Mouse.Button.RIGHT));
        }

        [Test]
        public void ButtonDownIsResetProperly()
        {
            providerMock.SetupSequence(o => o.GetState())
                .Returns(GetStateS(ButtonState.Pressed))
                .Returns(GetStateS(ButtonState.Pressed))
                .Returns(IdleState);
            input.Update();
            Assert.IsTrue(input.Mouse.Is.Down(InputStateManager.Inputs.Mouse.Button.LEFT));
            input.Update();
            Assert.IsTrue(input.Mouse.Is.Down(InputStateManager.Inputs.Mouse.Button.LEFT));
            input.Update();
            Assert.IsFalse(input.Mouse.Is.Down(InputStateManager.Inputs.Mouse.Button.LEFT));
        }

        [Test]
        public void ButtonUpTriggers()
        {
            providerMock.SetupSequence(o => o.GetState())
                .Returns(GetStateS(ButtonState.Pressed))
                .Returns(IdleState);
            input.Update();
            Assert.IsFalse(input.Mouse.Is.Up(InputStateManager.Inputs.Mouse.Button.LEFT));
            input.Update();
            Assert.IsTrue(input.Mouse.Is.Up(InputStateManager.Inputs.Mouse.Button.LEFT));
        }

        [Test]
        public void ButtonUpMultipleInputsTriggers()
        {
            providerMock.SetupSequence(o => o.GetState())
                .Returns(GetStateM(ButtonState.Pressed, ButtonState.Pressed))
                .Returns(IdleState);
            input.Update();
            Assert.IsFalse(input.Mouse.Is.Up(InputStateManager.Inputs.Mouse.Button.LEFT,
                InputStateManager.Inputs.Mouse.Button.RIGHT));
            input.Update();
            Assert.IsTrue(input.Mouse.Is.Up(InputStateManager.Inputs.Mouse.Button.LEFT,
                InputStateManager.Inputs.Mouse.Button.RIGHT));
        }

        [Test]
        public void ButtonOneUpTriggers()
        {
            providerMock.SetupSequence(o => o.GetState())
                .Returns(GetStateS(ButtonState.Pressed))
                .Returns(IdleState);
            input.Update();
            Assert.IsTrue(input.Mouse.Is.OneUp(InputStateManager.Inputs.Mouse.Button.LEFT,
                InputStateManager.Inputs.Mouse.Button.RIGHT));
            input.Update();
            Assert.IsTrue(input.Mouse.Is.OneUp(InputStateManager.Inputs.Mouse.Button.LEFT,
                InputStateManager.Inputs.Mouse.Button.RIGHT));
        }

        [Test]
        public void ButtonUpIsResetProperly()
        {
            providerMock.SetupSequence(o => o.GetState())
                .Returns(GetStateS(ButtonState.Pressed))
                .Returns(IdleState)
                .Returns(GetStateS(ButtonState.Pressed));
            input.Update();
            Assert.IsFalse(input.Mouse.Is.Up(InputStateManager.Inputs.Mouse.Button.LEFT));
            input.Update();
            Assert.IsTrue(input.Mouse.Is.Up(InputStateManager.Inputs.Mouse.Button.LEFT));
            input.Update();
            Assert.IsFalse(input.Mouse.Is.Up(InputStateManager.Inputs.Mouse.Button.LEFT));
        }
    }
}