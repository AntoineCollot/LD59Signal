using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour, IHealth
{
    [SerializeField] protected float baseHP;
    [SerializeField] protected Slider slider;
    protected float _currentHealth;

    [SerializeField] protected bool godMode;

    public float MaxHealth => baseHP;
    public float CurrentHealth => _currentHealth;

    //Hit scale
    protected float lastHitTime;
    public const float HIT_SCALE = 0.7f;
    public const float HIT_SCALE_DURATION = 0.3f;

    //Anim
    protected Animator anim;
    protected static readonly int HURT_ANIM = Animator.StringToHash("Hurt");

    protected void Start()
    {
        _currentHealth = MaxHealth;
        anim = GetComponentInChildren<Animator>();

        XPManager.Instance.OnLevelUp += OnLevelUp;
    }

    void OnDestroy()
    {
        if (XPManager.Instance != null)
            XPManager.Instance.OnLevelUp -= OnLevelUp;
    }

    protected void Update()
    {
        slider.value = _currentHealth / MaxHealth;

        float hitTimeDist = 1 - (Time.time - lastHitTime) / HIT_SCALE_DURATION;
        anim.transform.localScale = Vector3.one * Curves.QuadEaseInOut(1, HIT_SCALE, Mathf.Clamp01(hitTimeDist));
    }

    public virtual void Damage(GameObject source, float power, float freq, bool overrideFreq)
    {
#if UNITY_EDITOR
        if (godMode)
            return;
#endif

        ScreenShaker.Instance.MediumShake();
        _currentHealth -= power;
        lastHitTime = Time.time;
        anim.SetTrigger(HURT_ANIM);
        SFXManager.PlaySound(GlobalSFX.PayerDamaged);

        if (_currentHealth < 0)
            Die();
    }

    public virtual void Die()
    {
        ScreenShaker.Instance.LargeShake();
        SFXManager.PlaySound(GlobalSFX.PlayerDeath);
        GameManager.Instance.GameOver();
    }

    private void OnLevelUp()
    {
        Heal(10);
    }

    public void Heal(float amount)
    {
        _currentHealth += amount;
        _currentHealth = Mathf.Min(MaxHealth, _currentHealth);
    }
}
