using Godot;
using System;

public class MainMenu : Control
{
    public override void _Ready()
    {

    }

    public void Play()
    {
        GetTree().ChangeScene("res://MainLevel.tscn");
    }

    public void Quit()
    {
        GetTree().Quit();
    }
}
