using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Posion Effect */
public class DamagePerTurn : ICardAction, IActorTurnStartListener, IBuff
{
    private int _generalDamage = 0;
    private int _boostedDamage = 0;
    private int _suppressedDamage = 0;
    private int _turns = 0;

    //temporary value
    private Actor _target;
    private Actor _source;
    private BattleContext _mgr;
    private Card _card;

    public int GeneralDamage
    {
        get
        {
            return _generalDamage;
        }
        set
        {
            _generalDamage = value;
        }
    }

    public int BoostedDamage
    {
        get
        {
            return _boostedDamage;
        }
        set
        {
            _boostedDamage = value;
        }
    }

    public int SuppressedDamage
    {
        get
        {
            return _suppressedDamage;
        }
        set
        {
            _suppressedDamage = value;
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
        this._source = source;
        this._mgr = manager;
        this._card = card;

        // add to effect list
        bool isInsertSuccess = target.BuffList.TryInsert(this);

        if (isInsertSuccess)
        {
            // register turn start listener
            target.AddTurnStartListener(this);
        }
    }

    // on turn start
    public void OnTurnStart()
    {

        // run attack effect
        Damage damage = new Damage();
        damage.BasicValue = _generalDamage;
        damage.BoostedValue = _boostedDamage;
        damage.SuppressedValue = _suppressedDamage;
        damage.RunAction(_source, _target, this._card, _mgr);

        if (_turns >= 0)
        {
            _turns--;
            if (_turns <= 0)
            {
                // unregister listeners
                this._target.RemoveTurnStartListener(this);
                this._target.BuffList.RemoveItem(this);
            }
        }
    }

    public bool IsPositiveEffect()
    {
        return false;
    }
}
