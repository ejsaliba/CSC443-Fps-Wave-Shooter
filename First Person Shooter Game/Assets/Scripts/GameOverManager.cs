using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager Instance;

    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private TextMeshProUGUI scoreText;

    private bool isGameOver = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        gameOverUI.SetActive(false);
    }

    public void TriggerGameOver()
    {
        isGameOver = true;

        gameOverUI.SetActive(true);

        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (scoreText != null)
        {
            scoreText.text = "Total Score: " + ScoreManager.Instance.totalScore;
        }
    }

    public bool IsGameOver()
    {
        return isGameOver;
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }
}