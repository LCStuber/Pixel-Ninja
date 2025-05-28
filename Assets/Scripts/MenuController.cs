using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [Header("Botões do Menu")]
    public Button jogarButton;       // arraste aqui o BtnJogar
    public Button tutorialButton;    // arraste aqui o BtnTutorial
    public Button multiplayerButton;    // arraste aqui o BtnMulti

    [Header("Build Index das Cenas")]
    public int gameSceneBuildIndex = 1;      // índice da cena de jogo
    public int tutorialSceneBuildIndex = 2;  // índice da cena de tutorial
    public int multiplayerSceneBuildIndex = 3;  // índice da cena de tutorial

    void Start()
    {
        // validação rápida
        if (jogarButton == null)    Debug.LogError("MenuController: 'jogarButton' não atribuído!");
        if (tutorialButton == null) Debug.LogError("MenuController: 'tutorialButton' não atribuído!");
        if (multiplayerButton == null) Debug.LogError("MenuController: 'multiplayerButton' não atribuído!");

        // registra os listeners
        jogarButton.onClick.AddListener(OnPlayClicked);
        tutorialButton.onClick.AddListener(OnTutorialClicked);
        multiplayerButton.onClick.AddListener(OnMultiplayerClicked);
    }

    void OnPlayClicked()
    {
        Debug.Log("Jogar clicado! Carregando cena de jogo...");
        SceneManager.LoadScene(gameSceneBuildIndex);
    }

    void OnTutorialClicked()
    {
        Debug.Log("Tutorial clicado! Carregando cena de tutorial...");
        SceneManager.LoadScene(tutorialSceneBuildIndex);
    }

    void OnMultiplayerClicked()
    {
        Debug.Log("Multiplayer clicado! Carregando cena de multiplayer...");
        SceneManager.LoadScene(multiplayerSceneBuildIndex);
    }
}
