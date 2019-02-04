using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCardInstance : Card
{

    private JMonsterCardProperties _monsterCardProperties;

    public MonsterCardInstance(string language, JMonsterCardProperties properties) : base(properties.Element, properties.TargetEffects, properties.PlayerEffects, properties.IsAOE, properties.IsAgainstEnemy)
    {
        this._monsterCardProperties = properties;

        // language
        foreach (JCardLocale local in properties.locales)
        {
            if (language.Equals(local.language))
            {
                break;
            }
        }
    }
}
