using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GoodSlime : MonoBehaviour
{
    public float moveSpeed = 3;
    public byte lifeTime = 25;
    public byte dropDelayMin = 10;
    public byte dropDelayMax = 17;
    public GameObject heart;

    private Vector3 vector;

    private void Awake()
    {
        vector = new(Random.Range(-1, 2), Random.Range(-1, 2));

        StartCoroutine(SpawnHeart((byte)Random.Range(dropDelayMin, dropDelayMax)));
        StartCoroutine(Die(lifeTime));
    }

    private void Update()
    {
        transform.position += vector * moveSpeed * Time.deltaTime;
    }

    private IEnumerator Die(byte delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        Destroy(gameObject);
    }

    private IEnumerator SpawnHeart(byte delayTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(delayTime);
            Instantiate(heart, transform.position, Quaternion.identity);
        }
    }
}