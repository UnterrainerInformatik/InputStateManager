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
    [Category("InputStateManager.Pad.Triggers")]
    public partial class PadTests
    {
        private const float EPSILON = float.Epsilon;

        private static GamePadState GetTriggers(float l, float r) => new GamePadState(
            new GamePadThumbSticks(Vector2.Zero, Vector2.Zero),
            new GamePadTriggers(l, r), new GamePadButtons(0),
            new GamePadDPad(ButtonState.Released, ButtonState.Released, ButtonState.Released, ButtonState.Released));

        [Test]
        public void TriggersWork()
        {
            providerMock.SetupSequence(o => o.GetState(0))
                .Returns(IdleState)
                .Returns(GetTriggers(0.2f, 0.3f))
                .Returns(GetTriggers(0.3f, 0.4f))
                .Returns(IdleState);
            input.Update();
            Assert.AreEqual(0f, input.Pad().Is.Triggers.Left, EPSILON);
            Assert.AreEqual(0f, input.Pad().Is.Triggers.Right, EPSILON);
            input.Update();
            Assert.AreEqual(0.2f, input.Pad().Is.Triggers.Left, EPSILON);
            Assert.AreEqual(0.3f, input.Pad().Is.Triggers.Right, EPSILON);
            input.Update();
            Assert.AreEqual(0.3f, input.Pad().Is.Triggers.Left, EPSILON);
            Assert.AreEqual(0.4f, input.Pad().Is.Triggers.Right, EPSILON);
            input.Update();
            Assert.AreEqual(0f, input.Pad().Is.Triggers.Left, EPSILON);
            Assert.AreEqual(0f, input.Pad().Is.Triggers.Right, EPSILON);
        }

        [Test]
        public void WasTriggersWork()
        {
            providerMock.SetupSequence(o => o.GetState(0))
                .Returns(IdleState)
                .Returns(GetTriggers(0.2f, 0.3f))
                .Returns(GetTriggers(0.3f, 0.4f))
                .Returns(IdleState);
            input.Update();
            input.Update();
            Assert.AreEqual(0f, input.Pad().Was.Triggers.Left, EPSILON);
            Assert.AreEqual(0f, input.Pad().Was.Triggers.Right, EPSILON);
            input.Update();
            Assert.AreEqual(0.2f, input.Pad().Was.Triggers.Left, EPSILON);
            Assert.AreEqual(0.3f, input.Pad().Was.Triggers.Right, EPSILON);
            input.Update();
            Assert.AreEqual(0.3f, input.Pad().Was.Triggers.Left, EPSILON);
            Assert.AreEqual(0.4f, input.Pad().Was.Triggers.Right, EPSILON);
        }

        [Test]
        public void TriggersDeltasWork()
        {
            providerMock.SetupSequence(o => o.GetState(0))
                .Returns(IdleState)
                .Returns(GetTriggers(0.2f, 0.4f))
                .Returns(IdleState)
                .Returns(GetTriggers(0.3f, 0.5f))
                .Returns(GetTriggers(0.3f, 0.8f));
            input.Update();
            Assert.AreEqual(0f, input.Pad().Is.Triggers.LeftDelta, EPSILON);
            Assert.AreEqual(0f, input.Pad().Is.Triggers.RightDelta, EPSILON);
            input.Update();
            Assert.AreEqual(0.2f, input.Pad().Is.Triggers.LeftDelta, EPSILON);
            Assert.AreEqual(0.4f, input.Pad().Is.Triggers.RightDelta, EPSILON);
            input.Update();
            Assert.AreEqual(-0.2f, input.Pad().Is.Triggers.LeftDelta, EPSILON);
            Assert.AreEqual(-0.4f, input.Pad().Is.Triggers.RightDelta, EPSILON);
            input.Update();
            Assert.AreEqual(0.3f, input.Pad().Is.Triggers.LeftDelta, EPSILON);
            Assert.AreEqual(0.5f, input.Pad().Is.Triggers.RightDelta, EPSILON);
            input.Update();
            Assert.AreEqual(0f, input.Pad().Is.Triggers.LeftDelta, EPSILON);
            Assert.AreEqual(0.3f, input.Pad().Is.Triggers.RightDelta, EPSILON);
        }
    }
}