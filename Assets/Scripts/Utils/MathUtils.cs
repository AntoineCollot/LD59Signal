using UnityEngine;

public static class MathUtils
{
    /// <summary>
    /// Inverse square function.
    /// Gives very high values when entry gets close to 0.
    /// </summary>
    /// <param name="x">Distance to target</param>
    /// <param name="s">Softness factor</param>
    public static float Inv(float x, float s)
    {
        //Avoid dividing by zero using espilon
        float value = x / s + Mathf.Epsilon;

        //Do not use the pow function since it can be quite expensive
        return 1 / (value * value);
    }
}