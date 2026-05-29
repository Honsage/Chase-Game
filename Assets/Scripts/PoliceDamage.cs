using UnityEngine;

public class PoliceDamage : MonoBehaviour
{
    public float damageAmount = 10f;
    public float damageCooldown = 1f;
    private float lastDamageTime = 0f;

    private void OnCollisionEnter(Collision collision)
    {
        if (Time.time - lastDamageTime < damageCooldown) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            HeroStats heroStats = collision.gameObject.GetComponent<HeroStats>();
            if (heroStats != null)
            {
                heroStats.TakeDamage(damageAmount);
                lastDamageTime = Time.time;
                Debug.Log($"Полиция нанесла {damageAmount} урона игроку!");
            }
        }
    }
}