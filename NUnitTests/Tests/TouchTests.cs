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
using InputStateManager;
using InputStateManager.Inputs.InputProviders.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using Moq;
using NUnit.Framework;

namespace NUnitTests.Tests
{
    [TestFixture]
    [Category("InputStateManager.Mouse")]
    public class TouchTests
    {
        private InputManager input;
        private Mock<ITouchInputProvider> providerMock;

        [SetUp]
        public void Setup()
        {
            providerMock = new Mock<ITouchInputProvider>();
            input = new InputManager(null, null, null, providerMock.Object);
        }

        [Test]
        public void MouseTests()
        {
            providerMock.SetupSequence(o => o.GetState())
                .Returns(new TouchCollection());
            providerMock.SetupSequence(o => o.GetDisplayOrientation())
                .Returns(DisplayOrientation.Default);
            providerMock.SetupSequence(o => o.GetDisplayHeight())
                .Returns(780);
            providerMock.SetupSequence(o => o.GetDisplayWidth())
                .Returns(1280);
            providerMock.SetupSequence(o => o.GetEnableMouseGestures())
                .Returns(true);
            providerMock.SetupSequence(o => o.GetEnableMouseTouchPoint())
                .Returns(true);
            providerMock.SetupSequence(o => o.GetEnabledGestures())
                .Returns(GestureType.FreeDrag | GestureType.Flick);
            providerMock.SetupSequence(o => o.GetIsGestureAvailable())
                .Returns(true);
            providerMock.SetupSequence(o => o.ReadGesture())
                .Returns(new GestureSample(GestureType.Hold, TimeSpan.FromMilliseconds(12), Vector2.Zero, Vector2.Zero,
                    Vector2.Zero, Vector2.Zero));
            input.Update();
            Assert.IsTrue(input.Touch.IsGestureAvailable);
        }
    }
}