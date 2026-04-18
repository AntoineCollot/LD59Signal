using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    PlayerAim aim;
    [SerializeField] Signal signal;

    void Start()
    {
        aim = GetComponent<PlayerAim>();
    }

    void Update()
    {
        UpdateSignalObj();
    }

    void UpdateSignalObj()
    {
        signal.SetFrequency(1 - aim.AimDist01);
        signal.transform.position = transform.position;
        signal.transform.LookAt(aim.mousePoint);
    }
}
