using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    [SerializeField] private GameObject dropItem;
    [SerializeField, Range(1, 100)] private int countMin = 1;
    [SerializeField, Range(1, 100)] private int countMax = 3;
    [SerializeField] private Item neededItemInInventory;
    [SerializeField, Range(10f, 100f)] private float ownHealth = 12;
    [SerializeField, Range(0.1f, 30f)] private float impactForce = 4;
    [SerializeField, Range(0.1f, 10f)] private float maxDistance = 2f;

    private InventorySlot[] slots = new InventorySlot[2];

    public void OnClick()
    {
        slots = Inventory.instance.items;

        for (int i = 0; i < slots.Length ; i++)
        {
            float distance = Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position);
            if (distance > maxDistance)
            {
                return;
            }

            if (slots[i].CureItem == neededItemInInventory)
            {
                float impact = impactForce + Random.Range(-1, 2);

                ownHealth -= impact;
            }

        }

        if (ownHealth <= 0)
        {
            int count = Random.Range(countMin, countMax);
            Vector3 position = transform.position;

            for (int i = 0; i < count; i++)
            {
                float x = Random.Range(-1f, 1f) + position.x;
                float y = Random.Range(-1f, 1f) + position.y;

                Instantiate(dropItem, new Vector3(x + i / 2, y + i / 2, position.z), Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }
}
