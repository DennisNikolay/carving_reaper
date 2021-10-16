using Godot;
using System;

public class CarvingReaper : KinematicBody2D
{

    [Export]
    float acceleration = 20;

    [Export]
    float maxSpeed = 200;

    [Export]
    float friction = 10;

    [Export]
    float baseSpeed = 5;

    [Export]
    bool debug = false;

    protected CarvingReaperMovementState movementState;
    protected Line2D debugArrow;

    public CarvingReaper()
    {
        movementState = new CarvingReaperMovementState(
            new MovementData(acceleration, maxSpeed, friction, baseSpeed)
        );
    }

    public override void _Ready()
    {
        base._Ready();
        CallDeferred(nameof(AddDebugArrow));
    }


    public override void _PhysicsProcess(float delta)
    {
        Vector2 velocityAfterInput = movementState.MoveByInput(delta, GetUserMovementInput());
        KinematicCollision2D obstacle = MoveAndCollide(velocityAfterInput);
        if (debug && debugArrow != null)
            DrawDebugLine(delta);
    }


    public void HandleObstacleCollision()
    {
        GD.Print("Obstacle hit");
        GetTree().ReloadCurrentScene();
    }

    protected void AddDebugArrow()
    {
        debugArrow = new Line2D();
        GetParent().AddChild(debugArrow);
    }
    protected void DrawDebugLine(float delta)
    {
        Vector2 velocityTarget = GlobalPosition + 10 * movementState.MoveByInput(delta, GetUserMovementInput());
        debugArrow.RemovePoint(0);
        debugArrow.RemovePoint(1);
        debugArrow.AddPoint(GlobalPosition, 0);
        debugArrow.AddPoint(velocityTarget, 1);
        debugArrow.Update();
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

}
