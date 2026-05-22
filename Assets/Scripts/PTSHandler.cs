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
        NetworkManager.Singleton.CustomMessagingManager.RegisterNamedMessageHandler(PTS_TOPIC, OnReceivePTSPacket);
    }
    
    public void SendPTSData(int health, Vector3 position)
    {
        var writer = new FastBufferWriter(1100, Allocator.Temp);
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
        
        Debug.Log($"[PTS Пакет] Получены данные от {senderId}: HP={health}, Pos={pos}");
        
        // Если пакет от другого игрока — обновляем здоровье (для врагов)
        if (senderId != NetworkManager.Singleton.LocalClientId && heroStats != null)
        {
            // Не наносим урон, а просто синхронизируем состояние
            // heroStats.SyncHealth(health); // можно добавить метод, если нужно
        }
    }
}