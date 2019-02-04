using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenceAddition : ICardAction
{

    public const string ACTION_DEFENCE = "ACTION_DEFENCE";

    private int _generalDefence = 0;
    private int _boostedDefence = 0;
    private int _suppressedDefence = 0;

    public int GeneralDefence
    {
        get
        {
            return _generalDefence;
        }
        set
        {
            _generalDefence = value;
        }
    }

    public int BoostedDefence
    {
        get
        {
            return _boostedDefence;
        }
        set
        {
            _boostedDefence = value;
        }
    }

    public int SuppressedDefence
    {
        get
        {
            return _suppressedDefence;
        }
        set
        {
            _suppressedDefence = value;
        }
    }

    // On effect added the the actor
    public void RunAction(Actor source, Actor target, Card card, BattleContext manager)
    {


        int defence = 0;

        // 檢查 相生 順向
        if (Utilities.IsGeneratingInteraction(Utilities.GetElement(target.Element), Utilities.GetElement(card.Element)))
        {
            defence += _boostedDefence;
        }
        // 檢查 相生 逆向
        else if (Utilities.IsGeneratingInteraction(Utilities.GetElement(card.Element), Utilities.GetElement(target.Element)))
        {
            defence += _suppressedDefence;
        }
        else
        {
            defence += _generalDefence;
        }

        target.Defence.Property += defence;
    }


}
