using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TrollBridge;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelController : MonoBehaviour
{
    public GameObject[] prefabMaps;
    public Count numberOfMaps = new Count(5, 8);

    public Dictionary<Vector2, GameObject> levelMaps = new Dictionary<Vector2, GameObject>();
    private GridMapGenerator mapGenerator;

    void Awake()
    {
        this.mapGenerator = new GridMapGenerator(this.numberOfMaps.GetFixedRandomValue());

        foreach(BoxController box in this.mapGenerator.finalGrid) {
            var map = this.SpawnRandomMap(box);
        }
    }

    protected void DisableUnusedExits(BoxController box, GameObject map)
    {
        List<Direction> boxExits = new List<Direction>();
        foreach (Vector2Int vecExit in box.Exits)
        {
            if (vecExit.Equals(new Vector2Int(-1, -1)))
            {
                continue;
            }

            boxExits.Add((vecExit - (new Vector2(box.X, box.Y) )).ToDirection());
        }

        foreach (GameObject exit in map.transform.FindObjectsWithTag("Exit"))
        {
            Direction direction = exit.GetComponent<HasDirection>().direction;

            if (!boxExits.Contains(direction))
            {
                exit.SetActive(false);
            }
        }
    }

    protected void DestroyUnusedPlayerSpawners(GameObject map) {
        List<GameObject> spawners = map.transform.FindObjectsWithTag("PlayerSpawn");

        foreach (GameObject spawner in spawners) {
            Destroy(spawner);
        }
    }

    protected GameObject GetRandomMap()
    {
        int randomMapIndex = Random.Range(0, this.prefabMaps.Length);

        return this.prefabMaps[randomMapIndex];
    }

    protected GameObject SpawnRandomMap(BoxController box)
    {
        Vector2 position = new Vector2(box.X, box.Y);

        GameObject randomMap = this.GetRandomMap();
        Vector2 mapSize = GetMapController(randomMap).mapSize + new Vector2(10, 8);

        Vector2 spawnPosition = new Vector2(position.x * mapSize.x, position.y * mapSize.y);
        GameObject newMap = Instantiate(randomMap, spawnPosition, Quaternion.identity, this.transform);
        newMap.name = "Map " + this.levelMaps.Count + " (" + position.x + ", " + position.y + ")";

        MapController mapController = this.GetMapController(newMap);

        if (box.IsBoss) {
            newMap.name += " Boss";
            mapController.isBoss = true;
            newMap.tag = "MapBoss";
        } else if (box.IsKey) {
            newMap.name += " Key";
            mapController.isKey = true;
            newMap.tag = "MapKey";
        } else if (position.Equals(this.mapGenerator.GetStartCoordinates())) {
            newMap.name += " Start";
            newMap.tag = "MapStart";

            GameController.instance.SetupCameraBoundaries(newMap);
        }

        this.DisableUnusedExits(box, newMap);
 
        if (!position.Equals(this.mapGenerator.GetStartCoordinates())) {
            this.DestroyUnusedPlayerSpawners(newMap);
        } else {
            var player = GameObject.FindWithTag("Player");
            player.transform.position = newMap.transform
                .FindObjectsWithTag("PlayerSpawn")
                .First()
                .transform
                .position;
            
            player.GetComponent<PlayerController>().SetCurrentMap(newMap);
        }

        mapController.SetupMap();

        this.levelMaps.Add(position, newMap);

        return newMap;
    }

    protected MapController GetMapController(GameObject mapObject)
    {
        return mapObject.GetComponent<MapController>();
    }
}
