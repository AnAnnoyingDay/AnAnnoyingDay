using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelController : MonoBehaviour
{
    public GameObject[] prefabMaps;
    public Count numberOfMaps = new Count(5, 8);

    private Dictionary<Vector2, GameObject> levelMaps = new Dictionary<Vector2, GameObject>();

    void Awake()
    {
        GameObject startingMap = this.SpawnRandomMapAt(new Vector2(0, 0));

        // For each exit:
        //      Get the exit (#1) direction
        //      Find a map which has a door at the opposite of exit #1
        //      Instanciate the new map at the opposite of the exit #1 direction
        //      --> the two exits should be glued
        List<GameObject> exits = startingMap.transform.FindObjectsWithTag("Exit");
        foreach (GameObject exit in exits)
        {
            this.SpawnRandomMapCloseToExit(exit);
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

        GameObject newMap = Instantiate(randomMap, position, Quaternion.identity);
        newMap.name = "Map " + this.levelMaps.Count;

        newMap.transform.SetParent(this.transform);

        return newMap;
    }

    protected GameObject SpawnRandomMapCloseToExit(GameObject exit)
    {
        Direction exitDirection = exit.GetComponent<HasDirection>().direction;

        Vector2 positionToSpawnNewMap = new Vector2();
        Vector2 sourceMapSize = this.GetMapController(exit.transform.parent.gameObject).mapSize;

        switch (exitDirection)
        {
            case Direction.TOP:
                positionToSpawnNewMap = new Vector2(0, sourceMapSize.y);
                break;

            case Direction.RIGHT:
                positionToSpawnNewMap = new Vector2(-sourceMapSize.x, 0);
                break;

            case Direction.BOTTOM:
                positionToSpawnNewMap = new Vector2(0, -sourceMapSize.y);
                break;

            case Direction.LEFT:
                positionToSpawnNewMap = new Vector2(sourceMapSize.x, 0);
                break;
        }

        return this.SpawnRandomMapAt(positionToSpawnNewMap);
    }

    protected MapController GetMapController(GameObject mapObject) {
        return mapObject.GetComponent<MapController>();
    }
}
