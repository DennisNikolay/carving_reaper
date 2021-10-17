using Godot;
using System;

public class Level : Node2D
{
    Game game;

    [Export]
    NodePath scoreLabelPath;
    [Export]
    NodePath playerPath;
    [Export]
    NodePath gameOverPath;

    Label scoreLabel;
    CarvingReaper player;
    float prevScorePos = 0;
    bool gameOverActive = false;
    Sprite snowSprite;

    public override void _Ready()
    {
        scoreLabel = GetNode<Label>(scoreLabelPath);
        snowSprite = GetNode<Sprite>("Snow");
        snowSprite.Texture = ResourceLoader.Load<Texture>($"res://sprites/Level/SchneebodenTile{Game.RandomRange(1, 3)}.png");
        player = GetNode<CarvingReaper>(playerPath);
        game = new Game();
        game.gameOverEvent += GameOver;
        game.changeScoreEvent += ChangeScoreText;
        ChangeScoreText(0);
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        IncreaseScoreOverTime(delta);

        //Back to main Menu
        if (Input.IsKeyPressed((int)KeyList.Escape))
        {
            GetTree().ChangeScene("res://MainMenu.tscn");
        }
    }

    void IncreaseScoreOverTime(float delta)
    {
        if (-player.GlobalPosition.y > prevScorePos + 400)
        {
            prevScorePos = -player.GlobalPosition.y;
            Game.IncreaseScore(1);
        }

    }

    void GameOver()
    {
        GameOver gameOver = GetNode<GameOver>(gameOverPath);
        gameOver.ShowGameOver();
        Engine.TimeScale = 0;
        gameOverActive = true;
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

    public override void _Input(InputEvent @event)
    {
        if (!gameOverActive)
            return;

        //On Game Over screen reload when any key is pressed
        if ((@event is InputEventKey || @event is InputEventJoypadButton) && @event.IsPressed())
        {
            Engine.TimeScale = 1;
            Reload();
            gameOverActive = false;
        }
    }
}
