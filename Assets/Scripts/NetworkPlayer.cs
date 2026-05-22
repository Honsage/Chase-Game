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
        if (carController != null)
        {
            carController.enabled = IsOwner;
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
                }
            }
        }
        
        Debug.Log($"NetworkPlayer: IsOwner={IsOwner}");
    }
}