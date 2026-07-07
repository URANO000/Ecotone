using UnityEngine;

public class EnemyBrain : MonoBehaviour
{
    public Transform Player { get; private set; }

    [SerializeField] private float detectionRange = 4f;
    [SerializeField] private float attackRange = 1.2f;

    public bool PlayerDetected { get; private set; }
    public bool PlayerInAttackRange { get; private set; }

    private void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            Player = playerObject.transform;
        }
    }

    private void Update()
    {
        if (Player == null)
        {
            return;
        }

        float distance = Vector2.Distance(transform.position, Player.position);

        PlayerDetected = distance <= detectionRange;
        PlayerInAttackRange = distance <= attackRange;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}