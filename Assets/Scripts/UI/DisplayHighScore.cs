using TMPro;
using UnityEngine;

public class DisplayHighScore : MonoBehaviour
{
    private void OnEnable()
    {
        int highScore = PlayerPrefs.GetInt(ScoreManager.SCORE_KEY, 0);
        GetComponent<TextMeshProUGUI>().text = $"Best Score: {highScore}";
    }
}
