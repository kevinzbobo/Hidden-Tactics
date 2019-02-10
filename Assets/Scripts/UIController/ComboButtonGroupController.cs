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

    public void HideAll()
    {
        MetalController.SetVisible(false);
        WoodController.SetVisible(false);
        WaterController.SetVisible(false);
        FireController.SetVisible(false);
        EarthController.SetVisible(false);
    }

    public void BindUltimateCard(int position, UltimateCardInstance card)
    {
        switch(position){
            case 0:{
                    MetalController.Bind(card);
                    MetalController.SetVisible(true);
                    break;
            }
            case 1:
                {
                    WoodController.Bind(card);
                    WoodController.SetVisible(true);
                    break;
                }
            case 2:
                {
                    WaterController.Bind(card);
                    WaterController.SetVisible(true);
                    break;
                }
            case 3:
                {
                    FireController.Bind(card);
                    FireController.SetVisible(true);
                    break;
                }
            case 4:
                {
                    EarthController.Bind(card);
                    EarthController.SetVisible(true);
                    break;
                }
        }
    }

}
