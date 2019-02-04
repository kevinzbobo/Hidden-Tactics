using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipTurnIfBuffExists : ICardAction
{
    private int _turns = 0;
    private string _buffName;

    public int Turns
    {
        get
        {
            return _turns;
        }
        set
        {
            _turns = value;
        }
    }

    public string Buff
    {
        get
        {
            return _buffName;
        }
        set
        {
            _buffName = value;
        }
    }

    public int GetPriority()
    {
        return 0;
    }

    // On effect added th the actor
    public void RunAction(Actor source, Actor target, Card card, BattleContext manager)
    {

        //search buffs
        bool isBuffFound = false;
        List<IBuff> buffList = target.BuffList.GetAll();
        foreach (IBuff buff in buffList)
        {
            if (buff.GetType().Name.Equals(_buffName))
            {
                isBuffFound = true;
                break;
            }
        }

        if (isBuffFound)
        {
            SkipTurn skipTurn = new SkipTurn();
            skipTurn.Turns = _turns;

            bool isInserted = target.BuffList.TryInsert(skipTurn);
            if (isInserted)
            {
                skipTurn.RunAction(source, target, card, manager);
            }
        }

    }

}
