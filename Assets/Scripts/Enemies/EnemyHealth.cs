using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class EnemyHealth : MonoBehaviour, IHealth
{
    [SerializeField]protected float baseHealth;
    [SerializeField] protected DamageFreq damageFreq;
    [SerializeField] protected int sparkOnKill = 1;
    protected float _currentHealth;
    protected EnemyMatGetter matGetter;

    public float MaxHealth => baseHealth;
    public float CurrentHealth => _currentHealth;
    protected Dictionary<GameObject, float> lastHitTimes;

    static protected readonly int DAMAGE_TIME_PROPERTY = Shader.PropertyToID("_DamageTime");
    static protected readonly int DAMAGE_PRECISION_PROPERTY = Shader.PropertyToID("_DamagePrecision");

    protected virtual void Start()
    {
        matGetter = GetComponentInChildren<EnemyMatGetter>();
        _currentHealth = MaxHealth;
        lastHitTimes = new();
    }

    public void Damage(GameObject source, float power, float freq)
    {
        bool damageAllowed = true;
        //Check if has been hit too recently by this source
        if (lastHitTimes.TryGetValue(source, out float lastHitTime))
        {
            if (Time.time < lastHitTime + StatsManager.Instance.DamageTickInterval)
                damageAllowed = false;
            else
                lastHitTimes[source] = Time.time;
        }
        else
            lastHitTimes.Add(source, Time.time);

        float damages = ComputeDamages(power, freq, in damageFreq, out float precision);
        UpdateMaterials(damages, precision);

        if (damageAllowed && damages > 0)
        {
            _currentHealth -= damages;
            FXManager.Instance.EmitDamage(transform.position, damages);

            if (_currentHealth < 0)
            {
                Die();
            }
        }
    }

    protected virtual void UpdateMaterials(float damages, float precision)
    {
        if (damages > 0)
        {
            matGetter.InstancedMaterial.SetFloat(DAMAGE_TIME_PROPERTY, Time.time);
        }
        matGetter.InstancedMaterial.SetFloat(DAMAGE_PRECISION_PROPERTY, precision);
    }

    public void Heal(float amount)
    {
        throw new System.NotImplementedException();
    }

    public void Die()
    {
        if (sparkOnKill > 0)
            XPManager.Instance.SpawnSparks(transform.position,sparkOnKill);
        gameObject.SetActive(false);
    }

    static protected float ComputeDamages(float power, float freq, in DamageFreq damageFreq, out float precision)
    {
        precision = damageFreq.GetFrequencyPrecision(freq);
        return power * precision;
    }

#if UNITY_EDITOR
    protected void OnDrawGizmos()
    {
        Handles.Label(transform.position + Vector3.right * 0.5f, CurrentHealth.ToString("N1"));
    }
#endif
}
