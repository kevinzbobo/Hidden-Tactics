using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorDrop : ICardAction
{
    //constant
    public const string ACTION_DROP_ARMOR = "ARMOR_DROP";

    //properties
    private int _generalDrop = 0;
    private int _overcomingDrop = 0;
    private int _interactingDrop = 0;

    public int GeneralDrop
    {
        get
        {
            return _generalDrop;
        }
        set
        {
            _generalDrop = value;
        }
    }

    public int OvercomingDrop
    {
        get
        {
            return _overcomingDrop;
        }
        set
        {
            _overcomingDrop = value;
        }
    }

    public int InteractingDrop
    {
        get
        {
            return _interactingDrop;
        }
        set
        {
            _interactingDrop = value;
        }
    }

    //On effect added to the actor
    public void RunAction(Actor source, Actor target, Card card, BattleContext manager)
    {

        int drop = _generalDrop;

        if (Utilities.IsOvercominInteraction(Utilities.GetElement(target.Element), Utilities.GetElement(card.Element)))
        {
            drop = _overcomingDrop;
        }
        else if (Utilities.IsOvercominInteraction(Utilities.GetElement(card.Element), Utilities.GetElement(target.Element)))
        {
            drop = _interactingDrop;
        }

        target.Armor.Property -= drop;
    }

}