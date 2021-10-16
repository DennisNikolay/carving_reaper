using Godot;
using System;

public class Victim : KinematicBody2D
{
    Vector2 velocity, direction;
    [Export] float baseMaxSpeed = 1000;
    [Export] float acceleration = 900;
    float maxSpeed;
    AnimationPlayer animationPlayer;
    bool dying = false;
    const string bloodFolder = "res://sprites/Blood/";

    public override void _Ready()
    {
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        baseMaxSpeed = baseMaxSpeed * (1.1f - Game.RandomValue * 0.2f);
        maxSpeed = baseMaxSpeed * (1.2f - 0.4f * Game.RandomValue);
        direction = Vector2.Up + (0.1f - 0.2f * Game.RandomValue) * Vector2.Right;
    }


    public override void _PhysicsProcess(float delta)
    {
        if (dying)
            return;

        base._PhysicsProcess(delta);

        velocity = velocity.MoveToward(direction * maxSpeed, delta * acceleration);
        velocity = MoveAndSlide(velocity);
    }

    public void OnHit()
    {
        if (dying)
            return;

        Game.IncreaseScore(20);
        float rng = Game.RandomValue;
        BloodSpawn();
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

    void BloodSpawn()
    {
        float rng = Game.RandomValue * 6 + 1;
        SpawnBlood(Mathf.FloorToInt(rng));
    }

    void SpawnBlood(int id)
    {
        string path = $"{bloodFolder}Blood0{id}.png";
        StreamTexture bloodTexture = ResourceLoader.Load<StreamTexture>(path);
        Sprite bloodSprite = new Sprite();
        bloodSprite.ZIndex = 1;
        AddChild(bloodSprite);
        bloodSprite.Texture = bloodTexture;
        bloodSprite.GlobalPosition = GlobalPosition;
    }
}
