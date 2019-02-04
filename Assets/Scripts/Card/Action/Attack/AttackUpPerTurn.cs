﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Attack power buff */
public class AttackUpPerTurn : ICardAction, IActorTurnStartListener, IStrategyDecorator<AttackInfo>, IBuff
{
    private int _up = 0;
    private int _turns = 0;

    // temporary value
    private Actor _target;

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

    // On effect added to the actor
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
        if (_turns <= 0)
        {
            this._target.AttackDecoratorList.RemoveItem(this);
            this._target.RemoveTurnStartListener(this);
            this._target.BuffList.RemoveItem(this);
        }
    }

    // decorate
    public AttackInfo Decorate(Actor source, Actor target, AttackInfo value, BattleContext mgr)
    {
        //增強攻擊力
        if (value.Attack > 0)
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