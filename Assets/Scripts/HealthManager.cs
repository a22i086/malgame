using UnityEngine;

public class HealthManager : MonoBehaviour, IHealth
{
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;

    public float Health => currentHealth;
    public float MaxHealth => maxHealth;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} died!");
        Destroy(gameObject);
    }
}
