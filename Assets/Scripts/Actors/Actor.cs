using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// actor parent class
public abstract class Actor
{

    // player data
    private ListenableProperty<int> _maxHp; //Max HP
    private ListenableProperty<int> _armor; //Armor
    private ListenableProperty<int> _defence; //defence
    private ListenableRangeProperty<int> _healthPoint; //current HP, MAX and min hp are set
    private ListenableProperty<string> _image;    //character image
    private ListenableProperty<string> _element;    //character element
    private ListenableProperty<bool> _frozen;    //frozen
    private ListenableProperty<bool> _alive;    //alive
    private ListenableProperty<bool> _attackEnable;    //Attack enable
    private ListenableProperty<bool> _focusAble;

    // Decoratiors
    private ListenableList<IStrategyDecorator<AttackInfo>> _attackDecoratorList;    // Decorate active actions
    private ListenableList<IStrategyDecorator<AttackInfo>> _underAttackDecoratorList;    // Decorate active actions

    /* Buffs */
    private FilteredListenableList<IBuff> _buffList;

    // listeners
    private ListenableList<IActorTurnStartListener> _turnStartListeners;

    public Actor(int maxHp, int armor, int defence, string image, string element)
    {
        this._maxHp = new ListenableProperty<int>(maxHp);
        this._armor = new ListenableProperty<int>(armor);
        this._defence = new ListenableProperty<int>(defence);
        this._healthPoint = new ListenableRangeProperty<int>(maxHp, maxHp, 0);
        this._frozen = new ListenableProperty<bool>(false);
        this._alive = new ListenableProperty<bool>(true);
        this._attackEnable = new ListenableProperty<bool>(true);
        this._focusAble = new ListenableProperty<bool>(true);

        this._buffList = new FilteredListenableList<IBuff>();
        this._image = new ListenableProperty<string>(image);
        this._element = new ListenableProperty<string>(element);

        this._turnStartListeners = new ListenableList<IActorTurnStartListener>();

        this._attackDecoratorList = new ListenableList<IStrategyDecorator<AttackInfo>>();
        this._underAttackDecoratorList = new ListenableList<IStrategyDecorator<AttackInfo>>();

        this._buffList.SetSorting(new IBuffComparator());
        this._attackDecoratorList.SetSorting(new IDecoratorComparator());
        this._underAttackDecoratorList.SetSorting(new IDecoratorComparator());
        this._turnStartListeners.SetSorting(new ITurnStartListenerComparator());
    }

    public FilteredListenableList<IBuff> BuffList
    {
        get
        {
            return _buffList;
        }
    }

    public string Image
    {
        get
        {
            return this._image.Property;
        }
    }

    public string Element
    {
        get
        {
            return this._element.Property;
        }
    }

    public ListenableProperty<int> MaxHp
    {
        get
        {
            return _maxHp;
        }
    }

    public ListenableProperty<int> Armor
    {
        get
        {
            return _armor;
        }
    }

    public ListenableProperty<int> Defence
    {
        get
        {
            return _defence;
        }
    }

    public ListenableRangeProperty<int> HealthPoint
    {
        get
        {
            return _healthPoint;
        }
    }

    public ListenableProperty<bool> Frozen
    {
        get
        {
            return _frozen;
        }
    }

    public ListenableProperty<bool> Alive
    {
        get
        {
            return _alive;
        }
    }

    public ListenableProperty<bool> AttackEnable
    {
        get
        {
            return _attackEnable;
        }
    }

    public ListenableProperty<bool> FocusAble
    {
        get
        {
            return _focusAble;
        }
    }

    public ListenableList<IStrategyDecorator<AttackInfo>> AttackDecoratorList
    {
        get
        {
            return _attackDecoratorList;
        }
    }

    public ListenableList<IStrategyDecorator<AttackInfo>> UnderAttackDecoratorList
    {
        get
        {
            return _underAttackDecoratorList;
        }
    }


    //reset
    public virtual void Reset()
    {
        //delete all listeners
        this._attackDecoratorList.Clear();
        this._underAttackDecoratorList.Clear();
        this._buffList.Clear();
        this._turnStartListeners.Clear();
        this._maxHp.RemoveAllListeners();
        this._armor.RemoveAllListeners();
        this._defence.RemoveAllListeners();
        this._healthPoint.RemoveAllListeners();
        this._image.RemoveAllListeners();
        this._element.RemoveAllListeners();
        this._frozen.RemoveAllListeners();
        this._alive.RemoveAllListeners();
        this._attackEnable.RemoveAllListeners();

        this._armor.Property = 0;
        this._defence.Property = 0;
        this._healthPoint.Property = this._maxHp.Property;
        this._frozen.Property = false;
        this._alive.Property = true;
        this._attackEnable.Property = true;
    }

    // Turn start
    public virtual void StartTurn(BattleContext mgr)
    {

        //重置減傷值
        this._defence.Property = 0;

        //回合開始事件通知
        List<IActorTurnStartListener> listeners = new List<IActorTurnStartListener>();
        listeners.AddRange(_turnStartListeners.GetAll());
        foreach (IActorTurnStartListener listener in listeners)
        {
            listener.OnTurnStart();
        }
    }

    // Turn end
    public virtual void EndTurn(BattleContext mgr)
    {
    }

    public void AddTurnStartListener(IActorTurnStartListener listener)
    {
        _turnStartListeners.AddItem(listener);
    }

    public void RemoveTurnStartListener(IActorTurnStartListener listener)
    {
        _turnStartListeners.RemoveItem(listener);
    }

    public virtual void PrintData()
    {
        Debug.Log("Actor Element: " + Element + ",Armor: " + _armor.Property + ",Defence: " + _defence.Property + ",HP: " + _healthPoint.Property + ",Max HP: " + _maxHp.Property + ",Buff Count: " + _buffList.GetCount());

        foreach (ICardAction buff in _buffList.GetAll())
        {
            Debug.Log("Effect Data: " + buff.ToString());
        }
    }

    private class IBuffComparator : IComparer<IBuff>
    {
        public int Compare(IBuff x, IBuff y)
        {
            return x.GetPriority() - y.GetPriority();
        }
    }

    private class IDecoratorComparator : IComparer<IStrategyDecorator<AttackInfo>>
    {
        public int Compare(IStrategyDecorator<AttackInfo> x, IStrategyDecorator<AttackInfo> y)
        {
            return x.GetPriority() - y.GetPriority();
        }
    }

    private class ITurnStartListenerComparator : IComparer<IActorTurnStartListener>
    {
        public int Compare(IActorTurnStartListener x, IActorTurnStartListener y)
        {
            return x.GetPriority() - y.GetPriority();
        }
    }
}

// 回合開始監聽器
public interface IActorTurnStartListener
{
    void OnTurnStart();
    int GetPriority();
}

public interface IStrategyDecorator<T>
{
    T Decorate(Actor source, Actor target, T value, BattleContext mgr);
    int GetPriority();
}

public interface IBuff
{
    int GetPriority();
    bool IsPositiveEffect();
}
