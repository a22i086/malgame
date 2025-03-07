using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacter
{
    void Move(Vector3 targetPosition);
    void Attack();
}