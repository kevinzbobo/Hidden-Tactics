using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
public class ComboPaneUIController : MonoBehaviour
{
    public ComboCardUIController comboCardUIController;

    public GameObject CombineCardRight;
    public GameObject CombineCardLeft;
    public Texture2D TargetCurse;
    private UltimateCardInstance ultimate;

    public void setUltimateCardInstance(UltimateCardInstance _ultimate)
    {
        ultimate = _ultimate;
    }

    public void SetVisible(bool isVisible)
    {
        this.transform.gameObject.SetActive(isVisible);
    }

    public void Cancel()
    {
        //return the uncombine card to the handheld
        if (CombineCardRight.transform.childCount > 0)
        {
            ElementCardInstance tempInstance = CombineCardRight.transform.GetChild(0).GetComponent<ElementCardUIController>().GetCardInstance();
            DataManager.Instance.BattleContext.Player.HandheldSet.AddItem(tempInstance);
            Destroy(CombineCardRight.transform.GetChild(0).gameObject);
        }

        if (CombineCardLeft.transform.childCount > 0)
        {
            ElementCardInstance tempInstance = CombineCardLeft.transform.GetChild(0).GetComponent<ElementCardUIController>().GetCardInstance();
            DataManager.Instance.BattleContext.Player.HandheldSet.AddItem(tempInstance);
            Destroy(CombineCardLeft.transform.GetChild(0).gameObject);
        }

        //setvisible
        PlayerPrefs.SetInt("ComboCardPanelState", 0);
        comboCardUIController.SetVisible(false);
        this.SetVisible(false);

        //remove highlight
        GameObject[] highLight = GameObject.FindGameObjectsWithTag("CardHighlight");
        foreach (GameObject obj in highLight)
        {
            obj.SetActive(false);
        }

        HoverPreview.PreviewsAllowed = true;
    }

 

    public void ComboCardPolymery()
    { 
        //combine the card
        if (CombineCardLeft.transform.childCount > 0 && CombineCardRight.transform.childCount > 0)
        {
            GameObject left = CombineCardLeft.transform.GetChild(0).gameObject;
            GameObject right = CombineCardRight.transform.GetChild(0).gameObject;
            ElementCardInstance leftElement = left.GetComponent<ElementCardUIController>().GetCardInstance();
            ElementCardInstance rightElement = right.GetComponent<ElementCardUIController>().GetCardInstance();
            if (ultimate.IsCardPlayable(leftElement, rightElement))
            {

                Destroy(CombineCardRight.transform.GetChild(0).gameObject);
                Destroy(CombineCardLeft.transform.GetChild(0).gameObject);
                PlayerPrefs.SetInt("ComboCardPanelState", 0);
                DataManager.Instance.BattleContext.UltimateCard.Property = ultimate;
                comboCardUIController.SetVisible(false);
                this.SetVisible(false);
                HoverPreview.PreviewsAllowed = true;
                Cursor.SetCursor(TargetCurse,Vector2.zero, CursorMode.Auto);
            }
        }

        //remove highlight card from hand
        GameObject[] highLight = GameObject.FindGameObjectsWithTag("CardHighlight");
        foreach (GameObject obj in highLight)
        {
            if (obj.transform.parent.parent.parent.parent.tag == "Card")
            {
                ElementCardInstance card = obj.transform.parent.parent.parent.parent.GetComponent<ElementCardUIController>().GetCardInstance();
                DataManager.Instance.BattleContext.Player.HandheldSet.RemoveItem(card);
            }
        }
       
    }


}
