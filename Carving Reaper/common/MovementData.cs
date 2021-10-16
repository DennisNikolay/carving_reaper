using Godot;
public struct MovementData
{
    public MovementData(float acceleration, float maxSpeed, float friction, float baseSpeed)
    {
        this.acceleration = acceleration;
        this.maxSpeed = maxSpeed;
        this.friction = friction;
        this.breakFriction = 3 * friction;
        MoveDirection = Vector2.Up;
    }
    public Vector2 MoveDirection;

    public float acceleration;
    public float maxSpeed;
    public float friction;
    public float breakFriction;

    public Vector2 GetBaseVelocity()
    {
        return Vector2.Up * breakFriction;
    }
}