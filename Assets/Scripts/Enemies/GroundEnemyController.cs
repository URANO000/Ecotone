using UnityEngine;

public class GroundEnemyController : MonoBehaviour
{
    [Header("Patrol")]
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private float patrolSpeed = 1f;

    [Header("Chase")]
    [SerializeField] private float detectionRange = 2.6f;
    [SerializeField] private float chaseSpeed = 1.5f;

    [Header("Attack")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = 0.8f;
    [SerializeField] private int damage = 1;
    [SerializeField] private float attackCooldown = 2f;

    [Header("Sprite")]
    [SerializeField] private bool spriteFacesRight = true;

    private Transform player;
    private Transform targetPoint;

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Rigidbody2D rb;

    private float nextAttackTime;
    private float attackPointOffsetX;

    private enum EnemyMode
    {
        Patrol,
        Chase,
        Attack
    }

    private EnemyMode currentMode = EnemyMode.Patrol;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        if (attackPoint != null)
        {
            attackPointOffsetX = Mathf.Abs(attackPoint.localPosition.x);
        }
    }

    private void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }

        if (pointB != null)
        {
            targetPoint = pointB;
        }
    }

    private void Update()
    {
        DetermineMode();

        if (currentMode == EnemyMode.Attack)
        {
            HandleAttack();
        }
    }

    private void FixedUpdate()
    {
        switch (currentMode)
        {
            case EnemyMode.Patrol:
                Patrol();
                break;

            case EnemyMode.Chase:
                Chase();
                break;

            case EnemyMode.Attack:
                StopRunning();
                break;
        }
    }

    private void DetermineMode()
    {
        if (player == null)
        {
            currentMode = EnemyMode.Patrol;
            return;
        }

        float distanceToPlayer =
            Vector2.Distance(transform.position, player.position);

        float attackDistance = Mathf.Infinity;

        if (attackPoint != null)
        {
            attackDistance =
                Vector2.Distance(attackPoint.position, player.position);
        }

        if (attackDistance <= attackRange)
        {
            currentMode = EnemyMode.Attack;
        }
        else if (distanceToPlayer <= detectionRange)
        {
            currentMode = EnemyMode.Chase;
        }
        else
        {
            currentMode = EnemyMode.Patrol;
        }
    }

    private void Patrol()
    {
        if (pointA == null || pointB == null || rb == null)
        {
            StopRunning();
            return;
        }

        if (targetPoint == null)
        {
            targetPoint = pointB;
        }

        SetRunning(true);

        Vector2 targetPosition = new Vector2(
            targetPoint.position.x,
            rb.position.y
        );

        Vector2 newPosition = Vector2.MoveTowards(
            rb.position,
            targetPosition,
            patrolSpeed * Time.fixedDeltaTime
        );

        rb.MovePosition(newPosition);

        float directionX =
            targetPoint.position.x - rb.position.x;

        FaceDirection(directionX);

        if (Mathf.Abs(rb.position.x - targetPoint.position.x) < 0.1f)
        {
            targetPoint =
                targetPoint == pointA
                    ? pointB
                    : pointA;
        }
    }

    private void Chase()
    {
        if (player == null || rb == null)
        {
            return;
        }

        SetRunning(true);

        float directionX =
            player.position.x - rb.position.x;

        if (Mathf.Abs(directionX) < 0.05f)
        {
            return;
        }

        Vector2 targetPosition = new Vector2(
            player.position.x,
            rb.position.y
        );

        Vector2 newPosition = Vector2.MoveTowards(
            rb.position,
            targetPosition,
            chaseSpeed * Time.fixedDeltaTime
        );

        rb.MovePosition(newPosition);

        FaceDirection(directionX);
    }

    private void HandleAttack()
    {
        if (player == null)
        {
            return;
        }

        StopRunning();
        FacePlayer();

        if (Time.time < nextAttackTime)
        {
            return;
        }

        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        PlayerHealth playerHealth =
            player.GetComponent<PlayerHealth>();

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }

        Debug.Log(gameObject.name + " atacó a GusGus.");

        nextAttackTime = Time.time + attackCooldown;
    }

    private void StopRunning()
    {
        SetRunning(false);
    }

    private void SetRunning(bool running)
    {
        if (animator != null)
        {
            animator.SetBool("Running", running);
        }
    }

    private void FacePlayer()
    {
        if (player == null)
        {
            return;
        }

        float directionX =
            player.position.x - transform.position.x;

        FaceDirection(directionX);
    }

    private void FaceDirection(float directionX)
    {
        if (spriteRenderer == null)
        {
            return;
        }

        bool facingRight = directionX > 0;

        if (spriteFacesRight)
        {
            spriteRenderer.flipX = !facingRight;
        }
        else
        {
            spriteRenderer.flipX = facingRight;
        }

        UpdateAttackPoint(facingRight);
    }

    private void UpdateAttackPoint(bool facingRight)
    {
        if (attackPoint == null)
        {
            return;
        }

        Vector3 localPosition = attackPoint.localPosition;

        localPosition.x =
            facingRight
                ? attackPointOffsetX
                : -attackPointOffsetX;

        attackPoint.localPosition = localPosition;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(
            transform.position,
            detectionRange
        );

        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(
                attackPoint.position,
                attackRange
            );
        }
    }
}