using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ComboCardUIController : MonoBehaviour
{
    private UltimateCardInstance _ultimateCard;

    public Image CardBody;
    public GameObject CombineButton;
    public GameObject CancelButton;

    public void Bind(UltimateCardInstance card)
    {
        this._ultimateCard = card;

        Sprite cardImage = Resources.Load<Sprite>(Constants.UltimateCardIconPath + Path.DirectorySeparatorChar + this._ultimateCard.Image);
        if (null != cardImage)
        {
            CardBody.sprite = cardImage;
        }
    }

    public void UnBind()
    {

    }

    public void SetVisible(bool isVisible)
    {
        this.transform.gameObject.SetActive(isVisible);
    }

    public UltimateCardInstance GetCardInstance()
    {
        return this._ultimateCard;
    }

    public void Cancel()
    {
        PlayerPrefs.SetInt("ComboCardPanelState", 0);
        this.SetVisible(false);

        GameObject[] highLight = GameObject.FindGameObjectsWithTag("CardHighlight");
        foreach (GameObject obj in highLight)
        {
            obj.SetActive(false);
        }
    }

    public void ComboCardPolymery()
    {
        //remove highlight card from hand
        GameObject[] highLight = GameObject.FindGameObjectsWithTag("CardHighlight");
        foreach (GameObject obj in highLight)
        {
            if (obj.transform.parent.parent.parent.parent.tag == "Card")
            {
                ElementCardInstance card = obj.transform.parent.parent.parent.parent.GetComponent<ElementCardUIController>().GetCardInstance();
                DataManager.Instance.BattleContext.Player.HandheldSet.RemoveItem(card);
            }      
        }

        PlayerPrefs.SetInt("ComboCardPanelState", 0);
        this.SetVisible(false);
        //逻辑待会补，待续
        
        //BattleContext context = DataManager.Instance.BattleContext;
        //if (null != context)
        //{
        //    UltimateCardPlayEvent cardEvent = new UltimateCardPlayEvent();

        //    cardEvent.Card = _ultimateCard;
        //    cardEvent.Targets = null;

        //    bool isPlayable = false;
        //    ListenableList<ElementCardInstance> handHeldSet = context.Player.HandheldSet;
        //    int totalCnt = handHeldSet.GetCount();
        //    for (int i = 0; i < totalCnt; i++)
        //    {
        //        for (int j = 0; j < totalCnt; j++)
        //        {

        //            Debug.Log(_ultimateCard.Description);
        //            if (_ultimateCard.IsCardPlayable(handHeldSet.Get(i), handHeldSet.Get(j)))
        //            {
        //                isPlayable = true;
        //                cardEvent.LeftCard = handHeldSet.Get(i);
        //                cardEvent.RightCard = handHeldSet.Get(j);
        //                EventManager.TriggerEvent(BattleManager.PLAY_ULTIMATE_CARD, cardEvent);
        //                break;
        //            }
        //            else if (_ultimateCard.IsCardPlayable(handHeldSet.Get(j), handHeldSet.Get(i)))
        //            {
        //                isPlayable = true;
        //                cardEvent.LeftCard = handHeldSet.Get(j);
        //                cardEvent.RightCard = handHeldSet.Get(i);
        //                EventManager.TriggerEvent(BattleManager.PLAY_ULTIMATE_CARD, cardEvent);
        //                break;
        //            }
        //        }
        //    }
        //}
    }

}
