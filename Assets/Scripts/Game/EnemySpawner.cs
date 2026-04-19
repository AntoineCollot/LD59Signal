using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public const float TIME_BEFORE_ATTACK = 3;
    public const float TIME_BEFORE_MOVEMENT = 2;

    [System.Serializable]
    public struct SpawnFreqData
    {
        public GameObject prefab;
        public float startTime;
        public float baseSpawnPerSec;
        public float spawnPerSecIncrease;
        public float maxSpawnPerSec;
    }

    [Header("Spawn Frequency")]
    [SerializeField] SpawnFreqData[] spawnFrequencies;
    float[] spawnChance;

    [Header("Locations")]
    [SerializeField] Transform[] spawnPositions;

    [Header("Wave")]
    [SerializeField] float waveInterval;
    [SerializeField] float waveDuration;

    void Start()
    {
        GameManager.Instance.onGameStart.AddListener(OnGameStart);
        spawnChance = new float[spawnFrequencies.Length];
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.onGameStart.RemoveListener(OnGameStart);
        }
    }

    void Update()
    {
        for (int i = 0; i < spawnFrequencies.Length; i++)
        {
            ProcessSpawnFreq(i);
        }
    }

    private void OnGameStart()
    {
        StartCoroutine(WaveLoop());
    }

    void ProcessSpawnFreq(int id)
    {
        if (GameManager.Instance.gameTime < spawnFrequencies[id].startTime)
            return;

        spawnChance[id] += GetSpawnChanceOfIdForFrame(id);

        if (spawnChance[id] > 1)
        {
            spawnChance[id]--;
            SpawnPrefab(spawnFrequencies[id].prefab);
        }
    }

    float GetSpawnChanceOfIdForFrame(int id)
    {
        float time = GameManager.Instance.gameTime - spawnFrequencies[id].startTime;
        return Mathf.Min(spawnFrequencies[id].maxSpawnPerSec,(spawnFrequencies[id].baseSpawnPerSec + spawnFrequencies[id].spawnPerSecIncrease * time) * Time.deltaTime);
    }

    void SpawnPrefab(GameObject prefab)
    {
        if (!GameManager.Instance.GameIsPlaying)
            return;

        GameObject newEnemy = Instantiate(prefab, null);
        newEnemy.transform.position = GetSpawnPosition();
    }

    public Vector3 GetSpawnPosition()
    {
        return spawnPositions[Random.Range(0, spawnPositions.Length)].position;
    }

    IEnumerator WaveLoop()
    {
        float elapsedTime = 0;
        while (!GameManager.Instance.gameIsOver)
        {
            //Wait for next wave
            while (!GameManager.Instance.gameIsOver && elapsedTime<1)
            {
                if (GameManager.Instance.GameIsPlaying)
                    elapsedTime += Time.deltaTime / waveInterval;

                yield return null;
            }

            yield return StartCoroutine(WaveSpawn());
            elapsedTime = 0;
        }
    }

    IEnumerator WaveSpawn()
    {
        int enemyWaveID = Random.Range(0,spawnFrequencies.Length);
        Debug.Log($"Start wave of enemy ({enemyWaveID}): {spawnFrequencies[enemyWaveID].prefab.name}");
        float t = 0;
        while (t < 1)
        {
            if (GameManager.Instance.GameIsPlaying)
            {
                t += Time.deltaTime / waveDuration;

                //Increase more and more the spawn chance of selected enemy
                spawnChance[enemyWaveID] += GetSpawnChanceOfIdForFrame(enemyWaveID) * Curves.QuadEaseInOut(0, 1, Mathf.Clamp01(t));
            }
            yield return null;
        }
    }
}
