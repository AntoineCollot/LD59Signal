using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PowerDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] TextMeshProUGUI description;
    [SerializeField] Image icon;
    [SerializeField] Graphic panel;

    LevelUpDisplay levelUpDisplay;
    ScriptablePower power;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnSelect);
    }

    public void Display(LevelUpDisplay source, ScriptablePower power)
    {
        this.power = power;
        levelUpDisplay = source;

        panel.color = power.color;
        title.text = power.Title;
        description.text = power.description;
        icon.sprite = power.sprite;
    }

    public void OnSelect()
    {
        levelUpDisplay.Select(power);
    }
}
