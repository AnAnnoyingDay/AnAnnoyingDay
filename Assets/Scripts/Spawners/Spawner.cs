using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class Spawner : MonoBehaviour
{
    protected MapController map;

    protected abstract GameObject[] GetPrefabs();

    protected virtual bool ShouldHandle()
    {
        return true;
    }

    public GameObject Spawn(MapController map)
    {
        this.map = map;

        if (!this.ShouldHandle())
            return null;

        GameObject[] prefabs = this.GetPrefabs();

        if (prefabs.Length == 0)
            return null;

        GameObject selectedPrefab = prefabs[Random.Range(0, prefabs.Length)];
        GameObject instance = Instantiate(selectedPrefab, this.transform.position, Quaternion.identity, map.gameObject.transform);

        Destroy(this);

        return instance;
    }
}
