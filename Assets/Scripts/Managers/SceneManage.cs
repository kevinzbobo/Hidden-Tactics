using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class SceneManage : MonoBehaviour
{

    //Level data
    private LevelData _leveldata;

    // public data
    public GameObject Background;
    public GameObject MusicController;
    public GameObject PlayerGroup;
    public GameObject EnemyGroup;
    public GameObject HandHeld;
    public GameObject TurnEndButton;
    public GameObject PowerGroup;
    public GameObject ScrollingBar;

    // temporary data
    private SpriteRenderer _backgroundImage;
    private AudioSource _backgroundAudioSource;
    private BattleManager _battleManager;
    private Resolution _screenResolution;   // detect screen resolution change

    void Start()
    {
        // for testing
        LevelData leveldata = LevelLoader.LoadLevelData(1, 1);

        _screenResolution = Screen.currentResolution;

        SearchViews();
        LoadLevel(leveldata);

        Debug.Log("Screen Width : " + Screen.width);
    }

    private void SearchViews()
    {
        // search background
        _backgroundImage = Background.GetComponent<SpriteRenderer>();

        // search audio controller
        _backgroundAudioSource = MusicController.GetComponent<AudioSource>();
    }

    // load level data
    private void LoadLevel(LevelData leveldata)
    {
        this._leveldata = leveldata;

        // load background image
        Sprite bgImg = Resources.Load<Sprite>(Constants.BackgroundImagePath + Path.DirectorySeparatorChar + leveldata.BackgroundImage);
        Debug.Log(Constants.BackgroundImagePath + Path.DirectorySeparatorChar + leveldata.BackgroundImage);
        if (null != _backgroundImage && null != bgImg)
        {
            _backgroundImage.sprite = bgImg;
        }
        else
        {
            Debug.LogError("Unable to load background image");
        }

        // load music controller
        AudioClip bgMusicAudio = Resources.Load<AudioClip>(Constants.AudioPath + Path.DirectorySeparatorChar + leveldata.Music);
        Debug.Log(Constants.AudioPath + Path.DirectorySeparatorChar + leveldata.Music);
        if (null != _backgroundAudioSource && null != bgMusicAudio)
        {
            _backgroundAudioSource.clip = bgMusicAudio;
            _backgroundAudioSource.Play();
        }
        else
        {
            Debug.LogError("Unable to load background music");
        }

        ReScale();

        _battleManager = new BattleManager();
        _battleManager.Initialize(CreateContext(leveldata));
    }

    private LevelContext CreateContext(LevelData leveldata)
    {

        CardLoader _cardLoader = new CardLoader();
        MonsterLoader monsterLoader = new MonsterLoader();
        CharacterLoader _characterLoader = new CharacterLoader();

        //init level context
        LevelContext context = new LevelContext();
        context.LevelData = leveldata;
        context.Player = _characterLoader.LoadUserCharacter();
        context.EnemyList.AddRange(monsterLoader.LoadEnemies(leveldata.Level, leveldata.Mission));
        context.ElementCardList.AddRange(_cardLoader.LoadUserCards(context.Player.Properties.ElementCards));
        context.UltimateCardList.AddRange(_cardLoader.LoadUltimateCards(context.Player.Properties.UltimateCards));

        return context;
    }

    // adjust position according to screen size
    private void ReScale()
    {
        // scale
        float width = _backgroundImage.sprite.bounds.size.x;
        float height = _backgroundImage.sprite.bounds.size.y;

        float worldScreenHeight = Camera.main.orthographicSize * 2.0f;
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        // adjust background
        _backgroundImage.transform.localScale = new Vector3(worldScreenWidth / width, worldScreenHeight / height, 0);
        _backgroundImage.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 1));

        PlayerGroup.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 0.21f, Screen.height * 0.43f, 1));
        EnemyGroup.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 0.65f, Screen.height * 0.43f, 1));

        HandHeld.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 0.5f, Screen.height * 0.26f, 1));
        TurnEndButton.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 0.92f, Screen.height * 0.23f, 1));

        PowerGroup.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 0.5f, Screen.height * 0.06f, 1));

        ScrollingBar.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 0.5f, Screen.height * 0.93f, 1));
    }

    void Update()
    {

        if (!Screen.currentResolution.Equals(_screenResolution))
        {
            _screenResolution = Screen.currentResolution;
            ReScale();
        }
    }

}



