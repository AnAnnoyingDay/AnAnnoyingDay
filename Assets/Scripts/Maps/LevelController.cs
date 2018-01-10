﻿using System.Collections;
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
            Destroy(player);
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
        Vector2 mapSize = GetMapController(randomMap).mapSize + new Vector2(10, 8);

        Vector2 spawnPosition = new Vector2(position.x * mapSize.x, position.y * mapSize.y);
        GameObject newMap = Instantiate(randomMap, spawnPosition, Quaternion.identity);
        newMap.name = "Map " + this.levelMaps.Count + " ("+position.x+", " + position.y + ")";
        newMap.transform.SetParent(this.transform);

        this.levelMaps.Add(position, newMap);

        return newMap;
    }

    protected MapController GetMapController(GameObject mapObject)
    {
        return mapObject.GetComponent<MapController>();
    }

    public GameObject GetCurrentMap()
    {
        return GameController.instance.GetPlayer().transform.parent.gameObject;
    }

    public void MovePlayerToMap(Direction direction)
    {
        Vector2 positionCurrentMap = this.levelMaps.FirstOrDefault(x => x.Value.Equals(this.GetCurrentMap())).Key;

        Vector2 newPosition = positionCurrentMap + direction.ToVector();

        Debug.Log("----- POS -----");
        Debug.Log(positionCurrentMap);
        Debug.Log(newPosition);
        Debug.Log("------ END POS -----");

        GameObject newMap = this.levelMaps[newPosition];

        GameController.instance.GetPlayer().transform.parent = newMap.transform;

        GameObject newExit = null;
        Debug.Log("---- EXITS MAP " + newMap.name + "----");
        foreach (var exit in newMap.transform.FindObjectsWithTag("Exit"))
        {
            Debug.Log(exit.GetComponent<HasDirection>().direction);
            if (exit.GetComponent<HasDirection>().direction.Equals(direction.Inverse())) {
                newExit = exit;
            }
        }
        Debug.Log("---- FIN EXITS ----");

        Vector2 teleportLocation = (Vector2) newExit.transform.position + direction.ToVector() * 1.4f;

        GameController.instance.GetPlayer().transform.position = teleportLocation;

    }
}
