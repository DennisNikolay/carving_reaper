using NUnit.Framework;
using Godot;

namespace Tests
{
    public class CarvingReaperTest
    {

        [Test]
        public void TestMoveLeft()
        {
            CarvingReaperMovementState carvingReaperMovementState = new CarvingReaperMovementState();
            Vector2 velocity = carvingReaperMovementState.MoveByInput(1, Vector2.Left);
            Assert.AreEqual(
                new Vector2(-20, 0),
                velocity
            );
        }
    }
}