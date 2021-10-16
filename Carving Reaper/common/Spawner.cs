using Godot;
using Godot.Collections;
using System;

public class Spawner : Node2D
{
    [Export]
    PackedScene victimScene;
    [Export]
    float enemySpawnDelay = 4;
    [Export]
    float obstacleMaxSpawnDelay = 5;
    [Export]
    public NodePath cameraPath;
    [Export]
    float spawnYOffset = -800;
    [Export]
    int yVariation = 600;
    [Export]
    int enemySpawnDelayFactor = 50;
    [Export]
    int obstacleMaxSimultaneously = 5;

    ObstacleSpawner obstacleSpawner;
    ObstacleSpawner enemySpawner;
    Camera2D mainCam;
    Timer obstacleSpawnTimer;

    public override void _Ready()
    {
        InitializeObstacleSpawner();
        InitializeEnemySpawner();
        mainCam = GetNode<Camera2D>(cameraPath);
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
    }

    public void InitializeObstacleSpawner()
    {
        obstacleSpawner = new ObstacleSpawner(ReadConfigFromJson("res://configs/defaultSpawnConfig.json"));
        Timer timer = new Timer();
        timer.WaitTime = obstacleMaxSpawnDelay;
        timer.Autostart = true;
        timer.Connect("timeout", this, nameof(SpawnRandomObject));
        AddChild(timer);
        obstacleSpawnTimer = timer;
    }

    public void InitializeEnemySpawner()
    {
        enemySpawner = new ObstacleSpawner(ReadConfigFromJson("res://configs/enemySpawnConfig.json"));
        Timer timer = new Timer();
        timer.WaitTime = enemySpawnDelay;
        timer.Autostart = true;
        timer.Connect("timeout", this, nameof(SpawnRandomEnemy));
        AddChild(timer);
    }

    public SpawnableConfig[] ReadConfigFromJson(string filepath)
    {
        File configFile = new File();
        configFile.Open(filepath, File.ModeFlags.Read);
        JSONParseResult parsedJson = JSON.Parse(configFile.GetAsText());
        SpawnableConfig[] spawnableConfigs = new SpawnableConfig[(parsedJson.Result as Godot.Collections.Array).Count];
        int lastIndex = 0;
        foreach (Dictionary dictionary in parsedJson.Result as Godot.Collections.Array)
        {
            SpawnableConfig objConf = new SpawnableConfig();
            objConf.resource = (string)dictionary["resource"];
            objConf.weight = (float)dictionary["weight"];
            spawnableConfigs[lastIndex] = objConf;
            lastIndex++;
        }
        return spawnableConfigs;
    }

    public void SpawnRandomObject()
    {
        SpawnHelper(obstacleSpawner, obstacleMaxSimultaneously);
        obstacleSpawnTimer.WaitTime = (obstacleMaxSpawnDelay - (Game.Score / enemySpawnDelayFactor));
    }

    public void SpawnRandomEnemy()
    {
        SpawnHelper(enemySpawner, 1);
    }

    public void SpawnHelper(ObstacleSpawner spawner, int maxSimultaneously)
    {
        Random rng = new Random();
        int randomCount = rng.Next(1, maxSimultaneously);
        for (int i = 0; i < randomCount; i++)
        {
            float xOffset = 900 - rng.Next(0, 1800);
            float smallYvariaton = yVariation / 2 - rng.Next(0, yVariation);
            Node2D node2d = spawner.SpawnRandomObject(
                mainCam.GlobalPosition.x + xOffset,
                mainCam.GlobalPosition.y + spawnYOffset + smallYvariaton
            );
            if (node2d != null)
                AddChild(node2d);
        }
    }

}