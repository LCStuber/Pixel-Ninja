using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public enum Mode { Classic, Tutorial, Multiplayer, Bonanza }

    [Header("Modo de Jogo")]
    public Mode currentMode = Mode.Classic;

    [Header("Bonanza")]
    public float bonanzaDuration = 60f;
    public TMP_Text bonanzaTimerText;
    private float bonanzaTimer;

    [Header("UI dos Jogadores (arrays, tamanho = número de ninjas)")]
    public TMP_Text[] scoreTexts;
    public TMP_Text[] lifeTexts;

    [Header("UI da Vila (cooperativa)")]
    public TMP_Text villageLifeText;

    [Header("UI de Fim de Jogo")]
    public TMP_Text gameOverText;
    public Button restartButton;

    [Header("Configurações Iniciais")]
    public int initialLife = 3;
    public int initialVillageLife = 5;

    private int[] scores;
    private int[] lives;
    private int villageLife;
    private bool gameOver = false;
    public bool IsGameOver => gameOver;
    public Button backButton;

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void Awake()
    {
        // Singleton
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        // só inicializamos arrays de jogadores (score/life)
        int playerCount = scoreTexts.Length;
        scores = new int[playerCount];
        lives = Enumerable.Repeat(initialLife, playerCount).ToArray();

        // vila única
        villageLife = initialVillageLife;
    }

    void Start()
    {
        if (currentMode == Mode.Bonanza)
        {
            // sem vila e sem corações de vida
            if (villageLifeText) villageLifeText.gameObject.SetActive(false);
            foreach (var lt in lifeTexts) lt.gameObject.SetActive(false);

            bonanzaTimer = bonanzaDuration;
            bonanzaTimerText.gameObject.SetActive(true);
        }
        else
        {
            if (bonanzaTimerText) bonanzaTimerText.gameObject.SetActive(false);
        }
        // esconde UI de Game Over
        if (gameOverText) gameOverText.gameObject.SetActive(false);
        if (restartButton) restartButton.gameObject.SetActive(false);
        if (backButton) backButton.gameObject.SetActive(false);

        // inicializa textos dos jogadores
        for (int i = 0; i < scores.Length; i++)
        {
            UpdateScoreText(i);
            UpdateLifeText(i);
        }
        // inicializa vida da vila
        UpdateVillageLife(villageLife);
    }

    void Update()
    {
        if (currentMode == Mode.Bonanza && !gameOver)
        {
            bonanzaTimer -= Time.deltaTime;
            bonanzaTimerText.text = $"Tempo: {bonanzaTimer:0}s";

            if (bonanzaTimer <= 0f)
                FinishBonanza();
        }
    }

    void FinishBonanza()
    {
        gameOverText.text = $"FIM DO BONANZA!\nPontuação total: {GetCombinedScore()}";
        GameOver();
    }


    // → Chamado pelo NinjaController quando vence um inimigo
    public void AddScore(int playerId, int amount)
    {
        if (playerId < 0 || playerId >= scores.Length) return;
        scores[playerId] += amount;
        UpdateScoreText(playerId);
    }

    void UpdateScoreText(int playerId)
    {
        scoreTexts[playerId].text = $"P{playerId + 1} Score: {scores[playerId]}";
    }

    // → Chamado pelo NinjaController quando toma dano
    public void UpdateLife(int playerId, int newLife)
    {
        if (currentMode != Mode.Bonanza && newLife <= 0) PlayerDied(playerId);

        if (playerId < 0 || playerId >= lives.Length) return;
        lives[playerId] = newLife;
        UpdateLifeText(playerId);

        if (newLife <= 0)
            PlayerDied(playerId);
    }

    void UpdateLifeText(int playerId)
    {
        if (currentMode != Mode.Bonanza)
        lifeTexts[playerId].text = string.Concat(Enumerable.Repeat("♥", lives[playerId]));
    }

    // → Chamado pela VillageController quando a vila toma dano
    public void UpdateVillageLife(int newLife)
    {
        villageLife = newLife;
        if (currentMode != Mode.Bonanza && villageLife <= 0) GameOver();
        if (villageLifeText)
            villageLifeText.text = string.Concat(Enumerable.Repeat("♥", villageLife));

        if (villageLife <= 0)
            GameOver();
    }

    // usado pelo ShurikenSpawner para escalar dificuldade
    public int GetCombinedScore()
    {
        return scores.Sum();
    }

    // quando um ninja morre
    public void PlayerDied(int playerId)
    {
        // destrói apenas o ninja daquele player
        var ninja = FindObjectsOfType<NinjaController>()
                       .FirstOrDefault(n => n.playerId == playerId);
        if (ninja) Destroy(ninja.gameObject);

        // se não sobrar nenhum vivo → game over
        bool anyAlive = FindObjectsOfType<NinjaController>()
                           .Any(n => n.life > 0);

        if (currentMode != Mode.Bonanza && !anyAlive) GameOver();

        if (!anyAlive)
            GameOver();
    }

    // expõe publicamente o fim de jogo
    public void GameOver()
    {
        // limpa shurikens
        foreach (var s in GameObject.FindGameObjectsWithTag("Shuriken"))
            Destroy(s);

        if (gameOverText) gameOverText.gameObject.SetActive(true);
        if (restartButton)
        {
            restartButton.gameObject.SetActive(true);
            restartButton.onClick.RemoveAllListeners();
            // agora chama o método que despausa antes de recarregar
            restartButton.onClick.AddListener(RestartGame);
        }

        if (restartButton)
        {
            backButton.gameObject.SetActive(true);
            backButton.onClick.RemoveAllListeners();
            backButton.onClick.AddListener(() => { 
                Time.timeScale = 1f;
                SceneManager.LoadScene("MainMenu");
            }
            );
        }

        gameOver = true;
        Time.timeScale = 0f;
    }
}
