using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//合成卡牌
public class UltimateCardInstance : Card
{
    /* 原始從JSON讀取出來的數據 */
    private JUltimateCardProperties _jUltimateCardProperties;

    private ListenableProperty<string> _title;
    private ListenableProperty<string> _description;

    public UltimateCardInstance(string language, JUltimateCardProperties properties) : base(properties.Element, properties.TargetEffects, properties.PlayerEffects, properties.IsAOE, properties.IsAgainstEnemy)
    {
        this._jUltimateCardProperties = properties;
        this._title = new ListenableProperty<string>(null);
        this._description = new ListenableProperty<string>(null);

        //處理語言
        foreach (JCardLocale local in _jUltimateCardProperties.locales)
        {
            if (language.Equals(local.language))
            {
                this._title.Property = local.data.Title;
                this._description.Property = local.data.Description;
                break;
            }
        }
    }

    public int ID
    {
        get
        {
            return _jUltimateCardProperties.ID;
        }
    }

    public ListenableProperty<string> Title
    {
        get
        {
            return _title;
        }
    }

    public ListenableProperty<string> Description
    {
        get
        {
            return _description;
        }
    }

    public new string Element
    {
        get
        {
            return _jUltimateCardProperties.Element;
        }
    }

    public string Image
    {
        get
        {
            return _jUltimateCardProperties.Image;
        }
    }

    public string Icon
    {
        get
        {
            return _jUltimateCardProperties.Icon;
        }
    }

    public new bool IsAgainstEnemy
    {
        get
        {
            return _jUltimateCardProperties.IsAgainstEnemy;
        }
    }

    public bool IsAOE
    {
        get
        {
            return _jUltimateCardProperties.IsAOE;
        }
    }

    public JUltimateCardProperties Properties
    {
        get
        {
            return _jUltimateCardProperties;
        }
    }

    public JUltimateCardSubCard LeftCard
    {
        get
        {
            return _jUltimateCardProperties.LeftCard;
        }
    }

    public JUltimateCardSubCard RightCard
    {
        get
        {
            return _jUltimateCardProperties.RightCard;
        }
    }

    public bool IsCardPlayable(ElementCardInstance left, ElementCardInstance right)
    {
        if (left.Element.Equals(_jUltimateCardProperties.LeftCard.Element) && right.Element.Equals(_jUltimateCardProperties.RightCard.Element))
        {
            if (left.Cost.Property >= _jUltimateCardProperties.LeftCard.Cost && right.Cost.Property >= _jUltimateCardProperties.RightCard.Cost)
            {
                return true;
            }
        }

        return false;
    }

}
