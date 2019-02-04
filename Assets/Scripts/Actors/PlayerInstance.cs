using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInstance : Actor
{
    /* 原始從JSON讀取出來的數據，不要更改 */
    private JCharacterProperties _jCharacterProperties;

    /* 戰鬥中的數據，這些數據允許在戰鬥中更改 */
    private ListenableProperty<string> _name;
    private ListenableProperty<int> _power;
    private ListenableProperty<int> _drawCount;

    // 角色卡牌
    private ListenableList<UltimateCardInstance> _ultimateCards;         //卡庫
    private ListenableList<ElementCardInstance> _cardDeck;         //卡庫
    private ListenableList<ElementCardInstance> _graveyard;       //手牌
    private ListenableList<ElementCardInstance> _handheldSet;   //墓地

    public PlayerInstance(string language, JCharacterProperties properties) : base(properties.MaxHp, properties.Armor, properties.Defence, properties.Image, properties.Element)
    {
        this._jCharacterProperties = properties;

        this._power = new ListenableProperty<int>(properties.Power);
        this._drawCount = new ListenableProperty<int>(properties.DrawCount);
        this._name = new ListenableProperty<string>(null);

        this._ultimateCards = new ListenableList<UltimateCardInstance>();
        this._cardDeck = new ListenableList<ElementCardInstance>();
        this._graveyard = new ListenableList<ElementCardInstance>();
        this._handheldSet = new ListenableList<ElementCardInstance>();

        foreach (JCharacterLocale local in properties.locales)
        {
            if (language.Equals(local.language))
            {
                this._name.Property = local.data.Name;
                break;
            }
        }
    }

    public JCharacterProperties Properties
    {
        get
        {
            return _jCharacterProperties;
        }
    }

    public int DrawCount
    {
        get
        {
            return _drawCount.Property;
        }
        set
        {
            _drawCount.Property = value;
        }
    }

    public ListenableProperty<int> Power
    {
        get
        {
            return _power;
        }
    }

    public ListenableList<ElementCardInstance> CardDeck
    {
        get
        {
            return _cardDeck;
        }
    }

    public ListenableList<ElementCardInstance> Graveyard
    {
        get
        {
            return _graveyard;
        }
    }

    public ListenableList<ElementCardInstance> HandheldSet
    {
        get
        {
            return _handheldSet;
        }
    }

    public ListenableList<UltimateCardInstance> UltimateCards
    {
        get
        {
            return _ultimateCards;
        }
    }

    //reset
    public override void Reset()
    {
        base.Reset();
        this._cardDeck.Clear();
        this._graveyard.Clear();
        this._handheldSet.Clear();
        this._ultimateCards.Clear();
    }

    /* 回合開始，被Battle manager呼叫 */
    public override void StartTurn(BattleContext mgr)
    {
        //重置 power
        this._power.Property += _jCharacterProperties.PowerAdd;
        if (this._power.Property > _jCharacterProperties.MaxPower)
        {
            this._power.Property = _jCharacterProperties.MaxPower;
        }

        // 抽卡
        Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!drawing: " + DrawCount);
        HandheldSet.AddItems(CardDeck.Pop(DrawCount));
        Debug.Log("count: " + HandheldSet.GetCount());
        // 如果卡牌數量不足，把墓地的咭放重新放進卡牌庫
        if (HandheldSet.GetCount() < DrawCount)
        {
            CardDeck.AddItems(Graveyard.GetAll());
            Graveyard.Clear();
            HandheldSet.AddItems(CardDeck.Pop(DrawCount - HandheldSet.GetCount()));
        }

        base.StartTurn(mgr);
    }

    /* 回合結束，被Battle manager呼叫 */
    public override void EndTurn(BattleContext mgr)
    {
        //放棄手牌
        Graveyard.AddItems(HandheldSet.GetAll());
        HandheldSet.Clear();

        base.EndTurn(mgr);
    }

    public override void PrintData()
    {

        base.PrintData();
        Debug.Log("Name: " + _name.Property + ", Power: " + _power.Property + ",draw count: " + _drawCount.Property + "Card Deck: " + CardDeck.GetCount() + ",Hand: " + _handheldSet.GetCount() + ",Graveyard: " + _graveyard.GetCount());

        if (CardDeck.GetCount() > 0)
        {
            Debug.Log("Card Deck");
            foreach (ElementCardInstance card in _cardDeck.GetAll())
            {
                card.PrintData();
            }
        }

        if (_graveyard.GetCount() > 0)
        {
            Debug.Log("Graveyard");
            foreach (ElementCardInstance card in _graveyard.GetAll())
            {
                card.PrintData();
            }
        }

        if (_handheldSet.GetCount() > 0)
        {
            Debug.Log("Hand card");
            foreach (ElementCardInstance card in _handheldSet.GetAll())
            {
                card.PrintData();
            }
        }

    }
}


