using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Battle context */
public class BattleContext {

    public int TurnNumber;
    public ListenableProperty<Actor> CurrentActor;             //Current Actor
    public ListenableList<Actor> ActorList; //All actors

    public PlayerInstance Player; //Player
    public ListenableProperty<UltimateCardInstance> UltimateCard; // playing effect ultimatecard
    public ListenableList<EnemyInstance> EnemyList; //Enemy list
    public List<UltimateCardInstance> UltimateCards;
    public ListenableList<ElementCardInstance> HightLightCardsList; //highligt card list

    public void PrintData()
    {
        Debug.Log("Turn Number: " + TurnNumber);
        Debug.Log("Actor count: " + ActorList.GetCount());
        Debug.Log("Enemy count: " + EnemyList.GetCount());
    }
}