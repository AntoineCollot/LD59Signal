using System.Collections.Generic;
using UnityEngine;

public class WaterProjectilePool : MonoBehaviour
{
    [SerializeField] WaterProjectile prefab;
    [SerializeField] float spawnHeight;
    [SerializeField] float baseRandomChance = 0.3f;
    Queue<WaterProjectile> pool;

    public static WaterProjectilePool Instance;

    void Awake()
    {
        Instance = this;
        pool = new Queue<WaterProjectile>();
    }

    private void Start()
    {
        XPManager.Instance.OnEnemyKilled += OnEnemyKilled;
    }

    private void OnDestroy()
    {
        if (XPManager.Instance != null)
            XPManager.Instance.OnEnemyKilled -= OnEnemyKilled;

    }

    private void OnEnemyKilled()
    {
        if(PowerUpManager.Instance.HasPowerUp(PowerUp.WaterDrops))
        {
            if (StatsManager.Instance.RunRandomChance(baseRandomChance))
                EmitProjectile(PlayerState.Instance.transform.position);
        }
    }

    public void EmitProjectile(Vector3 fromPos)
    {
        WaterProjectile proj = GetWaterProjectile();
        proj.transform.position = fromPos + Vector3.up * spawnHeight;
        proj.Emit();
    }

    WaterProjectile GetWaterProjectile()
    {
        if (pool.TryDequeue(out WaterProjectile waterProjectile))
            return waterProjectile;
        else
            return Instantiate(prefab, null);
    }

    public void ReturnToPool(WaterProjectile waterProjectile)
    {
        pool.Enqueue(waterProjectile);
        waterProjectile.gameObject.SetActive(false);
    }
}
