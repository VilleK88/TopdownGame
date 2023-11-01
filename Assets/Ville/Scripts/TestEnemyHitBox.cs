using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyHitBox : MonoBehaviour
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
            TestPlayerHealth health = other.gameObject.GetComponent<TestPlayerHealth>();
            if (health != null)
            {
                health.TakeDamage(3);
            }
            TestPlayer player = other.gameObject.GetComponent<TestPlayer>();
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
