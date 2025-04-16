using UnityEngine;
using System.Collections;

public class NinjaController : MonoBehaviour
{
    public int life = 3;

    private bool isInvincible = false;
    private float invincibilityDuration = 0.25f; 
    private float invincibilityTimer = 0f;

    [Header("Sprites de Animação")]
    public Sprite idleSprite;
    public Sprite moveSprite;
    public Sprite jumpSprite;
    public Sprite attackPrepareSprite;
    public Sprite attackSprite;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    [Header("Configurações de Movimento")]
    public float moveSpeed = 5f;
    float jumpForce = 7f;

    [Header("Configurações de Ataque")]
    public float attackPrepareTime = 0.2f;
    public float attackDuration = 0.2f;

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

    void Update()
    {
        if (isInvincible)
        {
            invincibilityTimer -= Time.deltaTime;
            if (invincibilityTimer <= 0f)
            {
                isInvincible = false;
            }
        }

        float horizontal = Input.GetAxisRaw("Horizontal");

        if (!isAttacking)
        {
            rb.linearVelocity = new Vector2(horizontal * moveSpeed, rb.linearVelocity.y);
        }

        if (Input.GetButtonDown("Jump") && isGrounded && !isAttacking)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isGrounded = false;
        }

        if (Input.GetKeyDown(KeyCode.Z) && !isAttacking)
        {
            StartCoroutine(Attack());
        }

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

        if (horizontal < 0)
            spriteRenderer.flipX = true;
        else if (horizontal > 0)
            spriteRenderer.flipX = false;
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
        Vector3 position = transform.position;
        float screenHalfWidth = Camera.main.orthographicSize * Camera.main.aspect;

        position.x = Mathf.Clamp(position.x, -screenHalfWidth, screenHalfWidth);
        transform.position = position;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
    public void TakeDamage()
    {
        if (isInvincible) return;

        GameManager.Instance.UpdateLife(--life);

        if (life <= 0)
        {
            GameManager.Instance.GameOver();
            Destroy(gameObject);
        }
        else
        {
            isInvincible = true;
            invincibilityTimer = invincibilityDuration;
        }
    }
}
