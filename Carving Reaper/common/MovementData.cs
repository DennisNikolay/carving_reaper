using Godot;
public struct MovementData
{
    public MovementData(float acceleration, float maxSpeed, float friction, float breakPoint)
    {
        this.acceleration = acceleration;
        this.maxSpeed = maxSpeed;
        this.friction = friction;
        this.breakPoint = breakPoint;
        MoveDirection = Vector2.Up;
    }
    public Vector2 MoveDirection;

    public float acceleration;
    public float maxSpeed;
    public float friction;
    public float breakPoint;

    public Vector2 GetBaseVelocity()
    {
        return Vector2.Up * acceleration;
    }
}