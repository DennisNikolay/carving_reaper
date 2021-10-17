using Godot;
using System;

public class Highscore : Node
{

    public static void SetHighscore(int score){
        File file = new File();
        file.Open("user://highscore.txt", File.ModeFlags.Write);
        file.StoreString(score.ToString());
        file.Close();
    }

    public static string GetHighscore(){
        File file = new File();
        if(!file.FileExists("user://highscore.txt")){
            return "0";
        }
        file.Open("user://highscore.txt", File.ModeFlags.Read);
        string score = file.GetAsText();
        file.Close();
        return score;
    }

}
