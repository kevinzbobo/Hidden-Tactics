using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboCardCol : MonoBehaviour
{
    public ComboCardUIController ComboCardUIController;

    public void ComboCardPolymery()
    {
        ComboCardUIController.SetVisible(false);
        Debug.Log("play combo");

        BattleContext context = DataManager.Instance.BattleContext;
        if (null != context)
        {
            UltimateCardPlayEvent cardEvent = new UltimateCardPlayEvent();

            cardEvent.Card = ComboCardUIController.GetCardInstance();
            cardEvent.Targets = null;

            bool isPlayable = false;
            ListenableList<ElementCardInstance> handHeldSet = context.Player.HandheldSet;
            int totalCnt = handHeldSet.GetCount();
            for (int i = 0; i < totalCnt; i++)
            {
                for(int j = 0; j < totalCnt; j++)
                {
                    if (cardEvent.Card.IsCardPlayable(handHeldSet.Get(i), handHeldSet.Get(j)))
                    {
                        isPlayable = true;
                        cardEvent.LeftCard = handHeldSet.Get(i);
                        cardEvent.RightCard = handHeldSet.Get(j);
                        EventManager.TriggerEvent(BattleManager.PLAY_ULTIMATE_CARD, cardEvent);
                        break;
                    } else if (cardEvent.Card.IsCardPlayable(handHeldSet.Get(j), handHeldSet.Get(i)))
                    {
                        isPlayable = true;
                        cardEvent.LeftCard = handHeldSet.Get(j);
                        cardEvent.RightCard = handHeldSet.Get(i);
                        EventManager.TriggerEvent(BattleManager.PLAY_ULTIMATE_CARD, cardEvent);
                        break;
                    }
                }
            }

            Debug.Log("IsCombo Card Played: " + isPlayable.ToString());
        }
    }
}
