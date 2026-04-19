using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XPManager : MonoBehaviour
{
    [Header("Levels")]
    [SerializeField] int baseLevelXPAmount = 10;
    int previousLevelXP = 0;
    int nextLevelXP;
    public int currentLevel { get; private set; }
    public int totalXP { get; private set; }
    public float XPProgress => Mathf.InverseLerp(previousLevelXP, nextLevelXP, totalXP);

    [SerializeField] Spark sparkPrefab;
    [SerializeField] float spawnAreaSize;
    Queue<Spark> sparkPool;

    [Header("UI")]
    [SerializeField] Slider xpSlider;
    [SerializeField] LevelUpDisplay levelUpDisplay;

    public static XPManager Instance;

    void Awake()
    {
        Instance = this;
        sparkPool = new();

        currentLevel = 0;
        nextLevelXP = Fib(1); //Start at 1 for fibonacci

        levelUpDisplay.Hide();
    }

    public void SpawnSparks(Vector3 pos, int amount = 1)
    {
        pos.y = 0;
        Spark spark;
        for (int i = 0; i < amount; i++)
        {
            spark = GetSpark();

            if (amount > 1)
                spark.transform.position = GetRandomPosInArea(pos);
            else
                spark.transform.position = pos;
        }
    }

    Spark GetSpark()
    {
        if (sparkPool.TryDequeue(out Spark spark))
        {
            spark.gameObject.SetActive(true);
            return spark;
        }

        return Instantiate(sparkPrefab, transform);
    }

    Vector3 GetRandomPosInArea(Vector3 center)
    {
        Vector2 rand = Random.insideUnitCircle * spawnAreaSize;
        return rand.ToVector3() + center;
    }

    public void SparkPickedUp(Spark spark)
    {
        totalXP++;
        if (totalXP >= nextLevelXP)
            LevelUp();

        xpSlider.value = XPProgress;

        sparkPool.Enqueue(spark);
    }

    void LevelUp()
    {
        currentLevel++;
        previousLevelXP = nextLevelXP;
        nextLevelXP += Fib(currentLevel + 1); //Start at 1 for fibonacci
        Debug.Log($"Level Up! Next Level {nextLevelXP}");

        levelUpDisplay.Display();
    }

    static int Fib(int x)
    {
        if (x == 0) return 0;

        int prev = 0;
        int next = 1;
        for (int i = 1; i < x; i++)
        {
            int sum = prev + next;
            prev = next;
            next = sum;
        }
        return next;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, spawnAreaSize);
    }
#endif
}
