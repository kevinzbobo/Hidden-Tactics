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

    // MONOBEHAVIOUR METHODS
    void Awake()
    {
        da = GetComponent<DraggingActionsTest>();
    }

    void OnMouseDown()
    {
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
            if (hits[i].collider.tag == "Player" || hits[i].collider.tag == "Monster")
            {
                colObj = hits[i].collider.gameObject;
            }
        }

        //play effect
        if (colObj)
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
            EventManager.TriggerEvent("PLAY_ELEMENT_CARD", playEvent);
        }
        
        if (dragging)
        {
            dragging = false;
            HoverPreview.PreviewsAllowed = true;
            colObj = null;
            da.OnEndDrag();
        }
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
