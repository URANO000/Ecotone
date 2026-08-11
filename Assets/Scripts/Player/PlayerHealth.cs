using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private HealthBar healthBar;
    private Rigidbody2D rb;
    private PlayerMovement playerMovement;

    private int currentHealth;

    public Animator animator;

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float flashDuration = 0.15f;
    [SerializeField] private int flashCount = 3;

    private Color normalColor;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    private void Start()
    {
        if (healthBar != null)
        {
            healthBar.SetMaxHealth(maxHealth);
        }

        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        normalColor = spriteRenderer.color;
    }

    private bool isDead = false;

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        Debug.Log("GusGus recibió daño. Vida actual: " + currentHealth);

        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth);
        }

        StartCoroutine(FlashColor(Color.red));

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    public void Heal(int amount)
    {
        if (isDead) return;

        currentHealth += amount;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        Debug.Log("GusGus recibió curación. Vida actual: " + currentHealth);

        StartCoroutine(FlashColor(Color.green));

        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth);
        }
    }

    private void Die()
    {
        isDead = true;
        Debug.Log("GusGus murió.");
        animator.SetTrigger("die");

        if (rb != null) { rb.velocity = Vector2.zero; }
        if (playerMovement != null) { playerMovement.enabled = false; }

        if (GameManager.Instance != null)
        {
            Debug.Log("Hello, I work");
            StartCoroutine(ShowGameOverAfterDelay());
        }
    }

    private IEnumerator ShowGameOverAfterDelay()
    {
        yield return new WaitForSeconds(2f);

        GameManager.Instance?.GameOver();
    }

    private IEnumerator FlashColor(Color flashColor)
    {
        for (int i = 0; i < flashCount; i++)
        {
            spriteRenderer.color = flashColor;

            yield return new WaitForSeconds(flashDuration);

            spriteRenderer.color = normalColor;

            yield return new WaitForSeconds(flashDuration);
        }
    }
}