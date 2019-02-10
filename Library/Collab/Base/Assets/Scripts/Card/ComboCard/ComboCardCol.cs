using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboCardCol : MonoBehaviour
{
    public GameObject ComboCard;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ComboButtonClick()
    {
        ComboCard.SetActive(true);
    }

    public void ComboCardClose()
    {
        ComboCard.SetActive(false);
    }

    public void ComboCardPolymery()
    {
        ComboCard.SetActive(false);
        Debug.Log("play combo");
    }
}
