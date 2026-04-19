using UnityEngine;

public class SignalGun : MonoBehaviour
{
    [SerializeField] SignalSource mainSource;
    [SerializeField] SignalSource splitSourceLeft;
    [SerializeField] SignalSource splitSourceRight;

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
    }
}
