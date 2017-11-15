using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class Spawner : MonoBehaviour {

    protected MapController map;

    protected abstract GameObject[] GetPrefabs();

    public GameObject Spawn(MapController map)
    {
        this.map = map;

        GameObject[] prefabs = this.GetPrefabs();
        GameObject selectedPrefab = prefabs[Random.Range(0, prefabs.Length)];
        GameObject instance = Instantiate(selectedPrefab, this.transform.position, Quaternion.identity);

        instance.transform.SetParent(map.gameObject.transform);



        return instance;
    }
}
