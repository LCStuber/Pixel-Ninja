using UnityEngine;

public class Shuriken : MonoBehaviour
{
    [Header("Configurações de Movimento e Animação")]
    public float speed = 5f;
    public float animInterval = 0.2f;

    [Header("Sprites da Shuriken")]
    public Sprite spriteZero;
    public Sprite spriteFortyFive;

    [Header("Tutorial Mode")]
    public bool tutorialMode = false;
    public TutorialManager tutorialManager;

    private SpriteRenderer spriteRenderer;
    private float animTimer = 0f;
    private bool toggleSprite = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = spriteZero;
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
        // ignora colisões em tutorial com qualquer coisa que não seja Ninja
        if (!collision.CompareTag("Ninja") && tutorialMode) return;

        // se acertar uma vila
        if (collision.CompareTag("Village") && !tutorialMode)
        {
            var vila = collision.GetComponent<VillageController>();
            if (vila != null)
            {
                vila.TakeDamage();
                Destroy(gameObject);
            }
            else
            {
                // fallback: sem VillageController → encerra
                GameManager.Instance.GameOver();
            }
        }
        else
        {
            // se acertar um ninja
            var ninja = collision.GetComponent<NinjaController>();
            if (ninja != null)
            {
                if (ninja.IsAttacking)
                {
                    if (tutorialMode && tutorialManager != null)
                        tutorialManager.OnTutorialShurikenHit();
                    else
                        GameManager.Instance.AddScore(ninja.playerId, 1);
                    Destroy(gameObject);
                }
                else
                {
                    ninja.TakeDamage();
                    Destroy(gameObject);
                }
            }
        }
    }
}
