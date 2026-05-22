using UnityEngine;
using UnityEngine.UI;

public class UIHealthSubscriber : MonoBehaviour
{
    public Image healthBarFill;
    public Text healthText;
    public Text timerText;
    
    private HeroStats targetStats;
    
    void Start()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            var netObj = player.GetComponent<Unity.Netcode.NetworkObject>();
            if (netObj != null && netObj.IsOwner)
            {
                targetStats = player.GetComponent<HeroStats>();
                if (targetStats != null)
                {
                    targetStats.OnHealthChanged += UpdateHealth;
                    targetStats.OnTimeChanged += UpdateTimer;
                }
                break;
            }
        }
    }
    
    void UpdateHealth(float current, float max)
    {
        if (healthBarFill != null) healthBarFill.fillAmount = current / max;
        if (healthText != null) healthText.text = $"{Mathf.RoundToInt(current)} / {max}";
    }
    
    void UpdateTimer(float time)
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(time / 60);
            int seconds = Mathf.FloorToInt(time % 60);
            timerText.text = $"{minutes:00}:{seconds:00}";
        }
    }
}