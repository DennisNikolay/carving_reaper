using Godot;
using System;

public class CarvingReaper : KinematicBody2D
{

    CarvingReaperMovementState movementState;

   public CarvingReaper(){
       movementState = new CarvingReaperMovementState();
   }

    public override void _PhysicsProcess(float delta)
    {
        Vector2 velocityAfterInput = movementState.MoveByInput(delta, GetUserMovementInput());
        KinematicCollision2D obstacle = MoveAndCollide(velocityAfterInput);
    }


    public void HandleObstacleCollision()
    {
        GD.Print("Obstacle hit");
        GetTree().ReloadCurrentScene();
    }

    public Vector2 GetUserMovementInput()
    {
        return new Vector2(
            GetRightMovement() - GetLeftMovement(),
            GetDownMovement() - GetUpMovement()
        );
    }

    public float GetLeftMovement()
    {
        return Input.GetActionStrength("move_left");
    }

    public float GetRightMovement()
    {
        return Input.GetActionStrength("move_right");
    }
    public float GetUpMovement()
    {
        return Input.GetActionStrength("move_up");
    }
    public float GetDownMovement()
    {
        return Input.GetActionStrength("move_down");
    }

}
