using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constants
{

    // theme
    public const uint COLOR_FIRE = 0xffff0000;
    public const uint COLOR_WOOD = 0xff009933;
    public const uint COLOR_WATER = 0xff0099ff;
    public const uint COLOR_EARTH = 0xff993300;
    public const uint COLOR_METAL = 0xffffcc00;

    //language
    public const string language_en_us = "en-GB";
    public const string language_zh_tw = "zh-tw";

    public const string AudioPath = "Music/BackgroundMusic";

    //level
    public const string LevelJsonPath = "/Resources/Json/Levels/Level";
    public const string BackgroundImagePath = "Image/Backgrounds";

    //element card
    public const string CardJsonPath = "/Resources/Json/elementCards.json";
    public const string CardImagePath = "Image/Cards/ElementCards";
    public const float CardEnlargeRatio = 1.3f;

    //ultimate card
    public const string UltimateCardJsonPath = "/Resources/Json/ultimateCards.json";
    public const string UltimateCardImagePath = "Image/Cards/UltimateCards/Cards";
    public const string UltimateCardIconPath = "Image/Cards/UltimateCards/Icons";
    public const string UltimateCardElementImagePath = "Image/Cards/UltimateCards/Elements";

    //characters
    public const string CharactersJsonPath = "/Resources/Json/characters.json";
    public const string CharacterStandardImagePath = "Image/Player/Standard";
    public const string CharacterCartoonImagePath = "Image/Player/Cartoon";

    //Enemy
    public const string EnemyJsonPath = "/Resources/Json/Monsters/Monsters";
    public const string EnemyImagePath = "Image/Monster";

    public const string ElementNameFire = "Fire";
    public const string ElementNameWood = "Wood";
    public const string ElementNameWater = "Water";
    public const string ElementNameMetal = "Metal";
    public const string ElementNameEarth = "Earth";

    public enum Elements { Metal, Wood, Water, Fire, Earth };

}
