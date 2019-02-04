using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Frozen
public class SkipTurn : ICardAction, IActorTurnStartListener, IBuff
{

    private int _turns = 0;

    private Actor _actor;

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

        // add to effect list
        bool isInsertSuccess = target.BuffList.TryInsert(this);

        if (isInsertSuccess)
        {
            // register turn start listener
            this._actor.AddTurnStartListener(this);

            // frozen
            this._actor.Frozen.Property = true;
        }
    }

    // on turn start
    public void OnTurnStart()
    {
        _turns--;
        if (_turns <= 0)
        {
            // stop frozen
            bool onlyThisEffect = true;
            List<IBuff> buffList = this._actor.BuffList.GetAll();
            foreach (IBuff buff in buffList)
            {
                if (buff is SkipTurn && buff != this)
                {
                    onlyThisEffect = false;
                    break;
                }
            }
            if (onlyThisEffect)
            {
                _actor.Frozen.Property = false;
            }

            // unregister frozen effect
            this._actor.RemoveTurnStartListener(this);

            // remove Buff
            this._actor.BuffList.RemoveItem(this);
        }
    }

    public bool IsPositiveEffect()
    {
        return false;
    }
}
