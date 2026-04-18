using UnityEngine;

public interface IHealth
{
    float MaxHealth { get; }
    float CurrentHealth { get; }

    void Damage(GameObject source,float power, float freq);
    void Heal(float amount);
    void Die();
}
