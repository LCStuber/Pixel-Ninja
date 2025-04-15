using UnityEngine;
using System.Collections;

public class NinjaController : MonoBehaviour
{
    // === Sprites para cada estado ===
    [Header("Sprites de Animação")]
    public Sprite idleSprite;           // Sprite em que o ninja está parado
    public Sprite moveSprite;           // Sprite em que o ninja está se movendo
    public Sprite jumpSprite;           // Sprite de pulo
    public Sprite attackPrepareSprite;  // Sprite de preparação do ataque
    public Sprite attackSprite;         // Sprite do ataque propriamente dito

    // === Componentes necessários ===
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    // === Configurações de Movimento ===
    [Header("Configurações de Movimento")]
    public float moveSpeed = 5f;        // Velocidade de movimento horizontal
    public float jumpForce = 7f;        // Força do pulo

    // === Configurações de Ataque ===
    [Header("Configurações de Ataque")]
    public float attackPrepareTime = 0.2f;  // Tempo de preparação do ataque
    public float attackDuration = 0.2f;     // Duração do estado de ataque

    // === Estados do personagem ===
    private bool isGrounded = true;     // Indica se o ninja está no chão
    private bool isAttacking = false;   // Indica se o ninja está atacando

    // Propriedade pública para acesso do estado de ataque
    public bool IsAttacking
    {
        get { return isAttacking; }
    }

    void Awake()
    {
        // Obtém as referências dos componentes
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        // Congela a rotação para que o ninja não gire nas colisões
        rb.freezeRotation = true;
    }

    void Update()
    {
        // Leitura do input horizontal (setas ou A/D)
        float horizontal = Input.GetAxisRaw("Horizontal");

        // Se não estiver atacando, permite movimento
        if (!isAttacking)
        {
            rb.linearVelocity = new Vector2(horizontal * moveSpeed, rb.linearVelocity.y);
        }

        // Pulo: somente se estiver no chão e não estiver atacando
        if (Input.GetButtonDown("Jump") && isGrounded && !isAttacking)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isGrounded = false; // Depois do pulo, o ninja não está mais no chão
        }

        // Inicia ataque ao pressionar tecla Z (ou a que preferir)
        if (Input.GetKeyDown(KeyCode.Z) && !isAttacking)
        {
            StartCoroutine(Attack());
        }

        // Atualiza a sprite com base no estado atual do ninja
        if (isAttacking)
        {
            // Durante o ataque, a sprite é controlada pela coroutine Attack()
        }
        else if (!isGrounded)
        {
            spriteRenderer.sprite = jumpSprite;
        }
        else if (Mathf.Abs(horizontal) > 0.1f)
        {
            spriteRenderer.sprite = moveSprite;
        }
        else
        {
            spriteRenderer.sprite = idleSprite;
        }

        // Realiza o flip do sprite para que o ninja olhe na direção do movimento
        if (horizontal < 0)
            spriteRenderer.flipX = true;
        else if (horizontal > 0)
            spriteRenderer.flipX = false;
    }

    // Coroutine para controlar a sequência de ataque
    IEnumerator Attack()
    {
        isAttacking = true;

        // Fase de preparação do ataque
        spriteRenderer.sprite = attackPrepareSprite;
        yield return new WaitForSeconds(attackPrepareTime);

        // Fase do ataque propriamente dita
        spriteRenderer.sprite = attackSprite;
        yield return new WaitForSeconds(attackDuration);

        isAttacking = false;
    }

    // Detecta colisões para verificar se o ninja voltou a tocar o chão
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Certifique-se de que o objeto chão possua a tag "Ground"
        if (collision.collider.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}
