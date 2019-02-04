using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Reduce attack power buff */
public class AttackDownPerTurn : ICardAction, IActorTurnStartListener, IStrategyDecorator<AttackInfo>, IBuff
{
    private int _generalDown = 0;
    private int _overcomingDown = 0;
    private int _interactingDown = 0;
    private int _turns = 0;

    // temporary value
    private Actor _actor;
    private string _element;

    public int GeneralDown
    {
        get
        {
            return _generalDown;
        }
        set
        {
            _generalDown = value;
        }
    }

    public int OvercomingDown
    {
        get
        {
            return _overcomingDown;
        }
        set
        {
            _overcomingDown = value;
        }
    }

    public int InteractingDown
    {
        get
        {
            return _interactingDown;
        }
        set
        {
            _interactingDown = value;
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
        this._element = card.Element;

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
        // remove attack power
        if (value.Attack > 0)
        {

            if (Utilities.IsOvercominInteraction(Utilities.GetElement(target.Element), Utilities.GetElement(_element)))
            {
                value.Attack = this._overcomingDown;
            }
            else if (Utilities.IsOvercominInteraction(Utilities.GetElement(_element), Utilities.GetElement(target.Element)))
            {
                value.Attack = this._interactingDown;
            }
            else
            {
                value.Attack -= _generalDown;
            }
        }
        if (value.Attack < 0)
        {
            value.Attack = 0;
        }
        return value;
    }

    public bool IsPositiveEffect()
    {
        return false;
    }
}
