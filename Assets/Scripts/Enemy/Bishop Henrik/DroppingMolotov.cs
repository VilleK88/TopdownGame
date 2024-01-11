using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppingMolotov : MonoBehaviour
{
    Rigidbody rb;
    public LayerMask groundLayer;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth health = collision.gameObject.GetComponent<PlayerHealth>();
            Player player = collision.gameObject.GetComponent<Player>();
            if (health != null && !player.ukko)
            {
                health.TakeDamage(20);
                Destroy(gameObject);
            }
            if (player != null && !player.ukko)
            {
                if (player.blocking)
                {
                    GameManager.manager.currentStamina -= 10;
                    Destroy(gameObject);
                }
            }
        }

        if (groundLayer == (groundLayer | (1 << collision.gameObject.layer)))
        {
            Destroy(gameObject);
        }
    }
}
