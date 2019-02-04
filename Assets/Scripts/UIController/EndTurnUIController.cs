using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndTurnUIController : MonoBehaviour, OnPropertyChangeListener<Actor>
{

    // Data
    private ListenableProperty<Actor> _currentActor;
    private bool _isClickable = true;

    // UI
    public Button Button;

    // Register listener
    void Start()
    {
        Button.onClick.AddListener(TaskOnClick);
    }

    // Unregister listener
    void OnDestroy()
    {
        if (null != _currentActor)
        {
            this._currentActor.RemoveListener(this);
        }
        Button.onClick.RemoveListener(TaskOnClick);
    }

    // Binding
    public void Bind(ListenableProperty<Actor> currentActor)
    {
        this._currentActor = currentActor;
        this._currentActor.AddListener(this);
        SetClickable(currentActor.Property is PlayerInstance);
    }

    private void SetClickable(bool clickable)
    {
        this._isClickable = clickable;
    }

    // On Currrent Actor changed
    public void OnPropertyChanged(ListenableProperty<Actor> property, Actor fromValue, Actor toValue)
    {
        SetClickable(property.Property is PlayerInstance);
    }

    // Trigger Turn End Button Clicked Event
    void TaskOnClick()
    {
        if (_isClickable)
        {
            EventManager.TriggerEvent(BattleManager.END_TURN, null);
        }
    }

}
