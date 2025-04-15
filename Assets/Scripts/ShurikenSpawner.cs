using UnityEngine;

public class ShurikenSpawner : MonoBehaviour
{
    public GameObject shurikenPrefab;
    public float spawnRate = 1.5f;
    public float spawnYMin = -2.5f;
    public float spawnYMax = 0f;

    void Start()
    {
        // Inicia o spawner das shurikens
        InvokeRepeating("SpawnShuriken", 0f, spawnRate);
    }

    void Update()
    {
        // Se o Game Over ocorreu, cancela o InvokeRepeating para parar de spawnar shurikens
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver)
        {
            CancelInvoke("SpawnShuriken");
        }
    }

    void SpawnShuriken()
    {
        // Verifica se o prefab está referenciado
        if (shurikenPrefab == null)
        {
            Debug.LogWarning("shurikenPrefab é nulo. Verifique a referência no Inspector.");
            return;
        }
        Vector3 spawnPosition = new Vector3(transform.position.x, Random.Range(spawnYMin, spawnYMax), 0);
        Instantiate(shurikenPrefab, spawnPosition, Quaternion.identity);
    }
}
