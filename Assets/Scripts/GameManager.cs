using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI do Jogo")]
    public TMP_Text gameOverText;    // Texto de Game Over (TMP)
    public Button restartButton;     // Botão para reiniciar
    public TMP_Text scoreText;       // Exibição da pontuação (TMP)

    private int score = 0;
    private bool gameOver = false;   // Indica se o jogo acabou

    // Propriedade para acesso externo do estado de Game Over
    public bool IsGameOver
    {
        get { return gameOver; }
    }

    void Awake()
    {
        // Implementação do singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Inicialmente, esconde os elementos de Game Over
        if (gameOverText != null)
            gameOverText.gameObject.SetActive(false);
        if (restartButton != null)
            restartButton.gameObject.SetActive(false);
        UpdateScoreText();
    }

    // Atualiza a pontuação e a exibição na tela
    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }

    // Chamada quando o ninja é atingido por uma shuriken
    public void GameOver()
    {
        // Remove todas as shurikens que ainda estão na cena
        GameObject[] shurikens = GameObject.FindGameObjectsWithTag("Shuriken");
        foreach (GameObject s in shurikens)
        {
            Destroy(s);
        }

        // Exibe a UI de Game Over
        if (gameOverText != null)
            gameOverText.gameObject.SetActive(true);
        if (restartButton != null)
        {
            restartButton.gameObject.SetActive(true);
            restartButton.onClick.RemoveAllListeners();
            restartButton.onClick.AddListener(RestartGame);
        }

        // Sinaliza o Game Over para outros scripts
        gameOver = true;
        // Pausa o jogo
        Time.timeScale = 0f;
    }

    // Reinicia a cena atual e reseta o Time.timeScale
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
