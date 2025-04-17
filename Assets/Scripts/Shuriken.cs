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
        if (!collision.CompareTag("Ninja") && tutorialMode) return;

        if (collision.CompareTag("Village") && !tutorialMode)
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
        } else {
            NinjaController ninja = collision.GetComponent<NinjaController>();
            if (ninja != null && ninja.IsAttacking)
            {
                if (tutorialMode && tutorialManager != null)
                {
                    tutorialManager.OnTutorialShurikenHit();
                }
                else
                {
                    GameManager.Instance.AddScore(1);
                }
                Destroy(gameObject);
            }
            else if (ninja != null && !ninja.IsAttacking)
            {
                ninja.TakeDamage();
                Destroy(gameObject);
            }
        }
    }
}