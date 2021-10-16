using Godot;
using System;

public class Obstacle : StaticBody2D
{

    public void PlayerEntered(Node body){
        if(body is Obstacle){
            if(body == this)
                return;
            body.QueueFree();
        }
        if(body is CarvingReaper){
            (body as CarvingReaper).HandleObstacleCollision();
        }
    }

}
