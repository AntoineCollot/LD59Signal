using UnityEngine;

public class WaterProjectile : MonoBehaviour
{
    [SerializeField] float damages;
    [SerializeField] float speed;
    [SerializeField] Sprite[] sprites;
    SpriteRenderer rend;

    Rigidbody body;
    float spawnTime;
    bool isActive;

    const float LIFETIME = 5;
    const float FRAMERATE = 8;

    private void Awake()
    {
        rend = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isActive && other.TryGetComponent(out IHealth health))
        {
            health.Damage(gameObject, damages, 0, true);
            isActive = false;
            WaterProjectilePool.Instance.ReturnToPool(this);
        }
    }

    private void Update()
    {
        if (isActive && GameManager.Instance.gameTime > spawnTime + LIFETIME)
        {
            isActive = false;
            WaterProjectilePool.Instance.ReturnToPool(this);
        }

        rend.sprite = sprites[Mathf.FloorToInt(Time.time * FRAMERATE) % sprites.Length];
    }

    public void Emit()
    {
        if (body == null)
            body = GetComponent<Rigidbody>();

        isActive = true;
        body.linearVelocity = Random.insideUnitCircle.normalized.ToVector3() * speed;
        spawnTime = GameManager.Instance.gameTime;
    }
}
