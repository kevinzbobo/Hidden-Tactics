using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A event listenable list
public class ListenableList<T>
{
    //data
    private List<T> _itemList = new List<T>();

    private List<IListChangeListener<T>> _listChangeListeners = new List<IListChangeListener<T>>(); //event listeners
    private IComparer<T> _comparator;   // sorting

    public void AddListener(IListChangeListener<T> listener)
    {
        _listChangeListeners.Add(listener);
    }

    public void RemoveListener(IListChangeListener<T> listener)
    {
        _listChangeListeners.Remove(listener);
    }

    // count
    public int GetCount()
    {
        return _itemList.Count;
    }

    // enable sorting
    public void SetSorting(IComparer<T> comparator)
    {
        this._comparator = comparator;
    }

    // pop an item
    public List<T> Pop(int count)
    {
        if (count <= 0) return new List<T>();
        int actualPopCount = _itemList.Count >= count ? count : _itemList.Count;

        List<T> drawList = _itemList.GetRange(0, actualPopCount);
        _itemList.RemoveRange(0, actualPopCount);

        foreach (T data in drawList)
        {
            foreach (IListChangeListener<T> listener in _listChangeListeners)
            {
                listener.OnItemRemove(data);
            }
        }

        return drawList;
    }

    public void AddItems<G> (List<G> items) where G : T
    {
        if (null == items || items.Count <= 0) return;

        // insert all
        foreach (G item in items)
        {
            _itemList.Add(item);
        }

        // sorting
        if (null != _comparator)
        {
            _itemList.Sort(_comparator);
        }

        // call listeners
        foreach (G data in items)
        {
            foreach (IListChangeListener<T> listener in _listChangeListeners)
            {
                listener.OnItemAdd(data);
            }
        }
    }

    public void AddItems<G>(IEnumerable<T> items) where G : T
    {
        if (null == items) return;

        // insert items
        _itemList.AddRange(items);

        // sorting
        if (null != _comparator)
        {
            _itemList.Sort(_comparator);
        }

        // call listeners
        foreach (T data in items)
        {
            foreach (IListChangeListener<T> listener in _listChangeListeners)
            {
                listener.OnItemAdd(data);
            }
        }
    }

    // return a bool indicate whether this item is insertable
    public void AddItem(T item)
    {
        // sorting
        if (null != _comparator)
        {
            bool isItemAdded = false;
            for (int cnt = 0; cnt < _itemList.Count; cnt++)
            {
                if (_comparator.Compare(_itemList[cnt], item) > 0)
                {
                    _itemList.Insert(cnt, item);
                    isItemAdded = true;
                    break;
                }
            }
            if (!isItemAdded)
            {
                _itemList.Add(item);
            }
        }
        else
        {
            _itemList.Add(item);
        }

        // call event listeners
        foreach (IListChangeListener<T> listener in _listChangeListeners)
        {
            listener.OnItemAdd(item);
        }
    }

    public void InsertItem(int index, T item)
    {
        _itemList.Insert(index, item);

        if (null != _comparator)
        {
            _itemList.Sort(_comparator);
        }

        foreach (IListChangeListener<T> listener in _listChangeListeners)
        {
            listener.OnItemAdd(item);
        }
    }

    // remove item
    public void RemoveItem(T item)
    {
        _itemList.Remove(item);

        foreach (IListChangeListener<T> listener in _listChangeListeners)
        {
            listener.OnItemRemove(item);
        }

    }

    public int IndexOf(T data)
    {
        return _itemList.IndexOf(data);
    }

    public T Get(int index)
    {
        return _itemList[index];
    }

    public List<T> GetAll()
    {
        List<T> itemList = new List<T>();
        itemList.AddRange(_itemList);
        return itemList;
    }

    public void Clear()
    {

        foreach (T data in _itemList)
        {
            foreach (IListChangeListener<T> listener in _listChangeListeners)
            {
                listener.OnItemRemove(data);
            }
        }

        _itemList.Clear();
    }

    public void Shuffle()
    {
        int n = _itemList.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            T value = _itemList[k];
            _itemList[k] = _itemList[n];
            _itemList[n] = value;
        }
    }

}

public interface IListChangeListener<T>
{
    void OnItemAdd(T data);
    void OnItemRemove(T data);
}

