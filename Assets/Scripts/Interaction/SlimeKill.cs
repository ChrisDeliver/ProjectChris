using System.Collections;
using UnityEngine;

public class SlimeKill : MonoBehaviour
{
    [SerializeField, Range(1, 100)] private int timeToDieMin = 4;
    [SerializeField, Range(1, 100)] private int timeToDieMax = 8;
    [SerializeField, Range(1, 100)] private int stepsToDie = 8;
    [SerializeField] private Item neededItemInInventory;
    [SerializeField, Range(0.1f, 10f)] private float maxDistance = 2f;

    private bool isStoned = false;
    private InventorySlot[] slots = new InventorySlot[2];

    public void OnClick()
    {
        slots = Inventory.instance.items;

        for (int i = 0; i < slots.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position);
            if (distance > maxDistance)
            {
                return;
            }

            if (slots[i].CureItem == neededItemInInventory && !isStoned)
            {
                isStoned = true;
                Destroy(GetComponent<BadSlime>());

                slots[i].RemoveItem();

                StartCoroutine(Die(Random.Range(timeToDieMin, timeToDieMax) / stepsToDie));
            }
        }
    }

    private IEnumerator Die(float delayTime)
    {
        for (int i = 0; i < stepsToDie; i++)
        { 
            transform.localScale -= new Vector3(1f / stepsToDie, 1f / stepsToDie);
            yield return new WaitForSeconds(delayTime);
        }
        Destroy(gameObject);
    }
}
