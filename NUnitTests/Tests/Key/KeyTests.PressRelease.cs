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

namespace NUnitTests.Tests.Key
{
    [TestFixture]
    [Category("InputStateManager.Key.PressRelease")]
    public partial class KeyTests
    {
        [Test]
        public void KeyPressTriggers()
        {
            providerMock.SetupSequence(o => o.GetState())
                .Returns(new KeyboardState(Keys.A));
            input.Update();
            Assert.IsTrue(input.Key.Is.Press(Keys.A));
        }

        [Test]
        public void KeyPressIsResetWhenKeyIsReleased()
        {
            providerMock.SetupSequence(o => o.GetState())
                .Returns(new KeyboardState(Keys.A))
                .Returns(new KeyboardState());
            input.Update();
            Assert.IsTrue(input.Key.Is.Press(Keys.A));
            input.Update();
            Assert.IsFalse(input.Key.Is.Press(Keys.A));
        }

        [Test]
        public void KeyPressIsResetWhenKeyIsStillDown()
        {
            providerMock.SetupSequence(o => o.GetState())
                .Returns(new KeyboardState(Keys.A))
                .Returns(new KeyboardState(Keys.A));
            input.Update();
            Assert.IsTrue(input.Key.Is.Press(Keys.A));
            input.Update();
            Assert.IsFalse(input.Key.Is.Press(Keys.A));
        }

        [Test]
        public void KeyReleaseTriggers()
        {
            providerMock.SetupSequence(o => o.GetState())
                .Returns(new KeyboardState(Keys.A))
                .Returns(new KeyboardState(Keys.A))
                .Returns(new KeyboardState());
            input.Update();
            Assert.IsFalse(input.Key.Is.Release(Keys.A));
            input.Update();
            Assert.IsFalse(input.Key.Is.Release(Keys.A));
            input.Update();
            Assert.IsTrue(input.Key.Is.Release(Keys.A));
        }

        [Test]
        public void KeyReleaseIsResetProperly()
        {
            providerMock.SetupSequence(o => o.GetState())
                .Returns(new KeyboardState(Keys.A))
                .Returns(new KeyboardState())
                .Returns(new KeyboardState());
            input.Update();
            Assert.IsFalse(input.Key.Is.Release(Keys.A));
            input.Update();
            Assert.IsTrue(input.Key.Is.Release(Keys.A));
            input.Update();
            Assert.IsFalse(input.Key.Is.Press(Keys.A));
        }

        [Test]
        public void PressAndReleaseWorksWithSeveralKeys()
        {
            providerMock.SetupSequence(o => o.GetState())
                .Returns(new KeyboardState(Keys.A, Keys.B))
                .Returns(new KeyboardState(Keys.A));
            input.Update();
            Assert.IsTrue(input.Key.Is.Press(Keys.A, Keys.B));
            Assert.IsFalse(input.Key.Is.Release(Keys.A, Keys.B));
            input.Update();
            Assert.IsFalse(input.Key.Is.Press(Keys.A, Keys.B));
            Assert.IsFalse(input.Key.Is.Release(Keys.A, Keys.B));
            input.Update();
            Assert.IsFalse(input.Key.Is.Press(Keys.A, Keys.B));
            // A and B were not released at once.
            Assert.IsFalse(input.Key.Is.Release(Keys.A, Keys.B));
        }

        [Test]
        public void OnePressAndOneReleaseWorksWithSeveralKeys()
        {
            providerMock.SetupSequence(o => o.GetState())
                .Returns(new KeyboardState(Keys.A))
                .Returns(new KeyboardState(Keys.A, Keys.B))
                .Returns(new KeyboardState(Keys.A));
            input.Update();
            Assert.IsTrue(input.Key.Is.OnePress(Keys.A, Keys.B));
            Assert.IsFalse(input.Key.Is.OneRelease(Keys.A, Keys.B));
            input.Update();
            Assert.IsTrue(input.Key.Is.OnePress(Keys.A, Keys.B));
            Assert.IsFalse(input.Key.Is.OneRelease(Keys.A, Keys.B));
            input.Update();
            Assert.IsFalse(input.Key.Is.OnePress(Keys.A, Keys.B));
            Assert.IsTrue(input.Key.Is.OneRelease(Keys.A, Keys.B));
            input.Update();
            Assert.IsFalse(input.Key.Is.OnePress(Keys.A, Keys.B));
            Assert.IsTrue(input.Key.Is.OneRelease(Keys.A, Keys.B));
        }
    }
}