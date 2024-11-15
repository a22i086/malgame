using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : Character
{
    void Awake()
    {
        health = 100f;
        speed = 5f;
    }
    public override void Attack()
    {
        Debug.Log("Dog is attacking!");
    }
}
