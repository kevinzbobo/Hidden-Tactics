using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour, OnPropertyChangeListener<int>, IListChangeListener<IBuff>
{
    // messages
    public const string MESSAGE_PLAYER_FOCUS_ON = "PLAYER_FOCUS_ON";
    public const string MESSAGE_PLAYER_FOCUS_OFF = "PLAYER_FOCUS_OFF";

    // Data
    private PlayerInstance _player;

    // UI
    public Image Image;
    public SpriteRenderer _focusRender;    // Focus image
    public Image MaxHP;
    public Image CurrentHP;    //HP
    public GameObject ArmorGroup;
    public Text Armor;
    public GameObject DefenceGroup;
    public Text Defence;
    public GameObject AttackGroup;
    public Text Attack;

    public void Start()
    {
        // Register messages
        EventManager.StartListening(MESSAGE_PLAYER_FOCUS_ON, SetFocusOn);
        EventManager.StartListening(MESSAGE_PLAYER_FOCUS_OFF, SetFocusOff);
    }

    void OnDestroy()
    {
        // Unregister messages
        EventManager.StopListening(MESSAGE_PLAYER_FOCUS_ON, SetFocusOn);
        EventManager.StopListening(MESSAGE_PLAYER_FOCUS_OFF, SetFocusOff);

        UnBind();
    }

    private void UnBind()
    {
        if (null != _player)
        {
            _player.HealthPoint.RemoveListener(this);
            _player.Armor.RemoveListener(this);
            _player.Defence.RemoveListener(this);
            _player.BuffList.RemoveListener(this);
        }
    }

    public void Bind(PlayerInstance player)
    {
        UnBind();

        this._player = player;
        Image.sprite = Resources.Load<Sprite>(Constants.CharacterCartoonImagePath + Path.DirectorySeparatorChar + this._player.Image);
        if (null == Image.sprite)
        {
            Debug.Log("CardInstance: unable to load character image: " + Constants.CardImagePath + Path.DirectorySeparatorChar + this._player.Image);
        }

        SetHP(player.HealthPoint.Property);
        SetArmor(player.Armor.Property);
        SetDefence(player.Defence.Property);
        SetBuffCnt(player.BuffList.GetCount());
        SetElement(player.Element);

        _player.HealthPoint.AddListener(this);
        _player.Armor.AddListener(this);
        _player.Defence.AddListener(this);
        _player.BuffList.AddListener(this);
    }

    private void SetHP(int value)
    {
        float percentage = (float)value / _player.MaxHp.Property;
        CurrentHP.transform.localScale = new Vector3(percentage, CurrentHP.transform.localScale.y, CurrentHP.transform.localScale.z);
    }

    private void SetArmor(int value)
    {
        Armor.text = value.ToString();
    }

    private void SetDefence(int value)
    {
        Defence.text = value.ToString();
    }

    private void SetBuffCnt(int value)
    {
        //_buffs.text = "Buffs:" + value.ToString();
    }

    private void SetElement(string value)
    {
        if (value.Equals(Constants.ElementNameMetal))
        {
            CurrentHP.color = Utilities.IntToColor(Constants.COLOR_METAL);
        } 
        else if (value.Equals(Constants.ElementNameEarth))
        {
            CurrentHP.color = Utilities.IntToColor(Constants.COLOR_EARTH);
        }
        else if (value.Equals(Constants.ElementNameFire))
        {
            CurrentHP.color = Utilities.IntToColor(Constants.COLOR_FIRE);
        }
        else if (value.Equals(Constants.ElementNameWood))
        {
            CurrentHP.color = Utilities.IntToColor(Constants.COLOR_WOOD);
        }
        else if (value.Equals(Constants.ElementNameWater))
        {
            CurrentHP.color = Utilities.IntToColor(Constants.COLOR_WATER);
        }
    }

    private void SetFocusOn(System.Object arg)
    {
        if (_player.Alive.Property)
        {
            _focusRender.enabled = true;
        }
    }

    private void SetFocusOff(System.Object arg)
    {
        if (_player.Alive.Property)
        {
            _focusRender.enabled = false;
        }
    }

    public void OnPropertyChanged(ListenableProperty<int> property, int fromProperty, int toProperty)
    {
        if (property == _player.HealthPoint)
        {
            SetHP(toProperty);
        } else if (property == _player.Armor)
        {
            SetArmor(toProperty);
        }
        else if (property == _player.Defence)
        {
            SetDefence(toProperty);
        }
    }

    public void OnItemAdd(IBuff data)
    {
        SetBuffCnt(_player.BuffList.GetCount());
    }

    public void OnItemRemove(IBuff data)
    {
        SetBuffCnt(_player.BuffList.GetCount());
    }

}
