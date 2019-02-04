using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAddDamagePerTurn : ICardAction
{

    private float _probability;
    private int _generalDamage = 0;
    private int _boostedDamage = 0;
    private int _suppressedDamage = 0;
    private int _turns = 0;

    public float Probability
    {
        get
        {
            return _probability;
        }
        set
        {
            _probability = value;
        }
    }

    public int GeneralDamage
    {
        get
        {
            return _generalDamage;
        }
        set
        {
            _generalDamage = value;
        }
    }

    public int BoostedDamage
    {
        get
        {
            return _boostedDamage;
        }
        set
        {
            _boostedDamage = value;
        }
    }

    public int SuppressedDamage
    {
        get
        {
            return _suppressedDamage;
        }
        set
        {
            _suppressedDamage = value;
        }
    }

    public int Turns
    {
        get
        {
            return _turns;
        }
        set
        {
            _turns = value;
        }
    }

    // 當效果被附加至對象對被呼叫
    public void RunAction(Actor source, Actor target, Card card, BattleContext manager)
    {
        //根據機率附加新效果
        float result = Random.Range(0f, 1f);
        if (result <= _probability)
        {
            DamagePerTurn effect = new DamagePerTurn();
            effect.GeneralDamage = _generalDamage;
            effect.BoostedDamage = _boostedDamage;
            effect.SuppressedDamage = _suppressedDamage;
            effect.Turns = _turns;

            effect.RunAction(source, target, card, manager);
        }
    }

}
