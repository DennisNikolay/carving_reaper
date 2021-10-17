using Godot;
using System;

public class Obstacle : StaticBody2D
{

    public float deltaSum;

    public override void _Process(float delta)
    {
        deltaSum += delta;
    }

    public void PlayerEntered(Node body)
    {
        if (body is Obstacle)
        {
            if (body == this)
                return;
            body.QueueFree();
        }
        if (body is Victim)
        {
            if (deltaSum <= 0.2f)
            {
                body.QueueFree();
            }
            else
            {
                (body as Victim).WasSlashed = false;
                (body as Victim).PlayDeadAnimation();
            }
        }
        if (body is CarvingReaper)
        {
            (body as CarvingReaper).HandleObstacleCollision();
        }
    }

}
