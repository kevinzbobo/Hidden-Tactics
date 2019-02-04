using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingPercentage : ICardAction
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

    // On effect added the the actor
    public void RunAction(Actor source, Actor target, Card card, BattleContext manager)
    {
        // 檢查 相生 順向
        if (Utilities.IsGeneratingInteraction(Utilities.GetElement(target.Element), Utilities.GetElement(card.Element)))
        {
            target.HealthPoint.Property += (int)(target.HealthPoint.Property * _boostedRatio);
        }
        // 檢查 相生 逆向
        else if (Utilities.IsGeneratingInteraction(Utilities.GetElement(card.Element), Utilities.GetElement(target.Element)))
        {
            target.HealthPoint.Property += (int)(target.HealthPoint.Property * _suppressedRatio);
        }
        else
        {
            target.HealthPoint.Property += (int)(target.HealthPoint.Property * _generalRatio);
        }
    }


}

