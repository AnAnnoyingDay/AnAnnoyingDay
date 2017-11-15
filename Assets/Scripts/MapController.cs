using System.Collections.Generic;
using Tiled2Unity;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapController : MonoBehaviour
{
    [HideInInspector]
    public Vector2 mapSize;
    public Count itemCount = new Count(1, 4);
    public Count enemiesCount = new Count(50, 90);
    public GameObject[] enemyPrefabs;
    public GameObject[] itemPrefabs;
    public bool isBossMap = false;

    private List<Vector2> availablePositions = new List<Vector2>();

    private void Awake()
    {
        TiledMap tileScript = GetComponent<TiledMap>();
        this.mapSize = new Vector2(tileScript.NumTilesWide, tileScript.NumTilesHigh);

        this.SetupMap();
    }

    private void FillAvailablePositions()
    {
        this.availablePositions.Clear();
    }

    public void SetupMap()
    {
        Spawner[] enemySpawners = GetComponentsInChildren<EnemySpawner>();
        Spawner[] itemSpawners = GetComponentsInChildren<ItemSpawner>();

        this.CallSpawner(enemySpawners, this.enemiesCount);
        this.CallSpawner(itemSpawners, this.itemCount);
    }

    protected void CallSpawner(Spawner[] spawners, Count nbToSpawn) {
        if (spawners.Length == 0) return;

        for (int i = 0; i < nbToSpawn.GetRandomValue(); i++)
        {
            int randomIndex = Random.Range(0, spawners.Length);
            Spawner spawner = spawners[randomIndex];

            spawner.Spawn(this);
        }
    }

}
