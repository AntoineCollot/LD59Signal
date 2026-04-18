using UnityEngine;

public class PlayerPowerUps : MonoBehaviour
{
    [SerializeField] float baseDamageTickInterval;
    public float DamageTickInterval => baseDamageTickInterval;

    public float PickUpDistanceMult => 1;

    public static PlayerPowerUps Instance;

    void Awake()
    {
        Instance = this;
    }
}
