using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldGenerator : MonoBehaviour
{
    #region SerFields
    [Header("Generation settings")]
    [SerializeField, Range(-32768, 32767)] private int seed = 0;
    [SerializeField, Range(1f, 100f)] private float smoothness = 18f;
    [SerializeField, Range(0.21f, 0.5f)] private float sandMax = 0.26f;
    [SerializeField, Range(0f, 0.2f)] private float waterMax = 0.185f;
    [SerializeField, Range(0, 30000)] private int theEndOfTheWorld = 15000;

    [Header("Render Info")]
    [SerializeField] private byte cameraView = 10;
    [SerializeField] private byte renderDistance = 8;

    [Header("Tiles settings")]
    [SerializeField] private List<Tilemap> tilemaps;
    [SerializeField] private List<RuleTile> allTiles;

    [Header("Overs settings")]
    [SerializeField] private int oversSeed = 0;
    [SerializeField, Range(1f, 100f)] private float oversSmoothness = 1.1f;
    [SerializeField, Range(0.5f, 1f)] private float stoneMin = 0.85f;
    [SerializeField] private List<GameObject> allOvermains;

    [Header("Slimes")]
    [SerializeField] private GameObject goodSlime;
    [SerializeField] private GameObject badSlime;

    [Header("Safe Appearance")]
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject pickaxe;
    #endregion

    private Vector2 checkDistance;
    private Vector2 cameraPosition;
    private Vector3Int tilePos;

    private void Start()
    {
        if (seed == 0)
        {
            seed = Random.Range(-32768, 32767);
        }
        oversSeed = Random.Range(-1000000, 1000000);
        checkDistance = new Vector2(cameraView + renderDistance, cameraView + renderDistance);
        GenerateTiles((cameraView * 2 + renderDistance) / 2, (cameraView + renderDistance) / 2,
                      0 - (cameraView * 2 + renderDistance) / 2, 0 - (cameraView + renderDistance) / 2);

        Vector3Int playerPosition = new();

        for (int x = -15; x < 15; x++)
        {
            for (int y = -15; y < 15; y++)
            {
                playerPosition = new(x, y);

                if (tilemaps[1].GetSprite(playerPosition) != null && tilemaps[1].GetSprite(playerPosition).name.Contains("grass"))
                {
                    break;
                }
            }
        }

        Instantiate(pickaxe, playerPosition, Quaternion.identity);
        player.transform.position = playerPosition;

        CreateEndline();
    }

    private void FixedUpdate()
    {
        cameraPosition = new Vector2Int(Mathf.RoundToInt(Camera.main.transform.position.x), Mathf.RoundToInt(Camera.main.transform.position.y));

        if (Physics2D.OverlapBox(cameraPosition, checkDistance, 0))
        {
            GenerateTiles(Mathf.CeilToInt(cameraPosition.x) + cameraView + renderDistance,
                Mathf.CeilToInt(cameraPosition.y) + cameraView / 2 + renderDistance,
                Mathf.CeilToInt(cameraPosition.x) - cameraView - renderDistance,
                Mathf.CeilToInt(cameraPosition.y) - cameraView / 2 - renderDistance);
        }
    }

    private void GenerateTiles(int maxX, int maxY, int minX = 0, int minY = 0)
    {
        for (int x = minX; x < maxX; x++)
        {
            for (int y = minY; y < maxY; y++)
            {
                tilePos = new(x, y, 0);

                if (tilemaps[1].GetTile(tilePos) != null)
                {
                    continue;
                }

                float height = GetHeight(x, y, seed, smoothness);
                float oversHeight = GetHeight(x, y, oversSeed, oversSmoothness);

                if (height > waterMax && height <= sandMax)
                {
                    tilemaps[1].SetTile(tilePos, allTiles[1]);
                }

                else if (height > sandMax && height < stoneMin)
                {
                    tilemaps[1].SetTile(tilePos, allTiles[0]);
                    if (oversHeight > stoneMin && height > sandMax + 0.1f)
                    {
                        int r = Random.Range(-3, 2);
                        Vector2 position = new(x + 0.5f, y + 0.5f);

                        switch (r)
                        {
                            case 0:
                                Instantiate(badSlime, position, Quaternion.identity);
                                break;
                            case 1:
                                Instantiate(goodSlime, position, Quaternion.identity);
                                break;
                            default:
                                Instantiate(allOvermains[0], position, Quaternion.identity);
                                break;
                        }
                    }
                }

                else
                {
                    for (int xOwn = -2; xOwn < 3; xOwn++)
                    {
                        for (int yOwn = -2; yOwn < 3; yOwn++)
                        {
                            if ((tilemaps[1].GetSprite(tilePos + new Vector3Int(xOwn, yOwn)) != null && tilemaps[1].GetSprite(tilePos + new Vector3Int(xOwn, yOwn)).name.Contains("grass")) &&
                                (tilemaps[1].GetSprite(tilePos + new Vector3Int(-xOwn, -yOwn)) != null && tilemaps[1].GetSprite(tilePos + new Vector3Int(-xOwn, -yOwn)).name.Contains("grass")))
                            {
                                tilemaps[1].SetTile(tilePos, allTiles[0]);
                                tilemaps[0].SetTile(tilePos, null);
                            }
                        }
                    }
                }
            }
        }

        for (int x = minX+2; x < maxX-2; x++)
        {
            for (int y = minY+2; y < maxY-2; y++)
            {
                tilePos = new(x, y, 0);

                if (tilemaps[1].GetTile(tilePos))
                {
                    continue;
                }

                for (int xOwn = -1; xOwn < 2; xOwn++)
                {
                    for (int yOwn = -1; yOwn < 2; yOwn++)
                    {
                        if (tilemaps[0].GetTile(tilePos + new Vector3Int(xOwn, yOwn)) == null)
                        {
                            tilemaps[0].SetTile(tilePos + new Vector3Int(xOwn, yOwn), allTiles[2]);
                        }
                    }
                }
            }
        }
    }

    private float GetHeight(int x, int y, int seed, float smoothness)
    {
        float xPerlin = (x + seed) / smoothness;
        float yPerlin = (y + seed) / smoothness;
        return Mathf.PerlinNoise(xPerlin, yPerlin);
    }

    private void CreateEndline()
    {
        int theHalf = theEndOfTheWorld / 2;
        Vector3Int pos; 

        for (int i = theHalf * -1; i < theHalf; i++)
        {
            pos = new Vector3Int(i, theHalf);
            tilemaps[0].SetTile(pos, allTiles[2]);
            pos = new Vector3Int(i, -theHalf);
            tilemaps[0].SetTile(pos, allTiles[2]);
            pos = new Vector3Int(theHalf, i);
            tilemaps[0].SetTile(pos, allTiles[2]);
            pos = new Vector3Int(-theHalf, i);
            tilemaps[0].SetTile(pos, allTiles[2]);
        }
    }
}