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

    [Export]
    bool debug = false;

    protected CarvingReaperMovementState movementState;
    protected Line2D debugArrow;
    HitBox hitBox;
    AnimationPlayer animationPlayer;
    Sprite characterSprite;
    Node2D shadow;

    public static CarvingReaper activePlayer;

    const string attackAnim = "attack", idleAnim = "idle", slideStartAnim = "slide_start", slideAnim = "slide", slideEndAnim = "slide_end";

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
        CallDeferred(nameof(AddDebugArrow));
        hitBox = GetNode<HitBox>("HitBox");
        shadow = GetNode<Node2D>("Shadow");
        characterSprite = GetNode<Sprite>("CharacterSprite");
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        animationPlayer.Connect("animation_finished", this, nameof(OnAnimationFinished));
    }

    public override void _Process(float delta)
    {
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
                shadow.Scale = new Vector2(characterSprite.FlipH ? 1 : -1, 1);
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
        Vector2 velocityAfterInput = movementState.MoveByInput(delta, GetUserMovementInput());
        movementState.Velocity = MoveAndSlide(velocityAfterInput);
        HandleSlideAnimation(velocityAfterInput);
        if (debug && debugArrow != null)
            DrawDebugLine(delta);
    }


    public void HandleObstacleCollision()
    {
        Game.GameOver();
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

    protected bool IsAttackJustPressed()
    {
        return Input.IsActionJustPressed("attack");
    }
}
