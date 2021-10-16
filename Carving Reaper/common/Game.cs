using Godot;
using System;
public class Game
{
    static Random random = new Random();
    public static float RandomValue { get { return (float)random.NextDouble(); } }
    public static Game instance;
    public static int Score;

    public delegate void GameOverEvent();
    public GameOverEvent gameOverEvent;
    public delegate void ChangeScoreEvent(int newScore);
    public ChangeScoreEvent changeScoreEvent;

    public Game()
    {
        instance = this;
        Score = 0;
    }

    public static void GameOver()
    {
        instance.gameOverEvent?.Invoke();
    }

    public static void IncreaseScore(int amount)
    {
        Score += amount;
        instance.changeScoreEvent?.Invoke(Score);
    }
}