using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attack power buff (multiplication)
public class AttackMultiplicationPerTurn : ICardAction, IActorTurnStartListener, IStrategyDecorator<AttackInfo>, IBuff
{
    public float _ratio;
    private int _turns = 0;

    //temporaty value
    private Actor _target;

    public float Ratio
    {
        get
        {
            return _ratio;
        }
        set
        {
            _ratio = value;
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

    // decorate
    public AttackInfo Decorate(Actor source, Actor target, AttackInfo value, BattleContext mgr)
    {

        // increase attack power
        if (value.Attack > 0)
        {
            value.Attack =(int)(value.Attack * _ratio);
        }
        return value;
    }

    // On effect added to the player
    public void RunAction(Actor source, Actor target, Card card, BattleContext manager)
    {
        this._target = target;

        // add to effect list
        bool isInsertSuccess = target.BuffList.TryInsert(this);

        if (isInsertSuccess)
        {
            // register turn start listener
            target.AddTurnStartListener(this);

            // register decorator
            target.AttackDecoratorList.AddItem(this);
        }
    }

    // on turn start
    public void OnTurnStart()
    {
        _turns--;
        if (_turns == 0)
        {
            // unregister decorator
            this._target.AttackDecoratorList.RemoveItem(this);
            // unregister turn start listener
            this._target.RemoveTurnStartListener(this);
            // remove buff
            this._target.BuffList.RemoveItem(this);
        }
    }

    public bool IsPositiveEffect()
    {
        return true;
    }
}
