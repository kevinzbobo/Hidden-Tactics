using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusBarUIController : MonoBehaviour
{
    public GameObject Icon;
    public GameObject Settings;

    private Resolution _screenResolution;   // detect screen resolution change

    void Start()
    {
    }

    void Update()
    {

        if (!Screen.currentResolution.Equals(_screenResolution))
        {

        }
    }
}
