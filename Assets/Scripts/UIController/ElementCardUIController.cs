using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

// Card UI Controller
public class ElementCardUIController : MonoBehaviour, OnPropertyChangeListener<string>, OnPropertyChangeListener<int>
{

    // UI
    public GameObject CardFront;
    public GameObject CardBack;
    public Text Star;
    public Text Title;
    public Text Description;
    public Image CardBody;
    public Image CardImage;

    // Preview UI
    public GameObject PreviewCardFront;
    public GameObject PreviewCardBack;
    public Text PreviewStar;
    public Text PreviewTitle;
    public Text PreviewDescription;
    public Image PreviewCardBody;
    public Image PreviewCardImage;

    // Card Data
    private ElementCardInstance _elementCard;

    // Unregister listener
    void OnDestroy()
    {
        UnBind();
    }

    private void UnBind()
    {
        if (null != this._elementCard)
        {
            this._elementCard.Cost.RemoveListener(this);
            this._elementCard.Title.RemoveListener(this);
            this._elementCard.Description.RemoveListener(this);
            this._elementCard = null;
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

    private void SetStar(int star)
    {
        Star.text = star.ToString();
        PreviewStar.text = star.ToString();
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

    public ElementCardInstance GetCardInstance()
    {
        return _elementCard;
    }

    // Load Data
    public void Bind(ElementCardInstance card)
    {
        UnBind();

        this._elementCard = card;
        SetStar(this._elementCard.Cost.Property);
        SetTitle(this._elementCard.Title.Property);
        SetDescription(this._elementCard.Description.Property);

        Sprite cardBody = Resources.Load<Sprite>(Constants.CardImagePath + Path.DirectorySeparatorChar + this._elementCard.Image);
        if (null == cardBody) Debug.Log("Unable to load resource: " + Constants.CardImagePath + Path.DirectorySeparatorChar + this._elementCard.Image);
        SetCardBody(cardBody);

        //Sprite cardImage = Resources.Load<Sprite>(Constants.CardImagePath + Path.DirectorySeparatorChar + this._elementCard.Image);
        //if (null == cardBody) Debug.Log("Unable to load resource: " + Constants.CardImagePath + Path.DirectorySeparatorChar + this._elementCard.Image);
        SetCardImage(null);

        //register data change listener
        this._elementCard.Cost.AddListener(this);
        this._elementCard.Title.AddListener(this);
        this._elementCard.Description.AddListener(this);

    }

    public void OnPropertyChanged(ListenableProperty<string> property, string fromValue, string toValue)
    {
        if (property == this._elementCard.Title)
        {
            SetTitle(toValue);
        } 
        else if (property == this._elementCard.Description)
        {
            SetDescription(toValue);
        }
    }

    public void OnPropertyChanged(ListenableProperty<int> property, int fromValue, int toValue)
    {
        if (property == this._elementCard.Cost)
        {
            SetStar(toValue);
        }
    }

    public void TestCard()
    {

        BattleContext context = DataManager.Instance.BattleContext;
        if (null != context && context.Player.Power.Property >= _elementCard.Cost.Property)
        {
            Debug.Log("testing card");
            ElementCardPlayEvent cardEvent = new ElementCardPlayEvent();
            cardEvent.Card = _elementCard;
            cardEvent.Targets = null;
            EventManager.TriggerEvent(BattleManager.PLAY_ELEMENT_CARD, cardEvent);
        }

    }
}
