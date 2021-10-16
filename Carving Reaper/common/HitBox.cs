using Godot;
using System;
using System.Threading.Tasks;

public class HitBox : Area2D
{
    CollisionShape2D collisionShape;
    public override void _Ready()
    {
        collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
        Connect("body_entered", this, nameof(HitBoxEnter));
        Connect("body_exited", this, nameof(HitBoxExit));
        Disable();
    }

    public void Enable()
    {
        if (collisionShape == null)
            throw new Exception("Collision Shape is missing!");
        collisionShape.Disabled = false;
    }

    public void Disable()
    {
        if (collisionShape == null)
            throw new Exception("Collision Shape is missing!");
        collisionShape.Disabled = true;
    }

    public async void Attack(float interval = 0.8f)
    {
        Enable();
        await Task.Delay(TimeSpan.FromSeconds(interval));
        Disable();
    }

    //Set with signals
    public void HitBoxEnter(Node body)
    {
        if (!(body is Victim) || !IsInstanceValid(body))
            return;

        Victim victim = body as Victim;
        victim?.OnHit();
    }

    public void HitBoxExit(Node body)
    {

    }
}
