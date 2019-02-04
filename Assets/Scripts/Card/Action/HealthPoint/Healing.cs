using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Healing Buff */
public class Healing : ICardAction
{

    public const string ACTION_HEALING = "ACTION_HEALING";

    private int _generalHealing = 0;
    private int _boostedHealing = 0;
    private int _suppressedHealing = 0;

    public int GeneralHealing
    {
        get
        {
            return _generalHealing;
        }
        set
        {
            _generalHealing = value;
        }
    }

    public int BoostedHealing
    {
        get
        {
            return _boostedHealing;
        }
        set
        {
            _boostedHealing = value;
        }
    }

    public int SuppressedHealing
    {
        get
        {
            return _suppressedHealing;
        }
        set
        {
            _suppressedHealing = value;
        }
    }

    // On effect added the the actor
    public void RunAction(Actor source, Actor target, Card card, BattleContext context)
    {

        int hpchange = 0;

        // 檢查 相生 順向
        if (Utilities.IsGeneratingInteraction(Utilities.GetElement(target.Element), Utilities.GetElement(card.Element)))
        {
            hpchange += _boostedHealing;
        }
        // 檢查 相生 逆向
        else if (Utilities.IsGeneratingInteraction(Utilities.GetElement(card.Element), Utilities.GetElement(target.Element)))
        {
            hpchange += _suppressedHealing;
        }
        else
        {
            hpchange += _generalHealing;
        }
        target.HealthPoint.Property += hpchange;
    }


}
