using Godot;
using System;
using Godot.Collections;
public class Victim : KinematicBody2D
{
    Vector2 velocity, direction;
    [Export] float baseMaxSpeed = 1000;
    [Export] float acceleration = 900;
    [Export] Array<Texture> skins = new Array<Texture>();
    float maxSpeed;
    AnimationPlayer animationPlayer;
    Sprite characterSprite;
    bool dying = false;
    const string bloodFolder = "res://sprites/Blood/";

    public override void _Ready()
    {
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        characterSprite = GetNode<Sprite>("Sprite");
        characterSprite.Texture = skins[Game.RandomRange(0, skins.Count)];
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
        SpawnBlood(Game.RandomRange(1, 6));
    }

    void SpawnBlood(int id)
    {
        string path = $"{bloodFolder}Blood0{id}.png";
        StreamTexture bloodTexture = ResourceLoader.Load<StreamTexture>(path);
        Sprite bloodSprite = new Sprite();
        bloodSprite.ZIndex = 0;
        AddChild(bloodSprite);
        bloodSprite.Texture = bloodTexture;
        bloodSprite.GlobalPosition = GlobalPosition;
    }
}
