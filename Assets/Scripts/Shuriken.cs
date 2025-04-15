using UnityEngine;

public class Shuriken : MonoBehaviour
{
    [Header("Configura��es de Movimento e Anima��o")]
    public float speed = 5f;
    public float animInterval = 0.2f;  // Tempo entre a troca das sprites

    [Header("Sprites da Shuriken")]
    public Sprite spriteZero;      // Sprite com rota��o 0�
    public Sprite spriteFortyFive; // Sprite com rota��o 45�

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

        // Controle da anima��o: troca a sprite a cada animInterval segundos
        animTimer += Time.deltaTime;
        if (animTimer >= animInterval)
        {
            toggleSprite = !toggleSprite;
            spriteRenderer.sprite = toggleSprite ? spriteFortyFive : spriteZero;
            animTimer = 0f;
        }
    }

    // Quando a shuriken sair da c�mera, destr�i o objeto para evitar ac�mulo de objetos na cena
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    // Detecta colis�es via trigger com o objeto ninja
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ninja"))
        {
            // Tenta obter o componente NinjaController do ninja
            NinjaController ninja = collision.GetComponent<NinjaController>();
            if (ninja != null)
            {
                // Se o ninja estiver atacando, a shuriken � quebrada
                if (ninja.IsAttacking)
                {
                    // Incrementa a pontua��o e destr�i a shuriken
                    GameManager.Instance.AddScore(1);
                    Destroy(gameObject);
                }
                else
                {
                    // Se o ninja n�o estiver atacando, ele � atingido e o jogo acaba
                    GameManager.Instance.GameOver();
                    // Aqui voc� pode optar por destruir o ninja ou desativar o seu controle
                    Destroy(collision.gameObject);
                }
            }
            else
            {
                // Caso n�o consiga encontrar o componente ninja, assume-se que � um acerto
                GameManager.Instance.GameOver();
            }
        }
    }
}
