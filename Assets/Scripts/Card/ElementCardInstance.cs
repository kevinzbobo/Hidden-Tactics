using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Element Card */
public class ElementCardInstance : Card
{
    /* JSON DATA */
    private JElementCardProperties _jElementCardProperties;

    //properties
    private ListenableProperty<string> _title;
    private ListenableProperty<string> _description;
    private ListenableProperty<int> _cost;

    public ElementCardInstance (string language, JElementCardProperties properties) : base(properties.Element, properties.TargetEffects, properties.PlayerEffects, properties.IsAOE, properties.IsAgainstEnemy)
    {
        this._jElementCardProperties = properties;
        this._title = new ListenableProperty<string>(null);
        this._description = new ListenableProperty<string>(null);

        // language
        foreach (JCardLocale local in _jElementCardProperties.locales)
        {
            if (language.Equals(local.language))
            {
                this._title.Property = local.data.Title;
                this._description.Property = local.data.Description;
                break;
            }
        }

        this.Reset();
    }

    public void Reset()
    {
        this._cost = new ListenableProperty<int>(this._jElementCardProperties.Cost);
    }

    public int ID
    {
        get
        {
            return _jElementCardProperties.ID;
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

    public string Image
    {
        get
        {
            return _jElementCardProperties.Image;
        }
    }


    public ListenableProperty<int> Cost
    {
        get
        {
            return _cost;
        }
    }

    public JElementCardProperties Properties
    {
        get
        {
            return _jElementCardProperties;
        }
    }

    public override void PrintData()
    {

        Debug.Log("Card Title: " + _title.Property +
            ", Desc: " + _title.Property +
            ", Cost: " + _jElementCardProperties.Cost +  
            ", IsAOE: " + _jElementCardProperties.IsAOE +
            ", Element: " + _jElementCardProperties.Element +
            ", IsTargetingEnemy" + _jElementCardProperties.IsAgainstEnemy);

    }

}

