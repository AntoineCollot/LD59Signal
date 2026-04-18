using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-100)] //Before player state init
public class EnemyTargetManager : MonoBehaviour
{
    HashSet<Transform> targets;

    public static EnemyTargetManager Instance;

    private void Awake()
    {
        Instance = this;
        targets = new HashSet<Transform>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void RegisterTarget(Transform target)
    {
        targets.Add(target);
    }

    public void DeregisterTarget(Transform target)
    {
        targets.Remove(target);
    }

    public Transform GetTargetForPos(Vector2 pos)
    {
        Transform closestTarget = null;
        float minDist = Mathf.Infinity;
        foreach (Transform target in targets)
        {
            float dist = Vector2.Distance(target.position.ToVector2(), pos);
            if(dist < minDist)
            {
                minDist = dist;
                closestTarget = target;
            }
        }

        return closestTarget;
    }
}
