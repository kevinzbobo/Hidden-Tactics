using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Healing Buff */
public class HPDrop : ICardAction
{

    private int _generalDrop = 0;
    private int _boostedDrop = 0;
    private int _suppressedDrop = 0;

    public int GeneralDrop
    {
        get
        {
            return _generalDrop;
        }
        set
        {
            _generalDrop = value;
        }
    }

    public int BoostedDrop
    {
        get
        {
            return _boostedDrop;
        }
        set
        {
            _boostedDrop = value;
        }
    }

    public int SuppressedDrop
    {
        get
        {
            return _suppressedDrop;
        }
        set
        {
            _suppressedDrop = value;
        }
    }

    // On effect added the the actor
    public void RunAction(Actor source, Actor target, Card card, BattleContext manager)
    {


        int hpchange = 0;

        // 檢查 相生 順向
        if (Utilities.IsGeneratingInteraction(Utilities.GetElement(target.Element), Utilities.GetElement(card.Element)))
        {
            hpchange = _boostedDrop;
        }
        // 檢查 相生 逆向
        else if (Utilities.IsGeneratingInteraction(Utilities.GetElement(card.Element), Utilities.GetElement(target.Element)))
        {
            hpchange = _suppressedDrop;
        }
        else
        {
            hpchange = _generalDrop;
        }


        target.HealthPoint.Property -= hpchange;
    }


}
