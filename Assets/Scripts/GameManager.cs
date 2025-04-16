using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI do Jogo")]
    public TMP_Text gameOverText;    
    public Button restartButton;    
    public TMP_Text scoreText;       
    public TMP_Text lifeText;       
    public TMP_Text lifeVillageText;       

    public int score = 0;
           
    private bool gameOver = false;   

    public bool IsGameOver
    {
        get { return gameOver; }
    }

    void Awake()
    {
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
        if (gameOverText != null)
            gameOverText.gameObject.SetActive(false);
        if (restartButton != null)
            restartButton.gameObject.SetActive(false);
        UpdateScoreText();
        UpdateLife(3);
        UpdateVillageLife(5);
    }

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

    public void UpdateLife(int life)
    {
        lifeText.text = ""; 
        for (int i = 0; i < life; i++)
        {
            if (lifeText != null)
                lifeText.text += "♥";
        }
    }

    public void UpdateVillageLife(int life)
    {
        lifeVillageText.text = ""; 
        for (int i = 0; i < life; i++)
        {
            if (lifeVillageText != null)
                lifeVillageText.text += "♥";
        }
    }

    
    public void GameOver()
    {
        GameObject[] shurikens = GameObject.FindGameObjectsWithTag("Shuriken");
        foreach (GameObject s in shurikens)
        {
            Destroy(s);
        }

        if (gameOverText != null)
            gameOverText.gameObject.SetActive(true);
        if (restartButton != null)
        {
            restartButton.gameObject.SetActive(true);
            restartButton.onClick.RemoveAllListeners();
            restartButton.onClick.AddListener(RestartGame);
        }

        gameOver = true;
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
