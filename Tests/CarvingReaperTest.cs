using NUnit.Framework;
using Godot;

namespace Tests
{
    public class CarvingReaperTest
    {

        [Test]
        public void TestMoveLeft()
        {
            BasicMovementDirectionTest(Vector2.Left, new Vector2(-20, 0));
        }

        [Test]
        public void TestMoveRight()
        {
            BasicMovementDirectionTest(Vector2.Right, new Vector2(20, 0));
        }

        [Test]
        public void TestMoveUp()
        {
            BasicMovementDirectionTest(Vector2.Up, new Vector2(0, -20));
        }

        [Test]
        public void TestMoveDown()
        {
            BasicMovementDirectionTest(Vector2.Down, new Vector2(0, 20));
        }

        [Test]
        public void TestMoveDoesNotStopImmediately()
        {

        }        

        protected void BasicMovementDirectionTest(Vector2 direction, Vector2 expectedVelocity){
            CarvingReaperMovementState carvingReaperMovementState = new CarvingReaperMovementState();
            Vector2 velocity = carvingReaperMovementState.MoveByInput(1, direction);
            Assert.AreEqual(
                expectedVelocity,
                velocity
            );
        }
    }
}