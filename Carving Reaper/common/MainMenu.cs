using Godot;
using System;

public class MainMenu : Control
{
    Button playButton, creditsButton;
    TextureRect credits;
    bool creditsActive = false;
    public override void _Ready()
    {
        playButton = GetNode<Button>("PlayButton");
        creditsButton = GetNode<Button>("CreditsButton");
        credits = GetNode<TextureRect>("Credits");
        playButton.GrabFocus();
    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        if (!creditsActive && credits.Visible)
        {
            credits.Visible = false;
            creditsButton.Disabled = false;
        }
    }

    public void Play()
    {
        GetTree().ChangeScene("res://MainLevel.tscn");
    }

    public void Quit()
    {
        GetTree().Quit();
    }

    public void Credits()
    {
        if (creditsActive)
            return;

        credits.Visible = true;
        creditsActive = true;
        creditsButton.Disabled = true;
    }

    public override void _Input(InputEvent @event)
    {
        if (!creditsActive)
            return;

        if ((@event is InputEventKey || @event is InputEventJoypadButton) && @event.IsPressed())
        {
            creditsActive = false;
        }
    }
}
