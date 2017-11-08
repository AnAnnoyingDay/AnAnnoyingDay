using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelController : MonoBehaviour {

    public GameObject[] instanciableMaps;
    public Count numberOfMaps = new Count(5, 8);

    private Dictionary<Vector2, GameObject> levelMaps = new Dictionary<Vector2, GameObject>();

	void Start () {
        GameObject startingMap = this.GetRandomMap();

        this.levelMaps.Add(new Vector2(0, 0), this.GetRandomMap());

        List<GameObject> exits = startingMap.transform.FindObjectsWithTag("Exit");
        foreach (GameObject exit in exits)
        {
            this.SpawnRandomMapAt(exit.transform.position);
        }
    }

    protected GameObject GetRandomMap() {
        int randomMapIndex = Random.Range(0, this.instanciableMaps.Length);

        return this.instanciableMaps[randomMapIndex];
    }

    protected void SpawnRandomMapAt(Vector2 position) {
        GameObject randomMap = this.GetRandomMap();
        this.levelMaps.Add(position, randomMap);

        Instantiate(randomMap, position, Quaternion.identity);
    }
}
