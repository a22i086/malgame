using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;

public class Elephant : Character, IHealth
{
    private HealthManager healthManager;
    public float Health => healthManager.Health;
    public float MaxHealth => healthManager.MaxHealth;
    protected override void Awake()
    {
        base.Awake();
        health = 200f;
        speed = 20f;
        attackPower = 30f;
        attackRange = 10f;
        attackCooldown = 3.0f;
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
                animator.SetBool("isWalking", false);
            }
            else
            {
                animator.SetBool("isWalking", true);
                animator.SetBool("isAttacking", false);
                // animator.SetBool("isWalking", false);
            }
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }

    public override void Move(Vector3 targetPosition)
    {
        // animator.SetBool("isWalking", true);
        animator.SetBool("isAttacking", false);
        Vector3 fixedPosition = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);
        agent.SetDestination(fixedPosition);
    }

    public override void Attack()
    {
        if (isDead) return;
        // animator.SetBool("isWalking", false);
        // agent.isStopped = true;
        animator.SetBool("isAttacking", true);
        Debug.Log("Elephant is attacking with power: " + attackPower + target);
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

        //ここの下の三つでピタッと止まるようにしている
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
            base.Die();
        }
    }
}
