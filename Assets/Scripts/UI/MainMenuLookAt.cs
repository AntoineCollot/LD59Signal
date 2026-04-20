using UnityEngine;
using UnityEngine.InputSystem;

public class MainMenuLookAt : MonoBehaviour
{
    [SerializeField] float lookRange;
    float baseAngle;

    float angle, refAngle;
    const float SMOOTH = 0.3f;

    void Start()
    {
        baseAngle = transform.localEulerAngles.y;
    }

    void Update()
    {
        float mouseX = Mouse.current.position.ReadValue().x;
        mouseX /= Screen.width;
        float targetAngle = Mathf.Lerp(-lookRange, lookRange, mouseX);

        angle = Mathf.SmoothDampAngle(angle, targetAngle, ref refAngle, SMOOTH);
        transform.rotation = Quaternion.Euler(0, baseAngle+angle, 0);
    }
}
