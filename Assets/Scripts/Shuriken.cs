using UnityEngine;

public class Shuriken : MonoBehaviour
{
    [Header("Configura��es de Movimento e Anima��o")]
    public float speed = 5f;
    public float animInterval = 0.2f;
    [Header("Sprites da Shuriken")]
    public Sprite spriteZero;
    public Sprite spriteFortyFive;
    private SpriteRenderer spriteRenderer;
    private float animTimer = 0f;
    private bool toggleSprite = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null && spriteZero != null)
        {
            spriteRenderer.sprite = spriteZero;
        }
    }

    void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        animTimer += Time.deltaTime;
        if (animTimer >= animInterval)
        {
            toggleSprite = !toggleSprite;
            spriteRenderer.sprite = toggleSprite ? spriteFortyFive : spriteZero;
            animTimer = 0f;
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ninja"))
        {
            NinjaController ninja = collision.GetComponent<NinjaController>();
            if (ninja != null)
            {
                if (ninja.IsAttacking)
                {
                    GameManager.Instance.AddScore(1);
                    Destroy(gameObject);
                }
                else
                {
                    ninja.TakeDamage();
                    Destroy(gameObject);
                }
            }
            else
            {
                GameManager.Instance.GameOver();
            }
        }
        if (collision.CompareTag("Village"))
        {
            VillageController vila = collision.GetComponent<VillageController>();
            if (vila != null)
            {
                vila.TakeDamage();
                Destroy(gameObject);
            }
            else
            {
                GameManager.Instance.GameOver();
            }
        }


    }
}
