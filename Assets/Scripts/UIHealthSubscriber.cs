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

            UpdateHealth(targetStats.currentHealth.Value, targetStats.maxHealth);
            UpdateTimer(targetStats.gameTime);
        }
    }

    private void UpdateHealth(float current, float max)
    {
        if (healthBarFill != null)
            healthBarFill.fillAmount = Mathf.Clamp01(current / max);

        if (healthText != null)
            healthText.text = $"{Mathf.RoundToInt(current)} / {Mathf.RoundToInt(max)}";
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

    private void Update()
    {
        if (targetStats != null && healthBarFill.fillAmount == 0 && targetStats.currentHealth.Value > 0)
        {
            UpdateHealth(targetStats.currentHealth.Value, targetStats.maxHealth);
        }
    }
}