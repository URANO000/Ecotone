using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playerknockback : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerMovement playerMovement;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    public void Knockback(Transform enemyTransform, float knockbackForce)
    {
        Vector2 direction =
            (transform.position - enemyTransform.position).normalized;

        playerMovement.enabled = false;

        Debug.Log("Direction: " + direction);
        Debug.Log("Force: " + knockbackForce);
        Debug.Log("Rigidbody: " + rb);

        rb.velocity = direction * knockbackForce;

        Debug.Log("After velocity: " + rb.velocity);

        StartCoroutine(EndKnockback());
    }

    private IEnumerator EndKnockback()
    {
        yield return new WaitForSeconds(0.2f);

        rb.velocity = Vector2.zero;
        playerMovement.enabled = true;
    }
}
