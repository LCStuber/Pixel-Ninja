// NinjaController.cs
using UnityEngine;
using System.Collections;

public class NinjaController : MonoBehaviour
{
    [Header("Identificação do Jogador")]
    public int playerId = 0;                          // 0 = P1, 1 = P2
    public string horizontalAxis = "Horizontal";      // ex.: "Horizontal1", "Horizontal2"
    public string jumpButton = "Jump";           // ex.: "Jump1", "Jump2"
    public KeyCode attackKey = KeyCode.Z;        // ex.: KeyCode.Z, KeyCode.X

    [Header("Vidas iniciais")]
    public int life = 3;

    [Header("Sprites de Animação")]
    public Sprite idleSprite;
    public Sprite moveSprite;
    public Sprite jumpSprite;
    public Sprite attackPrepareSprite;
    public Sprite attackSprite;

    [Header("Movimento e Ataque")]
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    public float attackPrepareTime = 0.2f;
    public float attackDuration = 0.2f;

    [Header("Tutorial Mode")]
    public bool tutorialMode = false;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private bool isInvincible = false;
    private float invincibilityDuration = 0.25f;
    private float invincibilityTimer = 0f;
    private bool isGrounded = true;
    private bool isAttacking = false;

    public bool IsAttacking
    {
        get { return isAttacking; }
    }

   void Awake()
   {
       spriteRenderer = GetComponent<SpriteRenderer>();
       rb = GetComponent<Rigidbody2D>();
       rb.freezeRotation = true;
   }

void Start()
   {
           // agora sim temos certeza que GameManager.Instance já existe
    GameManager.Instance.UpdateLife(playerId, life);
       }

void Update()
    {
        // invencibilidade
        if (isInvincible)
        {
            invincibilityTimer -= Time.deltaTime;
            if (invincibilityTimer <= 0f) isInvincible = false;
        }

        // input
        float horizontal = Input.GetAxisRaw(horizontalAxis);
        if (!isAttacking)
            rb.linearVelocity = new Vector2(horizontal * moveSpeed, rb.linearVelocity.y);

        if (Input.GetButtonDown(jumpButton) && isGrounded && !isAttacking)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isGrounded = false;
        }

        if (Input.GetKeyDown(attackKey) && !isAttacking)
            StartCoroutine(Attack());

        // animações
        if (isAttacking) { /* coroutine já cuida das sprites */ }
        else if (!isGrounded) spriteRenderer.sprite = jumpSprite;
        else if (Mathf.Abs(horizontal) > 0.1f) spriteRenderer.sprite = moveSprite;
        else spriteRenderer.sprite = idleSprite;

        // flip
        if (horizontal < 0) spriteRenderer.flipX = true;
        else if (horizontal > 0) spriteRenderer.flipX = false;
    }

    IEnumerator Attack()
    {
        isAttacking = true;
        spriteRenderer.sprite = attackPrepareSprite;
        yield return new WaitForSeconds(attackPrepareTime);
        spriteRenderer.sprite = attackSprite;
        yield return new WaitForSeconds(attackDuration);
        isAttacking = false;
    }

    void LateUpdate()
    {
        // limita dentro da câmera
        Vector3 pos = transform.position;
        float halfW = Camera.main.orthographicSize * Camera.main.aspect;
        pos.x = Mathf.Clamp(pos.x, -halfW, halfW);
        transform.position = pos;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.CompareTag("Ground")) isGrounded = true;
    }

    public void TakeDamage()
    {
        if (GameManager.Instance.currentMode == GameManager.Mode.Bonanza)
        {
            // só perde 1 ponto em vez de vida
            GameManager.Instance.AddScore(playerId, -1);
            return;
        }
        if (GameManager.Instance.currentMode == GameManager.Mode.Bonanza)
            return;
        if (tutorialMode || isInvincible) return;

        life--;
        GameManager.Instance.UpdateLife(playerId, life);

        if (life <= 0)
        {
            GameManager.Instance.PlayerDied(playerId);
        }
        else
        {
            isInvincible = true;
            invincibilityTimer = invincibilityDuration;
        }
    }

    // Exemplo de chamada de pontuação (invocado quando acerta um inimigo, por exemplo)
    public void OnEnemyDefeated(int points)
    {
        GameManager.Instance.AddScore(playerId, points);
    }
}
