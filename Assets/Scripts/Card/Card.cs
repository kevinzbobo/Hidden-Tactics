using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Card
{

    //data
    private string _element;    // card element
    private ListenableProperty<bool> _isAOE;    // is AOE card
    private ListenableProperty<bool> _isAgainstEnemy;    // is again enemy
    private ListenableProperty<bool> _isFrozen;    // is card frozen
    private JClassInfo[] _targetEffectList; // buffs applied to the target
    private JClassInfo[] _sourceEffectList; // buffs applied to the player

    public string Element
    {
        get
        {
            return _element;
        }
    }

    public bool IsAgainstEnemy
    {
        get
        {
            return _isAgainstEnemy.Property;
        }
    }

    public bool IsAOE
    {
        get
        {
            return _isAOE.Property;
        }
    }

    public ListenableProperty<bool> isFrozen
    {
        get
        {
            return _isFrozen;
        }
    }

    protected Card(string element, JClassInfo[] targetEffectList, JClassInfo[] sourceEffectList, bool isAOE, bool isAgainEnemy)
    {
        this._element = element;
        this._isAgainstEnemy = new ListenableProperty<bool>(isAgainEnemy);
        this._isAOE = new ListenableProperty<bool>(isAOE);
        this._targetEffectList = targetEffectList;
        this._sourceEffectList = sourceEffectList;
    }

    public virtual void Play(Actor source, List<Actor> targets, BattleContext mgr)
    {
        foreach (Actor actor in targets)
        {
            // Run effects
            if (null != _targetEffectList)
            {
                foreach (JClassInfo property in _targetEffectList)
                {
                    // 創建Effect
                    ObjectBuilder effectBuilder = new ObjectBuilder(property.Name);
                    foreach (JKeyValuePair attributes in property.Properties)
                    {
                        effectBuilder.SetProperty(attributes.Key, attributes.Value);
                    }
                    ICardAction effect = (ICardAction)effectBuilder.Build();
                    effect.RunAction(source, actor, this, mgr);
                }
            }

            // Run effects
            if (null != _sourceEffectList)
            {
                foreach (JClassInfo property in _sourceEffectList)
                {
                    // 創建Effect
                    ObjectBuilder effectBuilder = new ObjectBuilder(property.Name);
                    foreach (JKeyValuePair attributes in property.Properties)
                    {
                        effectBuilder.SetProperty(attributes.Key, attributes.Value);
                    }
                    ICardAction effect = (ICardAction)effectBuilder.Build();
                    effect.RunAction(actor, source, this, mgr);
                }
            }
        }
    }

    public virtual void PrintData()
    {
    }

}
