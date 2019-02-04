using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboButtonGroupController : MonoBehaviour
{
    public ComboButtonController MetalController;
    public ComboButtonController WoodController;
    public ComboButtonController WaterController;
    public ComboButtonController FireController;
    public ComboButtonController EarthController;

    public void BindUltimateCard(int position, UltimateCardInstance card)
    {
        switch(position){
            case 0:{
                    MetalController.Bind(card);
                    break;
            }
            case 1:
                {
                    WoodController.Bind(card);
                    break;
                }
            case 2:
                {
                    WaterController.Bind(card);
                    break;
                }
            case 3:
                {
                    FireController.Bind(card);
                    break;
                }
            case 4:
                {
                    EarthController.Bind(card);
                    break;
                }
        }
    }

}
