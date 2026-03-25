using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int score = 0;
    public TextMeshProUGUI scoreText;
    public GameObject gameOverPanel;
    public TextMeshProUGUI finalScoreText;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        UpdateScore();
        gameOverPanel.SetActive(false);
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScore();
    }

    void UpdateScore()
    {
        scoreText.text = "Score: " + score;
    }

    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
        finalScoreText.text = "Score: " + score;
        Time.timeScale = 0f;
    }

    public void NewGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}