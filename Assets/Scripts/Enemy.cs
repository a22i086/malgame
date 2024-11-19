using UnityEngine;

public class Enemy : MonoBehaviour, IHealth
{
    private HealthManager healthManager;

    public float Health => healthManager.Health;
    public float MaxHealth => healthManager.MaxHealth;

    void Awake()
    {
        healthManager = GetComponent<HealthManager>();
    }

    public void TakeDamage(float amount)
    {
        healthManager.TakeDamage(amount);
    }
}
