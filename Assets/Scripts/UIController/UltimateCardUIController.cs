using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

// Card UI Controller
public class UltimateCardUIController : MonoBehaviour, OnPropertyChangeListener<string>
{

    // UI
    public GameObject CardFront;
    public GameObject CardBack;
    public Text Title;
    public Text Description;
    public Image CardBody;
    public Image CardImage;

    // Preview UI
    public GameObject PreviewCardFront;
    public GameObject PreviewCardBack;
    public Text PreviewTitle;
    public Text PreviewDescription;
    public Image PreviewCardBody;
    public Image PreviewCardImage;

    // Card Data
    private UltimateCardInstance _ultimateCard;

    // Unregister listener
    void OnDestroy()
    {
        Unbind();
    }

    private void Unbind()
    {
        if (null != _ultimateCard)
        {
            this._ultimateCard.Title.RemoveListener(this);
            this._ultimateCard.Description.RemoveListener(this);
            this._ultimateCard = null;
        }
    }

    //showing card back or showing card front
    public void SetShowingBack(bool showingBack)
    {
        if (showingBack)
        {
            CardFront.SetActive(false);
            CardBack.SetActive(true);
            PreviewCardFront.SetActive(false);
            PreviewCardBack.SetActive(true);
        }
        else
        {
            CardFront.SetActive(true);
            CardBack.SetActive(false);
            PreviewCardFront.SetActive(false);
            PreviewCardBack.SetActive(true);
        }
    }

    private void SetTitle(string title)
    {
        Title.text = title;
        PreviewTitle.text = title;
    }

    private void SetDescription(string description)
    {
        Description.text = description;
        PreviewDescription.text = description;
    }

    private void SetCardBody(Sprite sprite)
    {
        CardBody.sprite = sprite;
        PreviewCardBody.sprite = sprite;
    }

    private void SetCardImage(Sprite sprite)
    {
        if (null == sprite)
        {
            CardImage.enabled = false;
            PreviewCardImage.enabled = false;
        }
        else
        {
            CardImage.sprite = sprite;
            PreviewCardImage.sprite = sprite;
        }
    }

    public UltimateCardInstance GetCardInstance()
    {
        return _ultimateCard;
    }

    // Load Data
    public void Bind(UltimateCardInstance card)
    {

        Unbind();

        this._ultimateCard = card;
        SetTitle(this._ultimateCard.Title.Property);
        SetDescription(this._ultimateCard.Description.Property);

        Sprite cardBody = Resources.Load<Sprite>(Constants.CardImagePath + Path.DirectorySeparatorChar + this._ultimateCard.Image);
        if (null == cardBody) Debug.Log("Unable to load resource: " + Constants.UltimateCardImagePath + Path.DirectorySeparatorChar + this._ultimateCard.Image);
        SetCardBody(cardBody);

        //Sprite cardImage = Resources.Load<Sprite>(Constants.CardImagePath + Path.DirectorySeparatorChar + this._elementCard.Image);
        //if (null == cardBody) Debug.Log("Unable to load resource: " + Constants.CardImagePath + Path.DirectorySeparatorChar + this._elementCard.Image);
        SetCardImage(null);

        //register data change listener
        this._ultimateCard.Title.AddListener(this);
        this._ultimateCard.Description.AddListener(this);

    }

    public void OnPropertyChanged(ListenableProperty<string> property, string fromValue, string toValue)
    {
        if (property == this._ultimateCard.Title)
        {
            SetTitle(toValue);
        } 
        else if (property == this._ultimateCard.Description)
        {
            SetDescription(toValue);
        }
    }

    public void TestCard()
    {
        Debug.Log("testing card");
        UltimateCardPlayEvent cardEvent = new UltimateCardPlayEvent();
        cardEvent.Card = _ultimateCard;
        cardEvent.Targets = null;
        EventManager.TriggerEvent(BattleManager.PLAY_ULTIMATE_CARD, cardEvent);
    }
}
