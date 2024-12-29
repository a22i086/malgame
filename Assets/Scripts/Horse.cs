using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horse : Character, IHealth
{
    private HealthManager healthManager;
    public float Health => healthManager.Health;
    public float MaxHealth => healthManager.MaxHealth;
    private GameManager gameManager;

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
        gameManager = FindObjectOfType<GameManager>();
    }

    protected override void Update()
    {
        if (target == null || !target.gameObject.activeInHierarchy)
        {
            FindTarget();
        }

        if (target != null)
        {
            MoveTowardsTarget();
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                Attack();
                lastAttackTime = Time.time;
            }
        }
    }

    public override void Move(Vector3 targetPosition)
    {
        Vector3 fixedPosition = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);
        agent.SetDestination(fixedPosition);
    }

    public override void Attack()
    {
        Debug.Log("Horse is attacking with power: " + attackPower);
        if (target != null)
        {
            IHealth enemyHealth = target.GetComponent<IHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(attackPower);
                ShowAttackEffect();
            }
        }
    }

    public void TakeDamage(float amount)
    {
        healthManager.TakeDamage(amount);
    }
}
