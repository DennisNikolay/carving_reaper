using Godot;
using System;
using Godot.Collections;
public class SnowTrail : Node2D
{
    Line2D line2D;
    Vector2 targetPoint;
    Vector2 curPos;
    public override void _Ready()
    {
        line2D = GetNode<Line2D>("SnowTrail");
        line2D.SetAsToplevel(true);
        curPos = GlobalPosition;
    }

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        curPos.y = GlobalPosition.y;
        curPos.x = Mathf.Lerp(curPos.x, GlobalPosition.x, delta * 30);
        line2D.AddPoint(curPos);
        if (line2D.Points.Length > 100)
            line2D.RemovePoint(0);
    }
}
