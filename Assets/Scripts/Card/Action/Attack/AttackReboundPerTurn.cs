using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Rebound attack
public class AttackReboundPerTurn : ICardAction, IActorTurnStartListener, IStrategyDecorator<AttackInfo>, IBuff
{
    private float _ratio = 0;
    private int _turns = 0;

    // temporary value
    private Actor _actor;
    private Card _card;

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

    // On effect added to the actor
    public void RunAction(Actor source, Actor target, Card card, BattleContext manager)
    {
        this._actor = target;
        this._card = card;

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
            this._actor.AttackDecoratorList.RemoveItem(this);
            this._actor.RemoveTurnStartListener(this);
            this._actor.BuffList.RemoveItem(this);
        }
    }

    // decorate
    public AttackInfo Decorate(Actor source, Actor target, AttackInfo value, BattleContext mgr)
    {

        if (value.Attack > 0)
        {
            // run attack effect
            Attack attack = new Attack();
            attack.BasicAttack = (int)(value.Attack * _ratio);
            attack.BoostedAttack = (int)(value.Attack * _ratio);
            attack.SuppressedAttack = (int)(value.Attack * _ratio);

            attack.RunAction(target, source, this._card, mgr);
        }

        return value;
    }

    public bool IsPositiveEffect()
    {
        return false;
    }
}
