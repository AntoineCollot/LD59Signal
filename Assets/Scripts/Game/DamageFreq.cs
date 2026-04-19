using UnityEngine;

[System.Serializable]
public struct DamageFreq
{
    [Range(0, 1)] public float center;
    [Range(0, 1)] public float perfectWidth;
    [Range(0, 1)] public float range;

    public float GetFrequencyPrecision(float freq)
    {
        float precision = (Mathf.Abs(center - freq) - perfectWidth) / (range+perfectWidth);
        return Mathf.Clamp01(1 - precision);
    }
}
