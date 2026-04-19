using UnityEngine;

public class SignalSource : MonoBehaviour
{
    [SerializeField] Signal mainSignal;
    [SerializeField] Signal oppositeSignal;
    float offsetAngle;

    private void Awake()
    {
        offsetAngle = transform.eulerAngles.y;
    }

    private void OnEnable()
    {
        PowerUpManager.onAnyUpdate += PowerUpManager_onAnyUpdate;
        if (PowerUpManager.Instance != null)
            PowerUpManager_onAnyUpdate();
    }

    private void OnDisable()
    {
        PowerUpManager.onAnyUpdate -= PowerUpManager_onAnyUpdate;
    }

    private void PowerUpManager_onAnyUpdate()
    {
        if (oppositeSignal != null)
            oppositeSignal.gameObject.SetActive(PowerUpManager.Instance.HasPowerUp(PowerUp.OppositeWave));
    }

    void Update()
    {
        transform.position = transform.position;
        transform.LookAt(PlayerAim.Instance.mousePoint);
        transform.Rotate(Vector3.up * offsetAngle, Space.World);

        UpdateSignal(mainSignal);
    }

    void UpdateSignal(Signal signal)
    {
        signal.SetFrequency(1 - PlayerAim.Instance.AimDist01);
    }
}
