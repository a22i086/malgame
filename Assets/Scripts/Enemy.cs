using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health = 100f;

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        // 敵が死亡したときの処理（例：アニメーション再生、オブジェクト破壊など）
        Debug.Log("Enemy died!");
        Destroy(gameObject);
    }
}
