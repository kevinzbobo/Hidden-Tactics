using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackByHP : ICardAction
{
    private float _ratio = 0;

    public float Ratio
    {
        get
        {
            return _ratio;
        }
        set
        {
            _ratio = value;
        }
    }

    //On effect added to the actor
    public void RunAction(Actor source, Actor target, Card card, BattleContext manager)
    {

        Attack attack = new Attack();
        attack.BasicAttack = (int)(source.HealthPoint.Property * _ratio);
        attack.BoostedAttack = (int)(source.HealthPoint.Property * _ratio);
        attack.SuppressedAttack = (int)(source.HealthPoint.Property * _ratio);

        attack.RunAction(source, target, card, manager);
    }

}

