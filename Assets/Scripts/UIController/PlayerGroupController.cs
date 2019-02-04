using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* 綁定 UI 跟 玩家List */

public class PlayerGroupController : MonoBehaviour
{
    // prefabs of a player
    public GameObject playerPrefab;
    public float Scale = 1.0f;


    public void Bind(PlayerInstance player)
    {

        GameObject newPlayer = Instantiate(playerPrefab, transform.position, transform.localRotation) as GameObject;
        newPlayer.transform.SetParent(this.transform);
        newPlayer.transform.localScale = new Vector3(Scale, Scale, Scale);
        PlayerUIController cardController = newPlayer.GetComponent<PlayerUIController>();
        cardController.Bind(player);

    }


}
