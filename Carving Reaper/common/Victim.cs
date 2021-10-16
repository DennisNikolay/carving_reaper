using Godot;
using System;

public class Victim : KinematicBody2D
{
    Vector2 velocity, direction;
    [Export] float baseMaxSpeed = 200;
    [Export] float acceleration = 500;
    float maxSpeed;

    public override void _Ready()
    {
        maxSpeed = baseMaxSpeed * (1.2f - 0.4f * Game.RandomValue);
        direction = Vector2.Right.Rotated(2 * Mathf.Pi * Game.RandomValue);

    }

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);

        velocity = velocity.MoveToward(direction * maxSpeed, delta * acceleration);
        velocity = MoveAndSlide(velocity);
    }

    public void OnHit()
    {
        Game.IncreaseScore(100);
        QueueFree();
    }
}
