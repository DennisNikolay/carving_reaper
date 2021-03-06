using Godot;
using System;
using Godot.Collections;

public class Victim : KinematicBody2D
{
    Vector2 velocity, direction;
    [Export] float baseMaxSpeed = 1000;
    [Export] float acceleration = 900;
    [Export] Array<AudioStream> deadSounds = new Array<AudioStream>();
    [Export] Array<Texture> skins = new Array<Texture>();
    float maxSpeed;
    AnimationPlayer animationPlayer;
    Sprite characterSprite;
    bool dying = false;
    const string bloodFolder = "res://sprites/Blood/";
    public bool WasSlashed = true;
    AudioStreamPlayer2D deadSoundSource;

    Vector2 targetAvoid;
    bool avoiding;
    CollisionShape2D collisionShape;
    float avoidSum;
    const string idleAnim = "idle", slideStartAnim = "slide_start", slideAnim = "slide", slideEndAnim = "slide_end";


    public override void _Ready()
    {
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
        deadSoundSource = GetNode<AudioStreamPlayer2D>("DeadSound");
        characterSprite = GetNode<Sprite>("Sprite");
        characterSprite.Texture = skins[Game.RandomRange(0, skins.Count)];
        maxSpeed = baseMaxSpeed * (1.1f - Game.RandomValue * 0.2f);
        direction = Vector2.Up + (0.05f - 0.1f * Game.RandomValue) * Vector2.Right;
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        animationPlayer.Connect("animation_finished", this, nameof(OnAnimationFinished));
    }


    public override void _PhysicsProcess(float delta)
    {
        if (dying)
            return;

        base._PhysicsProcess(delta);
        CheckForObstaclesAndAvoid();

        if (avoiding)
        {
            avoidSum += delta;
            velocity = velocity.MoveToward(targetAvoid, delta * acceleration * 2f);
        }
        else
        {
            avoidSum = 0;
            velocity = velocity.MoveToward(direction * maxSpeed, delta * acceleration);
        }

        velocity = MoveAndSlide(velocity);
        HandleSlideAnimation(velocity);
    }

    public void OnHit()
    {
        if (dying)
            return;

        Game.IncreaseScore(20);
        PlayDeadAnimation();
    }

    public void CheckForObstaclesAndAvoid()
    {
        Physics2DDirectSpaceState spaceState = GetWorld2d().DirectSpaceState;
        Dictionary rayCastMiddle = spaceState.IntersectRay(GlobalPosition, GlobalPosition + velocity * 100, null, 4);
        Dictionary rayCastLeft = spaceState.IntersectRay(GlobalPosition - new Vector2(150, 0), GlobalPosition - new Vector2(150, 0) + velocity * 100, null, 4);
        Dictionary rayCastRight = spaceState.IntersectRay(GlobalPosition + new Vector2(150, 0), GlobalPosition + new Vector2(150, 0) + velocity * 100, null, 4);

        if ((rayCastLeft.Count > 0
            || rayCastMiddle.Count > 0
            || rayCastRight.Count > 0
        ) && avoidSum < 1f
        )
        {
            targetAvoid = new Vector2(-100000, GlobalPosition.y - 1000);
            avoiding = true;
        }
        else
        {
            avoiding = false;
        }
    }

    void OnAnimationFinished(string name)
    {
        switch (name)
        {
            case slideEndAnim:
                animationPlayer.Play(idleAnim);
                break;
            case slideStartAnim:
                animationPlayer.Play(slideAnim);
                break;
            default:
                break;
        }
    }

    void HandleSlideAnimation(Vector2 velocity)
    {
        if (Mathf.Abs(velocity.x) > baseMaxSpeed * 0.1f)
        {
            if (animationPlayer.CurrentAnimation == idleAnim)
            {
                animationPlayer.Play(slideStartAnim);
                characterSprite.FlipH = velocity.x < 0;
            }
        }
        else
        {
            if (animationPlayer.CurrentAnimation != idleAnim)
                animationPlayer.Play(slideEndAnim);
        }
    }


    public void PlayDeadAnimation()
    {
        collisionShape.SetDeferred("disabled", true);
        if (WasSlashed)
        {
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
        }
        else
        {
            int die = Game.RandomRange(1, 9);
            if (die > 4)
                die = 4;
            animationPlayer.Play($"die{die}");
        }



        dying = true;
    }

    void BloodSpawn()
    {
        if (!WasSlashed)
            return;

        SpawnBlood(Game.RandomRange(1, 7));
        deadSoundSource.Stream = deadSounds[Game.RandomRange(0, deadSounds.Count)];
        deadSoundSource.VolumeDb = Game.RandomRange(0.0f, 5.0f);
        deadSoundSource.PitchScale = Game.RandomRange(0.9f, 1.1f);
        deadSoundSource.Play();
    }

    void SpawnBlood(int id)
    {
        string path = $"{bloodFolder}Blood0{id}.png";
        StreamTexture bloodTexture = ResourceLoader.Load<StreamTexture>(path);
        Sprite bloodSprite = new Sprite();
        bloodSprite.ZIndex = 1;
        bloodSprite.ZAsRelative = false;
        AddChild(bloodSprite);
        bloodSprite.Texture = bloodTexture;
        bloodSprite.GlobalPosition = GlobalPosition;
    }

}