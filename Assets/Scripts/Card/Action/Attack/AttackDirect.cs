using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDirect : ICardAction
{

    private int _basicAttack = 0;
    private int _boostedAttack = 0;
    private int _suppressedAttack = 0;

    public int BasicAttack
    {
        get
        {
            return _basicAttack;
        }
        set
        {
            _basicAttack = value;
        }
    }

    public int BoostedAttack
    {
        get
        {
            return _boostedAttack;
        }
        set
        {
            _boostedAttack = value;
        }
    }

    public int SuppressedAttack
    {
        get
        {
            return _suppressedAttack;
        }
        set
        {
            _suppressedAttack = value;
        }
    }

    // On effect run to the actor
    public void RunAction(Actor source, Actor target, Card card, BattleContext manager)
    {
        Attack attack = new Attack();
        attack.BasicAttack = _basicAttack;
        attack.BoostedAttack = _boostedAttack;
        attack.SuppressedAttack = _suppressedAttack;
        attack.ArmorMask = target.Armor.Property;

        attack.RunAction(source, target, card, manager);

    }

}
