using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorDropPerTurn : ICardAction, IActorTurnStartListener, IBuff
{
    //properties
    private int _generalDrop = 0;
    private int _overcomingDrop = 0;
    private int _interactingDrop = 0;
    private int _turns = 0;

    //data
    private Actor _source;
    private Actor _target;
    private Card _card;
    private BattleContext _mgr;
    private ArmorDrop _effect;

    public int GeneralDrop
    {
        get
        {
            return _generalDrop;
        }
        set
        {
            _generalDrop = value;
        }
    }

    public int OvercomingDrop
    {
        get
        {
            return _overcomingDrop;
        }
        set
        {
            _overcomingDrop = value;
        }
    }

    public int InteractingDrop
    {
        get
        {
            return _interactingDrop;
        }
        set
        {
            _interactingDrop = value;
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

        this._effect = new ArmorDrop();
        this._effect.GeneralDrop = _generalDrop;
        this._effect.OvercomingDrop = _overcomingDrop;
        this._effect.InteractingDrop = _interactingDrop;

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
        return false;
    }
}
