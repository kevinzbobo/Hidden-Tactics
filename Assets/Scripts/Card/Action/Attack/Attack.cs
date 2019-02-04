using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : ICardAction
{

    private int _basicAttack = 0;
    private int _boostedAttack = 0;
    private int _suppressedAttack = 0;
    private int _armorMask = 0;

    public int BasicAttack
    {
        get
        {
            return _basicAttack;
        }
        set
        {
            _basicAttack = value;
        }
    }

    public int BoostedAttack
    {
        get
        {
            return _boostedAttack;
        }
        set
        {
            _boostedAttack = value;
        }
    }

    public int SuppressedAttack
    {
        get
        {
            return _suppressedAttack;
        }
        set
        {
            _suppressedAttack = value;
        }
    }

    public int ArmorMask
    {
        get
        {
            return _armorMask;
        }
        set
        {
            _armorMask = value;
        }
    }

    // On effect run to the actor
    public void RunAction(Actor source, Actor target, Card card, BattleContext manager)
    {
        AttackInfo info = new AttackInfo();
        info.Attack = this._basicAttack;

        if (Utilities.IsOvercominInteraction(Utilities.GetElement(target.Element), Utilities.GetElement(card.Element)))
        {
            info.Attack = this._boostedAttack;
        }
        else if (Utilities.IsOvercominInteraction(Utilities.GetElement(card.Element), Utilities.GetElement(target.Element)))
        {
            info.Attack = this._suppressedAttack;
        }

        // Execute Attack Decorators
        foreach (IStrategyDecorator<AttackInfo> decorator in source.AttackDecoratorList.GetAll())
        {
            info = decorator.Decorate(source, target, info, manager);
        }

        // Execute Underattack Decorators
        foreach (IStrategyDecorator<AttackInfo> decorator in target.UnderAttackDecoratorList.GetAll())
        {
            info = decorator.Decorate(source, target, info, manager);
        }

        int mask = _armorMask + info.ArmorMask;

        if (_armorMask >= target.Armor.Property)
        {
            int targetHP = target.HealthPoint.Property;
            int targetDefence = target.Defence.Property;

            info.Attack -= targetDefence;
            if (info.Attack > 0)
            {
                target.HealthPoint.Property = targetHP - info.Attack;
            }
        }
        else
        {
            int targetArmor = target.Armor.Property - _armorMask;
            int targetHP = target.HealthPoint.Property;
            int targetDefence = target.Defence.Property;

            info.Attack -= targetDefence;

            if (info.Attack > 0)
            {
                targetArmor -= info.Attack; //Reduce armor
                if (targetArmor < 0)
                {
                    targetHP += (targetArmor);
                }

                targetArmor += _armorMask;

                if (targetArmor < 0)
                {
                    targetArmor = 0;
                }

                target.HealthPoint.Property = targetHP;
                target.Armor.Property = targetArmor;
            }
        }

    }

}
