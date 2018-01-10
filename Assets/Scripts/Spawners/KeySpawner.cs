using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class KeySpawner : Spawner
{
    public GameObject keyPrefab;

    protected override bool ShouldHandle()
    {
        return this.map.isKey;
    }

    protected override GameObject[] GetPrefabs()
    {
        return new GameObject[] { this.keyPrefab };
    }
}
