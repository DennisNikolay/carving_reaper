
using System;
using Godot;

public class ObstacleSpawner 
{

    SpawnableConfig[] spawnConfig;

    public ObstacleSpawner(SpawnableConfig[] spawnConfig){
        this.spawnConfig = spawnConfig;
        GD.Print(spawnConfig[0].resource);
    }

    public Node2D SpawnRandomObject(float x, float y){
        float totalWeight = 0;
        foreach (SpawnableConfig objConf in spawnConfig)
        {
            totalWeight += objConf.weight;
        }
        Random rng = new Random();
        float randomValue = rng.Next(100);
        foreach (SpawnableConfig objConf in spawnConfig)
        {
            if(randomValue <= (objConf.weight / totalWeight) * 100){
                return SpawnObjectIntoScene(objConf.resource, x, y);
            }
        }
        return null;
    }

    public Node2D SpawnObjectIntoScene(string resourcePath, float x, float y){
        PackedScene scene = ResourceLoader.Load<PackedScene>(resourcePath);
        Node2D spawnObj = scene.Instance() as Node2D;
        spawnObj.GlobalPosition = new Vector2(x, y);
        return spawnObj;
    }

}