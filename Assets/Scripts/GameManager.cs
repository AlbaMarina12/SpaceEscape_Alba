using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Score")]
    public int score = 0;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI finalScoreText;

    [Header("UI")]
    public GameObject gameOverPanel;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Time.timeScale = 1f;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        UpdateScore();

        StartCoroutine(WaitForFirebaseAndLoad());
    }

    IEnumerator WaitForFirebaseAndLoad()
    {
        while (FirebaseManager.Instance == null || !FirebaseManager.Instance.IsFirebaseReady)
        {
            yield return null;
        }

        score = FirebaseManager.Instance.totalCoins;
        UpdateScore();
        Debug.Log("Score cargado desde Firebase: " + score);
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScore();

        if (FirebaseManager.Instance != null && FirebaseManager.Instance.IsFirebaseReady)
        {
            FirebaseManager.Instance.totalCoins = score;
            FirebaseManager.Instance.SaveProgress();
        }
    }

    void UpdateScore()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }

    public void ShowGameOver()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        if (finalScoreText != null)
        {
            finalScoreText.text = "Score: " + score;
        }

        Time.timeScale = 0f;
    }

    public void NewGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}