using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorDouble : ICardAction
{

    //On effect added to the actor
    public void RunAction(Actor source, Actor target, Card card, BattleContext manager)
    {
        target.Armor.Property += target.Armor.Property;
    }

}