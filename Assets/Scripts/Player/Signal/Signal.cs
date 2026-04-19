using UnityEngine;

public class Signal : MonoBehaviour
{
    static readonly int FREQUENCY_PARAM = Shader.PropertyToID("_Frequency");
    static readonly int TIME_PARAM = Shader.PropertyToID("_SignalTime");
    static readonly int TRIANGULIZE_PARAM = Shader.PropertyToID("_Triangulize");
    static readonly int SPEED_PARAM = Shader.PropertyToID("_Speed");

    //Freq
    const float MIN_FREQ = 2;
    const float MAX_FREQ = 30;

    //Current State
    float signalTime;
    float frequency;
    public float power = 1;
    public float signalLength = 20;
    public float signalWidth = 2;
    public float speed = 2;

    //Hit
    Collider[] hits;
    int currentHitCount;
    const int MAX_HIT = 100;

    [Header("Renderer")]
    SpriteRenderer spriteRenderer;
    Material instancedMat;
    float signalRendererHeight;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hits = new Collider[MAX_HIT];
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        instancedMat = spriteRenderer.material;
        signalRendererHeight = spriteRenderer.transform.position.y;
    }

    void Update()
    {
        signalTime += Time.deltaTime * speed * Mathf.Lerp(MIN_FREQ, MAX_FREQ * 0.5f, frequency);

        spriteRenderer.transform.localScale = new Vector3(signalLength, signalWidth * 2, 1);
        spriteRenderer.transform.localPosition = new Vector3(0, signalRendererHeight, signalLength * 0.5f);

        ApplyDamages();
        UpdateMaterial();
    }

    void UpdateMaterial()
    {
        instancedMat.SetFloat(TIME_PARAM, signalTime);
        instancedMat.SetFloat(FREQUENCY_PARAM, Mathf.Lerp(MIN_FREQ, MAX_FREQ, frequency));
        instancedMat.SetFloat(TRIANGULIZE_PARAM, 0);
    }

    void ApplyDamages()
    {
        if (!GameManager.Instance.GameIsPlaying)
            return;

        Vector3 sourcePos = transform.position;
        sourcePos.y = 0;
        Vector3 sphere0 = sourcePos + transform.forward * signalWidth;
        Vector3 sphere1 = sourcePos + transform.forward * (signalLength - signalWidth);

        currentHitCount = Physics.OverlapCapsuleNonAlloc(sphere0,
            sphere1,
            signalWidth,
            hits,
            LayerUtils.ENEMY_LAYER_MASK);

        Debug.DrawLine(sphere0, sphere1, Color.green);

        float effectivePower = StatsManager.Instance.ApplyPassDamageIncrease(power, frequency);
        for (int i = 0; i < currentHitCount; i++)
        {
            if (hits[i].TryGetComponent(out IHealth health))
            {
                health.Damage(gameObject, effectivePower, frequency);
            }
        }
    }

    public void SetFrequency(float frequency)
    {
        this.frequency = Curves.QuintEaseIn(0, 1, frequency);
    }
}
