using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStrongAttackHitBox : MonoBehaviour
{
    public Stat damage;
    public Stat armor;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            EnemyHealth health = other.gameObject.GetComponent<EnemyHealth>();
            bool ifBlocking = other.gameObject.GetComponent<EnemyHealth>().blockingPlayer;
            if (health != null)
            {
                if (health.enemyClass == EnemyClass.Priest)
                {
                    bool ifAgro = other.gameObject.GetComponent<Priest>().isAgro;
                    if (!ifAgro)
                    {
                        health.TakeDamage(100 + damage.GetValue());
                        //Debug.Log("Sneak kill priest.");
                    }
                    else
                    {
                        health.TakeDamage(20 + damage.GetValue());
                        //Debug.Log("Hit priest.");
                    }
                }


                if (health.enemyClass == EnemyClass.Knight)
                {
                    bool ifAgro = other.gameObject.GetComponent<Knight>().isAgro;
                    if (!ifBlocking && !ifAgro)
                    {
                        health.TakeDamage(100 + damage.GetValue());
                        //Debug.Log("Sneak kill knight.");
                    }
                    else if (!ifBlocking)
                    {
                        health.TakeDamage(20 + damage.GetValue());
                        //Debug.Log("Hit knight.");
                    }
                }


                if (health.enemyClass == EnemyClass.Peasant)
                {
                    bool ifAgro = other.gameObject.GetComponent<Peasant>().isAgro;
                    if (!ifAgro)
                    {
                        health.TakeDamage(100 + damage.GetValue());
                        //Debug.Log("Sneak kill peasant.");
                    }
                    else
                    {
                        health.TakeDamage(20 + damage.GetValue());
                        //Debug.Log("Hit peasant.");
                    }
                }
            }
        }
    }
}