using Unity.Netcode;
using UnityEngine;
using Unity.Collections;

public class PTSHandler : NetworkBehaviour
{
    private const string PTS_TOPIC = "EnemyStatusUpdate";
    private HeroStats heroStats;

    private void Awake()
    {
        heroStats = GetComponent<HeroStats>();
    }

    public override void OnNetworkSpawn()
    {
        if (NetworkManager.Singleton != null)
            NetworkManager.Singleton.CustomMessagingManager.RegisterNamedMessageHandler(PTS_TOPIC, OnReceivePTSPacket);
    }

    public void SendPTSData(int health, Vector3 position)
    {
        if (!IsOwner) return;

        var writer = new FastBufferWriter(256, Allocator.Temp);
        using (writer)
        {
            writer.WriteValueSafe(health);
            writer.WriteValueSafe(position);
            NetworkManager.Singleton.CustomMessagingManager.SendNamedMessageToAll(PTS_TOPIC, writer);
        }
    }

    private void OnReceivePTSPacket(ulong senderId, FastBufferReader reader)
    {
        reader.ReadValueSafe(out int health);
        reader.ReadValueSafe(out Vector3 pos);
        Debug.Log($"[PTS] Получено от {senderId}: HP={health}, Pos={pos}");
    }
}