using UnityEngine;

public class ShurikenSpawner : MonoBehaviour
{
    public GameObject shurikenPrefab;
    private float spawnRate = 1.5f;
    public float spawnYMin = -2.5f;
    public float spawnYMax = 0f;

    private bool spawnCondition = false;


    void Start()
    {
        InvokeRepeating("SpawnShuriken", 0f, spawnRate);

    }

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver)
        {
            CancelInvoke("SpawnShuriken");
        }
        int score = GameManager.Instance.score;
        if (score > 0 && score % 5 == 0 && score < 50 && !spawnCondition){
            spawnCondition = true;
            spawnRate = spawnRate -0.07f;
            CancelInvoke("SpawnShuriken");
            InvokeRepeating("SpawnShuriken", 0f, spawnRate);
        }
        if (score % 5 != 0 && spawnCondition){
            spawnCondition = false;
        }
    }

    void SpawnShuriken()
    {
        if (shurikenPrefab == null)
        {
            Debug.LogWarning("shurikenPrefab � nulo. Verifique a refer�ncia no Inspector.");
            return;
        }

        Vector3 spawnPosition = new Vector3(transform.position.x, Random.Range(spawnYMin, spawnYMax), 0);
        Instantiate(shurikenPrefab, spawnPosition, Quaternion.identity);
    }
}
