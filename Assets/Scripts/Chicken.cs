using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Chicken : Character, IHealth
{
    private HealthManager healthManager;
    public float Health => healthManager.Health;
    public float MaxHealth => healthManager.MaxHealth;
    private Vector3 initialPosition;
    protected override void Awake()
    {
        base.Awake();
        health = 75f;
        speed = 7f;
        attackPower = 10f;
        attackRange = 5f;
        attackCooldown = 2.0f;
        agent.speed = speed;
        agent.updatePosition = false; // NavMeshAgentの高さ制御を無効化
        lastAttackTime = -attackCooldown;
        healthManager = GetComponent<HealthManager>();
    }

    public override void Move(Vector3 targetPosition)
    {
        Vector3 fixedPosition = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);
        agent.SetDestination(fixedPosition);
        StartCoroutine(UpdatePosition());
    }

    public override void Attack()
    {
        Debug.Log("Chicken is attacking with power: " + attackPower);
        if (target != null)
        {
            Enemy enemy = target.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(attackPower);
            }
            ShowAttackEffect();
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

    private IEnumerator UpdatePosition() //Y座標を固定したまま移動制御
    {
        /*agent.pathPendingはエージェントが新しい経路を計算している間trueになる。
          agent.remainingDistanceはエージェントが目的地までの残り距離を返す。
          agent.stoppingDistanceはエージェントが目的地に到達とみなす距離を示す。*/
        while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
        {
            Vector3 position = agent.nextPosition; // エージェントの次のフレームでの予測位置をpositionに代入
            position.y = initialPosition.y;
            agent.transform.position = position;
            yield return null;
        }
    }
}
