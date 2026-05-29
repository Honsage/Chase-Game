using Unity.Netcode;
using UnityEngine;

public class NetworkPlayer : NetworkBehaviour
{
    private CarController carController;
    
    private void Awake()
    {
        carController = GetComponent<CarController>();
    }
    
    public override void OnNetworkSpawn()
    {
        Debug.Log($"=== NetworkPlayer OnNetworkSpawn ===");
        Debug.Log($"GameObject: {gameObject.name}");
        Debug.Log($"IsOwner: {IsOwner}");
        Debug.Log($"IsLocalPlayer: {IsLocalPlayer}");
        Debug.Log($"OwnerClientId: {OwnerClientId}");
        
        if (carController != null)
        {
            carController.enabled = IsOwner;
            Debug.Log($"CarController.enabled set to: {carController.enabled}");
        }
        
        if (IsOwner)
        {
            Camera mainCam = Camera.main;
            if (mainCam != null)
            {
                FollowCamera followCamera = mainCam.GetComponent<FollowCamera>();
                if (followCamera != null)
                {
                    followCamera.target = transform;
                    Debug.Log($"Camera target set to: {gameObject.name}");
                }
            }
        }
    }
}