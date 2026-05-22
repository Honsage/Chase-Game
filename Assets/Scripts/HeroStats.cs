using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HeroStats : NetworkBehaviour
{
    public float maxHealth = 100f;
    public NetworkVariable<float> currentHealth = new NetworkVariable<float>();
    public float gameTime = 0f;
    
    public System.Action<float, float> OnHealthChanged;
    public System.Action<float> OnTimeChanged;
    
    private const string BEST_TIME_KEY = "BestTime";
    private bool isGameOver = false;
    
    [Header("Effects")]
    public ParticleSystem hitParticles;
    public AudioSource hitSound;
    
    private PTSHandler ptsHandler;
    
    public override void OnNetworkSpawn()
    {
        ptsHandler = GetComponent<PTSHandler>();
        
        if (IsOwner)
        {
            currentHealth.Value = maxHealth;
        }
        
        currentHealth.OnValueChanged += (oldVal, newVal) =>
        {
            OnHealthChanged?.Invoke(newVal, maxHealth);
            if (newVal <= 0 && IsOwner) GiveUp();
        };
    }
    
    void Update()
    {
        if (!IsOwner || isGameOver) return;
        
        gameTime += Time.deltaTime;
        OnTimeChanged?.Invoke(gameTime);
    }
    
    public void TakeDamage(float damage)
    {
        if (!IsOwner || isGameOver) return;
        
        if (hitSound != null) hitSound.Play();
        if (hitParticles != null) hitParticles.Play();
        
        currentHealth.Value -= damage;
        
        if (ptsHandler != null)
        {
            ptsHandler.SendPTSData((int)currentHealth.Value, transform.position);
        }
        
        Debug.Log($"Получен урон! Здоровье: {currentHealth.Value}/{maxHealth}");
    }
    
    void GiveUp()
    {
        isGameOver = true;
        
        float bestTime = PlayerPrefs.GetFloat(BEST_TIME_KEY, 999999f);
        if (gameTime < bestTime)
        {
            PlayerPrefs.SetFloat(BEST_TIME_KEY, gameTime);
            PlayerPrefs.Save();
        }
        
        SceneManager.LoadScene("PreliminaryScene");
    }
}