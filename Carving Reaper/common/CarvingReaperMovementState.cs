using Godot;
using System;

public class CarvingReaperMovementState
{

    protected MovementData movementSettings;
    protected Vector2 velocity = Vector2.Zero;

    public CarvingReaperMovementState(MovementData initialMovementSettings)
    {
        movementSettings = initialMovementSettings;
    }

    public Vector2 MoveByInput(float delta, Vector2 moveInput)
    {
        if (moveInput.Length() > movementSettings.maxSpeed)
        {
            moveInput = moveInput * (movementSettings.maxSpeed / moveInput.Length());
        }

        float velocityX = moveInput.x;
        float velocityY = moveInput.y;


        velocity += delta * movementSettings.acceleration * new Vector2(
            velocityX,
            velocityY
        );

        if (velocity.y >= movementSettings.breakPoint)
        {
            velocity.y = (float)movementSettings.breakPoint;
        }

        if (moveInput.Length() == 0)
            velocity = velocity.MoveToward(movementSettings.GetBaseVelocity(), delta * movementSettings.friction);

        if (velocity.x > movementSettings.maxSpeed)
        {
            velocity.x = movementSettings.maxSpeed;
        }

        if (velocity.y > movementSettings.maxSpeed)
        {
            velocity.y = movementSettings.maxSpeed;
        }

        return velocity;
    }

    public Vector2 GetCurrentVelocity()
    {
        return velocity;
    }

}
