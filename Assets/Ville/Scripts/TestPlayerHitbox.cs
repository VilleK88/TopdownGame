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
            TestPriestHealth priestHealth = other.gameObject.GetComponent<TestPriestHealth>();
            bool ifBlocking = other.gameObject.GetComponent<TestEnemyHealth>().blockingPlayer;
            if(health != null)
            {
                if(gameObject.name == "TestEnemy")
                {
                    if(!ifBlocking)
                    {
                        health.TakeDamage(10);
                    }
                }
                if(gameObject.name == "Priest")
                {
                    priestHealth.TakeDamage(10);
                }
                else
                {
                    health.TakeDamage(10);
                }
            }
        }

        /*if (other.gameObject.layer == LayerMask.NameToLayer("Enemy") && other.gameObject.name == "TestPeasant")
        {
            TestEnemyHealth health = other.gameObject.GetComponent<TestEnemyHealth>();
            bool ifBlocking = other.gameObject.GetComponent<TestEnemyHealth>().blockingPlayer;
            if (health != null)
            {
                if (!ifBlocking)
                {
                    health.TakeDamage(10);
                }
            }
        }*/

        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy") && other.gameObject.name == "Priest")
        {
            TestPriestHealth priestHealth = other.gameObject.GetComponent<TestPriestHealth>();
            if (priestHealth != null)
            {
                priestHealth.TakeDamage(10);
            }
        }
    }
}
