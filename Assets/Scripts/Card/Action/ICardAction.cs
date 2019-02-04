using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Effect
public interface ICardAction
{
    // On effecr added the the actor
    void RunAction(Actor source, Actor target, Card card, BattleContext manager);

}

