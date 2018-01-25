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
    [Category("InputStateManager.Pad.Was")]
    public partial class PadTests
    {
        [Test]
        public void WasDownGivesOldState()
        {
            providerMock.SetupSequence(o => o.GetState(0))
                .Returns(GetStateB(Buttons.A))
                .Returns(IdleState);
            input.Update();
            Assert.IsTrue(input.Pad().Is.Down(Buttons.A));
            Assert.IsFalse(input.Pad().Was.Down(Buttons.A));
            input.Update();
            Assert.IsFalse(input.Pad().Is.Down(Buttons.A));
            Assert.IsTrue(input.Pad().Was.Down(Buttons.A));
        }

        [Test]
        public void WasUpGivesOldState()
        {
            providerMock.SetupSequence(o => o.GetState(0))
                .Returns(GetStateB(Buttons.A))
                .Returns(IdleState);
            input.Update();
            Assert.IsFalse(input.Pad().Is.Up(Buttons.A));
            Assert.IsTrue(input.Pad().Was.Up(Buttons.A));
            input.Update();
            Assert.IsTrue(input.Pad().Is.Up(Buttons.A));
            Assert.IsFalse(input.Pad().Was.Up(Buttons.A));
        }

        [Test]
        public void WasDownGivesOldStateMultipleInputs()
        {
            providerMock.SetupSequence(o => o.GetState(0))
                .Returns(GetStateB(Buttons.A | Buttons.B))
                .Returns(IdleState);
            input.Update();
            Assert.IsTrue(input.Pad().Is.Down(Buttons.A, Buttons.B));
            Assert.IsFalse(input.Pad().Was.Down(Buttons.A, Buttons.B));
            input.Update();
            Assert.IsFalse(input.Pad().Is.Down(Buttons.A, Buttons.B));
            Assert.IsTrue(input.Pad().Was.Down(Buttons.A, Buttons.B));
        }

        [Test]
        public void WasUpGivesOldStateMultipleInputs()
        {
            providerMock.SetupSequence(o => o.GetState(0))
                .Returns(GetStateB(Buttons.A | Buttons.B))
                .Returns(IdleState);
            input.Update();
            Assert.IsFalse(input.Pad().Is.Up(Buttons.A, Buttons.B));
            Assert.IsTrue(input.Pad().Was.Up(Buttons.A, Buttons.B));
            input.Update();
            Assert.IsTrue(input.Pad().Is.Up(Buttons.A, Buttons.B));
            Assert.IsFalse(input.Pad().Was.Up(Buttons.A, Buttons.B));
        }
    }
}