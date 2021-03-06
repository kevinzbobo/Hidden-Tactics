﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ComboCardUIController : MonoBehaviour
{
    private UltimateCardInstance _ultimateCard;

    public Image CardBody;

    public GameObject ComboPanel;

    public void Bind(UltimateCardInstance card)
    {
        Debug.Log("Bind");
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
        ComboPanel.GetComponent<ComboPaneUIController>().setUltimateCardInstance(GetCardInstance());
        Debug.Log(_ultimateCard.LeftCard.Element);
        Debug.Log(_ultimateCard.RightCard.Element);
    }

    public UltimateCardInstance GetCardInstance()
    {
        return this._ultimateCard;
    }


}
