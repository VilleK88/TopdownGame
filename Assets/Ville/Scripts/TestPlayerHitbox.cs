using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerHitbox : MonoBehaviour
{
    public Stat damage;
    public Stat armor;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            TestEnemyHealth health = other.gameObject.GetComponent<TestEnemyHealth>();
            //TestPriestHealth priestHealth = other.gameObject.GetComponent<TestPriestHealth>();
            bool ifBlocking = other.gameObject.GetComponent<TestEnemyHealth>().blockingPlayer;
            if(health != null)
            {
                if(other.gameObject.name == "TestEnemy")
                {
                    bool ifAgro = other.gameObject.GetComponent<TestEnemy>().isAgro;
                    if (!ifBlocking && !ifAgro)
                    {
                        health.TakeDamage(100 + damage.GetValue());
                    }
                    else if(!ifBlocking)
                    {
                        health.TakeDamage(10 + damage.GetValue());
                    }
                }
                /*if(gameObject.name == "Priest")
                {
                    priestHealth.TakeDamage(10);
                }*/
                else if(other.gameObject.name == "Priest")
                {
                    bool ifAgro = other.gameObject.GetComponent<TestPriest>().isAgro;
                    if (!ifAgro)
                    {
                        health.TakeDamage(100 + damage.GetValue());
                    }
                    else
                    {
                        health.TakeDamage(15 + damage.GetValue());
                    }
                }
                else
                {
                    health.TakeDamage(15 + damage.GetValue());
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
    }
}
