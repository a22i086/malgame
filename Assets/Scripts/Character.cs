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
    private float Difference2Enemy = 5.0f;
    protected NavMeshAgent agent;
    protected Transform target;
    public GameObject attackEffectPrefab;
    public GameManager gameManager;
    protected Animator animator;

    public int team;
    public bool isPlayerControlled; //敵動物かどうか
    public bool isSpawnConfirmed; // 召喚が確定されたかどうか
    protected bool isDead = false;

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
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

    protected virtual bool CanSeeTarget(Transform potentialTarget)
    {
        float heightDifference = Mathf.Abs(transform.position.y - potentialTarget.transform.position.y);
        if (heightDifference > Difference2Enemy)
        {
            return false;
        }
        else
            return true;
    }

    protected void FindTarget()
    {
        // GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        // float shortestDistance = Mathf.Infinity;
        // GameObject nearestEnemy = null;

        List<Character> enemies = gameManager.GetEnemies(this);

        float shortestDistance = Mathf.Infinity;
        Transform nearestEnemy = null;

        foreach (Character enemy in enemies)
        {
            if (enemy != null && enemy.gameObject != null && enemy.gameObject.activeInHierarchy && enemy.team != this.team)
            {
                if (!CanSeeTarget(enemy.transform)) continue;
                float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                if (distanceToEnemy < shortestDistance)
                {
                    shortestDistance = distanceToEnemy;
                    nearestEnemy = enemy.transform;
                }
            }

        }
        string targetTag = team == 0 ? "EnemyTower" : "PlayerTower";
        GameObject[] enemyTowers = GameObject.FindGameObjectsWithTag(targetTag);
        foreach (GameObject tower in enemyTowers)
        {
            if (tower != null && tower.activeInHierarchy)
            {
                Tower towerComponent = tower.GetComponent<Tower>();
                if (towerComponent != null && towerComponent.team != this.team)
                {
                    float distanceToTower = Vector3.Distance(transform.position, tower.transform.position);
                    if (distanceToTower < shortestDistance)
                    {
                        shortestDistance = distanceToTower;
                        nearestEnemy = tower.transform;
                    }
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
            GameObject attackEffect = Instantiate(attackEffectPrefab, target.position, Quaternion.identity);
            Renderer renderer = attackEffect.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.sortingLayerName = "AttackEffect";
                renderer.sortingOrder = 100;
            }
            Destroy(attackEffect, 3.0f);
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
            StartCoroutine(Die());
        }
    }

    protected virtual IEnumerator Die()
    {
        // animator.SetBool("isAttacking", false);
        // animator.SetBool("isRunning", false);
        // animator.SetBool("Idle", false);
        animator.SetBool("isDead", true);
        agent.isStopped = true;

        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        gameManager.RemoveAnimal(this);
        Destroy(gameObject);
    }
}
