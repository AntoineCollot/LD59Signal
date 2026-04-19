using System.Collections.Generic;
using UnityEngine;

public class FXManager : MonoBehaviour
{
    [Header("Attacks")]
    [SerializeField] ParticleSystem radioAttackParticles;
    public enum AttackType { Radio }

    [Header("Particles")]
    [SerializeField] ParticleSystem damageParticles;
    [SerializeField] ParticleSystem dieParticlesRed;
    [SerializeField] ParticleSystem dieParticlesSmoke;
    [SerializeField] ParticleSystem waterSplashParticles;

    [Header("Numbers")]
    [SerializeField] DamageNumber damageNumberPrefab;
    [SerializeField] Transform damageNumberParent;
    const int NUMBER_POOL_SIZE = 200;

    Queue<DamageNumber> numberPool;

    public static FXManager Instance;

    void Awake()
    {
        Instance = this;
        InitDamageNumberPool();
    }

    public void EmitAttack(AttackType attackType, Vector3 position)
    {
        switch (attackType)
        {
            case AttackType.Radio:
                EmitRadioAttack(position);
                break;
            default:
                throw new System.NotImplementedException();
        }
    }

    void EmitRadioAttack(Vector3 position)
    {
        radioAttackParticles.transform.position = position;
        radioAttackParticles.Emit(1);
    }

    public void EmitDamageEffect(Vector3 position)
    {
        damageParticles.transform.position = position;
        damageParticles.Emit(5);
    }

    public void EmitDieEffect(Vector3 position)
    {
        dieParticlesRed.transform.position = position;
        dieParticlesRed.Emit(7);
        dieParticlesSmoke.Emit(10);
    }

    public void EmitWaterSplash(Vector3 position)
    {
        waterSplashParticles.transform.position = position;
        waterSplashParticles.Play();
    }

    #region DamageNumber
    void InitDamageNumberPool()
    {
        numberPool = new Queue<DamageNumber>();
        for (int i = 0; i < NUMBER_POOL_SIZE; i++)
        {
            DamageNumber number = Instantiate(damageNumberPrefab, damageNumberParent);
            number.gameObject.SetActive(false);
            ReturnDamageToPool(number);
        }
    }

    public void EmitDamage(Vector3 pos, float damage)
    {
        if(numberPool.TryDequeue(out DamageNumber number))
        {
            number.Display(pos, damage);
        }

        EmitDamageEffect(pos);
    }

    public void ReturnDamageToPool(DamageNumber number)
    {
        numberPool.Enqueue(number);
    }
    #endregion
}
