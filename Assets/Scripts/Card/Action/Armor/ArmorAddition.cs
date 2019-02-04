using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorAddition : ICardAction
{
    //constant
    public const string ACTION_ADD_ARMOR = "ARMOR_ADD";

    //properties
    private int _generalAddition = 0;
    private int _boostedAddition = 0;
    private int _suppressedAddition = 0;

    public int GeneralAddition
    {
        get
        {
            return _generalAddition;
        }
        set
        {
            _generalAddition = value;
        }
    }

    public int BoostedAddition
    {
        get
        {
            return _boostedAddition;
        }
        set
        {
            _boostedAddition = value;
        }
    }

    public int SuppressedAddition
    {
        get
        {
            return _suppressedAddition;
        }
        set
        {
            _suppressedAddition = value;
        }
    }

    //On effect added to the actor
    public void RunAction(Actor source, Actor target, Card card, BattleContext manager)
    {

        int incrememnt = _generalAddition;

        if (Utilities.IsGeneratingInteraction(Utilities.GetElement(target.Element), Utilities.GetElement(card.Element)))
        {
            incrememnt = _boostedAddition;
        }
        else if (Utilities.IsGeneratingInteraction(Utilities.GetElement(card.Element), Utilities.GetElement(target.Element)))
        {
            incrememnt = _suppressedAddition;
        }

        target.Armor.Property += incrememnt;
    }

}