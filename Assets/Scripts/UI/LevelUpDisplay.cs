using UnityEngine;

public class LevelUpDisplay : MonoBehaviour
{
    [SerializeField] PowerDisplay[] displays;

    public void Display()
    {
        gameObject.SetActive(true);
        GameManager.Instance.Pause();

        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        for (int i = 0; i < PowerUpManager.POWER_PER_LEVEL; i++)
        {
            ScriptablePower power = PowerUpManager.Instance.GetPowerForLevel(XPManager.Instance.currentLevel);
            displays[i].Display(this,power);
        }
    }

    public void Select(ScriptablePower power)
    {
        PowerUpManager.Instance.ObtainPower(power);
        Hide();
    }

    public void Hide()
    {
        GameManager.Instance.Resume();
        gameObject.SetActive(false);
    }
}
