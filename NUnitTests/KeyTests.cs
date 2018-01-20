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

namespace NUnitTests
{
    [TestFixture]
    [Category("InputStateManager")]
    public class KeyTests
    {
        private InputManager input;
        private Mock<IKeyInputProvider> providerMock;

        [SetUp]
        public void Setup()
        {
            providerMock = new Mock<IKeyInputProvider>();
            providerMock.SetupSequence(o => o.GetState())
                .Returns(new KeyboardState(Keys.A, Keys.LeftShift, Keys.LeftControl, Keys.LeftAlt))
                .Returns(new KeyboardState(Keys.B)).Returns(new KeyboardState(Keys.C))
                .Returns(new KeyboardState(Keys.D));

            input = new InputManager(providerMock.Object, null, null, null);
        }

        [Test]
        public void TestMocking()
        {
            input.Update(); 
            Assert.IsTrue(input.Key.Is.Down(Keys.A));
            input.Update();
            Assert.IsTrue(input.Key.Is.Down(Keys.B));
            input.Update();
            Assert.IsTrue(input.Key.Is.Down(Keys.C));
            input.Update();
            Assert.IsTrue(input.Key.Is.Down(Keys.D));
        }
    }
}