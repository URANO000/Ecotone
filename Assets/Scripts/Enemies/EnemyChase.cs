using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    [SerializeField] private float chaseSpeed = 0.8f;
    [SerializeField] private bool spriteFacesRight = true;

    private EnemyBrain enemyBrain;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private void Awake()
    {
        enemyBrain = GetComponent<EnemyBrain>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (enemyBrain == null || enemyBrain.Player == null)
        {
            return;
        }

        if (!enemyBrain.PlayerDetected)
        {
            if (animator != null)
            {
                animator.SetBool("Running", false);
            }

            return;
        }

        if (enemyBrain.PlayerInAttackRange)
        {
            if (animator != null)
            {
                animator.SetBool("Running", false);
            }

            return;
        }

        if (animator != null)
        {
            animator.SetBool("Running", true);
        }

        float directionX =
            enemyBrain.Player.position.x - transform.position.x;

        Vector3 newPosition = transform.position;

        newPosition.x +=
            Mathf.Sign(directionX) *
            chaseSpeed *
            Time.deltaTime;

        transform.position = newPosition;

        if (spriteRenderer != null)
        {
            if (spriteFacesRight)
            {
                spriteRenderer.flipX = directionX < 0;
            }
            else
            {
                spriteRenderer.flipX = directionX > 0;
            }
        }
    }
}