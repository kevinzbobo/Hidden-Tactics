using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class DraggableTestWithActions : MonoBehaviour {

    public bool UsePointerDisplacement = true;
    // PRIVATE FIELDS
    // a reference to a DraggingActionsTest script
    private DraggingActionsTest da;

    // a flag to know if we are currently dragging this GameObject
    private bool dragging = false;

    // distance from the center of this Game Object to the point where we clicked to start dragging 
    private Vector3 pointerDisplacement = Vector3.zero;

    // distance from camera to mouse on Z axis 
    private float zDisplacement;

    //col gameobject
    private GameObject colObj;

    //col gameobject type monster:1 player:2 combinecard:3 none:99
    private int coltype=99;

    //CardPrefab
    public GameObject CardPrefab;

    private Vector3 CobineLeftPos = new Vector3(255,290,0);
    private Vector3 CobineRightPos = new Vector3(508, 290, 0);
    private float CardScale = 41.0f;

    //combineCard
    private GameObject CombineCardLeft;
    private GameObject CombineCardRight;

    // MONOBEHAVIOUR METHODS
    void Awake()
    {
        da = GetComponent<DraggingActionsTest>();
    }

    void OnMouseDown()
    {
        //when combocard is active, can not drag
        if (da.CanDrag)
        {
            dragging = true;
            HoverPreview.PreviewsAllowed = false;
            da.OnStartDrag();
            zDisplacement = -Camera.main.transform.position.z + transform.position.z;
            if (UsePointerDisplacement)
                pointerDisplacement = -transform.position + MouseInWorldCoords();
            else
                pointerDisplacement = Vector3.zero;
        }
    }

    // Update is called once per frame
    void Update ()
    {
        if (dragging)
        { 
            Vector3 mousePos = MouseInWorldCoords();
            da.OnDraggingInUpdate();
            //Debug.Log(mousePos);
            transform.position = new Vector3(mousePos.x - pointerDisplacement.x, mousePos.y - pointerDisplacement.y, transform.position.z);   
        }
    }

    void OnMouseUp()
    {
        //check col gameobject
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray);
        for (int i= 0; i < hits.Length; i++)
        {
            Debug.Log(hits[i].collider.name);
            if (hits[i].collider.tag == "CombineCard")
            {
                colObj = hits[i].collider.gameObject;
                coltype = 3;
                break;
            }
            if (hits[i].collider.tag == "Monster")
            {
                colObj = hits[i].collider.gameObject;
                coltype = 1;
                break;
            }
            if (hits[i].collider.tag == "Player" )
            {
                colObj = hits[i].collider.gameObject;
                coltype = 2;
                break;
            }
        }

       
        //if is combinecard
        if (coltype == 3)
        {
            ElementCardInstance card = this.GetComponent<ElementCardUIController>().GetCardInstance();

            //at the combineCard position instantiate this card
            Vector3 pos = new Vector3();
            

            if (colObj.name == "CombineCardRight")
            {
                if (GameObject.Find("CombineCardRight").transform.childCount > 0)
                {
                    ElementCardInstance tempInstance = GameObject.Find("CombineCardRight").transform.GetChild(0).GetComponent<ElementCardUIController>().GetCardInstance();
                    DataManager.Instance.BattleContext.Player.HandheldSet.AddItem(tempInstance);
                    Destroy(GameObject.Find("CombineCardRight").transform.GetChild(0).gameObject);
                }
                pos = CobineRightPos;
                OnItemCard(card, pos);
            }
            else
            {
                if (GameObject.Find("CombineCardLeft").transform.childCount > 0)
                {
                    ElementCardInstance tempInstance = GameObject.Find("CombineCardLeft").transform.GetChild(0).GetComponent<ElementCardUIController>().GetCardInstance();
                    DataManager.Instance.BattleContext.Player.HandheldSet.AddItem(tempInstance);
                    Destroy(GameObject.Find("CombineCardLeft").transform.GetChild(0).gameObject);
                }
                pos = CobineLeftPos;
                OnItemCard(card, pos);
            }
            
        }

        //if is player or monster, play effect
        if (coltype < 3 && PlayerPrefs.GetInt("ComboCardPanelState") != 1)
        {          
                ElementCardPlayEvent playEvent = new ElementCardPlayEvent();
                playEvent.Card = this.GetComponent<ElementCardUIController>().GetCardInstance();
                Actor[] targetList = new Actor[1];
                if (colObj.tag == "Monster")
                {
                    targetList[0] = colObj.GetComponent<MonsterUIController>().EnemyInstance;
                }
                if (colObj.tag == "Player")
                {
                    targetList[0] = colObj.GetComponent<PlayerUIController>().PlayerInstance;
                }
                playEvent.Targets = targetList;
                EventManager.TriggerEvent(BattleManager.PLAY_ELEMENT_CARD, playEvent);         
        }

        
        if (dragging)
        {
            dragging = false;
            HoverPreview.PreviewsAllowed = true;
            colObj = null;
            coltype = 99;
            da.OnEndDrag();
        }
    }


    private void OnItemCard(ElementCardInstance card, Vector3 pos)
    {
        GameObject newCard = Instantiate(CardPrefab, pos, colObj.transform.localRotation) as GameObject;
        newCard.transform.localScale = new Vector3(CardScale, CardScale, 1);
        newCard.transform.SetParent(colObj.transform);
        ElementCardUIController controller = newCard.GetComponent<ElementCardUIController>();
        controller.Bind(card);
        DataManager.Instance.BattleContext.Player.HandheldSet.RemoveItem(card);
    }

    // returns mouse position in World coordinates for our GameObject to follow. 
    private Vector3 MouseInWorldCoords()
    {
        var screenMousePos = Input.mousePosition;
        //Debug.Log(screenMousePos);
        screenMousePos.z = zDisplacement;
        return Camera.main.ScreenToWorldPoint(screenMousePos);
    }
}
