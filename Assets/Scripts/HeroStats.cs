using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HeroStats : NetworkBehaviour
{
    public float maxHealth = 100f;
    public NetworkVariable<float> currentHealth = new NetworkVariable<float>(100f, 
        NetworkVariableReadPermission.Everyone, 
        NetworkVariableWritePermission.Owner);   // ← Изменено

    public float gameTime = 0f;
    private bool isGameOver = false;

    public System.Action<float, float> OnHealthChanged;
    public System.Action<float> OnTimeChanged;

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
            currentHealth.Value = maxHealth;

        currentHealth.OnValueChanged += (oldVal, newVal) =>
        {
            OnHealthChanged?.Invoke(newVal, maxHealth);
            if (newVal <= 0 && IsOwner)
                GiveUp();
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
        currentHealth.Value = Mathf.Max(0f, currentHealth.Value - damage);
    }

    private void GiveUp()
    {
        isGameOver = true;
        SceneManager.LoadScene("PreliminaryScene");
    }
}