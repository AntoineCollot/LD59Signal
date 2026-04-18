using TMPro;
using UnityEngine;

public class DamageNumber : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float lifetime;
    TextMeshProUGUI text;
    float elapsedTime;

    const float SPAWN_OFFSET = 0.5f;

    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;
        elapsedTime += Time.deltaTime / lifetime;

        if (elapsedTime >= 1)
        {
            ReturnToPool();
            return;
        }

        elapsedTime = Mathf.Clamp01(elapsedTime);
        text.alpha = Curves.QuadEaseInOut(1, 0, elapsedTime);
    }

    public void Display(Vector3 hitPos, float value)
    {
        transform.position = hitPos + Vector3.up * SPAWN_OFFSET;

        if (value < 1)
        {
            value *= 10;
            text.text = "."+value.ToString("N0");
        }
        else
            text.text = value.ToString("N0");

        elapsedTime = 0;
        gameObject.SetActive(true);
    }

    void ReturnToPool()
    {
        gameObject.SetActive(false);
        FXManager.Instance.ReturnDamageToPool(this);
    }
}
