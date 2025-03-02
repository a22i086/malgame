using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;

public class Panda : Character, IHealth
{
    private HealthManager healthManager;
    public float Health => healthManager.Health;
    public float MaxHealth => healthManager.MaxHealth;
    public Transform HealShoot; // ヒールポジション
    public float healAmount = 10f; // 回復量
    public float healCooldown = 1f; // 回復クールダウン
    public GameObject healProjectilePrefab; // 回復用のスフィアなどのオブジェクト
    private float lastHealTime;

    protected override void Awake()
    {
        base.Awake();
        health = 200f;
        speed = 0f;
        attackPower = 0f; // パンダは攻撃しない
        attackRange = 100f;
        attackCooldown = 0f;
        spawnCooldown = 5.0f;
        agent.speed = speed;
        lastAttackTime = -attackCooldown;
        lastHealTime = -healCooldown;
        healthManager = GetComponent<HealthManager>();
        animator = GetComponent<Animator>();
    }

    protected override void Update()
    {
        base.Update();
        HealAllies();
    }

    private void HealAllies()
    {
        if (Time.time >= lastHealTime + healCooldown)
        {
            Transform nearestAlly = null;
            float shortestDistance = Mathf.Infinity;

            var allies = gameManager.GetAllies(this);
            Debug.Log($"Allies count: {allies.Count}");

            foreach (Character ally in allies)
            {
                if (ally != null && ally.team == this.team && ally != this)
                {
                    float distanceToAlly = Vector3.Distance(transform.position, ally.transform.position);
                    Debug.Log($"Ally: {ally.name}, Distance: {distanceToAlly}");

                    if (distanceToAlly < shortestDistance && distanceToAlly <= attackRange)
                    {
                        shortestDistance = distanceToAlly;
                        nearestAlly = ally.transform;
                    }
                }
            }

            if (nearestAlly != null)
            {
                StartCoroutine(ShootHealProjectile(nearestAlly));
                lastHealTime = Time.time;
            }
        }
    }

    private IEnumerator ShootHealProjectile(Transform target)
    {
        // 回復用のスフィアなどのオブジェクトを生成
        GameObject healProjectile = Instantiate(healProjectilePrefab, HealShoot.position, HealShoot.rotation);

        // スフィアの初期速度と方向を設定
        Rigidbody rb = healProjectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            rb.velocity = direction * 50f; // 投げる速度を調整
        }

        // スフィアがターゲットに到達するまで待機
        while (healProjectile != null && target != null && Vector3.Distance(healProjectile.transform.position, target.position) > 0.5f)
        {
            yield return null;
        }

        // ターゲットに到達したら回復処理を実行
        if (healProjectile != null && target != null)
        {
            IHealth allyHealth = target.GetComponent<IHealth>();
            if (allyHealth != null && allyHealth.Health < allyHealth.MaxHealth)
            {
                allyHealth.TakeDamage(-healAmount); // 回復はダメージの負値として処理
                ShowHealEffect(target);
            }

            // 回復用のスフィアを削除
            Destroy(healProjectile);
        }
    }

    private void ShowHealEffect(Transform target)
    {
        if (attackEffectPrefab != null)
        {
            GameObject healEffect = Instantiate(attackEffectPrefab, target.position, Quaternion.identity);
            Destroy(healEffect, 3.0f); // エフェクトの寿命を設定
        }
    }

    public override void Move(Vector3 targetPosition)
    {

    }

    public override void Attack()
    {
        // パンダは攻撃しないので空のメソッド
    }

    public override void TakeDamage(float amount)
    {
        healthManager.TakeDamage(amount);
        if (healthManager.Health <= 0)
        {
            isDead = true;
            base.Die();
        }
    }


}
