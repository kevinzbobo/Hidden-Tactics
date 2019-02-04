using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Buff, Armor increment*/
public class ArmorAdditionPerTurn : ICardAction, IActorTurnStartListener, IBuff
{
    //properties
    private int _generalAddition = 0;
    private int _boostedAddition = 0;
    private int _suppressedAddition = 0;
    private int _turns = 0;

    //data
    private Actor _target;
    private Actor _source;
    private Card _card;
    private BattleContext _mgr;
    private ArmorAddition _effect;

    public int GeneralAddition
    {
        get
        {
            return _generalAddition;
        }
        set
        {
            _generalAddition = value;
        }
    }

    public int BoostedAddition
    {
        get
        {
            return _boostedAddition;
        }
        set
        {
            _boostedAddition = value;
        }
    }

    public int SuppressedAddition
    {
        get
        {
            return _suppressedAddition;
        }
        set
        {
            _suppressedAddition = value;
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

    public bool IsVisible()
    {
        return true;
    }

    //On effect added to the actor
    public void RunAction(Actor source, Actor target, Card card, BattleContext manager)
    {
        this._target = target;
        this._source = source;
        this._card = card;
        this._mgr = manager;

        // create armor addition effect
        this._effect = new ArmorAddition();
        this._effect.GeneralAddition = _generalAddition;
        this._effect.BoostedAddition = _boostedAddition;
        this._effect.SuppressedAddition = _suppressedAddition;

        // add to effect list
        bool isInsertSuccess = target.BuffList.TryInsert(this);
         
        if (isInsertSuccess)
        {
            // register turn start listener
            target.AddTurnStartListener(this);
        }
    }

    public void OnTurnStart()
    {
        // update armor
        this._effect.RunAction(this._source, this._target, this._card, this._mgr);

        _turns--;
        if (_turns == 0)
        {
            // unregister turn start listener
            this._target.RemoveTurnStartListener(this);

            // remove this Buff
            this._target.BuffList.RemoveItem(this);
        }
    }

    public bool IsPositiveEffect()
    {
        return true;
    }
}

