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
    [SerializeField] bool inverse;
    float signalTime;
    float frequency;
    public float power = 1;
    public float signalLength = 20;
    public float signalWidth = 2;
    public float speed = 2;

    //Triangle wave
    float endTriangleWaveTime;
    const float TRIANGLE_WAVE_DURATION = 2;
    const float TRIANGLE_WAVE_CHANCE = 0.1f;
    bool IsUsingTriangleWave => GameManager.Instance.gameTime < endTriangleWaveTime;

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

    private void OnEnable()
    {
        XPManager.Instance.OnEnemyKilled += OnEnemyKilled;
    }

    private void OnDisable()
    {
        if (XPManager.Instance != null)
            XPManager.Instance.OnEnemyKilled -= OnEnemyKilled;
    }

    void Update()
    {
        float triangleMult = 1;
        if (IsUsingTriangleWave)
            triangleMult = 0.25f;
        signalTime += Time.deltaTime * speed * Mathf.Lerp(MIN_FREQ, MAX_FREQ * 0.5f, frequency) * triangleMult;

        float amplitude = GetAmplitude() * 2;

        spriteRenderer.transform.localScale = new Vector3(signalLength, amplitude, 1);
        spriteRenderer.transform.localPosition = new Vector3(0, signalRendererHeight, signalLength * 0.5f);
        spriteRenderer.enabled = GameManager.Instance.GameIsPlaying;

        ApplyDamages();
        UpdateMaterial();
    }

    private void OnEnemyKilled()
    {
        //Triangle wave
        if (PowerUpManager.Instance.HasPowerUp(PowerUp.TriangleWave))
        {
            //Random chance
            if (StatsManager.Instance.RunRandomChance(TRIANGLE_WAVE_CHANCE))
            {
                endTriangleWaveTime = GameManager.Instance.gameTime + TRIANGLE_WAVE_DURATION;
            }
        }
    }

    void UpdateMaterial()
    {
        instancedMat.SetFloat(TIME_PARAM, signalTime);
        instancedMat.SetFloat(FREQUENCY_PARAM, Mathf.Lerp(MIN_FREQ, MAX_FREQ, Curves.QuintEaseIn(0, 1, frequency)));
        if (IsUsingTriangleWave)
            instancedMat.SetFloat(TRIANGULIZE_PARAM, 1);
        else
            instancedMat.SetFloat(TRIANGULIZE_PARAM, 0);
    }

    void ApplyDamages()
    {
        if (!GameManager.Instance.GameIsPlaying)
            return;

        Vector3 sourcePos = transform.position;
        sourcePos.y = 0;
        float amplitude = GetAmplitude();
        Vector3 sphere0 = sourcePos + transform.forward * amplitude;
        Vector3 sphere1 = sourcePos + transform.forward * (signalLength - amplitude);

        currentHitCount = Physics.OverlapCapsuleNonAlloc(sphere0,
            sphere1,
            amplitude,
            hits,
            LayerUtils.ENEMY_LAYER_MASK);

        Debug.DrawLine(sphere0, sphere1, Color.green);

        //Power
        float effectivePower = StatsManager.Instance.ApplyPassDamageIncrease(power, frequency);
        if (PowerUpManager.Instance.HasPowerUp(PowerUp.Precision))
        {
            effectivePower *= 2f;
        }
        if (IsUsingTriangleWave)
            effectivePower *= 2;

        for (int i = 0; i < currentHitCount; i++)
        {
            if (hits[i].TryGetComponent(out IHealth health))
            {
                health.Damage(gameObject, effectivePower, frequency, false);
            }
        }
    }

    public void SetFrequency(float frequency)
    {
        this.frequency = frequency;
        if (inverse)
            this.frequency = 1 - frequency;
    }

    public float GetAmplitude()
    {
        float amplitude = signalWidth;
        if (PowerUpManager.Instance.HasPowerUp(PowerUp.AmplitudeMax))
        {
            amplitude *= 2;
        }
        if (PowerUpManager.Instance.HasPowerUp(PowerUp.Precision))
        {
            amplitude *= 0.5f;
        }

        return amplitude;
    }
}
