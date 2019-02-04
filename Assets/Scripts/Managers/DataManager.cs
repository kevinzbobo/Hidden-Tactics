using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A manager holding game context
public class DataManager
{
    // data
    public LevelContext LevelContext;
    public BattleContext BattleContext;

    // this instance
    private static DataManager _dataManager;
    private static readonly object padlock = new object();

    public static DataManager Instance
    {
        get
        {
            if (null == _dataManager)
            {
                lock (padlock)
                {
                    if (null == _dataManager)
                    {
                        _dataManager = new DataManager();
                    }
                }
            }

            return _dataManager;
        }
    }
}
