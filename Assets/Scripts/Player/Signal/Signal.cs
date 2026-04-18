using UnityEngine;

public class Signal : MonoBehaviour
{
    static readonly int FREQUENCY_PARAM = Shader.PropertyToID("_Frequency");
    static readonly int TIME_PARAM = Shader.PropertyToID("_SignalTime");
    static readonly int TRIANGULIZE_PARAM = Shader.PropertyToID("_Triangulize");
    static readonly int SPEED_PARAM = Shader.PropertyToID("_Speed");

    //Freq
    const float MIN_FREQ = 3;
    const float MAX_FREQ = 30;

    Material instancedMat;
    SpriteRenderer spriteRenderer;

    //Current State
    float signalTime;
    float frequency;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        instancedMat = spriteRenderer.material;
    }

    void Update()
    {
        signalTime += Time.deltaTime;
        instancedMat.SetFloat(TIME_PARAM, signalTime);
        instancedMat.SetFloat(FREQUENCY_PARAM, Mathf.Lerp(MIN_FREQ, MAX_FREQ, frequency));
        instancedMat.SetFloat(TRIANGULIZE_PARAM, 0);
        instancedMat.SetFloat(SPEED_PARAM, -30);
    }

    public void SetFrequency(float frequency)
    {
        this.frequency = frequency;
    }
}
