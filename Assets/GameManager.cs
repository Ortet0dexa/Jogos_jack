using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("UI")]
    public GameObject gameOverPanel;   // Painel de Game Over
    public TMP_Text scoreTextUI;       // Texto de pontuação

    [Header("Configurações do jogo")]
    public float scoreMultiplier = 1f; // Pontos por segundo

    private float score;
    private bool isGameOver;

    void Awake()
    {
        // Singleton simples
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        score = 0;
        isGameOver = false;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        UpdateScoreUI();
    }

    void Update()
    {
        if (isGameOver) return;

        score += Time.deltaTime * scoreMultiplier;
        UpdateScoreUI();
    }

    // ================= GAME OVER =================
    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        // Atualiza a pontuação final no painel
        UpdateScoreUI();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void UpdateScoreUI()
    {
        if (scoreTextUI != null)
            scoreTextUI.text = "Pontos: " + Mathf.FloorToInt(score);
    }

    public int GetScore()
    {
        return Mathf.FloorToInt(score);
    }
}
