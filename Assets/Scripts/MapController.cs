using System.Collections;
using System.Collections.Generic;
using Tiled2Unity;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class MapController : MonoBehaviour
{
    public Count itemCount = new Count(1, 4);
    public Count enemiesCount = new Count(5, 10);
    public GameObject[] enemyPrefabs;
    public GameObject[] itemPrefabs;
    public bool isBossMap = false;

    private List<Vector2> availablePositions = new List<Vector2>();
    private Vector2 mapSize;

    private void Awake()
    {
        TiledMap tileScript = GetComponent<TiledMap>();
        this.mapSize = new Vector2(tileScript.NumTilesWide, tileScript.NumTilesHigh);

        this.SetupMap();
    }

    private void FillAvailablePositions()
    {
        this.availablePositions.Clear();

        GameObject player = GameObject.FindWithTag("Player");
        CircleCollider2D playerCollider = player.GetComponent<CircleCollider2D>();
        Collider2D[] colliders = GetComponentsInChildren<Collider2D>();

        for (int x = 0; x < this.mapSize.x; x++) {
            for (int y = 0; y < this.mapSize.y; y++) {
                Vector2 position = new Vector2(x, -y);

                foreach (Collider2D collider in colliders) {
                    if (!MapController.IsPositionInsideCollider(position, playerCollider)
                        && !MapController.IsPositionInsideCollider(position, collider)) {
                        this.availablePositions.Add(position);
                    }
                }
            }
        }

        // Remove circle collider since it is only usefull to prevent enemies from spawning in an area
        playerCollider.enabled = false;
    }

    private static bool IsPositionInsideCollider(Vector2 position, Collider2D collider)
    {
        return collider.OverlapPoint(position);
    }

    private Vector2 GetRandomPosition()
    {
        int randomIndex = Random.Range(0, this.availablePositions.Count);
        Vector2 position = this.availablePositions[randomIndex];

        this.availablePositions.RemoveAt(randomIndex);

        return position;
    }

    private void SpawnPrefabAtRandom(GameObject prefab)
    {
        GameObject newInstance = Instantiate(prefab, this.GetRandomPosition(), Quaternion.identity);
        newInstance.transform.parent = this.gameObject.transform;
    }

    private void SpawnRandomPrefab(GameObject[] prefabs, Count count)
    {
        int prefabToSpawn = count.GetRandomValue();
        for (int i = 0; i < prefabToSpawn; i++) {
            int randomIndex = Random.Range(0, prefabs.Length);
            GameObject prefab = prefabs[randomIndex];
            this.SpawnPrefabAtRandom(prefab);
        }
    }

    public void SetupMap()
    {
        this.FillAvailablePositions();

        this.SpawnRandomPrefab(this.enemyPrefabs, this.enemiesCount);
        this.SpawnRandomPrefab(this.itemPrefabs, this.itemCount);
    }
}
