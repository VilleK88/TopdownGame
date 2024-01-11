using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearAttackHitBox : MonoBehaviour
{
    BoxCollider boxCollider;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerHealth health = other.gameObject.GetComponent<PlayerHealth>();
            Player player = other.gameObject.GetComponent<Player>();
            if (health != null && !player.ukko)
            {
                health.TakeDamage(25);
            }
            if (player != null && !player.ukko)
            {
                if (player.blocking)
                {
                    GameManager.manager.currentStamina -= 10;
                }
            }
        }
    }
}
