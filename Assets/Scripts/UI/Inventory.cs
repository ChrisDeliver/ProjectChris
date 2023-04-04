using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private Transform slotParent;

    public static Inventory instance;

    private InventorySlot[] slots = new InventorySlot[2];

    public InventorySlot[] items => slots;

    private void Start()
    {
        for(int i = 0; i< slots.Length; i++)
        {
            slots[i] = slotParent.GetChild(i).GetComponent<InventorySlot>();
        }

    }

    public void Awake()
    {
        instance = this;
    }

    public bool PutInEmpty(Item item, GameObject gObject)
    {
        if (isEmpty() != -1)
        {
            slots[isEmpty()].PutIn(item, gObject);
            return true;
        }
        return false;
    }

    public sbyte isEmpty()
    {
        for (sbyte i = 0; i < slots.Length; i++)
        {
            if (slots[i].CureItem == null)
            {
                return i;
            }
        }
        return -1;
    }
}
