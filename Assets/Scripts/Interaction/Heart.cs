using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    [SerializeField, Range(0, 100)] private byte minHealingValue = 19;
    [SerializeField, Range(0, 100)] private byte maxHealingValue = 23;
    [SerializeField, Range(0, 100)] private byte lifeTime = 60;

    private void Awake()
    {
        StartCoroutine(Die(lifeTime));
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        HPSystem CollisionHP = collision.GetComponent<HPSystem>();

        if (CollisionHP)
        {
            var r = (sbyte)Random.Range(minHealingValue, maxHealingValue);
            CollisionHP.Heal(r);

            Destroy(gameObject);
        }
    }

    private IEnumerator Die(byte delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        Destroy(gameObject);
    }
}
