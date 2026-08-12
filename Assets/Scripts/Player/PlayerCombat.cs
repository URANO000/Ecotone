using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Transform attackPoint;
    public float attackRange = 0.3f;
    private float knockbackForce = 2;
    public LayerMask enemyLayer;
    private int damage = 7;

    public Animator animator;
    public PlayerAudio playerAudio;
    public float cooldown = 0.2f;
    private float timer;

    public bool hasStick;

    [SerializeField] private SpriteRenderer stickRenderer;

    private void Start()
    {
        stickRenderer.enabled = hasStick;
    }

    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
    }

    public void EquipStick()
    {
        hasStick = true;
        stickRenderer.enabled = true;
    }

    public void Attack()
    {
        if (timer <= 0)
        {
            if (hasStick)
            {
                animator.SetTrigger("AttackStick");

                Debug.Log("AttackStick trigger sent");
            }
            else
            {
                animator.SetTrigger("Attack");

                Debug.Log("Attack trigger sent");
            }
            playerAudio.OnAttack();

            Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

            if (enemies.Length > 0)
            {
                if (hasStick)
                {
                    damage = 15;
                }
                EnemyHealth enemyHealth = enemies[0].GetComponent<EnemyHealth>();

                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(damage);
                }

                EnemyKnockback enemyKnockback = enemies[0].GetComponent<EnemyKnockback>();

                if (enemyKnockback != null)
                {
                    enemyKnockback.Knockback(transform, knockbackForce);
                }
            }

            timer = cooldown;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
