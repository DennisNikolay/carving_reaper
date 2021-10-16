using NUnit.Framework;
using Godot;

namespace Tests
{
    public class CarvingReaperTest
    {

        protected MovementData testMovementState = new MovementData(20, 200, 10, 0);

        [Test]
        public void TestMoveLeft()
        {
            BasicMovementDirectionTest(Vector2.Left, new Vector2(-20, -10));
        }

        [Test]
        public void TestMoveRight()
        {
            BasicMovementDirectionTest(Vector2.Right, new Vector2(20, -10));
        }

        [Test]
        public void TestMoveUp()
        {
            BasicMovementDirectionTest(Vector2.Up, new Vector2(0, -20));
        }

        [Test]
        public void TestMoveDown()
        {
            BasicMovementDirectionTest(Vector2.Down, new Vector2(0, -10));
        }

        [Test]
        public void TestMoveDoesNotStopImmediately()
        {
            CarvingReaperMovementState carvingReaperMovementState = new CarvingReaperMovementState(testMovementState);
            carvingReaperMovementState.MoveByInput(1, Vector2.Up);
            Vector2 velocityAfterStop = carvingReaperMovementState.MoveByInput(1, Vector2.Zero);
            Assert.AreNotEqual(
                Vector2.Zero,
                velocityAfterStop
            );
            Assert.Less(
                Vector2.Up.Length(),
                velocityAfterStop.Length()
            );
        }

        [Test]
        public void TestMaxSpeed()
        {
            CarvingReaperMovementState carvingReaperMovementState = new CarvingReaperMovementState(
                new MovementData(1, 200, 10, 1)
            );
            Vector2 velocity = carvingReaperMovementState.MoveByInput(1, new Vector2(0, 400));
            Assert.AreEqual(
                new Vector2(0, -10),
                velocity
            );
        }

        protected void BasicMovementDirectionTest(Vector2 direction, Vector2 expectedVelocity)
        {
            CarvingReaperMovementState carvingReaperMovementState = new CarvingReaperMovementState(testMovementState);
            Vector2 velocity = carvingReaperMovementState.MoveByInput(1, direction);
            Assert.AreEqual(
                expectedVelocity,
                velocity
            );
        }
    }
}