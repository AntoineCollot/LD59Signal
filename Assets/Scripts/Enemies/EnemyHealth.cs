using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class EnemyHealth : MonoBehaviour, IHealth
{
    [SerializeField] protected float baseHealth;
    [SerializeField] protected DamageFreq damageFreq;
    [SerializeField] protected int sparkOnKill = 1;
    protected float _currentHealth;
    protected EnemyMatGetter matGetter;

    public float MaxHealth => baseHealth;
    public float CurrentHealth => _currentHealth;
    protected Dictionary<GameObject, float> lastHitTimes;
    protected float lastAnyDamagTime;

    static protected readonly int DAMAGE_TIME_PROPERTY = Shader.PropertyToID("_DamageTime");
    static protected readonly int DAMAGE_PRECISION_PROPERTY = Shader.PropertyToID("_DamagePrecision");

    protected void Awake()
    {
        matGetter = GetComponentInChildren<EnemyMatGetter>();
        _currentHealth = MaxHealth;
        lastHitTimes = new();
    }

    protected virtual void Start()
    {

    }

    public void Damage(GameObject source, float power, float freq, bool overrideFreq)
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

        float damages = ComputeDamages(power, freq, in damageFreq, overrideFreq, out float precision);
        UpdateMaterials(damages, precision);
        lastAnyDamagTime = Time.time;

        if (damageAllowed && damages > 0)
        {
            _currentHealth -= damages;
            FXManager.Instance.EmitDamage(transform.position, damages);
            SFXManager.PlaySound(GlobalSFX.MonsterDamaged);

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
            XPManager.Instance.SpawnSparks(transform.position, sparkOnKill);

        ScreenShaker.Instance.SmallShake();
        SFXManager.PlaySound(GlobalSFX.MonsterKill);
        ScoreManager.Instance.AddScore(Mathf.Max(1, sparkOnKill));
        FXManager.Instance.EmitDieEffect(transform.position);
        Destroy(gameObject);
    }

    static protected float ComputeDamages(float power, float freq, in DamageFreq damageFreq,bool overrideFreq, out float precision)
    {
        precision = damageFreq.GetFrequencyPrecision(freq);
        if (overrideFreq)
            precision = 1;
        return power * precision;
    }

#if UNITY_EDITOR
    protected void OnDrawGizmos()
    {
        Handles.Label(transform.position + Vector3.right * 0.5f, CurrentHealth.ToString("N1"));
    }
#endif
}
