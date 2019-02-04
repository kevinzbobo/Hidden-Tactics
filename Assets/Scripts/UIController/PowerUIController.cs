using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUIController : MonoBehaviour, OnPropertyChangeListener<int>
{
    // 資料
    private ListenableProperty<int> _power;

    public Sprite PowerSprite;
    public Sprite PrePowerSprite;
    public Sprite PostPowerSprite;
    public float distance = 0;
    public float scale = 1.0f;
    public string SortingLayer;

    // temporaty game object
    private GameObject _prePower;
    private GameObject _postPower;
    private List<GameObject> _powerList = new List<GameObject>();
    private bool isInitialized;

    void Init()
    {
        if (isInitialized)
        {
            return;
        }
        isInitialized = true;
        _prePower = new GameObject();
        _postPower = new GameObject();

        SpriteRenderer preSprite = _prePower.AddComponent<SpriteRenderer>();
        SpriteRenderer postSprite = _postPower.AddComponent<SpriteRenderer>();

        preSprite.sprite = PrePowerSprite;
        postSprite.sprite = PostPowerSprite;
        preSprite.sortingLayerName = SortingLayer;
        postSprite.sortingLayerName = SortingLayer;

        _prePower.transform.SetParent(this.transform);
        _postPower.transform.SetParent(this.transform);
    }

    // 綁定Power
    public void Bind(ListenableProperty<int> powerProperty)
    {
        this._power = powerProperty;
        Init();

        // 監聽 cardset改變事件
        this._power.AddListener(this);
        this.SetPower(powerProperty.Property);
    }

    private void SetPower(int power)
    {
        if (power > _powerList.Count)
        {
            for (int cnt = _powerList.Count; cnt < power && cnt >= 0; cnt++)
            {
                GameObject item = new GameObject();
                SpriteRenderer spriteRender = item.AddComponent<SpriteRenderer>();
                spriteRender.sprite = PowerSprite;
                spriteRender.sortingLayerName = SortingLayer;
                item.transform.localScale = new Vector3(scale, scale, 1.0f);
                _powerList.Add(item);
                item.transform.SetParent(this.transform);
            }
        }
        else if (power < _powerList.Count)
        {
            for (int cnt = power; cnt < _powerList.Count && cnt >= 0; cnt++)
            {
                GameObject item = _powerList[cnt];
                _powerList.Remove(item);
                Destroy(item);
            }
        }

        ReScale();
    }

    private void ReScale()
    {

        // align center

        _prePower.transform.localScale = new Vector3(scale, scale, 1);
        _postPower.transform.localScale = new Vector3(scale, scale, 1);

        foreach (GameObject obj in _powerList)
        {
            obj.transform.localScale = new Vector3(scale, scale, 1);
        }

        //calculate position
        float totalWidth = (_powerList.Count + 1 ) * (distance * scale);

        //arrange position
        _prePower.transform.localPosition = new Vector3(-totalWidth * 0.5f, 0, 1);
        _postPower.transform.localPosition = new Vector3(totalWidth * 0.5f, 0, 1);

        float startPoint = -totalWidth * 0.5f;
        foreach (GameObject power in _powerList)
        {
            startPoint += (distance * scale);
            power.transform.localPosition = new Vector3(startPoint, 0, 1);
        }
    }


    public void OnPropertyChanged(ListenableProperty<int> property, int fromValue, int toValue)
    {
        this.SetPower(toValue);
    }


}
