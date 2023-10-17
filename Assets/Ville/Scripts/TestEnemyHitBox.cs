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
                health.TakeDamage(2.5f);
            }
            TestPlayer stamina = other.gameObject.GetComponent<TestPlayer>();
            if(stamina != null)
            {
                if(stamina.blocking)
                {
                    stamina.currentStamina -= 10;
                }
            }
        }
    }
}
