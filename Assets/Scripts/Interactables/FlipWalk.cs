using UnityEngine;

public class FlipWalk : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float moveSpeed = 4f;

    private Rigidbody2D rb;
    private Vector2 nextPosition;
    private bool isMovingToB = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        nextPosition = pointB.position;
    }

    void FixedUpdate()
    {
        Vector2 currentPosition = rb.position;

        Vector2 newPosition = Vector2.MoveTowards(
            currentPosition,
            nextPosition,
            moveSpeed * Time.fixedDeltaTime
        );

        rb.MovePosition(newPosition);

        if (newPosition == nextPosition)
        {
            if (isMovingToB)
            {
                nextPosition = pointA.position;
                isMovingToB = false;

                FlipSprite(1);
            }
            else
            {
                nextPosition = pointB.position;
                isMovingToB = true;

                FlipSprite(-1);
            }
        }
    }

    void FlipSprite(int direction)
    {
        Vector3 newScale = transform.localScale;
        newScale.x = direction * Mathf.Abs(newScale.x);
        transform.localScale = newScale;
    }
}