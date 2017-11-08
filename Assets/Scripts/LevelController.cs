using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelController : MonoBehaviour {

    public GameObject[] instanciableMaps;
    public Count numberOfMaps = new Count(5, 8);

    private Dictionary<Vector2, GameObject> levelMaps = new Dictionary<Vector2, GameObject>();

	void Awake () {
        GameObject startingMap = this.SpawnRandomMapAt(new Vector2(0, 0));

        List<GameObject> exits = startingMap.transform.FindObjectsWithTag("Exit");
        foreach (GameObject exit in exits)
        {
            Debug.Log(exit.transform.position);
            this.SpawnRandomMapAt(exit.transform.position);
        }
    }

    protected GameObject GetRandomMap() {
        int randomMapIndex = Random.Range(0, this.instanciableMaps.Length);

        return this.instanciableMaps[randomMapIndex];
    }

    protected GameObject SpawnRandomMapAt(Vector2 position) {
        GameObject randomMap = this.GetRandomMap();
        this.levelMaps.Add(position, randomMap);

        GameObject newMap = Instantiate(randomMap, -position, Quaternion.identity);

        newMap.name = "Map " + this.levelMaps.Count;

        return newMap;
    }
}
