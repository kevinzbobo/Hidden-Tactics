using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ComboButtonController : MonoBehaviour
{

    private UltimateCardInstance _ultimateCard;

    public Image Icon;
    public Image Element;
    public ComboCardUIController ComboCardUIController;

    public void SetVisible(bool isVisible)
    {
        gameObject.SetActive(isVisible);
    }

    public void Bind(UltimateCardInstance card)
    {
        this._ultimateCard = card;

        Sprite cardIcon = Resources.Load<Sprite>(Constants.UltimateCardIconPath + Path.DirectorySeparatorChar + this._ultimateCard.Icon);
        if (null != cardIcon)
        {
            Icon.sprite = cardIcon;
        }

        Sprite elementIcon = null;
        if (card.Element.Equals(Constants.ElementNameFire))
        {
            elementIcon = Resources.Load<Sprite>(Constants.UltimateCardElementImagePath + Path.DirectorySeparatorChar + "ultimate_card_gold");
        } else if (card.Element.Equals(Constants.ElementNameWood))
        {
            elementIcon = Resources.Load<Sprite>(Constants.UltimateCardElementImagePath + Path.DirectorySeparatorChar + "ultimate_card_gold");
        } else if (card.Element.Equals(Constants.ElementNameWater))
        {
            elementIcon = Resources.Load<Sprite>(Constants.UltimateCardElementImagePath + Path.DirectorySeparatorChar + "ultimate_card_gold");
        } else if (card.Element.Equals(Constants.ElementNameEarth))
        {
            elementIcon = Resources.Load<Sprite>(Constants.UltimateCardElementImagePath + Path.DirectorySeparatorChar + "ultimate_card_gold");
        } else if (card.Element.Equals(Constants.ElementNameMetal))
        {
            elementIcon = Resources.Load<Sprite>(Constants.UltimateCardElementImagePath + Path.DirectorySeparatorChar + "ultimate_card_gold");
        }

        if (null != elementIcon)
        {
            Element.sprite = elementIcon;
        }
    }

    public UltimateCardInstance GetCardInstance()
    {
        return this._ultimateCard;
    }

    public void ComboButtonClick()
    {
        ComboCardUIController.Bind(_ultimateCard);
        ComboCardUIController.SetVisible(true);
    }

}
