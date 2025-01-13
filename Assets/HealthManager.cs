using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public Image healthBar;
    public float healthAmount = 100f;
    public int maxHits = 3;
    public int currentHits = 0;

    // Invincibility parameters
    private bool isInvincible = false;
    private float invincibilityDuration = 2f;
    private float invincibilityTimer = 0f;

    void Start()
    {
        currentHits = 0;
        healthAmount = 100f;
        UpdateHealthBar();
    }

    void Update()
    {
        // Handle invincibility timer
        if (isInvincible)
        {
            invincibilityTimer -= Time.deltaTime;
            if (invincibilityTimer <= 0)
            {
                isInvincible = false;
                Debug.Log("Invincibility ended");
            }
        }

        if (currentHits >= maxHits)
        {
            GameManager.Instance.GameOver();
        }
    }

    public void TakeDamage(float damage)
    {
        // If invincible, ignore damage
        if (isInvincible)
        {
            Debug.Log("Hit ignored - Player is invincible!");
            return;
        }

        Debug.Log($"TakeDamage called. Current hits before damage: {currentHits}");
        currentHits++;
        healthAmount -= damage;
        UpdateHealthBar();

        // Activate invincibility
        isInvincible = true;
        invincibilityTimer = invincibilityDuration;
        Debug.Log("Invincibility activated for 2 seconds");

        Debug.Log($"After damage: Hits={currentHits}, Health={healthAmount}");

        if (currentHits >= maxHits)
        {
            Debug.Log("Max hits reached, triggering game over");
            GameManager.Instance.GameOver();
        }
    }

    public void Heal(float healingAmount)
    {
        healthAmount += healingAmount;
        healthAmount = Mathf.Clamp(healthAmount, 0, 100);
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = healthAmount / 100f;
        }
        else
        {
            Debug.LogWarning("Health bar UI Image not assigned!");
        }
    }

    // Optional: Add visual feedback for invincibility
    public bool IsInvincible()
    {
        return isInvincible;
    }
}