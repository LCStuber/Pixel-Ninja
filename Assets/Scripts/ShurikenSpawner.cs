using UnityEngine;

public class ShurikenSpawner : MonoBehaviour
{
    public GameObject shurikenPrefab;
    public float spawnRate = 1.5f;
    public float spawnYMin = -2.5f;
    public float spawnYMax = 0f;

    private bool spawnCondition = false;

    private bool bonanzaMode;

    void Start()
    {
        bonanzaMode = GameManager.Instance.currentMode == GameManager.Mode.Bonanza;
        if (bonanzaMode) spawnRate = 0.5f;         // chuva rápida
        InvokeRepeating(nameof(SpawnShuriken), 0f, spawnRate);
    }

    void Update()
    {
        if (GameManager.Instance.IsGameOver)
        {
            CancelInvoke(nameof(SpawnShuriken));
            return;
        }

        if (bonanzaMode)
        {
            float target = Mathf.Max(0.1f, 0.5f - GameManager.Instance.GetCombinedScore() * 0.02f);

            if (target < spawnRate - 0.05f)
            {
                spawnRate = target;
                CancelInvoke(nameof(SpawnShuriken));
                InvokeRepeating(nameof(SpawnShuriken), 0f, spawnRate);
            }
            return;
        }

        int totalScore = GameManager.Instance.GetCombinedScore();
        if (totalScore > 0 && totalScore % 5 == 0 && totalScore < 50 && !spawnCondition)
        {
            spawnCondition = true;
            spawnRate = Mathf.Max(0.1f, spawnRate - 0.07f);
            CancelInvoke(nameof(SpawnShuriken));
            InvokeRepeating(nameof(SpawnShuriken), 0f, spawnRate);
        }
        else if (totalScore % 5 != 0 && spawnCondition)
        {
            spawnCondition = false;
        }
    }

    void SpawnShuriken()
    {
        if (shurikenPrefab == null)
        {
            Debug.LogWarning("shurikenPrefab é nulo. Verifique a referência no Inspector.");
            return;
        }

        if (bonanzaMode)
        {
            float side = Random.value < 0.5f ? -0.5f : 0.5f;
        }

        Vector3 spawnPosition = new Vector3(
            transform.position.x,
            Random.Range(spawnYMin, spawnYMax),
            0f
        );
        Instantiate(shurikenPrefab, spawnPosition, Quaternion.identity);
    }
}
