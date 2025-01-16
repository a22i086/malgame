using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;

public class Horse : Character, IHealth
{
    private HealthManager healthManager;
    public float Health => healthManager.Health;
    public float MaxHealth => healthManager.MaxHealth;
    //public Animator animator;
    // protected bool isDead = false;


    protected override void Awake()
    {
        base.Awake();
        health = 150f;
        speed = 50f;
        attackPower = 20f;
        attackRange = 10f;
        attackCooldown = 2.0f;
        agent.speed = speed;
        lastAttackTime = -attackCooldown;
        healthManager = GetComponent<HealthManager>();
        animator = GetComponent<Animator>();
    }

    protected override void Update()
    {
        base.Update();
        if (target != null)
        {
            float distanceToTarget = Vector3.Distance(transform.position, target.position);
            if (distanceToTarget <= attackRange)
            {
                animator.SetBool("isRunning", false);
            }
            else
            {
                animator.SetBool("isRunning", true);
                animator.SetBool("isAttacking", false);
                // animator.SetBool("isWalking", false);
            }
        }
        else
        {
            animator.SetBool("isRunning", false);
        }
    }

    public override void Move(Vector3 targetPosition)
    {
        animator.SetBool("isAttacking", false);
        Vector3 fixedPosition = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);
        agent.SetDestination(fixedPosition);
    }

    public override void Attack()
    {
        if (isDead) return;
        agent.isStopped = true;
        animator.SetBool("isAttacking", true);
        Debug.Log("Horse is attacking with power: " + attackPower);
        if (target != null && target.gameObject != null && target.gameObject.activeInHierarchy)
        {
            IHealth enemyHealth = target.GetComponent<IHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(attackPower);
                ShowAttackEffect();
            }

            if (enemyHealth != null && enemyHealth.Health <= 0)
            {
                target = null;
            }
        }
        agent.ResetPath();
        agent.velocity = Vector3.zero;
        agent.isStopped = false;
    }

    public override void TakeDamage(float amount)
    {
        healthManager.TakeDamage(amount);
        if (healthManager.Health <= 0)
        {
            isDead = true;
        }
    }
}
