using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//限制數值最大最小值
public class ListenableRangeProperty<T> : ListenableProperty<T> where T : System.IComparable
{
    private List<IOnPropertyLimitedListener<T>> _listenerList;
    private T _maxValue;
    private T _minValue;

    public ListenableRangeProperty(T property, T maxValue, T minValue) : base(property)
    {
        this._listenerList = new List<IOnPropertyLimitedListener<T>>();
        this._maxValue = maxValue;
        this._minValue = minValue;
    }

    public override T Property
    {
        get
        {
            return base.Property;
        }
        set
        {
            if (null != this._maxValue && value.CompareTo(_maxValue) > 0)
            {
                base.Property = _maxValue;

                foreach (IOnPropertyLimitedListener<T> listener in _listenerList)
                {
                    listener.OnLimited(this, value, _maxValue, _maxValue, _minValue);
                }

            } else if (null != this._minValue && _minValue.CompareTo(value) > 0)
            {
                base.Property = _minValue;

                foreach (IOnPropertyLimitedListener<T> listener in _listenerList)
                {
                    listener.OnLimited(this, value, _minValue, _maxValue, _minValue);
                }
            }
            else
            {
                base.Property = value;
            }
        }
    }

    public T MaxValue
    {
        get
        {
            return _maxValue;
        }
        set
        {
            _maxValue = value;
        }
    }

    public T MinValue
    {
        get
        {
            return _minValue;
        }
        set
        {
            _minValue = value;
        }
    }

    //Add limit event listener
    public void AddLimitListener(IOnPropertyLimitedListener<T> listener)
    {
        _listenerList.Add(listener);
    }

    //Remove limit event listener
    public void RemoveLimitListener(IOnPropertyLimitedListener<T> listener)
    {
        _listenerList.Remove(listener);
    }

    public override void RemoveAllListeners()
    {
        base.RemoveAllListeners();
        _listenerList.Clear();
    }

}

public interface IOnPropertyLimitedListener<T> where T : System.IComparable
{
    void OnLimited(ListenableRangeProperty<T> property, T original, T final, T max, T min);
}
