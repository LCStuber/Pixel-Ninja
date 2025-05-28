using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class TutorialManager : MonoBehaviour
{
    public NinjaController ninja;
    public GameObject shurikenPrefab;
    public Transform shurikenSpawnPoint;
    public TMP_Text instructionText;
    public TMP_Text scoreText;
    public Button backButton;

    enum Step { Move, Attack, Jump, Combat, Done }
    Step currentStep = Step.Move;

    int tutorialScore = 0;

    void Start()
    {
        ninja.tutorialMode = true;
        backButton.gameObject.SetActive(false);
        backButton.onClick.AddListener(() =>
            SceneManager.LoadScene("MainMenu")
        );

        UpdateInstruction();
        UpdateScoreUI();
    }

    void Update()
    {
        switch (currentStep)
        {
            case Step.Move:
                if (Mathf.Abs(Input.GetAxisRaw("Horizontal2")) > 0.1f)
                    NextStep();
                break;
            case Step.Attack:
                if (Input.GetKeyDown(KeyCode.Z))
                    NextStep();
                break;
            case Step.Jump:
                if (Input.GetButtonDown("Jump2"))
                    NextStep();
                break;
        }
    }

    void NextStep()
    {
        currentStep++;
        UpdateInstruction();

        if (currentStep == Step.Combat)
            StartCoroutine(CombatPhase());
    }

    void UpdateInstruction()
    {
        switch (currentStep)
        {
            case Step.Move:
                instructionText.text = "Use A e D para SE MOVER";
                break;
            case Step.Attack:
                instructionText.text = "Pressione Z para ATACAR";
                break;
            case Step.Jump:
                instructionText.text = "Pressione W para PULAR";
                break;
            case Step.Combat:
                instructionText.text = "Defenda‑se! Marque 5 pontos";
                break;
            case Step.Done:
                instructionText.text = "Tutorial Concluído!";
                backButton.gameObject.SetActive(true);
                break;
        }
    }

    IEnumerator CombatPhase()
    {
        while (tutorialScore < 5)
        {
            SpawnShuriken();
            yield return new WaitForSeconds(2f);
        }
        currentStep = Step.Done;
        UpdateInstruction();
    }

    void SpawnShuriken()
    {
        var obj = Instantiate(shurikenPrefab, shurikenSpawnPoint.position, Quaternion.identity);
        var sh = obj.GetComponent<Shuriken>();
        sh.tutorialMode = true;           // liga modo tutorial na shuriken
        sh.tutorialManager = this;        // passa o manager
    }

    // chamado pela Shuriken quando o ninja ataca ela
    public void OnTutorialShurikenHit()
    {
        tutorialScore++;
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        scoreText.text = "Score: " + tutorialScore;
    }
}
