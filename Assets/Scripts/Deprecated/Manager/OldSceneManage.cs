using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class OldSceneManage : MonoBehaviour
{

    //Level data
    private LevelData _leveldata;

    // public data
    public GameObject Background;
    public GameObject MusicController;

    // temporary data
    private SpriteRenderer _backgroundImage;
    private AudioSource _backgroundAudioSource;
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

        //ReScale();

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


    }

    void Update()
    {

        if (!Screen.currentResolution.Equals(_screenResolution))
        {
            _screenResolution = Screen.currentResolution;
            //ReScale();
        }
    }

}



