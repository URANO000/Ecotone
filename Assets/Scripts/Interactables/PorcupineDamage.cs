using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PorcupineDamage : MonoBehaviour
{
    [SerializeField] private int damageAmount = 5;
    public float knockbackForce = 5;

    public float cooldown = 0.2f;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
               if(timer <= 0)
                {
                    playerHealth.TakeDamage(damageAmount);

                    // Knockback effect
                    Playerknockback playerKnockback = other.GetComponent<Playerknockback>();
                    if (playerKnockback != null)
                    {
                        playerKnockback.Knockback(transform, knockbackForce);
                    }

                    timer = cooldown;
                }
            }
        }
    }
}
