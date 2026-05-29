using UnityEngine;
using UnityEngine.AI;
using Unity.Netcode;

public class SpawnCarRule : NetworkBehaviour
{
    [Header("Настройки спавна")]
    public GameObject enemyPrefab;
    public float spawnInterval = 10f;
    public int maxEnemies = 3;
    public float spawnRadius = 25f;
    public float minSpawnDistance = 10f;

    private int spawnedEnemies = 0;

    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;

        InvokeRepeating(nameof(TrySpawnEnemy), 2f, spawnInterval);
    }

    void TrySpawnEnemy()
    {
        if (spawnedEnemies >= maxEnemies) return;

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length == 0) return;

        Transform randomPlayer = players[Random.Range(0, players.Length)].transform;

        for (int i = 0; i < 15; i++)
        {
            Vector3 randomDir = Random.insideUnitSphere * spawnRadius + randomPlayer.position;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDir, out hit, spawnRadius, NavMesh.AllAreas))
            {
                if (Vector3.Distance(hit.position, randomPlayer.position) > minSpawnDistance)
                {
                    GameObject enemy = Instantiate(enemyPrefab, hit.position, Quaternion.identity);
                    NetworkObject netObj = enemy.GetComponent<NetworkObject>();

                    if (netObj != null)
                    {
                        netObj.Spawn(true); // Спавним на сервере
                        spawnedEnemies++;
                        Debug.Log($"[Spawn] Полицейская машина #{spawnedEnemies} заспавнена");
                    }
                    else
                    {
                        Debug.LogError("На префабе PoliceCar нет NetworkObject!");
                    }
                    return;
                }
            }
        }
    }

    public override void OnDestroy()
    {
        CancelInvoke(nameof(TrySpawnEnemy));
    }
}