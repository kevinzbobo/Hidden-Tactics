using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Battle manager
public class BattleManager
{

    // Messages
    public const string END_TURN = "END_TURN";
    public const string PLAY_ELEMENT_CARD = "PLAY_ELEMENT_CARD";
    public const string PLAY_ULTIMATE_CARD = "PLAY_ULTIMATE_CARD";
    public const string GAME_OVER = "GAME_OVER";
    public const string GAME_WIN = "GAME_WIN";
    // Context
    private BattleContext _battleContext;

    private List<BattleEventListener> _battleEventListener = new List<BattleEventListener>();

    // Testing
    public void Initialize(LevelContext context)
    {

        //init battle context
        this._battleContext = new BattleContext();
        DataManager.Instance.BattleContext = this._battleContext;

        _battleContext.EnemyList = new ListenableList<EnemyInstance>();
        _battleContext.ActorList = new ListenableList<Actor>();
        _battleContext.CurrentActor = new ListenableProperty<Actor>(null);
        _battleContext.HightLightCardsList = new ListenableList<ElementCardInstance>();

        // Load Characters
        _battleContext.Player = context.Player;
        _battleContext.ActorList.AddItem(_battleContext.Player);

        // Load Cards
        _battleContext.Player.CardDeck.AddItems(context.ElementCardList);
        _battleContext.Player.CardDeck.Shuffle();
        _battleContext.UltimateCards = context.UltimateCardList;

        // Load Enemy
        _battleContext.EnemyList.AddItems(context.EnemyList);
        _battleContext.ActorList.AddItems(_battleContext.EnemyList.GetAll());

        // Binding View
        OnBindView();

        // Register event listeners
        RegisterEvent();

        // Initialization
        _battleContext.TurnNumber = 0;
        
        // Start
        StartTurn();
    }

    // Binding View Model
    private void OnBindView()
    {
        HandHeldCardsController handheldCardsController = Object.FindObjectOfType<HandHeldCardsController>();
        PlayerGroupController playerGroupController = Object.FindObjectOfType<PlayerGroupController>();
        EnemyGroupController enemyGroupController = Object.FindObjectOfType<EnemyGroupController>();
        PowerUIController powerUIController = Object.FindObjectOfType<PowerUIController>();
        EndTurnUIController endTurnController = Object.FindObjectOfType<EndTurnUIController>();
        ComboButtonGroupController comboBtnGpController = Object.FindObjectOfType<ComboButtonGroupController>();

        //binding player UI
        playerGroupController.Bind(_battleContext.Player);
        enemyGroupController.Bind(_battleContext.EnemyList);

        //binding Power UI
        powerUIController.Bind(_battleContext.Player.Power);

        //binding cards UI
        handheldCardsController.Bind(_battleContext.Player.HandheldSet);

        // Binding End Turn Button
        endTurnController.Bind(_battleContext.CurrentActor);

        // Binding Ultimate Cards
        comboBtnGpController.HideAll();
        for (int cnt = 0; cnt < _battleContext.UltimateCards.Count; cnt++)
        {
            comboBtnGpController.BindUltimateCard(cnt, _battleContext.UltimateCards[cnt]);
        }
    }

    // Register event listeners
    private void RegisterEvent()
    {
        EventManager.StartListening(END_TURN, EndTurn);
        EventManager.StartListening(PLAY_ELEMENT_CARD, OnElementCardPlayed);
        EventManager.StartListening(PLAY_ULTIMATE_CARD, OnUltimateCardPlayed);
    }

    // Turn start
    private void StartTurn()
    {
        /* 更新回合資料 */
        if (_battleContext.CurrentActor.Property == _battleContext.Player)
        {
            _battleContext.TurnNumber++;
        }

        /* 取得回合控制角色 */
        if (null == _battleContext.CurrentActor.Property)
        {
            _battleContext.CurrentActor.Property = _battleContext.Player;
        }
        else
        {
            // 換成下一位角色
            int actorIdx = _battleContext.ActorList.IndexOf(_battleContext.CurrentActor.Property);
            if (actorIdx >= 0)
            {
                actorIdx = (actorIdx + 1) % _battleContext.ActorList.GetCount();
                _battleContext.CurrentActor.Property = _battleContext.ActorList.Get(actorIdx);
            }
        }

        // notify actor to start their turn
        _battleContext.CurrentActor.Property.StartTurn(_battleContext);

        Debugging();

        // if it is monster, call AI script and then end turn
        if (_battleContext.CurrentActor.Property is EnemyInstance)
        {
            if (!CheckGameEnded())
            {
                EndTurn(null);
            }
        }
    }

    private void Debugging()
    {
        if (true)
        {
            return;
        }

        //DEBUGGING
        Debug.Log("----------------start-------------------");
        _battleContext.PrintData();
        foreach (Actor actor in _battleContext.ActorList.GetAll())
        {
            actor.PrintData();
        }

        return;

        //_dataManager.Player.HandheldSet.Get(0).Play(_dataManager.Player, _dataManager.Player, _dataManager);
        for (int cnt = 0; cnt < _battleContext.Player.HandheldSet.GetCount(); cnt++)
        {
            Debug.Log("----------------card-------------------: " + cnt);
            //_battleContext.Player.HandheldSet.Get(cnt).Play(_battleContext.Player, _battleContext.EnemyList.Get(0), _battleContext);
            foreach (Actor actor in _battleContext.ActorList.GetAll())
            {
                actor.PrintData();
            }
        }

    }

    // End Turn
    private void EndTurn(System.Object arg)
    {
        Debug.Log("End turn?");

        /* 發送回合結束事件 */
        _battleContext.CurrentActor.Property.EndTurn(_battleContext);

        // 開始對方回合
        StartTurn();
    }

    private bool CheckGameEnded()
    {
        // 檢查角色HP值
        foreach (Actor actor in _battleContext.ActorList.GetAll())
        {
            if (actor.HealthPoint.Property <= 0)
            {
                actor.Alive.Property = false;
                _battleContext.ActorList.RemoveItem(actor);
            }
        }

        // check battle ended (lost)
        if (_battleContext.Player.HealthPoint.Property <= 0)
        {
            // game over
            EventManager.TriggerEvent(GAME_OVER);

            List<BattleEventListener> listenerList = new List<BattleEventListener>();
            listenerList.AddRange(this._battleEventListener);
            foreach (BattleEventListener listener in listenerList)
            {
                listener.OnGameLost();
            }

            return true;
        }

        // check game ended (win)
        if (0 == _battleContext.EnemyList.GetCount())
        {
            EventManager.TriggerEvent(GAME_WIN);

            List<BattleEventListener> listenerList = new List<BattleEventListener>();
            listenerList.AddRange(this._battleEventListener);
            foreach (BattleEventListener listener in listenerList)
            {
                listener.OnGameWin();
            }

            return true;
        }

        return false;
    }

    // Playing element card
    private void OnElementCardPlayed(System.Object arg)
    {
        if (arg is ElementCardPlayEvent)
        {
            Debug.Log("on card played received");
            ElementCardPlayEvent playEvent = (ElementCardPlayEvent)arg;
            List<Actor> targets = new List<Actor>();
            if (null != playEvent.Targets)
            {
                targets.AddRange(playEvent.Targets);
            }
            else
            {
                //debug
                if (playEvent.Card.IsAgainstEnemy)
                {
                    if (playEvent.Card.IsAOE)
                    {
                        foreach (Actor actor in _battleContext.EnemyList.GetAll())
                        {
                            targets.Add(actor);
                        }
                    }
                    else
                    {
                        targets.Add(_battleContext.EnemyList.Get(0));
                    }
                }
                else
                {
                    targets.Add(_battleContext.Player);
                }
            }

            // 減少Power
            _battleContext.Player.Power.Property -= playEvent.Card.Cost.Property;

            // 使用卡牌
            playEvent.Card.Play(_battleContext.Player, targets, _battleContext);

            /*
            // debug
            _battleContext.PrintData();
            foreach (Actor actor in _battleContext.ActorList.GetAll())
            {
                actor.PrintData();
            }*/

            _battleContext.Player.HandheldSet.RemoveItem(playEvent.Card);
            _battleContext.Player.Graveyard.AddItem(playEvent.Card);

            // 檢查角色HP值
            CheckGameEnded();
        }
    }

    // Playing ultimate card
    private void OnUltimateCardPlayed(System.Object arg)
    {
        if (arg is UltimateCardPlayEvent)
        {
            UltimateCardPlayEvent playEvent = (UltimateCardPlayEvent)arg;

            // remove element cards
            _battleContext.Player.HandheldSet.RemoveItem(playEvent.LeftCard);
            _battleContext.Player.HandheldSet.RemoveItem(playEvent.RightCard);
            _battleContext.Player.Graveyard.AddItem(playEvent.LeftCard);
            _battleContext.Player.Graveyard.AddItem(playEvent.RightCard);

            List<Actor> targets = new List<Actor>();
            targets.AddRange(playEvent.Targets);
            playEvent.Card.Play(_battleContext.Player, targets, _battleContext);

            CheckGameEnded();
        }
    }

    public void Destroy()
    {

        if (null != this._battleContext)
        {
            this._battleContext.ActorList = null;
            this._battleContext.CurrentActor = null;
            this._battleContext.EnemyList = null;
            this._battleContext.Player = null;
            this._battleContext = null;
            this._battleContext.HightLightCardsList = null;
        }

        EventManager.StopListening(END_TURN, EndTurn);
        EventManager.StopListening(PLAY_ELEMENT_CARD, OnElementCardPlayed);
        EventManager.StopListening(PLAY_ULTIMATE_CARD, OnUltimateCardPlayed);
    }

    public void AddBattleEventListener(BattleEventListener listener)
    {
        this._battleEventListener.Add(listener);
    }

    public void RemoveBattleEventListener(BattleEventListener listener)
    {
        this._battleEventListener.Remove(listener);
    }
}


public interface BattleEventListener
{
    void OnGameLost();
    void OnGameWin();
}


