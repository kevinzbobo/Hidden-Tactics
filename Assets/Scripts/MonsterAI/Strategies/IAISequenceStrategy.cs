using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAISequenceStrategy : IAIStrategy
{

    private int[] _sequence;
    private int _currentSequence = 0;

    public int[] Sequence
    {
        get
        {
            return this._sequence;
        }
        set
        {
            this._sequence = value;
        }
    }

    public MonsterCardInstance RunStrategy(EnemyInstance self, BattleContext context)
    {
        _currentSequence = (_currentSequence + 1) % _sequence.Length;

       return self.GetCards().Get(_sequence[_currentSequence]);
    }
}
