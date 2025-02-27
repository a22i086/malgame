using UnityEngine;

public interface IHealth
{
    float Health { get; }
    float MaxHealth { get; }
    void TakeDamage(float amount);
}
