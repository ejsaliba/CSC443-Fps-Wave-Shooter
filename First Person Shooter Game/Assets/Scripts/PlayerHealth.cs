using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 15;
    private int currentHealth;

    public GameObject deathVFX;
    private bool isDead = false;

public int GetHPValue()
{
    return currentHealth;
}

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;

        if (deathVFX != null)
        {
            Instantiate(deathVFX, transform.position, transform.rotation);
        }

        Destroy(gameObject);
    }

    void Update()
    {
        if (Keyboard.current.kKey.wasPressedThisFrame)
        {
            TakeDamage(1);
        }
    }
}