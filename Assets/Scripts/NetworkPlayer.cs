using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkPlayer : NetworkBehaviour
{
    private CarController carController;
    private HeroStats heroStats;
    
    private void Awake()
    {
        carController = GetComponent<CarController>();
        heroStats = GetComponent<HeroStats>();
    }
    
    public override void OnNetworkSpawn()
    {
        // Включаем управление ТОЛЬКО у своей машины
        if (carController != null)
        {
            carController.enabled = IsOwner;
        }
        
        if (IsOwner)
        {
            // Находим камеру в сцене и настраиваем её на эту машину
            Camera mainCam = Camera.main;
            if (mainCam != null)
            {
                FollowCamera followCamera = mainCam.GetComponent<FollowCamera>();
                if (followCamera != null)
                {
                    followCamera.target = transform;
                }
            }
            
            // Находим Canvas в сцене и передаем ссылки в HeroStats
            if (heroStats != null)
            {
                Canvas canvas = FindObjectOfType<Canvas>();
                if (canvas != null)
                {
                    // Ищем элементы по имени (согласно твоей структуре)
                    Transform healthPanel = canvas.transform.Find("HealthPanel");
                    if (healthPanel != null)
                    {
                        Image healthBarFill = healthPanel.Find("HealthBarFill")?.GetComponent<Image>();
                        Text healthText = healthPanel.Find("HealthText")?.GetComponent<Text>();
                        
                        heroStats.SetUIReferences(healthBarFill, healthText);
                    }
                    
                    Text timerText = canvas.transform.Find("TimerText")?.GetComponent<Text>();
                    heroStats.SetTimerReference(timerText);
                }
            }
        }
        
        Debug.Log($"NetworkPlayer: IsOwner={IsOwner}, ClientId={NetworkManager.Singleton.LocalClientId}");
    }
}