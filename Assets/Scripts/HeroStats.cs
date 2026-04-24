using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HeroStats : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public Image healthBarFill;
    public Text healthText;
    public float gameTime = 0f;
    public Text timerText;
    public string gameOverSceneName = "PreliminaryScene";

    [Header("Effects")]
    public ParticleSystem hitParticles;

    private bool isGameOver = false;
    private const string BEST_TIME_KEY = "BestTime";
    
    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
        gameTime = 0f;
    }
    
    void Update()
    {
        if (!isGameOver)
        {
            gameTime += Time.deltaTime;
            UpdateTimerUI();
        }
    }
    
    public void TakeDamage(float damage)
    {
        if (isGameOver) return;
        
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();
        
        Debug.Log($"Получен урон! Здоровье: {currentHealth}/{maxHealth}");
        
        if (hitParticles != null)
        {
            hitParticles.Play();
        }


        if (currentHealth <= 0)
        {
            GiveUp();
        }
    }
    
    void UpdateHealthUI()
    {
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = currentHealth / maxHealth;
        }
        
        if (healthText != null)
        {
            healthText.text = $"{Mathf.RoundToInt(currentHealth)} / {maxHealth}";
        }
    }
    
    void UpdateTimerUI()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(gameTime / 60);
            int seconds = Mathf.FloorToInt(gameTime % 60);
            timerText.text = $"{minutes:00}:{seconds:00}";
        }
    }
    
    void GiveUp()
    {
        isGameOver = true;

        float bestTime = PlayerPrefs.GetFloat(BEST_TIME_KEY, 999999f);
        if (gameTime < bestTime)
        {
            PlayerPrefs.SetFloat(BEST_TIME_KEY, gameTime);
            PlayerPrefs.Save();
            Debug.Log($"Новый рекорд! Время: {gameTime:F1} секунд");
        }
        
        Debug.Log("Вы арестованы полицией!");
        Debug.Log($"Время выживания: {gameTime:F1} секунд");
        SceneManager.LoadScene(gameOverSceneName);
    }
}