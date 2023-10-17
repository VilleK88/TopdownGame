using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerHitbox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            TestEnemyHealth health = other.gameObject.GetComponent<TestEnemyHealth>();
            bool ifBlocking = other.gameObject.GetComponent<TestEnemyHealth>().blockingPlayer;
            if(health != null)
            {
                if(!ifBlocking)
                {
                    health.TakeDamage(10);
                }
            }
        }
    }
}
