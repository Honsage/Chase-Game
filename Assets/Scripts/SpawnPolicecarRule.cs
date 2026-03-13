using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnCarRule : MonoBehaviour
{
    public GameObject enemyPrefab;      
    public Transform playerCar;
    public float spawnInterval = 10f;
    public int maxEnemies = 3;
    public float spawnRadius = 30f;
    public float minSpawnDistance = 20f;
    
    private int spawnedEnemies = 0;
    
    void Start()
    {
        InvokeRepeating(nameof(TrySpawnEnemy), 2f, spawnInterval);
    }
    
    void TrySpawnEnemy()
    {
        if (spawnedEnemies >= maxEnemies || playerCar == null)
            return;
            
        for (int i = 0; i < 10; i++)
        {
            Vector3 randomDir = Random.insideUnitSphere * spawnRadius;
            randomDir += playerCar.position;
            
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDir, out hit, spawnRadius, NavMesh.AllAreas))
            {
                if (Vector3.Distance(hit.position, playerCar.position) > minSpawnDistance)
                {
                    GameObject enemy = Instantiate(enemyPrefab, hit.position, Quaternion.identity);
                    spawnedEnemies++;
                    
                    PoliceCar enemyScript = enemy.GetComponent<PoliceCar>();
                    if (enemyScript != null)
                        enemyScript.player = playerCar;
                    
                    return;
                }
            }
        }
        
        Debug.Log("Не удалось найти место для спавна полиции");
    }
    
    public void OnEnemyDestroyed()
    {
        spawnedEnemies--;
    }
}