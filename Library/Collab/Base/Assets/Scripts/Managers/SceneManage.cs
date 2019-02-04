using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class SceneManage : MonoBehaviour
{

    // Level data, 需要其他接口传入信息
    public string Level;
    // Level path
    private string path;
    //Level data
    private LevelData leveldata;
    public GameObject bg;
    public GameObject BgMusic;
    private int monsterCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        //加载bg
        bg = GameObject.Find("Background");
        if (bg != null)
        {
            string bgpath = "Image/Backgrounds/" + leveldata.Bg;
            Debug.Log(bgpath);
            Sprite bgImg = Resources.Load<Sprite>(bgpath);
            if (bgImg)
            {
                bg.GetComponent<SpriteRenderer>().sprite = bgImg;
                //全屏
                //float cameraHeight = Camera.main.orthographicSize * 2;
                //Vector2 cameraSize = new Vector2(Camera.main.aspect * cameraHeight, cameraHeight);
                //Vector2 spriteSize = bg.GetComponent<SpriteRenderer>().sprite.bounds.size;
                //Vector2 scale = bg.transform.localScale;
                //if (cameraSize.x >= cameraSize.y)
                //{ // Landscape (or equal)
                //    scale *= cameraSize.x / spriteSize.x;
                //}
                //else
                //{ // Portrait
                //    scale *= cameraSize.y / spriteSize.y;
                //}

                //bg.transform.position = Vector2.zero; 
                //bg.transform.localScale = scale;
                Debug.Log("Background img load success.");
            }
            else
            {
                Debug.Log("Load" + leveldata.Bg + "Img Failed. The bg path cannot be found.");
            }
        }
        else
        {
            Debug.Log("Cannot find the Gameobject Background.");
        }

        //加载monster
        foreach (var monster in leveldata.Monster)  
        {
            Vector3 monPosition = new Vector3(monster.Position[0], monster.Position[1], 0);
            Vector3 monScale = new Vector3(monster.Scale[0], monster.Scale[1], 0);
            Instantiate(Resources.Load("Prefabs/MonPrefab") as GameObject);
            GameObject tempMon = GameObject.Find("MonPrefab(Clone)");
            //改变属性
            if (tempMon)
            {
                //改变gameobject名字
                tempMon.name = "Monster" + monsterCount.ToString();
                monsterCount++;
                //改变大小位置
                tempMon.transform.position = monPosition;
                tempMon.transform.localScale = monScale;
                //改变monster对应的图片
                string bgpath = "Image/Monster/" + monster.Img;
                Debug.Log(bgpath);
                Sprite monImg = Resources.Load<Sprite>(bgpath);
                if (monImg)
                {
                    tempMon.GetComponent<SpriteRenderer>().sprite = monImg;
                }
                else
                {
                    Debug.Log("Load" + monster.Img + "Img Failed. The bg path cannot be found.");
                }
                //挂载脚本
                Type monScript = Type.GetType(monster.Script);
                if (monScript != null)
                {
                    tempMon.AddComponent(monScript);
                }
            }
            else
            {
                Debug.Log("Can't find the gameobject MonPrefab(clone)");
            }
        }

        //加载背景音乐
        BgMusic = GameObject.Find("BgMusicController");
        if (BgMusic)
        {
            string bgMusicPath = "Music/Backgroundmusic/" + leveldata.Music;
            Debug.Log(bgMusicPath);
            AudioClip bgMusicAudio = Resources.Load<AudioClip>(bgMusicPath);
            if (bgMusicAudio)
            {
                BgMusic.GetComponent<AudioSource>().clip = bgMusicAudio;
                Debug.Log("BgMusic load success.");
                BgMusic.GetComponent<AudioSource>().Play();
            }
            else
            {
                Debug.Log("BgMusic load failed.");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //call before the start
    void Awake()
    {
        //Level为测试值，需要后续传入
        Level = "Level1_2";
        //Debug.Log(Level);
        path = Application.dataPath + "/Resources/Json/" + Level + ".json";
        //Debug.Log(path);
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            leveldata = JsonUtility.FromJson<LevelData>(json);
            Debug.Log("Load " + Level + " data Success.");
            //Debug.Log("Level:" + leveldata.Level);
            //Debug.Log("MonsterElement:" + leveldata.Monster[1].Element);
            //Debug.Log("MonsterPosition:" + leveldata.Monster[1].Position[0].ToString());
        }
        else
        {
            Debug.Log("Load" + Level + "Data Failed. The json path cannot be found.");
        }


        // For 測試onlyCardLoader
        //BattleManager _battleController = new BattleManager();
        //_battleController.initialize();
    }
}

[Serializable]
    public struct LevelData
{
    public string Level;
    public string Bg;
    public string Music;
    public List<MonsterModel> Monster;    
}

[Serializable]
    public struct MonsterModel
{
    public string Element;
    public string Img;
    public string Script;
    public float[] Position;
    public float[] Scale;
}


