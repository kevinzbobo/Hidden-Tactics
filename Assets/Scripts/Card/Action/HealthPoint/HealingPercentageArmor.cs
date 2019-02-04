using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Healing Buff(armor)
public class HealingPercentageArmor : ICardAction
{

    private float _generalRatio = 0;
    private float _boostedRatio = 0;
    private float _suppressedRatio = 0;

    public float GeneralRatio
    {
        get
        {
            return _generalRatio;
        }
        set
        {
            _generalRatio = value;
        }
    }

    public float BoostedRatio
    {
        get
        {
            return _boostedRatio;
        }
        set
        {
            _boostedRatio = value;
        }
    }

    public float SuppressedRatio
    {
        get
        {
            return _suppressedRatio;
        }
        set
        {
            _suppressedRatio = value;
        }
    }

    public int GetPriority()
    {
        return 0;
    }

    // On effect added to the actor
    public void RunAction(Actor source, Actor target, Card card, BattleContext manager)
    {
        int originalHP = target.HealthPoint.Property;

        HealingPercentage healingEffect = new HealingPercentage();
        healingEffect.BoostedRatio = this._boostedRatio;
        healingEffect.GeneralRatio = this._generalRatio;
        healingEffect.SuppressedRatio = this._suppressedRatio;

        healingEffect.RunAction(source, target, card, manager);

        // add armor
        target.Armor.Property += (target.HealthPoint.Property - originalHP);
    }

}
