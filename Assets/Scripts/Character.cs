using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Character : MonoBehaviour, ICharacter
{
    protected float health;
    public float speed;
    protected float attackPower;
    public float attackRange;
    protected float attackCooldown;
    protected float lastAttackTime;
    protected NavMeshAgent agent;
    protected Transform target;
    public GameObject attackEffectPrefab;
    public GameManager gameManager;

    public int team;
    public bool isPlayerControlled; //敵動物かどうか
    public bool isSpawnConfirmed; // 召喚が確定されたかどうか
    protected bool isDead = false;

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        lastAttackTime = -attackCooldown;
        gameManager = FindObjectOfType<GameManager>();
    }

    protected virtual void Update()
    {
        if (isDead) return;
        if (!isSpawnConfirmed)
        {
            //召喚が確定されるまでは何もしない
            return;
        }
        if (target == null || !target.gameObject.activeInHierarchy)
        {
            FindTarget();
        }
        if (target != null && target.gameObject.activeInHierarchy)
        {
            MoveTowardsTarget();
            float distanceToTarget = Vector3.Distance(transform.position, target.position);
            if (distanceToTarget <= attackRange && Time.time >= lastAttackTime + attackCooldown)
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
        // GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        // float shortestDistance = Mathf.Infinity;
        // GameObject nearestEnemy = null;

        List<Character> enemies = gameManager.GetEnemies(this);

        float shortestDistance = Mathf.Infinity;
        Character nearestEnemy = null;

        foreach (Character enemy in enemies)
        {
            if (enemy != null && enemy.gameObject != null && enemy.gameObject.activeInHierarchy && enemy.team != this.team)
            {
                float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                if (distanceToEnemy < shortestDistance)
                {
                    shortestDistance = distanceToEnemy;
                    nearestEnemy = enemy;
                }
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

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public virtual void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0 && !isDead)
        {
            isDead = true;
            Die();
        }
    }

    protected virtual void Die()
    {
        gameManager.RemoveAnimal(this);
        Destroy(gameObject);
    }
    // protected void MoveBackwards()
    // {
    //     Vector3 directionToMoveBackwards = -transform.forward * 1.0f;
    //     Vector3 newPosition = transform.position + directionToMoveBackwards;
    //     agent.SetDestination(newPosition);
    // }
}
