using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Draw cards
public class RandomCardDraw : ICardAction
{
    private int _drawCount = 0;
    private string _element;

    public int DrawCount
    {
        get
        {
            return _drawCount;
        }
        set
        {
            _drawCount = value;
        }
    }

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

    public int GetPriority()
    {
        return 0;
    }

    public void RunAction(Actor source, Actor target, Card card, BattleContext manager)
    {

        if (target is PlayerInstance)
        {
            PlayerInstance player = (PlayerInstance)target;
            ListenableList<ElementCardInstance> handheldSet = player.HandheldSet;
            ListenableList<ElementCardInstance> cardDeck = player.CardDeck;

            List<ElementCardInstance> filteredList = null;

            // filter
            if (null != _element)
            {
                filteredList = cardDeck.GetAll().FindAll(item => item.Element.Equals(_element));
            }
            else
            {
                filteredList = cardDeck.GetAll();
            }

            // random
            List<ElementCardInstance> drawList = new List<ElementCardInstance>();
            if (filteredList.Count > 0)
            {
                for (int cnt = 0; cnt < _drawCount; cnt++)
                {
                    int random = Random.Range(0, filteredList.Count - 1);
                    drawList.Add(filteredList[random]);
                    filteredList.RemoveAt(random);
                }
            }

            // update
            foreach (ElementCardInstance instance in drawList)
            {
                cardDeck.RemoveItem(instance);
                handheldSet.AddItem(instance);
            }
        }

    }
}
