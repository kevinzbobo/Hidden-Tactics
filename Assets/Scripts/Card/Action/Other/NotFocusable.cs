using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotFocusable : ICardAction, IActorTurnStartListener, IBuff
{
    //properties
    private int _turns = 0;

    // temporary value
    private Actor _target;

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

    public int GetPriority()
    {
        return 0;
    }

    // On effect added to the actor
    public void RunAction(Actor source, Actor target, Card card, BattleContext manager)
    {
        this._target = target;
        this._target.AttackEnable.Property = false;

        // add to effect list
        bool isInsertSuccess = target.BuffList.TryInsert(this);

        if (isInsertSuccess)
        {
            target.FocusAble.Property = false;

            // add turn start listener
            target.AddTurnStartListener(this);

        }
    }

    public void OnTurnStart()
    {
        _turns--;
        if (_turns == 0)
        {
            _target.FocusAble.Property = true;

            //unregister listeners
            _target.RemoveTurnStartListener(this);
            _target.BuffList.RemoveItem(this);
        }
    }

    public bool IsPositiveEffect()
    {
        return true;
    }
}
