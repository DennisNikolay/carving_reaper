using Godot;
using System;
using Godot.Collections;

public class ReaperSFX : AudioStreamPlayer2D
{
    [Export] Array<AudioStream> slideSounds = new Array<AudioStream>();
    [Export] AudioStream idleSound;

    public override void _Ready()
    {
        base._Ready();
        Connect("finished", this, nameof(PlayLoopSound));
    }
    public void PlaySlideSound()
    {
        Stream = slideSounds[Game.RandomRange(0, slideSounds.Count)];
        Play();
    }

    public void PlayLoopSound()
    {
        Stream = idleSound;
        Play();
    }
}
