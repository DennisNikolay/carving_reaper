using Godot;
using System;

public class CarvingReaper : KinematicBody2D
{
    CarvingReaperMovementState movementState;
    HitBox hitBox;

    public CarvingReaper()
    {
        movementState = new CarvingReaperMovementState();
    }

    public override void _Ready()
    {
        base._Ready();
        hitBox = GetNode<HitBox>("HitBox");
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        if (IsAttackJustPressed())
        {
            hitBox?.Attack();
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        Vector2 velocityAfterInput = movementState.MoveByInput(delta, GetUserMovementInput());
        KinematicCollision2D obstacle = MoveAndCollide(velocityAfterInput);
    }


    public void HandleObstacleCollision()
    {
        GD.Print("Obstacle hit");
        Game.GameOver();
    }

    protected Vector2 GetUserMovementInput()
    {
        return new Vector2(
            GetRightMovement() - GetLeftMovement(),
            GetDownMovement() - GetUpMovement()
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

    protected bool IsAttackJustPressed()
    {
        return Input.IsActionJustPressed("attack");
    }
}
