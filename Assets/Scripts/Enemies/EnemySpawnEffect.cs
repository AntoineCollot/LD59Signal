using System.Collections;
using UnityEngine;

public class EnemySpawnEffect : MonoBehaviour
{
    [SerializeField] float animTime = 1;
    EnemyMatGetter matGetter;

    static readonly int SPAWN_PARAM = Shader.PropertyToID("_Spawn");

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        matGetter = GetComponent<EnemyMatGetter>();
        StartCoroutine(SpawnEffect());
    }

    IEnumerator SpawnEffect()
    {
        float t = 0;
        while(t<1)
        {
            t += Time.deltaTime / animTime;

            matGetter.InstancedMaterial.SetFloat(SPAWN_PARAM, Curves.QuadEaseInOut(0, 1, Mathf.Clamp01(t)));

            yield return null;
        }
    }
}
