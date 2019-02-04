using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Binding Enemy List and UI
public class EnemyGroupController : MonoBehaviour, IListChangeListener<EnemyInstance>
{
    // prefabs of a player
    public GameObject enemyPrefab;
    public float distance = 0;
    public float Scale = 1.0f;

    // Data
    private ListenableList<EnemyInstance> _enemyList;

    // Unregister listener
    void OnDestroy()
    {
        if (null != _enemyList)
        {
            this._enemyList.RemoveListener(this);
        }
        _enemyList = null;
    }

    public void Bind(ListenableList<EnemyInstance> enemyList)
    {
        this._enemyList = enemyList;
        this._enemyList.AddListener(this);

        foreach (EnemyInstance enemyInstance in enemyList.GetAll())
        {
            OnItemAdd(enemyInstance);
        }

    }

    // if there is new monsters, instantiate a prehabs
    public void OnItemAdd(EnemyInstance data)
    {

        Vector3 position = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        if (transform.childCount > 0)
        {
            Transform lastChild = transform.GetChild(transform.childCount - 1);
            position = new Vector3(lastChild.transform.position.x, lastChild.transform.position.y, lastChild.transform.position.z);
            position.x += lastChild.GetComponent<Collider>().bounds.size.x + distance; 
        }

        GameObject newMonster = Instantiate(enemyPrefab, position, transform.localRotation) as GameObject;
        newMonster.transform.SetParent(this.transform, true);
        newMonster.transform.localScale = new Vector3(Scale, Scale, 1.0f);
        MonsterUIController monsterController = newMonster.GetComponent<MonsterUIController>();
        monsterController.LoadMonster(data);

        AlignCenter();
    }

    private void AlignCenter()
    {
        float leftPosition = 0, rightPosition = 0;

        for(int idx = 0; idx < transform.childCount; idx++)
        {
            Transform child = transform.GetChild(idx);
            if (0 == idx)
            {
                leftPosition = child.localPosition.x;
                rightPosition = child.localPosition.x;
            }

            if (child.localPosition.x > rightPosition)
            {
                rightPosition = child.localPosition.x;
            }
            if (child.localPosition.x < leftPosition)
            {
                leftPosition = child.localPosition.x;
            }
        }

        float center = (rightPosition - leftPosition) * 0.5f;

        for (int cnt = 0; cnt < transform.childCount; cnt++)
        {
            Transform child = transform.GetChild(cnt);
            child.transform.localPosition -= new Vector3(center, 0, 0);
        }
    }

    // if monster is deleted from list, destroy it
    public void OnItemRemove(EnemyInstance data)
    {
        MonsterUIController[] childs = transform.GetComponentsInChildren<MonsterUIController>();
        foreach (MonsterUIController controller in childs)
        {
            if (controller.EnemyInstance == data)
            {
                Destroy(controller.gameObject);
                break;
            }
        }
    }
}

