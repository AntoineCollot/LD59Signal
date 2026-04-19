using UnityEngine;

[CreateAssetMenu(fileName = "ScriptablePower", menuName = "ScriptableObjects/ScriptablePower", order = 2)]
public class ScriptablePower : ScriptableObject
{
    [Header("Effect")]
    public PowerUp powerUpType;
    public PowerStat powerStat;

    [Header("Display")]
    public Color color;
    public Sprite sprite;
    public string description;

    public string Title
    {
        get
        {
            if(powerUpType != PowerUp.None)
                return powerUpType.ToString();

            return powerStat.ToString();
        }
    }
}
