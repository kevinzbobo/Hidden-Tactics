using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class MonsterLoader
{
    public List<EnemyInstance> LoadEnemies(int level, int mission)
    {
        List<EnemyInstance> enemies = new List<EnemyInstance>();

        string path = Application.dataPath + Constants.EnemyJsonPath + level.ToString() + "_" + mission.ToString() + ".json";
        if (File.Exists(path))
        {

            MonsterSet monsterSet = JsonUtility.FromJson<MonsterSet>(File.ReadAllText(path));
            Debug.Log("Load Monsters data Success.");

            foreach (MonsterModel model in monsterSet.Monsters)
            {
                EnemyInstance enemy = new EnemyInstance(model);
                enemies.Add(enemy);
            }  
        }
        else
        {
            Debug.Log("Failed to load Characters. The characters json path cannot be found.");
        }


        return enemies;
    }

}

[Serializable]
public struct MonsterSet
{
    public MonsterModel[] Monsters;
}

[Serializable]
public struct MonsterModel
{
    public string Element;
    public string Img;
    public string Script;
    public int XPosition;
    public float Scale;
    public int HealthPoint;
    public CardPlayStrategy Strategy;
    public JMonsterCardProperties[] MonsterCards;
}

[Serializable]
public struct CardPlayStrategy
{
    public string Name;
    public JKeyValuePair[] Properties;
}

[Serializable]
public struct JMonsterCardProperties
{
    public JCardLocale[] locales;
    public string Element;
    public bool IsAgainstEnemy;
    public bool IsAOE;
    public JClassInfo[] PlayerEffects;
    public JClassInfo[] TargetEffects;
}

