using Godot;
using Godot.Collections;

public class Victim : KinematicBody2D
{
    Vector2 velocity, direction;
    [Export] float baseMaxSpeed = 200;
    [Export] float acceleration = 500;
    float maxSpeed;
    AnimationPlayer animationPlayer;
    bool dying = false;

    Vector2 targetAvoid;
    object avoiding;

    float deltaSum;

    public override void _Ready()
    {
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        maxSpeed = baseMaxSpeed * (1.2f - 0.4f * Game.RandomValue);
        direction = Vector2.Up + (0.1f - 0.2f * Game.RandomValue) * Vector2.Right;
    }


    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        CheckForObstaclesAndAvoid();
        velocity = velocity.MoveToward(direction * maxSpeed, delta * acceleration);
        
        if(avoiding != null){
            velocity = velocity.MoveToward(targetAvoid, delta * acceleration * 1.5f);
            deltaSum += delta;
            if(deltaSum > 1){
                avoiding = null;
                deltaSum = 0;
            }
        }

        velocity = MoveAndSlide(velocity);

    }

    public void OnHit()
    {
        if (dying)
            return;

        Game.IncreaseScore(100);
        PlayDeadAnimation();
    }

    public void CheckForObstaclesAndAvoid(){
        Physics2DDirectSpaceState spaceState = GetWorld2d().DirectSpaceState;
        Dictionary rayCastResult = spaceState.IntersectRay(GlobalPosition, GlobalPosition + velocity * 100, null, 4);
        if(rayCastResult.Count > 0
            && rayCastResult["collider"] is Obstacle 
            && avoiding != rayCastResult["collider"]
        ){
            avoiding = rayCastResult["collider"];
            if(GlobalPosition.x < 2500){
                targetAvoid = new Vector2(20000, 0);
            }else{
                targetAvoid = new Vector2(20000, 0);
            }
        }
    }

    public void PlayDeadAnimation(){
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
