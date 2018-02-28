using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    protected void RemoveUnusedExits(BoxController box, GameObject map)
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
                Destroy(exit);
            }
        }
    }

    protected void DisableUnusedPlayers(GameObject map) {
        List<GameObject> players = map.transform.FindObjectsWithTag("Player");

        foreach (GameObject player in players) {
            Destroy(player);
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
        } else if (box.IsKey) {
            newMap.name += " Key";
            mapController.isKey = true;
        } else if (position.Equals(this.mapGenerator.GetStartCoordinates())) {
            newMap.name += " Start";
        }

        this.RemoveUnusedExits(box, newMap); 
 
        if (!position.Equals(this.mapGenerator.GetStartCoordinates())) {
            this.DisableUnusedPlayers(newMap); 
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
