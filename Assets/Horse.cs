using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horse : Character
{
    void Awake()
    {
        health = 200f;
        speed = 10f;
    }
    public override void Attack()
    {
        Debug.Log("Horse was attacked!");
    }
}
