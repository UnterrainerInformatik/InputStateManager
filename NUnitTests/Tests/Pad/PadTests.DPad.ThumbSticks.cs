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
    [Category("InputStateManager.Pad.DPad.ThumbSticks")]
    public partial class PadTests
    {
        private static GamePadState GetThumb(Vector2 l, Vector2 r) => new GamePadState(
            new GamePadThumbSticks(l, r), new GamePadTriggers(0f, 0f), new GamePadButtons(0),
            new GamePadDPad(ButtonState.Released, ButtonState.Released, ButtonState.Released, ButtonState.Released));
        
        [Test]
        public void ThumbSticksWork()
        {
            providerMock.SetupSequence(o => o.GetState(0))
                .Returns(IdleState)
                .Returns(GetThumb(Vector2.One, Vector2.One));
            input.Update();
            Assert.AreEqual(Vector2.Zero, input.Pad().Is.ThumbSticks.Left);
            Assert.AreEqual(Vector2.Zero, input.Pad().Is.ThumbSticks.Right);
            input.Update();
            Assert.AreEqual(Vector2.One, input.Pad().Is.ThumbSticks.Left);
            Assert.AreEqual(Vector2.One, input.Pad().Is.ThumbSticks.Right);
        }

        [Test]
        public void WasThumbSticksWork()
        {
            providerMock.SetupSequence(o => o.GetState(0))
                .Returns(IdleState)
                .Returns(GetThumb(Vector2.One, Vector2.One))
                .Returns(IdleState);
            input.Update();
            input.Update();
            Assert.AreEqual(Vector2.Zero, input.Pad().Was.ThumbSticks.Left);
            Assert.AreEqual(Vector2.Zero, input.Pad().Was.ThumbSticks.Right);
            input.Update();
            Assert.AreEqual(Vector2.One, input.Pad().Was.ThumbSticks.Left);
            Assert.AreEqual(Vector2.One, input.Pad().Was.ThumbSticks.Right);
        }

        [Test]
        public void ThumbSticksDeltasWork()
        {
            providerMock.SetupSequence(o => o.GetState(0))
                .Returns(IdleState)
                .Returns(GetThumb(Vector2.One, Vector2.One))
                .Returns(GetThumb(new Vector2(0.5f, 0.5f), new Vector2(0.4f, 0.4f)))
                .Returns(GetThumb(Vector2.One, Vector2.One))
                .Returns(IdleState);
            input.Update();
            Assert.AreEqual(Vector2.Zero, input.Pad().Is.ThumbSticks.LeftDelta);
            Assert.AreEqual(Vector2.Zero, input.Pad().Is.ThumbSticks.RightDelta);
            input.Update();
            Assert.AreEqual(Vector2.One, input.Pad().Is.ThumbSticks.LeftDelta);
            Assert.AreEqual(Vector2.One, input.Pad().Is.ThumbSticks.RightDelta);
            input.Update();
            Assert.AreEqual(new Vector2(-0.5f, -0.5f), input.Pad().Is.ThumbSticks.LeftDelta);
            Assert.AreEqual(new Vector2(-0.6f, -0.6f), input.Pad().Is.ThumbSticks.RightDelta);
            input.Update();
            Assert.AreEqual(new Vector2(0.5f, 0.5f), input.Pad().Is.ThumbSticks.LeftDelta);
            Assert.AreEqual(new Vector2(0.6f, 0.6f), input.Pad().Is.ThumbSticks.RightDelta);
            input.Update();
            Assert.AreEqual(-Vector2.One, input.Pad().Is.ThumbSticks.LeftDelta);
            Assert.AreEqual(-Vector2.One, input.Pad().Is.ThumbSticks.RightDelta);
        }
    }
}