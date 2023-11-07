using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitBox : MonoBehaviour
{
    BoxCollider boxCollider;
    //public GameObject player;

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
                health.TakeDamage(3);
            }
            Player player = other.gameObject.GetComponent<Player>();
            if(player != null)
            {
                if(player.blocking)
                {
                    //stamina.currentStamina -= 10;
                    GameManager.manager.currentStamina -= 10;
                }
            }
        }
    }
}
