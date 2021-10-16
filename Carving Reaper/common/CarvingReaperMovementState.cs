using Godot;
using System;

public class CarvingReaperMovementState
{

    [Export]
    float speed = 20;

    [Export] 
    float maxSpeed = 200;

    [Export] 
    float friction = 10;

    Vector2 velocity = Vector2.Zero;

    public Vector2 MoveByInput(float delta, Vector2 moveInput)
    {
        if (velocity.Length() < maxSpeed)
            velocity += delta * speed * moveInput;

        if (moveInput.Length() == 0)
            velocity = velocity.MoveToward(Vector2.Zero, delta * friction);
        
        return velocity;
    }

}
