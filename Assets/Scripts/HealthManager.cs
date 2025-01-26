using UnityEngine;

public class HealthManager : MonoBehaviour, IHealth
{
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;
    private GameManager gameManager;

    public float Health => currentHealth;
    public float MaxHealth => maxHealth;

    void Awake()
    {
        currentHealth = maxHealth;
        gameManager = FindObjectOfType<GameManager>();
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
        if (gameObject.layer == LayerMask.NameToLayer("EnemyTower") && gameManager != null)
        {
            gameManager.Game_Win();
        }
        if (gameObject.layer == LayerMask.NameToLayer("PlayerTower") && gameManager != null)
        {
            gameManager.Game_Over();
        }
        Destroy(gameObject);
    }
}
