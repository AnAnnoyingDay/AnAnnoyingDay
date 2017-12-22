using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelController : MonoBehaviour
{
    public GameObject[] prefabMaps;
    public Count numberOfMaps = new Count(5, 8);

    private Dictionary<Vector2, GameObject> levelMaps = new Dictionary<Vector2, GameObject>();
    private GridMapGenerator mapGenerator;

    void Awake()
    {
        this.mapGenerator = new GridMapGenerator(this.numberOfMaps.GetFixedRandomValue());

        foreach(BoxController box in this.mapGenerator.finalGrid) {
            Vector2 boxPos = new Vector2(box.X, box.Y);
            GameObject map = this.SpawnRandomMapAt(boxPos);
            if (box.IsBoss) {
                map.name += " Boss";
            } else if (box.IsKey){
                map.name += " Key";
            } else if (boxPos.Equals(this.mapGenerator.GetStartCoordinates())) {
                map.name += " Start";
            }

            this.RemoveUnusedExits(box, map);

            if (!boxPos.Equals(this.mapGenerator.GetStartCoordinates())) {
                this.DisableUnusedPlayers(map);
            }
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
            player.SetActive(false);
        }
    }

    protected GameObject GetRandomMap()
    {
        int randomMapIndex = Random.Range(0, this.prefabMaps.Length);

        return this.prefabMaps[randomMapIndex];
    }

    protected GameObject SpawnRandomMapAt(Vector2 position)
    {
        GameObject randomMap = this.GetRandomMap();
        this.levelMaps.Add(position, randomMap);
        Vector2 mapSize = GetMapController(randomMap).mapSize + new Vector2(10, 8);

        Vector2 spawnPosition = new Vector2(position.x * mapSize.x, position.y * mapSize.y);
        GameObject newMap = Instantiate(randomMap, spawnPosition, Quaternion.identity);
        newMap.name = "Map " + this.levelMaps.Count;
        newMap.transform.SetParent(this.transform);

        return newMap;
    }

    protected MapController GetMapController(GameObject mapObject)
    {
        return mapObject.GetComponent<MapController>();
    }
}
