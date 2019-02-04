using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDownPerTurn : ICardAction, IActorTurnStartListener, IStrategyDecorator<AttackInfo>, IBuff
{
    //properties
    private int _down = 0;
    private int _turns = 0;

    //temporary value
    private Actor _target;

    public int Down
    {
        get
        {
            return _down;
        }
        set
        {
            _down = value;
        }
    }

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

    public AttackInfo Decorate(Actor source, Actor target, AttackInfo value, BattleContext mgr)
    {
        //increase attack power
        if (value.Attack > 0)
        {
            value.Attack = value.Attack - _down;
        }
        if (value.Attack < 0)
        {
            value.Attack = 0;
        }
        return value;
    }

    // on effectr added the the actor
    public void RunAction(Actor source, Actor target, Card card, BattleContext manager)
    {
        this._target = target;

        // add to effect list
        bool isInsertSuccess = target.BuffList.TryInsert(this);

        if (isInsertSuccess)
        {
            // register turn start listener
            target.AddTurnStartListener(this);

            // register passive decorator
            target.UnderAttackDecoratorList.AddItem(this);
        }
    }

    // on turn start
    public void OnTurnStart()
    {
        _turns--;
        if (_turns <= 0)
        {
            this._target.UnderAttackDecoratorList.RemoveItem(this);
            this._target.RemoveTurnStartListener(this);
            _target.BuffList.RemoveItem(this);
        }
    }

    public bool IsPositiveEffect()
    {
        return true;
    }
}

