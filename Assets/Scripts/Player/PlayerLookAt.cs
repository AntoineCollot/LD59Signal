using UnityEngine;

public class PlayerLookAt : MonoBehaviour
{
    float lookAtAngle, refLookAtAngle;
    const float LOOK_AT_SMOOTH = 0.1f;
    EnemyAttack attack;

    void Start()
    {
    }

    void Update()
    {
        Vector3 toCursor = PlayerAim.Instance.mousePoint - transform.position;

        float targetAngle = -Vector2.SignedAngle(Vector2.up, toCursor.ToVector2());
        lookAtAngle = Mathf.SmoothDampAngle(lookAtAngle, targetAngle, ref refLookAtAngle, LOOK_AT_SMOOTH);
        transform.rotation = Quaternion.Euler(0, lookAtAngle, 0);
    }
}
