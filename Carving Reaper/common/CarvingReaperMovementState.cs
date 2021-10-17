using Godot;
using System;

public class CarvingReaperMovementState
{

    protected MovementData movementSettings;
    protected Vector2 velocity = Vector2.Zero;

    public Vector2 Velocity { get { return velocity; } set { velocity = value; } }

    public CarvingReaperMovementState(MovementData initialMovementSettings)
    {
        movementSettings = initialMovementSettings;
    }

    public Vector2 MoveByInput(float delta, Vector2 moveInput)
    {
        moveInput.x = Mathf.Clamp(moveInput.x, -1, 1);
        moveInput.y = Mathf.Clamp(moveInput.y, -1, 1);

        float velocityX = moveInput.x;
        float velocityY = moveInput.y;

        velocity.x += delta * movementSettings.acceleration * velocityX * 1.4f;
        velocity.y += delta * (velocityY > 0 ? movementSettings.breakAcceleration : movementSettings.acceleration) * velocityY;

        if (velocity.y >= movementSettings.breakPoint)
        {
            velocity.y = (float)movementSettings.breakPoint;
        }

        if (moveInput.Length() == 0)
            velocity = velocity.MoveToward(movementSettings.GetBaseVelocity(), delta * movementSettings.friction);


        velocity.x = Mathf.Clamp(velocity.x, -movementSettings.maxSpeedX, movementSettings.maxSpeedX);
        velocity.y = Mathf.Clamp(velocity.y, -movementSettings.maxSpeedY, movementSettings.maxSpeedY);

        if (velocity.x > movementSettings.maxSpeedX)
        {
            velocity.x = movementSettings.maxSpeedX;
        }

        if (velocity.x < -movementSettings.maxSpeedX)
        {
            velocity.x = -movementSettings.maxSpeedX;
        }

        if (velocity.y > movementSettings.maxSpeedY)
        {
            velocity.y = movementSettings.maxSpeedY;
        }

        if (velocity.y < -movementSettings.maxSpeedY)
        {
            velocity.y = -movementSettings.maxSpeedY;
        }

        return velocity;
    }

    public Vector2 GetCurrentVelocity()
    {
        return velocity;
    }

}
