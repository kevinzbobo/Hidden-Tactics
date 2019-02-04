using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class MonsterUIController : MonoBehaviour, OnPropertyChangeListener<int>, IListChangeListener<IBuff>
{

    // messages
    public const string MESSAGE_MONSTER_FOCUS_ON = "MONSTER_FOCUS_ON";
    public const string MESSAGE_MONSTER_FOCUS_OFF = "MONSTER_FOCUS_OFF";

    // 資料
    private EnemyInstance _enemyInstance;

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

    public EnemyInstance EnemyInstance
    {
        get
        {
            return _enemyInstance;
        }
    }

    void Start()
    {
        // Register messages
        EventManager.StartListening(MESSAGE_MONSTER_FOCUS_ON, SetFocusOn);
        EventManager.StartListening(MESSAGE_MONSTER_FOCUS_OFF, SetFocusOff);
    }

    void OnDestroy()
    {
        // Unregister messages
        EventManager.StopListening(MESSAGE_MONSTER_FOCUS_ON, SetFocusOn);
        EventManager.StopListening(MESSAGE_MONSTER_FOCUS_OFF, SetFocusOff);

        if (null != _enemyInstance)
        {
            _enemyInstance.HealthPoint.RemoveListener(this);
            _enemyInstance.Armor.RemoveListener(this);
            _enemyInstance.Defence.RemoveListener(this);
            _enemyInstance.BuffList.RemoveListener(this);
        }
    }

    public void LoadMonster(EnemyInstance enemy)
    {
        this._enemyInstance = enemy;
        Image.sprite = Resources.Load<Sprite>(Constants.EnemyImagePath + Path.DirectorySeparatorChar + this._enemyInstance.Image);
        if (null == Image.sprite)
        {
            Debug.Log("CardInstance: unable to load enemy image: " + Constants.EnemyImagePath + Path.DirectorySeparatorChar + this._enemyInstance.Image);
        }

        SetScale(enemy.GetModel().Scale);

        SetHP(_enemyInstance.HealthPoint.Property);
        SetArmor(_enemyInstance.Armor.Property);
        SetDefence(_enemyInstance.Defence.Property);
        SetBuffs(_enemyInstance.BuffList.GetCount());
        SetElement(_enemyInstance.Element);

        _enemyInstance.HealthPoint.AddListener(this);
        _enemyInstance.Armor.AddListener(this);
        _enemyInstance.Defence.AddListener(this);
        _enemyInstance.BuffList.AddListener(this);

    }

    private void SetScale(float scale)
    {
        Image.transform.localScale = new Vector3(scale, scale, 1);
    }

    private void SetHP(int value)
    {
        //_hp.text = "HP:" + value.ToString();
        float percentage = (float)value / _enemyInstance.MaxHp.Property;
        CurrentHP.transform.localScale = new Vector3(CurrentHP.transform.localScale.x * percentage, CurrentHP.transform.localScale.y, CurrentHP.transform.localScale.z);
    }

    private void SetArmor(int value)
    {
        Armor.text = value.ToString();
    }

    private void SetDefence(int value)
    {
        Defence.text = value.ToString();
    }

    private void SetBuffs(int value)
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
        _focusRender.enabled = true;
    }

    private void SetFocusOff(System.Object arg)
    {
        _focusRender.enabled = false;
    }

    public void OnPropertyChanged(ListenableProperty<bool> property, bool fromProperty, bool toProperty)
    {
        if (null == _focusRender)
        {
            _focusRender = transform.GetChild(1).GetComponent<SpriteRenderer>();
        }
        _focusRender.enabled = toProperty;
    }

    public void OnPropertyChanged(ListenableProperty<int> property, int fromProperty, int toProperty)
    {
        if (property == _enemyInstance.HealthPoint)
        {
            SetHP(property.Property);
        }
        else if (property == _enemyInstance.Armor)
        {
            SetArmor(property.Property);
        }
        else if (property == _enemyInstance.Defence)
        {
            SetDefence(property.Property);
        }
    }

    public void OnItemAdd(IBuff data)
    {
        SetBuffs(_enemyInstance.BuffList.GetCount());
    }

    public void OnItemRemove(IBuff data)
    {
        SetBuffs(_enemyInstance.BuffList.GetCount());
    }
}
