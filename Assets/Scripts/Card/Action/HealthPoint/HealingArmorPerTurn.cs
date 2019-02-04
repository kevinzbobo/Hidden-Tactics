using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Healing Buff(armor)
public class HealingArmorPerTurn : ICardAction, IActorTurnStartListener, IOnPropertyLimitedListener<int>, IBuff
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
            // Register turn start listener
            target.AddTurnStartListener(this);
        }
    }

    // On turn start
    public void OnTurnStart()
    {
        _turns--;
        if (_turns <= 0)
        {
            // unregister turn start listeenr
            this._actor.RemoveTurnStartListener(this);

            // RemoveBuffs
            this._actor.BuffList.RemoveItem(this);
        }
    }

    // 當HP被限制時
    public void OnLimited(ListenableRangeProperty<int> property, int original, int final, int max, int min)
    {
        if (original > final)
        {
            // increase armor
            this._actor.Armor.Property += (original - final);
        }
    }

    public bool IsPositiveEffect()
    {
        return true;
    }
}
