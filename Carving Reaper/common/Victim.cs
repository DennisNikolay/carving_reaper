using Godot;
using System;

public class Victim : KinematicBody2D
{
    Vector2 velocity, direction;
    [Export] float baseMaxSpeed = 200;
    [Export] float acceleration = 500;
    float maxSpeed;
    AnimationPlayer animationPlayer;
    bool dying = false;

    public override void _Ready()
    {
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        maxSpeed = baseMaxSpeed * (1.2f - 0.4f * Game.RandomValue);
        direction = Vector2.Up + (0.1f - 0.2f * Game.RandomValue) * Vector2.Right;
    }


    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);

        velocity = velocity.MoveToward(direction * maxSpeed, delta * acceleration);
        velocity = MoveAndSlide(velocity);
    }

    public void OnHit()
    {
        if (dying)
            return;

        Game.IncreaseScore(100);
        float rng = Game.RandomValue;
        if (rng < 0.33f)
        {
            animationPlayer.Play("die1");
        }
        else if (rng < 0.66f)
        {
            animationPlayer.Play("die2");
        }
        else
        {
            animationPlayer.Play("die3");
        }
        dying = true;
    }
}
