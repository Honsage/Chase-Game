using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HeroStats : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    
    // UI элементы (назначаются из NetworkPlayer)
    private Image healthBarFill;
    private Text healthText;
    private Text timerText;
    
    public float gameTime = 0f;
    public string gameOverSceneName = "PreliminaryScene";

    [Header("Effects")]
    public ParticleSystem hitParticles;
    public AudioSource hitSound;

    private bool isGameOver = false;
    private const string BEST_TIME_KEY = "BestTime";
    private bool isInitialized = false; // Флаг, что UI инициализирован
    
    public void SetUIReferences(Image fill, Text text)
    {
        healthBarFill = fill;
        healthText = text;
        isInitialized = true;
        UpdateHealthUI(); // Обновляем UI сразу после назначения ссылок
    }
    
    public void SetTimerReference(Text timer)
    {
        timerText = timer;
        UpdateTimerUI(); // Обновляем таймер сразу
    }
    
    void Start()
    {
        currentHealth = maxHealth;
        gameTime = 0f;
        
        // Если ссылки уже назначены (через NetworkPlayer), обновляем UI
        if (isInitialized)
        {
            UpdateHealthUI();
        }
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
        
        if (hitSound != null)
        {
            hitSound.Play();
        }
        
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