using UnityEngine;

public class SignalGun : MonoBehaviour
{
    [SerializeField] SignalSource mainSource;
    [SerializeField] SignalSource splitSourceLeft;
    [SerializeField] SignalSource splitSourceRight;
    [SerializeField] SignalSource behindSource;
    [SerializeField] SignalSource leftSource;
    [SerializeField] SignalSource rightSource;

    private void OnEnable()
    {
        PowerUpManager.onAnyUpdate += PowerUpManager_onAnyUpdate;
        PowerUpManager_onAnyUpdate();
    }

    private void OnDisable()
    {
        PowerUpManager.onAnyUpdate -= PowerUpManager_onAnyUpdate;
    }

    private void PowerUpManager_onAnyUpdate()
    {
        //Split
        bool hasSplit = PowerUpManager.Instance.HasPowerUp(PowerUp.Split);
        mainSource.gameObject.SetActive(!hasSplit);
        splitSourceLeft.gameObject.SetActive(hasSplit);
        splitSourceRight.gameObject.SetActive(hasSplit);

        //Behind & around
        bool hasBehind = PowerUpManager.Instance.HasPowerUp(PowerUp.InTheBack);
        bool hasAround = PowerUpManager.Instance.HasPowerUp(PowerUp.HeavyArtillery);
        behindSource.gameObject.SetActive(hasBehind || hasAround);
        leftSource.gameObject.SetActive(hasAround);
        rightSource.gameObject.SetActive(hasAround);
    }
}
