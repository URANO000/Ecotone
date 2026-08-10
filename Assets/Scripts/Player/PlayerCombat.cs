using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Transform attackPoint;
    public float attackRange = 1;
    public float knockbackForce = 50;
    public LayerMask enemyLayer;
    public int damage = 1;

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
                animator.SetBool("isAttackingStick", true);


                Debug.Log(
                    "AttackStick triggered. Current state: " +
                    animator.GetCurrentAnimatorStateInfo(0).shortNameHash
                );
            }
            else
            {
                animator.SetBool("isAttacking", true);
            }
            playerAudio.OnAttack();

            Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

            if (enemies.Length > 0)
            {
                enemies[0].GetComponent<EnemyHealth>().TakeDamage(damage);
                enemies[0].GetComponent<EnemyKnockback>().Knockback(transform, knockbackForce);
            }

            timer = cooldown;
        }
    }

    public void FinishAttacking()
    {
        if (hasStick)
        {
            animator.SetBool("isAttackingStick", false);
        }
        else
        {
            animator.SetBool("isAttacking", false);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
