using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDrop : ICardAction
{
    private int _count = 0;

    public int Count
    {
        get
        {
            return _count;
        }
        set
        {
            _count = value;
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

            ListenableList<ElementCardInstance> handheldList = player.HandheldSet;
            List<ElementCardInstance> changeList = new List<ElementCardInstance>();

            // if drop count greater than 0
            if (_count > 0)
            {
                for (int cnt = 0; cnt < _count; cnt++)
                {
                    changeList.Add(handheldList.Get(cnt));
                }

                //if drop count smaller than 0, drop all
            } else if (_count < 0)
            {
                changeList.AddRange(handheldList.GetAll());
            }

            //move card from handheld to graveard
            foreach (ElementCardInstance item in changeList)
            {
                handheldList.RemoveItem(item);
            }
            player.Graveyard.AddItems(changeList);
        }

    }
}
