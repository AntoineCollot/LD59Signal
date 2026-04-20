using UnityEngine;

public class PlayerArea : MonoBehaviour
{
    [SerializeField] float alpha = 0.5f;
    [Header("Areas")]
    [SerializeField] SpriteRenderer maxSprite;
    [SerializeField] SpriteRenderer minSprite;
    Color col;

    void Start()
    {
        col = maxSprite.color;
    }

    void Update()
    {
        Vector2 toMouse = (PlayerAim.Instance.mousePoint - transform.position).ToVector2();
        float angle = -Vector2.SignedAngle(Vector2.up, toMouse);
        transform.rotation = Quaternion.Euler(0, angle, 0);

        //Max
        col.a = Mathf.InverseLerp(0.7f, 1, PlayerAim.Instance.AimDist01) * alpha;
        maxSprite.color = col;

        //Min
        col.a = Mathf.InverseLerp(0.3f, 0f, PlayerAim.Instance.AimDist01) * alpha;
        minSprite.color = col;
    }
}
