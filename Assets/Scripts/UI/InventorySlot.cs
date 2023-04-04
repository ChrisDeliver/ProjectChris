using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Item CureItem;
    private Image icon;
    private Color transparent = new Color(1, 1, 1, 0);
    private GameObject itemObject;

    private void Start()
    {
        icon = gameObject.transform.GetChild(0).GetComponent<Image>();
    }

    public void PutIn(Item item, GameObject gObject)
    {
        icon.sprite = item.icon;
        icon.color = Color.white;

        CureItem = item;
        itemObject = gObject;
    }

    public void DropItem()
    {
        if (CureItem == null)
        {
            return;
        }

        Vector3 dropPosition = new Vector3(GameObject.FindGameObjectWithTag("Player").transform.position.x, GameObject.FindGameObjectWithTag("Player").transform.position.y - 0.5f);

        itemObject.transform.position = dropPosition;
        itemObject.SetActive(true);

        RemoveItem();
    }

    public void RemoveItem()
    {
        icon.color = transparent;
        icon.sprite = null;

        CureItem = null;
        itemObject = null;
    }
}
