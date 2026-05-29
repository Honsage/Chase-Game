using Unity.Netcode;
using UnityEngine;

public class NetworkPlayer : NetworkBehaviour
{
    private CarController carController;
    private HeroStats heroStats;

    public override void OnNetworkSpawn()
    {
        Debug.Log($"[NetworkPlayer] SPAWN | LocalClientId={NetworkManager.Singleton.LocalClientId} | IsOwner={IsOwner} | IsServer={IsServer} | Object={gameObject.name}");

        carController = GetComponent<CarController>();
        heroStats = GetComponent<HeroStats>();

        if (carController != null)
        {
            bool shouldEnable = IsOwner;
            carController.enabled = shouldEnable;
            
            Debug.Log($"[NetworkPlayer] CarController.enabled = {shouldEnable} на {gameObject.name} (IsOwner = {IsOwner})");
        }

        if (IsOwner)
        {
            Debug.Log($"[NetworkPlayer] Это МОЯ машина! Настраиваю камеру и UI.");

            var follow = Camera.main?.GetComponent<FollowCamera>();
            if (follow != null)
                follow.target = transform;

            var ui = FindObjectOfType<UIHealthSubscriber>();
            if (ui != null && heroStats != null)
                ui.SetTarget(heroStats);
        }
        else
        {
            Debug.Log($"[NetworkPlayer] Это ЧУЖАЯ машина. Управление отключено.");
        }
    }

    public override void OnNetworkDespawn()
    {
        if (carController != null)
            carController.enabled = false;
    }
}