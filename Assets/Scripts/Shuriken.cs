using UnityEngine;

public class Shuriken : MonoBehaviour
{
    [Header("Configurações de Movimento e Animação")]
    public float speed = 5f;
    public float animInterval = 0.2f;  // Tempo entre a troca das sprites

    [Header("Sprites da Shuriken")]
    public Sprite spriteZero;      // Sprite com rotação 0º
    public Sprite spriteFortyFive; // Sprite com rotação 45º

    private SpriteRenderer spriteRenderer;
    private float animTimer = 0f;
    private bool toggleSprite = false; // false usa spriteZero, true usa spriteFortyFive

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
        // Movimento para a esquerda
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        // Controle da animação: troca a sprite a cada animInterval segundos
        animTimer += Time.deltaTime;
        if (animTimer >= animInterval)
        {
            toggleSprite = !toggleSprite;
            spriteRenderer.sprite = toggleSprite ? spriteFortyFive : spriteZero;
            animTimer = 0f;
        }
    }

    // Quando a shuriken sair da câmera, destrói o objeto para evitar acúmulo de objetos na cena
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    // Detecta colisões via trigger com o objeto ninja
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ninja"))
        {
            // Tenta obter o componente NinjaController do ninja
            NinjaController ninja = collision.GetComponent<NinjaController>();
            if (ninja != null)
            {
                // Se o ninja estiver atacando, a shuriken é quebrada
                if (ninja.IsAttacking)
                {
                    // Incrementa a pontuação e destrói a shuriken
                    GameManager.Instance.AddScore(1);
                    Destroy(gameObject);
                }
                else
                {
                    // Se o ninja não estiver atacando, ele é atingido e o jogo acaba
                    GameManager.Instance.GameOver();
                    // Aqui você pode optar por destruir o ninja ou desativar o seu controle
                    Destroy(collision.gameObject);
                }
            }
            else
            {
                // Caso não consiga encontrar o componente ninja, assume-se que é um acerto
                GameManager.Instance.GameOver();
            }
        }
    }
}
