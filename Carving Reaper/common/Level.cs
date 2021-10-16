using Godot;
using System;

public class Level : Node2D
{
    Game game;

    [Export]
    NodePath scoreLabelPath;
    [Export]
    NodePath playerPath;

    Label scoreLabel;
    CarvingReaper player;
    float prevScorePos = 0;

    public override void _Ready()
    {
        scoreLabel = GetNode<Label>(scoreLabelPath);
        player = GetNode<CarvingReaper>(playerPath);
        game = new Game();
        game.gameOverEvent += Reload;
        game.changeScoreEvent += ChangeScoreText;
        ChangeScoreText(0);
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        IncreaseScoreOverTime(delta);
    }

    void IncreaseScoreOverTime(float delta)
    {
        if (-player.GlobalPosition.y > prevScorePos + 400)
        {
            prevScorePos = -player.GlobalPosition.y;
            Game.IncreaseScore(1);
        }

    }


    public void Reload()
    {
        GetTree().ReloadCurrentScene();
    }

    public void ChangeScoreText(int newScore)
    {
        if (scoreLabel == null)
            throw new Exception("Score Label is missing");

        scoreLabel.Text = $"{newScore}";
    }
}
