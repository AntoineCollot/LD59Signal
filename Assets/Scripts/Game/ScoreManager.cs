using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int highScore { get; private set; }
    public int currentScore { get; private set; }

    public const string SCORE_KEY = "HighScore";
    public static ScoreManager Instance;

    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GameManager.Instance.onGameOver.AddListener(OnGameOver);
        highScore = PlayerPrefs.GetInt(SCORE_KEY, 0);
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.onGameOver.RemoveListener(OnGameOver);
        }
    }

    private void OnGameOver()
    {
        SaveScore();
    }

    public void AddScore(int amount = 1)
    {
        currentScore += amount;
    }

    void SaveScore()
    {
        if (currentScore > highScore)
        {
            highScore = currentScore;
            PlayerPrefs.SetInt(SCORE_KEY, currentScore);
            PlayerPrefs.Save();
        }
    }
}
