using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageUpPerTurn : ICardAction, IActorTurnStartListener, IStrategyDecorator<AttackInfo>, IBuff
{
    private int _generalExtraDamage = 0;
    private int _boostedExtraDamage = 0;
    private int _suppressedExtraDamage = 0;
    private int _turns = 0;

    //temporary value
    private Actor _target;
    private string _element;

    public int GeneralExtraDamage
    {
        get
        {
            return _generalExtraDamage;
        }
        set
        {
            _generalExtraDamage = value;
        }
    }
    public int BoostedExtraDamage
    {
        get
        {
            return _boostedExtraDamage;
        }
        set
        {
            _boostedExtraDamage = value;
        }
    }

    public int SuppressedExtraDamage
    {
        get
        {
            return _suppressedExtraDamage;
        }
        set
        {
            _suppressedExtraDamage = value;
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
        // damage up
        if (value.Attack > 0)
        {
            if (Utilities.IsOvercominInteraction(Utilities.GetElement(target.Element), Utilities.GetElement(_element)))
            {
                value.Attack = this._boostedExtraDamage;
            }
            else if (Utilities.IsOvercominInteraction(Utilities.GetElement(_element), Utilities.GetElement(target.Element)))
            {
                value.Attack = this._suppressedExtraDamage;
            }
            else
            {
                value.Attack = value.Attack + _generalExtraDamage;
            }
        }
        if (value.Attack < 0)
        {
            value.Attack = 0;
        }
        return value;
    }

    // On effect added the the actor
    public void RunAction(Actor source, Actor target, Card card, BattleContext manager)
    {
        this._target = target;
        this._element = card.Element;

        // add to effect list
        bool isInsertSuccess = target.BuffList.TryInsert(this);

        if (isInsertSuccess)
        {
            // register turn start listener
            target.AddTurnStartListener(this);

            // register decorator
            target.UnderAttackDecoratorList.AddItem(this);
        }
    }

    // On turn start
    public void OnTurnStart()
    {
        _turns--;
        if (_turns <= 0)
        {
            this._target.UnderAttackDecoratorList.RemoveItem(this);
            this._target.RemoveTurnStartListener(this);
            this._target.BuffList.RemoveItem(this);
        }
    }

    public bool IsPositiveEffect()
    {
        return false;
    }
}
