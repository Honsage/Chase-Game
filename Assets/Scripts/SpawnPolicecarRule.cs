using UnityEngine;
using UnityEngine.AI;
using Unity.Netcode;
using System.Collections.Generic;

public class SpawnCarRule : NetworkBehaviour
{
    [Header("Префаб полиции")]
    public GameObject enemyPrefab;

    [Header("Настройки спавна")]
    public float spawnInterval = 8f;
    public int maxEnemies = 4;
    public float spawnRadius = 30f;
    public float minSpawnDistanceFromPlayer = 12f;
    public float minDistanceFromOtherEnemies = 8f;

    private int spawnedEnemies = 0;
    private List<GameObject> activePolice = new List<GameObject>();

    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;
        
        InvokeRepeating(nameof(TrySpawnEnemy), 3f, spawnInterval);
        Debug.Log("[SpawnCarRule] Сервер начал спавнить полицию");
    }

    void TrySpawnEnemy()
    {
        if (spawnedEnemies >= maxEnemies) return;

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length == 0) return;

        Transform targetPlayer = players[Random.Range(0, players.Length)].transform;

        for (int attempt = 0; attempt < 25; attempt++)
        {
            Vector3 randomPoint = targetPlayer.position + Random.insideUnitSphere * spawnRadius;
            randomPoint.y = 0;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, spawnRadius, NavMesh.AllAreas))
            {
                Vector3 spawnPos = hit.position;

                if (Vector3.Distance(spawnPos, targetPlayer.position) < minSpawnDistanceFromPlayer)
                    continue;

                bool tooClose = false;
                foreach (var police in activePolice)
                {
                    if (police != null && Vector3.Distance(spawnPos, police.transform.position) < minDistanceFromOtherEnemies)
                    {
                        tooClose = true;
                        break;
                    }
                }
                if (tooClose) continue;

                if (Physics.Raycast(spawnPos + Vector3.up * 1f, Vector3.up, 5f))
                {
                    continue;
                }

                GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
                NetworkObject netObj = enemy.GetComponent<NetworkObject>();

                if (netObj != null)
                {
                    netObj.Spawn(true);
                    spawnedEnemies++;
                    activePolice.Add(enemy);

                    Debug.Log($"[Spawn] Полиция #{spawnedEnemies} заспавнена успешно на NavMesh");
                    return;
                }
            }
        }

        Debug.LogWarning("[Spawn] Не удалось найти хорошую точку для спавна полиции");
    }

    public override void OnDestroy()
    {
        CancelInvoke(nameof(TrySpawnEnemy));
    }
}