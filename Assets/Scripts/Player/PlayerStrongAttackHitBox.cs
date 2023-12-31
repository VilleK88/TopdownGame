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
                    bool ifActiveBlocking = other.gameObject.GetComponent<Knight>().activeBlocking;
                    if (/*!ifBlocking*/ ! ifActiveBlocking && !ifAgro)
                    {
                        health.TakeDamage(100 + damage.GetValue());
                        //Debug.Log("Sneak kill knight.");
                    }
                    else if (/*!ifBlocking*/ !ifActiveBlocking)
                    {
                        health.TakeDamage(20 + damage.GetValue());
                        //Debug.Log("Hit knight.");
                    }
                }


                if (health.enemyClass == EnemyClass.Peasant)
                {
                    bool ifAgro = other.gameObject.GetComponent<Peasant>().isAgro;
                    bool ifConverted = other.gameObject.GetComponent<Peasant>().converted;
                    if (!ifAgro && ifConverted)
                    {
                        health.TakeDamage(100 + damage.GetValue());
                        //Debug.Log("Sneak kill peasant.");
                    }
                    else if(ifConverted)
                    {
                        health.TakeDamage(20 + damage.GetValue());
                        //Debug.Log("Hit peasant.");
                    }
                }

                if (health.enemyClass == EnemyClass.Bear)
                {
                    bool ifAgro = other.gameObject.GetComponent<Bear>().isAgro;
                    bool ifDead = other.gameObject.GetComponent<EnemyHealth>().dead;
                    if (!ifAgro && !ifDead)
                    {
                        health.TakeDamage(30 + damage.GetValue());
                    }
                    else if (!ifDead)
                    {
                        health.TakeDamage(20 + damage.GetValue());
                    }
                }

                if (health.enemyClass == EnemyClass.Bishop)
                {
                    bool ifAgro = other.gameObject.GetComponent<Bishop>().isAgro;
                    if (!ifAgro)
                    {
                        health.TakeDamage(30 + damage.GetValue());
                    }
                    else
                    {
                        health.TakeDamage(20 + damage.GetValue());
                    }
                }

                if (health.enemyClass == EnemyClass.Longbowman)
                {
                    bool ifAgro = other.gameObject.GetComponent<Longbowman>().isAgro;
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
            }
        }
    }
}
