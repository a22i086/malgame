using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horse : Character, IHealth
{
    private HealthManager healthManager;
    public float Health => healthManager.Health;
    public float MaxHealth => healthManager.MaxHealth;

    protected override void Awake()
    {
        base.Awake();
        health = 150f;
        speed = 12f;
        attackPower = 20f;
        attackRange = 10f;
        attackCooldown = 2.0f;
        agent.speed = speed;
        lastAttackTime = -attackCooldown;
        healthManager = GetComponent<HealthManager>();
    }

    public override void Move(Vector3 targetPosition)
    {
        Vector3 fixedPosition = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);
        agent.SetDestination(fixedPosition);
    }

    public override void Attack()
    {
        Debug.Log("Horse is attacking with power: " + attackPower);
        //敵にダメージを与える
        if (target != null)
        {
            Enemy enemy = target.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(attackPower);
            }
        }
        lastAttackTime = Time.time;
    }

    protected override void Update()
    {
        base.Update();
        if (target != null && Vector3.Distance(transform.position, target.position) <= attackRange)
        {
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                Attack();
            }
        }
    }
    public void TakeDamage(float amount)
    {
        healthManager.TakeDamage(amount);
    }
}
