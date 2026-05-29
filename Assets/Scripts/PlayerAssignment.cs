using Unity.Netcode;
using UnityEngine;

public class PlayerAssignment : MonoBehaviour
{
    void Start()
    {
        // Находим все машины игроков
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        
        // Сортируем по имени для предсказуемости
        System.Array.Sort(players, (a, b) => a.name.CompareTo(b.name));
        
        // Первая машина (по алфавиту) — хосту, вторая — клиенту
        for (int i = 0; i < players.Length; i++)
        {
            bool isHostCar = (i == 0);  // первая машина — хосту
            bool shouldEnable = false;
            
            if (NetworkManager.Singleton.IsHost && isHostCar)
                shouldEnable = true;
            else if (NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsHost && !isHostCar)
                shouldEnable = true;
            
            EnableControl(players[i], shouldEnable);
        }
    }
    
    void EnableControl(GameObject player, bool enable)
    {
        CarController controller = player.GetComponent<CarController>();
        if (controller != null)
        {
            controller.enabled = enable;
            Debug.Log($"{player.name}: CarController enabled = {enable}");
        }
        
        if (enable)
        {
            Camera mainCam = Camera.main;
            if (mainCam != null)
            {
                FollowCamera follow = mainCam.GetComponent<FollowCamera>();
                if (follow != null) follow.target = player.transform;
            }
        }
    }
}