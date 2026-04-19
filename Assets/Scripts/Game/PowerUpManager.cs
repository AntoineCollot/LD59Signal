using System;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    [SerializeField] PowerUp powerUps;
    [Header("Level Pool")]
    [SerializeField] ScriptablePower[] bankPowerMajor;
    [SerializeField] ScriptablePower[] bankPowerMinor;

    public const int POWER_PER_LEVEL = 3;
    const int MAJOR_LEVEL_EVERY = 3;

    public static event Action onAnyUpdate;
    public static PowerUpManager Instance;

    void Awake()
    {
        Instance = this;
    }

    public void ObtainPower(ScriptablePower power)
    {
        if (power.powerUpType != PowerUp.None)
            ObtainPowerUp(power.powerUpType);
        if(power.powerStat != PowerStat.None)
            StatsManager.Instance.ObtainStat(power.powerStat);
    }

    public void ObtainPowerUp(PowerUp powerUp)
    {
        powerUps |= powerUp;
        onAnyUpdate?.Invoke();
    }

    public bool HasPowerUp(PowerUp powerUp)
    {
        return powerUps.Contains(powerUp);
    }

    public ScriptablePower GetPowerForLevel(int level)
    {
        bool isMajor = level % MAJOR_LEVEL_EVERY == 0;

        ScriptablePower[] bank;
        if (isMajor)
            bank = bankPowerMajor;
        else
            bank = bankPowerMinor;

        ScriptablePower candidate = null;
        //Try get powers
        for (int i = 0; i < 100; i++)
        {
            int rand = UnityEngine.Random.Range(0, bank.Length);
            candidate = bank[rand];
            if (candidate.powerUpType == PowerUp.None || HasPowerUp(candidate.powerUpType))
                return candidate;
        }
        return candidate;
    }
}
