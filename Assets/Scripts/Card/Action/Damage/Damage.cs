using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : ICardAction
{

    private int _basicValue = 0;
    private int _boostedValue = 0;
    private int _suppressedValue = 0;

    public int BasicValue
    {
        get
        {
            return _basicValue;
        }
        set
        {
            _basicValue = value;
        }
    }

    public int BoostedValue
    {
        get
        {
            return _boostedValue;
        }
        set
        {
            _boostedValue = value;
        }
    }

    public int SuppressedValue
    {
        get
        {
            return _suppressedValue;
        }
        set
        {
            _suppressedValue = value;
        }
    }

    public int GetPriority()
    {
        return 0;
    }

    // On effect run to the actor
    public void RunAction(Actor source, Actor target, Card card, BattleContext manager)
    {
        int attack = this._basicValue;

        if (Utilities.IsOvercominInteraction(Utilities.GetElement(target.Element), Utilities.GetElement(card.Element)))
        {
            attack = this._boostedValue;
        }
        else if (Utilities.IsOvercominInteraction(Utilities.GetElement(card.Element), Utilities.GetElement(target.Element)))
        {
            attack = this._suppressedValue;
        }

        int targetHP = target.HealthPoint.Property;
        int targetDefence = target.Defence.Property;

        attack -= targetDefence;
        if (attack > 0)
        {
            target.HealthPoint.Property = targetHP - attack;
        }

    }

}
