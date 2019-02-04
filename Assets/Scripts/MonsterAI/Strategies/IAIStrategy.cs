using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAIStrategy
{
    MonsterCardInstance RunStrategy(EnemyInstance self, BattleContext context);
}
