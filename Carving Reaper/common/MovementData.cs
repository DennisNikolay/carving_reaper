using Godot;
public struct MovementData
{
    public MovementData(float acceleration, float maxSpeedX, float maxSpeedY, float friction, float breakPoint, float breakAcceleration)
    {
        this.acceleration = acceleration;
        this.maxSpeedX = maxSpeedX;
        this.maxSpeedY = maxSpeedY;
        this.friction = friction;
        this.breakPoint = breakPoint;
        this.breakAcceleration = breakAcceleration;
        MoveDirection = Vector2.Up;
    }
    public Vector2 MoveDirection;

    public float acceleration;
    public float maxSpeedX;
    public float maxSpeedY;
    public float friction;
    public float breakPoint;
    public float breakAcceleration;

    public Vector2 GetBaseVelocity()
    {
        return Vector2.Up * acceleration;
    }
}