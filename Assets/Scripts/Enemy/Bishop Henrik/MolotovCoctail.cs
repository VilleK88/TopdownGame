using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MolotovCoctail : MonoBehaviour
{
    Rigidbody rb;

    float throwStrength = 10;
    float upwardStrength = 5;

    public LayerMask groundLayer;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        ThrowMolotov();
    }

    void ThrowMolotov()
    {
        Vector3 throwDirection = transform.forward * throwStrength + transform.up * upwardStrength;
        rb.AddForce(throwDirection, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth health = collision.gameObject.GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.TakeDamage(5);
                Destroy(gameObject);
            }
            Player player = collision.gameObject.GetComponent<Player>();
            if (player != null)
            {
                if (player.blocking)
                {
                    GameManager.manager.currentStamina -= 10;
                    Destroy(gameObject);
                }
            }
        }

        if(groundLayer == (groundLayer | (1 << collision.gameObject.layer)))
        {
            Destroy(gameObject);
        }
    }
}
