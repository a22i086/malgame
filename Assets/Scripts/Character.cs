using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Character : MonoBehaviour, ICharacter
{
    protected float health;
    protected float speed;
    protected float attackPower;
    protected float attackRange;
    protected float attackCooldown;
    protected float lastAttackTime;
    protected NavMeshAgent agent;
    protected Transform target;
    public GameObject attackEffectPrefab;

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        lastAttackTime = -attackCooldown;
    }

    protected virtual void Update()
    {
        FindTarget();
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

    public abstract void Move(Vector3 targetPosition);
    public abstract void Attack();

    protected void FindTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance && distanceToEnemy <= attackRange)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }
    }

    protected void MoveTowardsTarget()
    {
        if (target != null)
        {
            agent.SetDestination(target.position);
        }
    }
    // エフェクトを表示させる
    protected void ShowAttackEffect()
    {
        if (attackEffectPrefab != null && target != null)
        {
            Instantiate(attackEffectPrefab, target.position, Quaternion.identity);
        }
    }
    // protected void MoveBackwards()
    // {
    //     Vector3 directionToMoveBackwards = -transform.forward * 1.0f;
    //     Vector3 newPosition = transform.position + directionToMoveBackwards;
    //     agent.SetDestination(newPosition);
    // }
}
