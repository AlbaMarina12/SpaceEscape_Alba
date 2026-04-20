using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

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
        gameOverPanel.SetActive(false);
        UpdateScore();

        // Espera a que Firebase esté listo antes de cargar el score guardado
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

        // Guardar score en Firebase
        if (FirebaseManager.Instance != null && FirebaseManager.Instance.IsFirebaseReady)
        {
            FirebaseManager.Instance.totalCoins = score;
            FirebaseManager.Instance.SaveProgress();
        }
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