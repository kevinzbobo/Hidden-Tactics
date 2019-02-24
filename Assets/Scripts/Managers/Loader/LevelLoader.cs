using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public static class LevelLoader
{
    public static LevelData LoadLevelData(int level, int mission)
    {
        string path = Application.dataPath + Constants.LevelJsonPath + level.ToString() + "_" + mission.ToString() + ".json";
        //Debug.Log(path);
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            return JsonUtility.FromJson<LevelData>(json);
        }
        else
        {
            Debug.Log("Load Level Data Failed. The json path cannot be found.");
        }
        return new LevelData();
    }
}

[Serializable]
public struct LevelSet
{
    public LevelData[] Levels;
}

[Serializable]
public struct LevelData
{
    public int ID;
    public int Level;
    public int Mission;
    public string Name;
    public string BackgroundImage;
    public string Music;
    public List<MonsterModel> Monster;
    public LevelReward Reward;
}

[Serializable]
public struct LevelReward
{
    public int Money;
    public List<ToolReward> Tools;
}

[Serializable]
public struct ToolReward
{
    public int Id;
    public float Probability;
}
