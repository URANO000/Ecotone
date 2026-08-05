using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdFly : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float moveSpeed = 4f;

    private Vector3 nextPosition;
    private bool isMovingToB = true;
    // Start is called before the first frame update
    void Start()
    {
        nextPosition = pointB.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, nextPosition, moveSpeed * Time.deltaTime);
        if (transform.position == nextPosition)
        {
            if (isMovingToB)
            {
                nextPosition = pointA.position;
                isMovingToB = false;

                FlipSprite(-1);
            }
            else
            {
                nextPosition = pointB.position;
                isMovingToB = true;

                FlipSprite(1);
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
