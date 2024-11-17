using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horse : Character
{
    protected override void Awake()
    {
        base.Awake();
        health = 150f;
        speed = 12f;
        attackPower = 20f;
        attackRange = 10f;
        agent.speed = speed;
    }

    public override void Move(Vector3 targetPosition)
    {
        Vector3 fixedPosition = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);
        agent.SetDestination(fixedPosition);
    }

    public override void Attack()
    {
        Debug.Log("Horse is attacking with power: " + attackPower);
    }
}
