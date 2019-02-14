using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInstance : Actor {

    private MonsterModel _monsterModel;
    private ListenableList<MonsterCardInstance> _cards; //playable card list
    private ListenableList<MonsterCardInstance> _cardQueue; //pending card list
    private IAIStrategy _strategy;
    private int _postpone = 0;

    public EnemyInstance(MonsterModel model) : base(model.HealthPoint, 0, 0, model.Img, model.Element)
    {
        this._monsterModel = model;
        this._cards = new ListenableList<MonsterCardInstance>();
        this._cardQueue = new ListenableList<MonsterCardInstance>();

        ObjectBuilder builder = new ObjectBuilder(model.Strategy.Name);
        foreach (JKeyValuePair pair in model.Strategy.Properties)
        {
            builder.SetProperty(pair.Key, pair.Value);
        }
        this._strategy = (IAIStrategy)builder.Build();

        foreach (JMonsterCardProperties cardData in model.MonsterCards)
        {
            this._cards.AddItem(new MonsterCardInstance(Constants.language_zh_tw, cardData));
        }
    }

    public MonsterModel GetModel()
    {
        return _monsterModel;
    }

    public ListenableList<MonsterCardInstance> GetCards()
    {
        return _cards;
    }

    public ListenableList<MonsterCardInstance> GetCardsQueue()
    {
        return _cardQueue;
    }

    // 敵人AI
    public override void StartTurn(BattleContext mgr)
    {
        base.StartTurn(mgr);

        if (this._postpone > 0)
        {
            this._postpone -= 1;
            return;
        }

        // get Card
        MonsterCardInstance card = null;
        // try to get card from queue
        if (_cardQueue.GetCount() > 0)
        {
            card = _cardQueue.Pop(1)[0];
        }
        else
        {
            // try to get card from strategy
            card = this._strategy.RunStrategy(this, mgr);

            if (null != card)
            {
                //postpone card
                if (card.Postpone > 0)
                {
                    _postpone += card.Postpone;
                    _cardQueue.AddItem(card);
                    return;
                }
            }
        }

        if (null != card)
        {

            // run card
            if (card.IsAgainstEnemy)
            {

                List<Actor> enemy = new List<Actor>();
                enemy.Add(mgr.Player);

                if (card.IsAOE)
                {
                    card.Play(this, enemy, mgr);
                }
                else
                {
                    List<Actor> singleTarget = new List<Actor>();
                    singleTarget.Add(enemy[Random.Range(0, enemy.Count)]);
                    card.Play(this, singleTarget, mgr);
                }
            }
            else
            {
                if (card.IsAOE)
                {

                    List<Actor> partners = new List<Actor>();
                    foreach (Actor partner in mgr.EnemyList.GetAll())
                    {
                        partners.Add(partner);
                    }

                    card.Play(this, partners, mgr);
                }
                else
                {
                    List<Actor> singleTarget = new List<Actor>();
                    singleTarget.Add(this);
                    card.Play(this, singleTarget, mgr);
                }
            }
        }

    }


}

