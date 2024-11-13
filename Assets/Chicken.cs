using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Chicken : Character
{
    private Vector3 initialPosition;
    void Awake()
    {
        health = 75f;
        speed = 7f;
    }
    public override void Move(Vector3 targetPosition)
    {
        Vector3 fixedPosition = new Vector3(targetPosition.x, initialPosition.y, targetPosition.z);
        StartCoroutine(MoveCoroutine(fixedPosition));
    }

    public override void Attack()
    {
        Debug.Log("Chicken was attacked!");
    }
}
