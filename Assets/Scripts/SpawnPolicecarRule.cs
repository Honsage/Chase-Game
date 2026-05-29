using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.Netcode;

public class SpawnCarRule : MonoBehaviour
{
    public GameObject enemyPrefab;      
    public float spawnInterval = 10f;
    public int maxEnemies = 3;
    public float spawnRadius = 25f;
    public float minSpawnDistance = 10f;
    
    private int spawnedEnemies = 0;
    
    void Start()
    {
        // Только хост спавнит полицию
        if (NetworkManager.Singleton.IsHost)
        {
            InvokeRepeating(nameof(TrySpawnEnemy), 2f, spawnInterval);
        }
    }
    
    void TrySpawnEnemy()
    {
        if (spawnedEnemies >= maxEnemies) return;
        
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length == 0) return;
        
        Transform randomPlayer = players[Random.Range(0, players.Length)].transform;
        
        for (int i = 0; i < 15; i++)
        {
            Vector3 randomDir = Random.insideUnitSphere * spawnRadius;
            randomDir += randomPlayer.position;
            
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDir, out hit, spawnRadius, NavMesh.AllAreas))
            {
                if (Vector3.Distance(hit.position, randomPlayer.position) > minSpawnDistance)
                {
                    GameObject enemy = Instantiate(enemyPrefab, hit.position, Quaternion.identity);
                    NetworkObject netObj = enemy.GetComponent<NetworkObject>();
                    
                    if (netObj != null)
                    {
                        netObj.Spawn();
                        spawnedEnemies++;
                        Debug.Log($"Полицейская машина заспавнена на хосте и синхронизирована");
                    }
                    else
                    {
                        Debug.LogError("На префабе полицейской машины нет компонента NetworkObject!");
                    }
                    return;
                }
            }
        }
    }
}