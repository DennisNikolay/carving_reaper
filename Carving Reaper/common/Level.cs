using Godot;
using System;

public class Level : Node2D
{
    Game game;
    [Export] NodePath scoreLabelPath;
    Label scoreLabel;
    float scoreTimer = 0;
    public override void _Ready()
    {
        scoreLabel = GetNode<Label>(scoreLabelPath);
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
        scoreTimer += delta;
        if (scoreTimer > 0.8f)
        {
            scoreTimer = 0;
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
