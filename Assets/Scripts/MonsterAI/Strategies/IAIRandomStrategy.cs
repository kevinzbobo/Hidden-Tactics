using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAIRandomStrategy : IAIStrategy
{

    private float[] _probabilities;

    public float[] Command
    {
        get
        {
            return this._probabilities;
        }
        set
        {
            this._probabilities = value;
        }
    }

    public MonsterCardInstance RunStrategy(EnemyInstance self, BattleContext context)
    {
        float result = Random.Range(0f, 1.0f);

        MonsterCardInstance card = null;
        for (int cnt = 0; cnt < _probabilities.Length; cnt++)
        {
            result -= _probabilities[cnt];
            if (result <= 0.0f)
            {
                if (self.GetCards().GetCount() > cnt)
                {
                    card = self.GetCards().Get(cnt);
                }
                break;
            }
        }

        return card;
    }
}
