using Godot;
using System;

public class Obstacle : StaticBody2D
{

    public override void _Ready()
    {

    }

    public void PlayerEntered(CarvingReaper body){
        if(!(body is CarvingReaper)){
            throw new Exception("Game cannot handle colliding things you stupid");
        }
        body.HandleObstacleCollision();
    }

    public void ObstacleCollision(Obstacle obstacle){
        obstacle.QueueFree();
    }

}
