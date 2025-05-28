using UnityEngine;

public class ShurikenSpawner : MonoBehaviour
{
    public GameObject shurikenPrefab;
    public float spawnRate = 1.5f;
    public float spawnYMin = -2.5f;
    public float spawnYMax = 0f;

    private bool spawnCondition = false;

    void Start()
    {
        InvokeRepeating(nameof(SpawnShuriken), 0f, spawnRate);
    }

    void Update()
    {
        if (GameManager.Instance.IsGameOver)
        {
            CancelInvoke(nameof(SpawnShuriken));
            return;
        }

        // agora usa o score combinado de todos os players
        int totalScore = GameManager.Instance.GetCombinedScore();
        if (totalScore > 0 && totalScore % 5 == 0 && totalScore < 50 && !spawnCondition)
        {
            spawnCondition = true;
            spawnRate -= 0.07f;
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

        Vector3 spawnPosition = new Vector3(
            transform.position.x,
            Random.Range(spawnYMin, spawnYMax),
            0f
        );
        Instantiate(shurikenPrefab, spawnPosition, Quaternion.identity);
    }
}
