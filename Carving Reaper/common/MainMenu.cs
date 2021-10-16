using Godot;
using System;

public class MainMenu : Control
{
    Button playButton;
    public override void _Ready()
    {
        playButton = GetNode<Button>("PlayButton");
        playButton.GrabFocus();
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
