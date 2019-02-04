using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackIgnoreArmorByTurn : ICardAction, IActorTurnStartListener, IBuff, IStrategyDecorator<AttackInfo>
{
    //properties
    private int _ignore = 0;
    private int _turns = 0;

    //data
    private Actor _source;
    private Actor _target;
    private Card _card;
    private BattleContext _mgr;

    public int Ignore
    {
        get
        {
            return _ignore;
        }
        set
        {
            _ignore = value;
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

    //On effect added to the actor
    public void RunAction(Actor source, Actor target, Card card, BattleContext manager)
    {
        this._target = target;
        this._source = source;
        this._card = card;
        this._mgr = manager;

        // add to effect list
        bool isInsertSuccess = target.BuffList.TryInsert(this);

        if (isInsertSuccess)
        {
            target.AttackDecoratorList.AddItem(this);

            //register turn start listener
            target.AddTurnStartListener(this);
        }
    }

    public void OnTurnStart()
    {
        _turns--;
        if (_turns == 0)
        {
            //unregister listeners
            _target.AttackDecoratorList.RemoveItem(this);
            _target.RemoveTurnStartListener(this);
            _target.BuffList.RemoveItem(this);
        }
    }

    public AttackInfo Decorate(Actor source, Actor target, AttackInfo value, BattleContext mgr)
    {
        value.ArmorMask += _ignore;

        return value;
    }

    public bool IsPositiveEffect()
    {
        return true;
    }
}
