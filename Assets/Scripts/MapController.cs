using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class MapController : MonoBehaviour
{
    public Count itemCount = new Count(1, 4);
    public GameObject[] enemyPrefabs;
    public GameObject[] itemPrefabs;

    private List<Vector2> availablePositions = new List<Vector2>();

    private void FillAvailablePositions()
    {
        this.availablePositions.Clear();

        Vector2 mapSize = new Vector2();

        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                this.availablePositions.Add(new Vector2(x, y));
            }
        }

        TilemapCollider2D[] tileMapColiders = this.GetComponentsInChildren<TilemapCollider2D>();
        foreach (TilemapCollider2D colider in tileMapColiders)
        {
            Tilemap tileMap = colider.gameObject.GetComponent<Tilemap>();
            foreach (Vector3 position in tileMap.cellBounds.allPositionsWithin)
            {
                this.availablePositions.Remove(position);
            }
        }
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
        Instantiate(prefab, this.GetRandomPosition(), Quaternion.identity);
    }

    private void SpawnRandomPrefab(GameObject[] prefabs, Count count)
    {
        int prefabToSpawn = count.GetRandomValue();
        for (int i = 0; i < prefabToSpawn; i++)
        {
            int randomIndex = Random.Range(0, prefabs.Length);
            GameObject prefab = prefabs[randomIndex];
            this.SpawnPrefabAtRandom(prefab);
        }
    }

    public void SetupMap()
    {
        this.FillAvailablePositions();

        this.SpawnRandomPrefab(this.enemyPrefabs, new Count(5, 10));
    }
}
