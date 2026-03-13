using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPolicecarRule : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnInterval = 20f;
    public int maxEnemies = 5;

    private int spawnedEnemies;
    
    void Start()
    {
        spawnedEnemies = 0;
        InvokeRepeating(nameof(SpawnEnemy), 1f, spawnInterval);   
    }

    void SpawnEnemy()
    {
        if (spawnedEnemies >= maxEnemies)
        {
            CancelInvoke(nameof(SpawnEnemy));
            return;
        }

        GameObject enemy = Instantiate(
            enemyPrefab,
            new Vector3(Random.value * 2f + 1f, 0.36f, Random.value * 2f + 1f),
            Quaternion.identity
        );

        spawnedEnemies++;
    }

}
