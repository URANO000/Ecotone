using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private float attackCooldown = 2f;

    private EnemyBrain enemyBrain;
    private Animator animator;
    private float nextAttackTime;

    private void Awake()
    {
        enemyBrain = GetComponent<EnemyBrain>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (enemyBrain == null || enemyBrain.Player == null)
        {
            return;
        }

        if (!enemyBrain.PlayerInAttackRange)
        {
            return;
        }

        if (Time.time < nextAttackTime)
        {
            return;
        }

        PlayerHealth playerHealth =
            enemyBrain.Player.GetComponent<PlayerHealth>();

        if (playerHealth != null)
        {
            if (animator != null)
            {
                animator.SetBool("Running", false);
                animator.SetTrigger("Attack");
            }

            playerHealth.TakeDamage(damage);

            Debug.Log("El enemigo atacó.");

            nextAttackTime = Time.time + attackCooldown;
        }
    }
}