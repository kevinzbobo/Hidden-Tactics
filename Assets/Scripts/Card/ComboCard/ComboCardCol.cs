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

    private void OnMouseEnter()
    {
        ComboCard.SetActive(true);
    }

    private void OnMouseExit()
    {
        ComboCard.SetActive(false);
    }
}
