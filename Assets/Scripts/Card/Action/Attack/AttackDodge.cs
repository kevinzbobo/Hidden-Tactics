using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDodge : ICardAction, IStrategyDecorator<AttackInfo>, IBuff
{
    // temporary value
    private Actor _target;

    // On effect added to the actor
    public void RunAction(Actor source, Actor target, Card card, BattleContext manager)
    {
        this._target = target;

        // add to effect list
        bool isInsertSuccess = target.BuffList.TryInsert(this);

        if (isInsertSuccess)
        {
            // register decorator
            target.UnderAttackDecoratorList.AddItem(this);
        }
    }

    // decorate
    public AttackInfo Decorate(Actor source, Actor target, AttackInfo value, BattleContext mgr)
    {

        value.Attack = 0;

        // remove this buffs
        this._target.UnderAttackDecoratorList.RemoveItem(this);
        this._target.BuffList.RemoveItem(this);

        // set attack to zero
        return value;
    }

    public int GetPriority()
    {
        return 0;
    }

    public bool IsPositiveEffect()
    {
        return true;
    }
}
