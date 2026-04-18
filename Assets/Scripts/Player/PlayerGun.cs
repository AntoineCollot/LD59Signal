using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    PlayerAim aim;
    [SerializeField] Signal signal;

    void Start()
    {
        aim = GetComponent<PlayerAim>();
    }

    // Update is called once per frame
    void Update()
    {
        signal.SetFrequency(1-aim.AimDist01);
        signal.transform.position = transform.position;
        signal.transform.LookAt(aim.mousePoint);
    }
}
