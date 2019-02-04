using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CharacterLoader
{
    public PlayerInstance LoadUserCharacter()
    {

        string path = Application.dataPath + Constants.CharactersJsonPath;
        if (File.Exists(path))
        {

            JCharacterList characterSet = JsonUtility.FromJson<JCharacterList>(File.ReadAllText(path));
            Debug.Log("Load Characters data Success.");

            return new PlayerInstance(Constants.language_zh_tw, characterSet.Characters[0]);
        }
        else
        {
            Debug.Log("Failed to load Characters. The characters json path cannot be found.");
        }

        return null;
    }
}

[Serializable]
public struct JCharacterList
{
    public JCharacterProperties[] Characters;
}

[Serializable]
public struct JCharacterProperties
{
    public string Image;
    public string Element;
    public int MaxHp;
    public int Power;
    public int MaxPower;
    public int PowerAdd;
    public int DrawCount;
    public int GroupId;
    public int Armor;
    public int Defence;
    public JCharacterLocale[] locales;
    public int[] ElementCards;
    public int[] UltimateCards;
}

[Serializable]
public struct JCharacterLocale
{
    public string language;
    public JCharacterDisplyData data;
}

[Serializable]
public struct JCharacterDisplyData
{
    public string Name;
}
