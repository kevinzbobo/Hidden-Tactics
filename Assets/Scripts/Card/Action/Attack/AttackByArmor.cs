using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackByArmor : ICardAction
{

    //On effect added to the actor
    public void RunAction(Actor source, Actor target, Card card, BattleContext manager)
    {

        Attack attack = new Attack();
        attack.BasicAttack = source.Armor.Property;
        attack.BoostedAttack = source.Armor.Property;
        attack.SuppressedAttack = source.Armor.Property;

        attack.RunAction(source, target, card, manager);
    }

}
