using System.Collections.Generic;
using UnityEngine;

public class FXManager : MonoBehaviour
{
    public enum AttackType { Radio }
    [SerializeField] ParticleSystem radioAttackParticles;

    const int NUMBER_POOL_SIZE = 200;
    [SerializeField] DamageNumber damageNumberPrefab;
    [SerializeField] Transform damageNumberParent;

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
    }

    public void ReturnDamageToPool(DamageNumber number)
    {
        numberPool.Enqueue(number);
    }
    #endregion
}
