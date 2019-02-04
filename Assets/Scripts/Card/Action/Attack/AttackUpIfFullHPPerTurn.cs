using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackUpUpIfFullHPPerTurn : ICardAction, IActorTurnStartListener, IStrategyDecorator<AttackInfo>, IBuff
{
    private int _up = 0;
    private int _turns = 0;

    // temporary value
    private Actor _actor;

    public int Up
    {
        get
        {
            return _up;
        }
        set
        {
            _up = value;
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

    // On effect applied
    public void RunAction(Actor source, Actor target, Card card, BattleContext manager)
    {
        this._actor = target;

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
        if (_turns <= 0)
        {
            this._actor.AttackDecoratorList.RemoveItem(this);
            this._actor.RemoveTurnStartListener(this);
            this._actor.BuffList.RemoveItem(this);
        }
    }

    // decorate
    public AttackInfo Decorate(Actor source, Actor target, AttackInfo value, BattleContext mgr)
    {
        // increase attack power
        if (value.Attack > 0 && source.HealthPoint.Property >= source.MaxHp.Property)
        {
            value.Attack = value.Attack + _up;
        }
        return value;
    }

    public bool IsPositiveEffect()
    {
        return true;
    }
}
