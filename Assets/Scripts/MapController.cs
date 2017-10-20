using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapController : MonoBehaviour
{

    public Count itemCount = new Count(1, 4);
    public GameObject[] enemyPrefabs;
    public GameObject[] itemPrefabs;

    private List<Vector2> availablePositions = new List<Vector2>();

    private void FillAvailablePositions()
    {
        this.availablePositions.Clear();

        Vector2 mapSize = GameController.instance.mapSize;

        TilemapCollider2D[] tileMapColliders = this.GetTilemapColliders();

        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                this.availablePositions.Add(new Vector2(x, y));
            }
        }
    }

    private TilemapCollider2D[] GetTilemapColliders()
    {
        return GetComponentsInChildren<TilemapCollider2D>();
    }

    public void SetupMap()
    {

    }
}
