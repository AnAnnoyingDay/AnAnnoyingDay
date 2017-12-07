using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : Spawner {

    protected override GameObject[] GetPrefabs() {
        return this.map.enemyPrefabs;
    }
}
