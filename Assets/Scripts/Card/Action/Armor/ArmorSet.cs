using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Set Armor
public class ArmorSet : ICardAction
{
    private int _value = 0;

    public int Value
    {
        get
        {
            return _value;
        }
        set
        {
            _value = value;
        }
    }

    //On effect added to the actor
    public void RunAction(Actor source, Actor target, Card card, BattleContext manager)
    {
        target.Armor.Property = _value;
    }

}
