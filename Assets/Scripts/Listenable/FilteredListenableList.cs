using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// proxy of listenablelist
public class FilteredListenableList<T>
{
    //data list
    private ListenableList<T> _list = new ListenableList<T>();
    private List<IListFilter<T>> _filterList = new List<IListFilter<T>>();    //filter list

    public void AddListener(IListChangeListener<T> listener)
    {
        _list.AddListener(listener);
    }

    public void RemoveListener(IListChangeListener<T> listener)
    {
        _list.RemoveListener(listener);
    }

    public int GetCount()
    {
        return _list.GetCount();
    }

    public void AddFilter(IListFilter<T> filter)
    {
        if (null == filter) return;
        this._filterList.Add(filter);
    }

    public void RemoveFilter(IListFilter<T> filter)
    {
        if (null == filter) return;
        this._filterList.Remove(filter);
    }

    // enable sorting
    public void SetSorting(IComparer<T> comparator)
    {
        this._list.SetSorting(comparator);
    }

    public bool TryInsert(T item)
    {
        bool isInsertable = true;
        foreach (IListFilter<T> filter in _filterList)
        {
            if (!filter.IsInsertable(item))
            {
                isInsertable = false;
                break;
            }
        }

        if (isInsertable)
        {
            _list.AddItem(item);
        }

        return isInsertable;
    }

    // remove item
    public void RemoveItem(T item)
    {
        this._list.RemoveItem(item);
    }

    public int IndexOf(T item)
    {
        return this._list.IndexOf(item);
    }

    public T Get(int index)
    {
        return this._list.Get(index);
    }

    public List<T> GetAll()
    {
        return this._list.GetAll();
    }

    public void Clear()
    {
        this._list.Clear();
    }

    public void Shuffle()
    {
        this._list.Shuffle();
    }
}

public interface IListFilter<T>
{
    bool IsInsertable(T item);
}
