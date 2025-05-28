using UnityEngine;

public class VillageController : MonoBehaviour
{
    public int life = 5;
    private bool isInvincible = false;
    private float invincibilityDuration = 1f;
    private float invincibilityTimer = 0f;

    void Update()
    {
        if (isInvincible)
        {
            invincibilityTimer -= Time.deltaTime;
            if (invincibilityTimer <= 0f)
                isInvincible = false;
        }
    }

    public void TakeDamage()
    {
        if (isInvincible) return;

        life--;
        GameManager.Instance.UpdateVillageLife(life);

        if (life > 0)
        {
            isInvincible = true;
            invincibilityTimer = invincibilityDuration;
        }
        // se life == 0, o próprio UpdateVillageLife já chama GameOver()
    }
}
