using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelContext
{

    private int _money;
    private LevelData _levelData;
    private List<EnemyInstance> _enemyList = new List<EnemyInstance>();
    private PlayerInstance _player;
    private List<ElementCardInstance> _elementCardList = new List<ElementCardInstance>();
    private List<UltimateCardInstance> _ultimateCardList = new List<UltimateCardInstance>();

    public List<EnemyInstance> EnemyList
    {
        get
        {
            return this._enemyList;
        }
    }

    public PlayerInstance Player
    {
        get
        {
            return this._player;
        }
        set
        {
            this._player = value;
        }
    }

    public List<ElementCardInstance> ElementCardList
    {
        get
        {
            return this._elementCardList;
        }
    }

    public List<UltimateCardInstance> UltimateCardList
    {
        get
        {
            return this._ultimateCardList;
        }
    }

    public LevelData LevelData
    {
        get
        {
            return this._levelData;
        }
        set
        {
            this._levelData = value;
        }
    }

    public int Money
    {
        get
        {
            return _money;
        }
        set
        {
            this._money = value;
        }
    }

}
