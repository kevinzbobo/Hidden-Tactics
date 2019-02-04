using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//可以發動數據改變事件的容器
public class ListenableProperty<T>
{
    private T _property;
    private List<OnPropertyChangeListener<T>> _listenerList;

    public ListenableProperty(T property)
    {
        this._property = property;
        this._listenerList = new List<OnPropertyChangeListener<T>>();
    }

    public virtual T Property
    {
        get
        {
            return _property;
        }
        set
        {
            T oldValue = _property;
            _property = value;

            if (null != oldValue)
            {
                if (!oldValue.Equals(value))
                {
                    foreach (OnPropertyChangeListener<T> listener in _listenerList)
                    {
                        listener.OnPropertyChanged(this, oldValue, _property);
                    }
                }
            }
            else
            {
                if (null != _property)
                {
                    foreach (OnPropertyChangeListener<T> listener in _listenerList)
                    {
                        listener.OnPropertyChanged(this, oldValue, _property);
                    }
                }
            }
        }
    }

    public void AddListener(OnPropertyChangeListener<T> listener){
        _listenerList.Add(listener);
    }

    public void RemoveListener(OnPropertyChangeListener<T> listener){
        _listenerList.Remove(listener);
    }

    public virtual void RemoveAllListeners()
    {
        _listenerList.Clear();
    }

}

public interface OnPropertyChangeListener<T>
{
    void OnPropertyChanged(ListenableProperty<T> property, T fromValue, T toValue);
}

