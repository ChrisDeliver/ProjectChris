using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BadSlime : MonoBehaviour
{
    public float moveSpeed = 3;
    public int maxDistance = 10;
    public byte lifeTime = 60;
    public float hitDistance = 1.4f;

    private GameObject player;
    private List<Vector2> pathToPlayer = new();
    private List<Vector2> randomPath = new();
    private List<Vector2> currentPath = new();
    private PathFinder PathFinder;
    private Transform playerTransform;
    private HPSystem hp;
    private bool isMoving;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        PathFinder = GetComponent<PathFinder>();
        hp = player.GetComponent<HPSystem>();
        playerTransform = player.transform;

        ReCalculatePath();
        isMoving = true;
        StartCoroutine(Die(lifeTime));
    }

    private void ReCalculatePath()
    {
        if (Vector2.Distance(transform.position, playerTransform.position) > maxDistance) return;

        pathToPlayer = PathFinder.GetPath(playerTransform.position);
        
        if (pathToPlayer.Count == 0)
        {
            Vector2 position;
            if(PathFinder.freeNodes.Count > 0)
            {
                var r = Random.Range(0, PathFinder.freeNodes.Count);
                position = PathFinder.freeNodes[r].Position;
            }
            else
            {
                position = playerTransform.position;
            }
            randomPath = PathFinder.GetPath(position);
            currentPath = randomPath;
        }
        else
        {
            currentPath = pathToPlayer;
        }
    }

    private void GiveDamage(sbyte source)
    {
        hp.TakeDamage(source);
        Destroy(gameObject);
    }

    private void Update()
    {
        if (currentPath.Count <= 1)
        {
            if (Vector2.Distance(transform.position, playerTransform.position) <= hitDistance)
            {
                var r = Random.Range(-10, 11);
                GiveDamage((sbyte)(30 + r));
                return;
            }
            else
            {
                ReCalculatePath();
                isMoving = true;
            }
        }
        if (isMoving && currentPath.Count > 1)
        {
            if(Vector2.Distance(transform.position, currentPath[^1]) > 0.1f)
            {
                transform.position = Vector2.MoveTowards(transform.position, currentPath[^1], moveSpeed * Time.deltaTime);
            }
            if(Vector2.Distance(transform.position, currentPath[^1]) <= 0.1f)
            {
                isMoving = false;
            }
        }
        else
        {
            ReCalculatePath();
            isMoving = true;
        }
    }

    private IEnumerator Die(byte delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        Destroy(gameObject);
    }
}