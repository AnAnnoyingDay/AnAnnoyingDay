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

        this.SetupMap(startingMap);
    }

    protected void SetupMap(GameObject sourceMap)
    {
        if (this.numberOfMaps.GetFixedRandomValue() == this.levelMaps.Count)
        {
            return;
        }

        List<GameObject> randomExits = this.SelectRandomExits(sourceMap);
        foreach (GameObject exit in randomExits)
        {
            // Recursive call to build the level with exits and stuff...
            this.SetupMap(this.SpawnRandomMapCloseToExit(exit));
        }
    }

    protected List<GameObject> SelectRandomExits(GameObject map)
    {
        List<GameObject> exits = map.transform.FindObjectsWithTag("Exit");

        // Ensure we keep at least 1 exit
        if (exits.Count == 1)
        {
            return exits;
        }

        int exitsToRemove = Random.Range(1, exits.Count);
        for (int i = 0; i < exitsToRemove; i++)
        {
            int removedIndex = Random.Range(0, exits.Count);
            Destroy(exits[removedIndex]);
            exits.RemoveAt(removedIndex);
        }

        return exits;
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

        return this.SpawnRandomMapAt(positionToSpawnNewMap + ((Vector2)exit.transform.position) - positionToSpawnNewMap);
    }

    protected MapController GetMapController(GameObject mapObject)
    {
        return mapObject.GetComponent<MapController>();
    }
}
