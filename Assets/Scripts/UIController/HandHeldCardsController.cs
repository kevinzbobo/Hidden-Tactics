using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Handheld cards controller
public class HandHeldCardsController : MonoBehaviour, IListChangeListener<ElementCardInstance>
{

    // Prefabs of a card instance
    public GameObject CardPrefab;

    // Config
    public float CardDistance = 0;
    public float CardScale = 1.0f;

    // Handheld cards
    private ListenableList<ElementCardInstance> _cardList;

    // Unregister listener
    void OnDestroy()
    {
        if (null != _cardList)
        {
            _cardList.RemoveListener(this);
        }
    }

    // Binding UI and ViewModel
    public void Bind(ListenableList<ElementCardInstance> cardList)
    {
        this._cardList = cardList;
        this._cardList.AddListener(this);

        // Update Cards
        foreach (ElementCardInstance card in cardList.GetAll())
        {
            OnItemAdd(card);
        }
    }

    // On Card Added
    public void OnItemAdd(ElementCardInstance data)
    {
        GameObject newCard = Instantiate(CardPrefab, transform.position, transform.localRotation) as GameObject;
        newCard.transform.SetParent(this.transform);
        newCard.transform.localScale = new Vector3(CardScale, CardScale, 1);
        ElementCardUIController controller = newCard.GetComponent<ElementCardUIController>();
        controller.Bind(data);

        //update position, align center
        AlignCenter();
    }

    // On Card Removed
    public void OnItemRemove(ElementCardInstance data)
    {
        ElementCardUIController[] childs = transform.GetComponentsInChildren<ElementCardUIController>();
        foreach (ElementCardUIController controller in childs)
        {
            if (controller.GetCardInstance() == data)
            {
                controller.gameObject.SetActive(false);
                controller.transform.SetParent(null);
                Destroy(controller.gameObject);
                break;
            }
        }

        //update position, align center
        AlignCenter();
    }

    // Rearrange position
    private void AlignCenter()
    {
        //calculate position
        float totalWidth = 0;
        for (int counter = 0; counter < transform.childCount; counter++){
            totalWidth += transform.GetChild(counter).GetComponent<Collider>().bounds.size.x;
        }
        if (transform.childCount > 0)
        {
            totalWidth += CardDistance * (transform.childCount - 1);
        }

        //arrange position
        float positionX = - totalWidth * 0.5f + transform.position.x;
        if (transform.childCount > 0)
        {
            positionX += transform.GetChild(0).GetComponent<Collider>().bounds.size.x;
        }
        for (int counter = 0; counter < transform.childCount; counter++)
        {
            Transform child = transform.GetChild(counter);
            child.position = new Vector3(positionX, child.position.y, child.position.z);
            positionX += child.GetComponent<Collider>().bounds.size.x + CardDistance;
        }
    }
}
