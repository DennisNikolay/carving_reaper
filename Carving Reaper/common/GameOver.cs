using Godot;
using System;

public class GameOver : Control
{
    [Export] NodePath scoreLabelPath;

    Label gameScoreLabel, overScoreLabel;
    bool gameOverActive = false;

    public override void _Ready()
    {
        gameScoreLabel = GetNode<Label>(scoreLabelPath);
        overScoreLabel = GetNode<Label>("ScoreText");
        Visible = false;
    }

    public void ShowGameOver()
    {
        Visible = true;
        gameScoreLabel.Visible = false;
        overScoreLabel.Text = $"Score: {Game.Score}";
    }
}
