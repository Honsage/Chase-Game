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
        // Включаем управление ТОЛЬКО у своей машины
        if (carController != null)
        {
            carController.enabled = IsOwner;
        }
        
        // Камера только у своей машины
        Camera cam = GetComponentInChildren<Camera>();
        if (cam != null)
        {
            cam.enabled = IsOwner;
        }
        
        // AudioListener только у своей машины
        AudioListener listener = GetComponentInChildren<AudioListener>();
        if (listener != null)
        {
            listener.enabled = IsOwner;
        }
        
        // Отключаем UI на чужих машинах
        Canvas canvas = GetComponentInChildren<Canvas>();
        if (canvas != null)
        {
            canvas.enabled = IsOwner;
        }
        
        Debug.Log($"NetworkPlayer: IsOwner={IsOwner}, ClientId={NetworkManager.Singleton.LocalClientId}");
    }
}