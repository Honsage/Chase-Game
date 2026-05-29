using UnityEngine;
using UnityEngine.UI;

public class UIHealthSubscriber : MonoBehaviour
{
    public Image healthBarFill;
    public Text healthText;
    public Text timerText;

    private HeroStats targetStats;

    public void SetTarget(HeroStats stats)
    {
        // Отписываемся от старого
        if (targetStats != null)
        {
            targetStats.OnHealthChanged -= UpdateHealth;
            targetStats.OnTimeChanged -= UpdateTimer;
        }

        targetStats = stats;

        if (targetStats != null)
        {
            targetStats.OnHealthChanged += UpdateHealth;
            targetStats.OnTimeChanged += UpdateTimer;

            // Принудительное обновление
            UpdateHealth(targetStats.currentHealth.Value, targetStats.maxHealth);
        }
    }

    private void UpdateHealth(float current, float max)
    {
        if (healthBarFill != null) healthBarFill.fillAmount = current / max;
        if (healthText != null) healthText.text = $"{Mathf.RoundToInt(current)} / {Mathf.RoundToInt(max)}";
    }

    private void UpdateTimer(float time)
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(time / 60);
            int seconds = Mathf.FloorToInt(time % 60);
            timerText.text = $"{minutes:00}:{seconds:00}";
        }
    }
}