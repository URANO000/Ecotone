using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float destroyDelay = 1.2f;

    private int currentHealth;
    private bool isDead = false;

    private Animator animator;
    private GroundEnemyController controller;
    private Rigidbody2D rb;
    private Collider2D enemyCollider;

    private void Awake()
    {
        currentHealth = maxHealth;

        animator = GetComponent<Animator>();
        controller = GetComponent<GroundEnemyController>();
        rb = GetComponent<Rigidbody2D>();
        enemyCollider = GetComponent<Collider2D>();
    }

    public void TakeDamage(int damage)
    {
        if (isDead)
        {
            return;
        }

        currentHealth -= damage;

        Debug.Log(
            gameObject.name +
            " recibió daño. Vida actual: " +
            currentHealth
        );

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead)
        {
            return;
        }

        isDead = true;

        // Detener la IA
        if (controller != null)
        {
            controller.enabled = false;
        }

        // Detener completamente al enemigo
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }

        // Ya no puede empujar ni recibir golpes
        if (enemyCollider != null)
        {
            enemyCollider.enabled = false;
        }

        // Reproducir animación de muerte
        if (animator != null)
        {
            animator.SetBool("Running", false);
            animator.SetTrigger("Die");
        }

        // Eliminar después de terminar la animación
        Destroy(gameObject, destroyDelay);
    }
}