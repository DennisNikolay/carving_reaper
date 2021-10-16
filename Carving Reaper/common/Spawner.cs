using Godot;
using Godot.Collections;
using System;

public class Spawner : Node2D
{
    [Export]
    PackedScene victimScene;
    [Export]
    float spawnDelay;
    [Export] 
    public NodePath cameraPath;

    ObstacleSpawner spawner;
    
    Camera2D mainCam;
    
    [Export] 
    float spawnYOffset = -800;
    [Export] 
    int yVariation = 600;

    public override void _Ready(){
        InitializeSpawner();
        mainCam = GetNode<Camera2D>(cameraPath);
    }

    public void InitializeSpawner(){
        spawner = new ObstacleSpawner(ReadConfigFromJson("res://configs/defaultSpawnConfig.json"));
        Timer timer = new Timer();
        timer.WaitTime = spawnDelay;
        timer.SetWaitTime(3);
        timer.SetAutostart(true);
        timer.Connect("timeout", this, nameof(SpawnRandomObject));
        AddChild(timer);
    }

    public SpawnableConfig[] ReadConfigFromJson(string filepath)
    {
        File configFile = new File();
        configFile.Open(filepath, File.ModeFlags.Read);
        JSONParseResult parsedJson = JSON.Parse(configFile.GetAsText());
        SpawnableConfig[] spawnableConfigs = new SpawnableConfig[(parsedJson.Result as Godot.Collections.Array).Count];
        int lastIndex = 0;
        foreach(Dictionary dictionary in parsedJson.Result as Godot.Collections.Array){
            SpawnableConfig objConf = new SpawnableConfig();
            objConf.resource = (string)dictionary["resource"];
            objConf.weight = (float)dictionary["weight"];
            spawnableConfigs[lastIndex] = objConf;
            lastIndex++;
        }
        return spawnableConfigs;
    }

    public void SpawnRandomObject(){
        Random rng = new Random();
        int randomCount = rng.Next(1, 5);
        for(int i = 0; i < randomCount; i++){
            float xOffset = 900 - rng.Next(0, 1800);
            float smallYvariaton = yVariation/2 - rng.Next(0, yVariation);
            Node2D node2d = spawner.SpawnRandomObject(
                mainCam.GlobalPosition.x + xOffset,
                mainCam.GlobalPosition.y + spawnYOffset + smallYvariaton
            );
            if(node2d != null)
                AddChild(node2d);
        }
    }

}