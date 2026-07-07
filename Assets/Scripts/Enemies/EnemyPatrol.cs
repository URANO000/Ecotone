using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] private float patrolSpeed = 1.2f;
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private bool moveInBothAxes = false;

    private EnemyBrain enemyBrain;
    private SpriteRenderer spriteRenderer;
    private Transform targetPoint;

    private void Awake()
    {
        enemyBrain = GetComponent<EnemyBrain>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        targetPoint = pointB;
    }

    private void Update()
    {
        if (enemyBrain == null || enemyBrain.PlayerDetected)
        {
            return;
        }

        if (pointA == null || pointB == null)
        {
            return;
        }

        transform.position = Vector2.MoveTowards(
            transform.position,
            targetPoint.position,
            patrolSpeed * Time.deltaTime
        );

        UpdateDirection(targetPoint.position.x);

        if (Vector2.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            targetPoint = targetPoint == pointA ? pointB : pointA;
        }
    }

    private void UpdateDirection(float targetX)
    {
        if (spriteRenderer == null)
        {
            return;
        }

        spriteRenderer.flipX = targetX < transform.position.x;
    }
}