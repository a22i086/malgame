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
    public GameManager gameManager;

    public int team;

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        lastAttackTime = -attackCooldown;
        gameManager = FindObjectOfType<GameManager>();
        Debug.Log("GameManager found: " + (gameManager != null));
    }

    protected virtual void Update()
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
            if (enemy.team == this.team)
            {
                continue;
            }
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
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

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
    // protected void MoveBackwards()
    // {
    //     Vector3 directionToMoveBackwards = -transform.forward * 1.0f;
    //     Vector3 newPosition = transform.position + directionToMoveBackwards;
    //     agent.SetDestination(newPosition);
    // }
}
