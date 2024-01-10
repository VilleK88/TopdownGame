using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearStrongAttackHitBox : MonoBehaviour
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
            if (health != null)
            {
                health.TakeDamage(40);
            }
            Player player = other.gameObject.GetComponent<Player>();
            if (player != null)
            {
                if (player.blocking)
                {
                    GameManager.manager.currentStamina -= 50;
                }
            }
        }
    }
}
