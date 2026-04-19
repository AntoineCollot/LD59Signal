using System.Collections;
using UnityEngine;

public class PlayerClone : MonoBehaviour
{
    [SerializeField] GameObject clone;
    [SerializeField] float spawnInterval = 30;
    [SerializeField] float spawnDistance = 3;
    const float SPAWN_ANIM_DURATION = 1;
    float nextSpawnTime = 0;
    bool isSpawningClone;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PowerUpManager.onAnyUpdate += OnPowerUpUpdate;
    }

    private void OnDestroy()
    {
        PowerUpManager.onAnyUpdate -= OnPowerUpUpdate;
    }

    private void OnPowerUpUpdate()
    {
    }

    void Update()
    {
        if (!isSpawningClone && PowerUpManager.Instance.HasPowerUp(PowerUp.Clone) && GameManager.Instance.gameTime>nextSpawnTime)
        {
            StartCoroutine(SpawnClone());
        }
    }

    IEnumerator SpawnClone()
    {
        Debug.Log("Spawning Clone");
        isSpawningClone = true;
        clone.SetActive(true);
        clone.transform.position = GetSpawnPosition();
        nextSpawnTime = GameManager.Instance.gameTime + spawnInterval;
        FXManager.Instance.EmitWaterSplash(clone.transform.position);

        EnemyTargetManager.Instance.RegisterTarget(clone.transform);

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / SPAWN_ANIM_DURATION;

            clone.transform.localScale = Vector3.one * Curves.Berp(0, 1, Mathf.Clamp01(t));

            yield return null;
        }
        isSpawningClone = false;
    }

    Vector3 GetSpawnPosition()
    {
        Vector3 playerPos = PlayerState.Instance.transform.position;
        return playerPos + Random.insideUnitCircle.normalized.ToVector3();
    }

    public void DisableClone()
    {
        FXManager.Instance.EmitWaterSplash(clone.transform.position);
        EnemyTargetManager.Instance.DeregisterTarget(clone.transform);
        clone.SetActive(false);
    }
}
