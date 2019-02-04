using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CardLoader
{
    /* 讀取用戶卡組,轉換成卡牌物件*/
    public List<ElementCardInstance> LoadUserCards(int[] ids)
    {
        //讀取所有卡組
        List<ElementCardInstance> userCardList = new List<ElementCardInstance>();
        string path = Application.dataPath + Constants.CardJsonPath;
        if (File.Exists(path))
        {
            JBasicCardListRoot cardSet = JsonUtility.FromJson<JBasicCardListRoot>(File.ReadAllText(path));
            Debug.Log("Load Card data Success.");


            //組成Default咭組
            foreach (int cardID in ids)
            {
                foreach (JElementCardProperties cardData in cardSet.Cards)
                {
                    if (cardData.ID == cardID)
                    {
                        ElementCardInstance cardInstance = new ElementCardInstance(Constants.language_zh_tw, cardData);
                        userCardList.Add(cardInstance);
                    }
                }
            }

        }
        else
        {
            Debug.LogError("Failed to load cards. The card json path cannot be found.");
        }

        return userCardList;
    }

    public List<UltimateCardInstance> LoadUltimateCards(int[] ids)
    {
        //讀取所有卡組
        List<UltimateCardInstance> userCardList = new List<UltimateCardInstance>();
        string path = Application.dataPath + Constants.CardJsonPath;
        if (File.Exists(path))
        {
            JUltimateCardListRoot cardSet = JsonUtility.FromJson<JUltimateCardListRoot>(File.ReadAllText(path));
            Debug.Log("Load Ultimate Card data Success.");

            //load user cards
            foreach (int cardID in ids)
            {
                foreach (JUltimateCardProperties cardData in cardSet.Cards)
                {
                    if (cardData.ID == cardID)
                    {
                        UltimateCardInstance cardInstance = new UltimateCardInstance(Constants.language_zh_tw, cardData);
                        userCardList.Add(cardInstance);
                    }
                }
            }

        }
        else
        {
            Debug.LogError("Failed to load cards. The card json path cannot be found.");
        }

        return userCardList;
    }

    private void PrintCard(JElementCardProperties cardData)
    {
        Debug.Log("Card Title: " + cardData.locales[0].data.Title);
        Debug.Log("Card Description: " + cardData.locales[0].data.Description);
        Debug.Log("Card Element: " + cardData.Element);
        Debug.Log("Card Image: " + cardData.Image);
        Debug.Log("Card Against Enemy: " + cardData.IsAgainstEnemy);
        Debug.Log("IS AOE: " + cardData.IsAOE);
        Debug.Log("Card Cost: " + cardData.Cost);


        foreach (JClassInfo effect in cardData.PlayerEffects)
        {
            PrintJClassInfo(effect);
        }

        foreach (JClassInfo effect in cardData.TargetEffects)
        {
            PrintJClassInfo(effect);
        }
    }

    private void PrintJClassInfo(JClassInfo data)
    {

        Debug.Log("Effect Name: " + data.Name);
        foreach (JKeyValuePair para in data.Properties)
        {
            Debug.Log("Key = " +  para.Key + " value: " +  para.Value);
        }
    }

}

[Serializable]
public struct JBasicCardListRoot
{
    public JElementCardProperties[] Cards;
}

[Serializable]
public struct JUltimateCardListRoot
{
    public JUltimateCardProperties[] Cards;
}

[Serializable]
public struct JElementCardProperties
{
    public int ID;
    public JCardLocale[] locales;
    public string Element;
    public string Image;
    public bool IsAgainstEnemy;
    public bool IsAOE;
    public int Cost;
    public JClassInfo[] PlayerEffects;
    public JClassInfo[] TargetEffects;
}

[Serializable]
public struct JUltimateCardProperties
{
    public int ID;
    public JCardLocale[] locales;
    public string Element;
    public string Image;
    public string Icon;
    public JUltimateCardSubCard LeftCard;
    public JUltimateCardSubCard RightCard;
    public bool IsAgainstEnemy;
    public bool IsAOE;
    public JClassInfo[] PlayerEffects;
    public JClassInfo[] TargetEffects;
}

[Serializable]
public struct JUltimateCardSubCard
{
    public string Element;
    public int Cost;
    public bool IsAgainEnemy;
}

[Serializable]
public struct JCardLocale
{
    public string language;
    public JCardDisplyData data;
}

[Serializable]
public struct JCardDisplyData
{
    public string Title;
    public string Description;
}

[Serializable]
public struct JClassInfo
{
    public string Name;
    public JKeyValuePair[] Properties;
}

[Serializable]
public struct JKeyValuePair
{
    public string Key;
    public string Value;
}

