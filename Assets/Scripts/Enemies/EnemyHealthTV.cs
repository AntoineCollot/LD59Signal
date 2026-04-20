using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class EnemyHealthTV : EnemyHealth, IHealth
{
    Material screenMat;
    [SerializeField] Renderer tvBody;

    protected override void Start()
    {
        screenMat = new Material(tvBody.sharedMaterials[1]);

        base.Start();

        tvBody.SetMaterials(new List<Material>() { matGetter.InstancedMaterial, screenMat });
    }

    protected override void UpdateMaterials(float damages, float precision)
    {
        base.UpdateMaterials(damages, precision);

        if (screenMat == null)
            return;

        if (damages > 0)
        {
            screenMat.SetFloat(DAMAGE_TIME_PROPERTY, Time.time);
        }
        screenMat.SetFloat(DAMAGE_PRECISION_PROPERTY, precision);
    }
}
