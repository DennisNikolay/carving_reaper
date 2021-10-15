using Godot;
using System;

public class CarvingReaper : KinematicBody2D
{

    [Export]
    float speed = 20;

    [Export] float maxSpeed = 200;
    [Export] float friction = 10;

    Vector2 velocity;

    public override void _Ready()
    {

    }

    public override void _PhysicsProcess(float delta)
    {
        Vector2 moveInput = GetUserMovementInput();

        if (velocity.Length() < maxSpeed)
            velocity += delta * speed * moveInput;

        if (moveInput.Length() == 0)
            velocity = velocity.MoveToward(Vector2.Zero, delta * friction);

        KinematicCollision2D obstacle = MoveAndCollide(velocity);
    }

    public void HandleObstacleCollision(){
        GD.Print("Obstacle hit");
        GetTree().ReloadCurrentScene();
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
