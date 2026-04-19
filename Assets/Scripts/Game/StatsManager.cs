using System.Collections.Generic;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    [Header("Damage Tick")]
    [SerializeField] float baseDamageTickInterval;
    [SerializeField] float damageTickIncrease;
    public float DamageTickInterval => baseDamageTickInterval * Mathf.Pow(1 - damageTickIncrease, obtainedStats[PowerStat.Overclock]);

    [Header("Pass")]
    [SerializeField] float passDamageIncrease;

    [Header("Loot")]
    public float LootDistanceMult => 1 + obtainedStats[PowerStat.Magnet] * lootDistMultIncrease;
    [SerializeField] float lootDistMultIncrease = 0.3f;

    [Header("Chance")]
    [SerializeField] float randomChanceIncrease = 0.3f;

    [Header("MoveSpeed")]
    [SerializeField] float moveSpeedMultIncrease = 0.3f;
    public float MoveSpeedMult => 1 + moveSpeedMultIncrease * obtainedStats[PowerStat.Faster];

    Dictionary<PowerStat, int> obtainedStats;

    public static StatsManager Instance;

    void Awake()
    {
        Instance = this;
        InitStats();
    }

    public void ObtainStat(PowerStat stat)
    {
        //Register the stat
        if (!obtainedStats.ContainsKey(stat))
        {
            obtainedStats.Add(stat, 0);
        }
        obtainedStats[stat]++;
    }

    void InitStats()
    {
        obtainedStats = new();
        foreach (var stat in EnumExtensions.GetValues<PowerStat>())
        {
            obtainedStats.Add(stat, 0);
        }
    }

    public float ApplyPassDamageIncrease(float damage, float freq)
    {
        //Low pass
        float mult = Curves.QuadEaseInOut(0, 1, 1 - freq) * passDamageIncrease * obtainedStats[PowerStat.LowPass];
        //High pass
        mult += Curves.QuadEaseInOut(0, 1, freq) * passDamageIncrease * obtainedStats[PowerStat.HighPass];

        damage *= 1 + mult;

        return damage;
    }

    public bool RunRandomChance(float chance01)
    {
        chance01 = 1 - chance01;
        chance01 *= Mathf.Pow(1-randomChanceIncrease,obtainedStats[PowerStat.Lucky]);

        float rand = UnityEngine.Random.Range(0f, 1f);

        return rand > 1 - chance01;
    }
}
