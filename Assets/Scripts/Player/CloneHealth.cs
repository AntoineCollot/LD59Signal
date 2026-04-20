using UnityEngine;

public class CloneHealth : MonoBehaviour, IHealth
{
    PlayerClone clone;
    public float MaxHealth => 3;
    float _currentHealth;
    public float CurrentHealth => _currentHealth;

    //Hit scale
    protected float lastHitTime;

    //Anim
    protected Animator anim;
    protected static readonly int HURT_ANIM = Animator.StringToHash("Hurt");
    float baseScale;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        baseScale = anim.transform.localScale.x;
        clone = GetComponentInParent<PlayerClone>();
    }

    void OnEnable()
    {
        _currentHealth = MaxHealth;
    }

    void Update()
    {
        float hitTimeDist = 1 - (Time.time - lastHitTime) / PlayerHealth.HIT_SCALE_DURATION;
        anim.transform.localScale = Vector3.one * baseScale * Curves.QuadEaseInOut(1, PlayerHealth.HIT_SCALE, Mathf.Clamp01(hitTimeDist));
    }

    public void Damage(GameObject source, float power, float freq, bool overrideFreq)
    {
        _currentHealth -= power;
        lastHitTime = Time.time;
        anim.SetTrigger(HURT_ANIM);

        if (_currentHealth < 0)
            Die();
    }

    public void Die()
    {
        ScreenShaker.Instance.MediumShake();
        clone.DisableClone();
    }

    public void Heal(float amount)
    {
        throw new System.NotImplementedException();
    }
}
