using Godot;
using System;

public class CarvingReaper : KinematicBody2D
{

    [Export]
    float speed  = 100;

    public override void _Ready()
    {

    }

    public override void _PhysicsProcess(float delta){
        GlobalPosition += delta * speed * GetUserMovementInput();
    }

    protected Vector2 GetUserMovementInput()
    {
        return new Vector2(
            GetRightMovement()-GetLeftMovement(),
            GetDownMovement()-GetUpMovement()
        );
    }

    protected float GetLeftMovement()
    {
        return Input.GetActionStrength("move_left");
    }

    protected float GetRightMovement()
    {
        return Input.GetActionStrength("move_right");
    }
    protected float GetUpMovement()
    {
        return Input.GetActionStrength("move_up");
    }
    protected float GetDownMovement()
    {
        return Input.GetActionStrength("move_down");
    }

}
