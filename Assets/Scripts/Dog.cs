using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : Character
{
    protected override void Awake()
    {
        base.Awake();
        health = 100f;
        speed = 10f;
        attackPower = 15f;
        attackRange = 7f;
        agent.speed = speed;
    }

    public override void Move(Vector3 targetPosition)
    {
        Vector3 fixedPosition = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);
        agent.SetDestination(fixedPosition);
    }

    public override void Attack()
    {
        Debug.Log("Dog is attacking with power: " + attackPower);
    }
    protected override void Update()
    {
        base.Update();
        if (target != null && Vector3.Distance(transform.position, target.position) <= attackRange)
        {
            Attack();
        }
    }
}
