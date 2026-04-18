using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour, IHealth
{
    [SerializeField] float baseHP;
    [SerializeField] Slider slider;
    float _currentHealth;

    public float MaxHealth => baseHP;
    public float CurrentHealth => _currentHealth;

    //Hit scale
    float lastHitTime;
    const float HIT_SCALE = 0.7f;
    const float HIT_SCALE_DURATION = 0.3f;

    //Anim
    Animator anim;
    static readonly int HURT_ANIM = Animator.StringToHash("Hurt");

    void Start()
    {
        _currentHealth = MaxHealth;
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        slider.value = _currentHealth / MaxHealth;

        float hitTimeDist = 1 - (Time.time - lastHitTime) / HIT_SCALE_DURATION;
        anim.transform.localScale = Vector3.one * Curves.QuadEaseInOut(1, HIT_SCALE, Mathf.Clamp01(hitTimeDist));
    }

    public void Damage(GameObject source, float power, float freq)
    {
        _currentHealth -= power;
        lastHitTime = Time.time;
        anim.SetTrigger(HURT_ANIM);
        if (_currentHealth < 0)
            Die();
    }

    public void Die()
    {
        Debug.Log("Game Over!");
    }

    public void Heal(float amount)
    {
        throw new System.NotImplementedException();
    }

}
