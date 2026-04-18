using UnityEngine;

public class EnemyMatGetter : MonoBehaviour
{
    [SerializeField] MeshRenderer[] renderers;
    Material instancedMaterial;
    public Material InstancedMaterial
    {
        get
        {
            if (instancedMaterial == null)
                InitMat();
            return instancedMaterial;
        }
    }

    void InitMat()
    {
        instancedMaterial = renderers[0].material;
        for (int i = 1; i < renderers.Length; i++)
        {
            renderers[i].material = instancedMaterial;
        }
    }
}
