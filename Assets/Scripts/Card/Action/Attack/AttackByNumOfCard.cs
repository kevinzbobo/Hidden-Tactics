using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackByNumOfCard : ICardAction
{
    //properties
    private string _element;
    private int _attack;

    public string Element
    {
        get
        {
            return _element;
        }
        set
        {
            _element = value;
        }
    }

    public int Attack
    {
        get
        {
            return _attack;
        }
        set
        {
            _attack = value;
        }
    }

    // On effect added to the actor
    public void RunAction(Actor source, Actor target, Card card, BattleContext manager)
    {
        if (source is PlayerInstance)
        {
            int cardCount = 0;
            List<ElementCardInstance> cardList = ((PlayerInstance)source).HandheldSet.GetAll();
            foreach (ElementCardInstance item in cardList)
            {
                if (item.Element.Equals(_element))
                {
                    cardCount++;
                }
            }

            Attack attack = new Attack();
            attack.BasicAttack = cardCount * _attack;
            attack.BoostedAttack = cardCount * _attack;
            attack.SuppressedAttack = cardCount * _attack;

            attack.RunAction(source, target, card, manager);
        }
    }

}
