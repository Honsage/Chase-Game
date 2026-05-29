using Unity.Netcode;
using UnityEngine;

public class PlayerSpawner : NetworkBehaviour
{
    [Header("Префабы машин")]
    public GameObject Hero1Prefab;
    public GameObject Hero2Prefab;

    [Header("Точки спавна")]
    public Transform SpawnPoint1;
    public Transform SpawnPoint2;

    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;

        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        SpawnPlayer(0);
    }

    private void OnClientConnected(ulong clientId)
    {
        SpawnPlayer(clientId);
    }

    private void SpawnPlayer(ulong clientId)
    {
        GameObject prefab = (clientId == 0) ? Hero1Prefab : Hero2Prefab;
        Transform spawnPoint = (clientId == 0) ? SpawnPoint1 : SpawnPoint2;

        GameObject player = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
        NetworkObject netObj = player.GetComponent<NetworkObject>();

        if (netObj != null)
        {
            netObj.SpawnAsPlayerObject(clientId, true);
            Debug.Log($"[Spawner] Игрок {clientId} заспавнен как {(clientId == 0 ? "Hero1" : "Hero2")}");
        }
        else
        {
            Debug.LogError("NetworkObject не найден на префабе!");
        }
    }

    public override void OnDestroy()
    {
        if (NetworkManager.Singleton != null)
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
    }
}