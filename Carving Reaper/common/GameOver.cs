using Godot;
using System;

public class GameOver : Control
{
    [Export] NodePath scoreLabelPath;

    Label gameScoreLabel, overScoreLabel, highScoreLabel;
    bool gameOverActive = false;

    public override void _Ready()
    {
        gameScoreLabel = GetNode<Label>(scoreLabelPath);
        overScoreLabel = GetNode<Label>("ScoreText");
        highScoreLabel = GetNode<Label>("HighScoreText");
        Visible = false;
    }

    public void ShowGameOver()
    {
        Visible = true;
        gameScoreLabel.Visible = false;
        highScoreLabel.Text = $"High Score: {Highscore.GetHighscore()}";
        overScoreLabel.Text = $"Your Score: {Game.Score}";
    }
}
