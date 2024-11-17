// using UnityEngine;
// using System.Collections;

// public abstract class Character : MonoBehaviour, ICharacter
// {
//     public float health;
//     public float speed;

//     public abstract void Attack();

//     public virtual void Move(Vector3 targetPosition) //引数として移動先の座標を受け取り、非同期の移動処理をする
//     {
//         StartCoroutine(MoveCoroutine(targetPosition));
//     }

//     protected IEnumerator MoveCoroutine(Vector3 targetPosition)
//     {
//         while (Vector3.Distance(transform.position, targetPosition) > Mathf.Epsilon)
//         {
//             transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
//             yield return null;
//         }
//     }
// }

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
    protected NavMeshAgent agent;
    protected Transform target;

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    protected virtual void Update()
    {
        FindTarget();
        if (target != null)
        {
            MoveTowardsTarget();
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
}
