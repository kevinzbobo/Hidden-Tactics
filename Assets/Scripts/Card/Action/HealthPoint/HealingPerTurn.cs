using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Healing Buff */
public class HealingPerTurn : ICardAction, IActorTurnStartListener, IBuff
{
    //properties
    private int _generalHealing = 0;
    private int _boostedHealing = 0;
    private int _suppressedHealing = 0;
    private int _turns = 0;

    //data
    private Actor _source;
    private Actor _target;
    private Card _card;
    private BattleContext _mgr;
    private Healing _effect;

    public int GeneralHealing
    {
        get
        {
            return _generalHealing;
        }
        set
        {
            _generalHealing = value;
        }
    }

    public int BoostedHealing
    {
        get
        {
            return _boostedHealing;
        }
        set
        {
            _boostedHealing = value;
        }
    }

    public int SuppressedHealing
    {
        get
        {
            return _suppressedHealing;
        }
        set
        {
            _suppressedHealing = value;
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

        this._effect = new Healing();
        this._effect.GeneralHealing = _generalHealing;
        this._effect.BoostedHealing = _boostedHealing;
        this._effect.SuppressedHealing = _suppressedHealing;

        // add to effect list
        bool isInsertSuccess = target.BuffList.TryInsert(this);

        if (isInsertSuccess)
        {
            //register turn start listener
            target.AddTurnStartListener(this);
        }
    }

    public void OnTurnStart()
    {
        // run effect
        this._effect.RunAction(_source, _target, _card, _mgr);

        _turns--;
        if (_turns == 0)
        {
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
