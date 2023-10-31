using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
                if (health.enemyClass == EnemyClass.Knight)
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
                if(health.enemyClass == EnemyClass.Peasant)
                {
                    bool ifAgro = other.gameObject.GetComponent<TestPeasant>().isAgro;
                    if (!ifAgro)
                    {
                        health.TakeDamage(100 + damage.GetValue());
                    }
                    else
                    {
                        health.TakeDamage(10 + damage.GetValue());
                    }
                }
                if(health.enemyClass == EnemyClass.Priest)
                {
                    bool ifAgro = other.gameObject.GetComponent<TestPriest>();
                    if (!ifAgro)
                    {
                        health.TakeDamage(100 + damage.GetValue());
                    }
                    else
                    {
                        health.TakeDamage(10 + damage.GetValue());
                    }
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
