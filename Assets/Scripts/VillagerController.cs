using UnityEngine;

public class VillageController : MonoBehaviour
{
    public int life = 5;

    private bool isInvincible = false;
    private float invincibilityDuration = 1.0f;
    private float invincibilityTimer = 0f;
    void Start()
    {

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
    }

    public void TakeDamage()
    {
        if (isInvincible) return;

        GameManager.Instance.UpdateVillageLife(--life);

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
