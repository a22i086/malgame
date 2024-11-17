using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Dog : Character, ICharacter
{
    private NavMeshAgent agent;
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    public override void Move(Vector3 targetPosition)
    {
        agent.SetDestination(targetPosition);
    }
    public override void Attack()
    {
        Debug.Log("Dog is attacking!");
    }
}
