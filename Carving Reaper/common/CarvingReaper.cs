using Godot;
using System;

public class CarvingReaper : KinematicBody2D
{
    [Export]
    float acceleration = 1200.0f;
    [Export]
    float breakAcceleration = 1800.0f;

    [Export]
    float maxSpeedX = 1400f, maxSpeedY = 2000f;

    [Export]
    float friction = 900;

    [Export]
    float breakPoint = -900;
    protected CarvingReaperMovementState movementState;
    protected Line2D debugArrow;
    HitBox hitBox;
    AnimationPlayer animationPlayer;
    Sprite characterSprite;
    Node2D pivot;

    public static CarvingReaper activePlayer;

    const string attackAnim = "attack", idleAnim = "idle", slideStartAnim = "slide_start", slideAnim = "slide", slideEndAnim = "slide_end";
    bool dying = false;
    public CarvingReaper()
    {
        movementState = new CarvingReaperMovementState(
            new MovementData(acceleration, maxSpeedX, maxSpeedY, friction, breakPoint, breakAcceleration)
        );
    }

    public override void _Ready()
    {
        base._Ready();
        activePlayer = this;
        hitBox = GetNode<HitBox>("HitBox");
        pivot = GetNode<Node2D>("Pivot");
        characterSprite = GetNode<Sprite>("CharacterSprite");
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        animationPlayer.Connect("animation_finished", this, nameof(OnAnimationFinished));
    }

    public override void _Process(float delta)
    {
        if (dying)
            return;

        base._Process(delta);
        if (IsAttackJustPressed())
        {
            hitBox?.Attack();
            animationPlayer.Play(attackAnim);
        }
    }

    void OnAnimationFinished(string name)
    {
        switch (name)
        {
            case attackAnim:
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
        if (animationPlayer.CurrentAnimation == attackAnim)
            return;

        if (Mathf.Abs(velocity.x) > maxSpeedX * 0.3f)
        {
            if (animationPlayer.CurrentAnimation == idleAnim)
            {
                animationPlayer.Play(slideStartAnim);
                characterSprite.FlipH = velocity.x < 0;
                pivot.Scale = new Vector2(characterSprite.FlipH ? 1 : -1, 1);
            }
        }
        else
        {
            if (animationPlayer.CurrentAnimation != idleAnim)
                animationPlayer.Play(slideEndAnim);
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        if (dying)
            return;

        Vector2 velocityAfterInput = movementState.MoveByInput(delta, GetUserMovementInput());
        movementState.Velocity = MoveAndSlide(velocityAfterInput);
        HandleSlideAnimation(velocityAfterInput);
    }


    public void HandleObstacleCollision()
    {
        if (dying)
            return;

        animationPlayer.Play("die");
        dying = true;
    }

    public void EndGame()
    {
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
