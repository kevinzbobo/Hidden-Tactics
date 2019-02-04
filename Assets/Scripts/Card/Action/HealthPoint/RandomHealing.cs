using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomHealing : ICardAction
{

    private int _from = 0;
    private int _to = 0;

    public int From
    {
        get
        {
            return _from;
        }
        set
        {
            _from = value;
        }
    }

    public int To
    {
        get
        {
            return _to;
        }
        set
        {
            _to = value;
        }
    }

    // On effect added the the actor
    public void RunAction(Actor source, Actor target, Card card, BattleContext manager)
    {

        target.HealthPoint.Property += Random.Range(_from, _to);
    }


}
