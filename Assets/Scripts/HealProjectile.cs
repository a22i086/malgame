using UnityEngine;

public class HealProjectile : MonoBehaviour
{
    public GameObject healEffectPrefab;

    private void OnTriggerEnter(Collider other)
    {
        IHealth allyHealth = other.GetComponent<IHealth>();
        if (allyHealth != null && allyHealth.Health < allyHealth.MaxHealth)
        {
            float healAmount = allyHealth.MaxHealth * 0.25f + 50f; // 体力の25％+50
            allyHealth.TakeDamage(-healAmount); // 回復はダメージの負値として処理
            ShowHealEffect(other.transform);
            Destroy(gameObject); // スフィアを削除
        }
    }

    private void ShowHealEffect(Transform target)
    {
        if (healEffectPrefab != null)
        {
            GameObject healEffect = Instantiate(healEffectPrefab, target.position, Quaternion.identity);
            Destroy(healEffect, 3.0f); // エフェクトの寿命を設定
        }
    }
}