using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator animator;
    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Jumping")]
    public float jumpPower = 10f;
    public int maxJumps = 2;
    private int jumpsRemaining;

    float horizontalMovement;

    [Header("GroundCheck")]
    public Transform groundCheckPos;
    public Vector2 groundCheckSize = new Vector2(0.49f, 0.03f);
    public LayerMask groundLayer;

    [Header("Gravity")]
    public float baseGravity = 2f;
    public float maxFallSpeed = 18f;
    public float fallGravityMult = 2f;

    bool isFacingRight = true;
    bool isGrounded;

    public PlayerCombat playerCombat;
    public PlayerAudio playerAudio;


    void Start()
    {
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontalMovement * moveSpeed, rb.velocity.y);
    }

    void Update()
    {
        GroundCheck();
        Gravity();
        Flip();

        animator.SetFloat("yVelocity", rb.velocity.y);
        animator.SetFloat("speed", Mathf.Abs(rb.velocity.x));

        if (Input.GetButtonDown("Hit"))
        {
            playerCombat.Attack();
        }
    }

    public void Gravity()
    {
        //falling gravity
        if (rb.velocity.y < 0)
        {
            rb.gravityScale = baseGravity * fallGravityMult; //fall faster and faster
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -maxFallSpeed)); //max fall speed
        }
        else
        {
            rb.gravityScale = baseGravity;
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (jumpsRemaining > 0)
        {
            if (context.performed)
            {
                //Hold down jump button = full height
                rb.velocity = new Vector2(rb.velocity.x, jumpPower);
                jumpsRemaining--;
                animator.SetTrigger("jump");
                playerAudio.OnJump();
            }
            else if (context.canceled && rb.velocity.y > 0)
            {
                //Light tap of jump button = half the height
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            }
        }
    }

    private void GroundCheck()
    {
        bool wasGrounded = isGrounded;

        isGrounded = Physics2D.OverlapBox(
            groundCheckPos.position,
            groundCheckSize,
            0,
            groundLayer);

        if (!wasGrounded && isGrounded)
        {
            jumpsRemaining = maxJumps;
        }

        animator.SetBool("grounded", isGrounded);
    }

    private void Flip()
    {
        if (isFacingRight && horizontalMovement < 0 || !isFacingRight && horizontalMovement > 0)
        {
            isFacingRight = !isFacingRight;
            Vector3 ls = transform.localScale;
            ls.x *= -1f;
            transform.localScale = ls;
        }
    }

    private void OnDrawGizmosSelected()
    {
        //Ground check visual
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(groundCheckPos.position, groundCheckSize);
    }
}