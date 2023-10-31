using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyClass
{
    Knight, Peasant, Priest, Dog
}

public class TestEnemyHealth : MonoBehaviour
{
    Animator anim;
    [SerializeField] GameObject enemySprite;
    public EnemyClass enemyClass;

    float startingHealth = 100;
    float maxHealth = 100;
    public float currentHealth; //{ get; set; }
    public bool dead = false;

    public bool playerBlockFetch;
    int blockOrNot;
    public bool blockingPlayer; // this is fetched from the Enemy -script
    int gettingHitOrNot;
    public bool gettingHit = false;

    float expAmount = 100;

    private void Start()
    {
        currentHealth = maxHealth;
        anim = enemySprite.GetComponent<Animator>();
    }

    private void Update()
    {
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if(currentHealth <= 0)
        {
            Die();
            dead = true;
        }
        else
        {
            if(enemyClass == EnemyClass.Knight)
            {
                blockOrNot = Random.Range(0, 1);
                if (blockOrNot == 0)
                {
                    blockingPlayer = true;
                    anim.SetBool("PikeBlock", true);
                    StartCoroutine(StopBlocking());
                }
            }
            else if(enemyClass == EnemyClass.Priest)
            {

            }
            else if(enemyClass == EnemyClass.Peasant)
            {
                gettingHitOrNot = Random.Range(0, 1);
                if (gettingHitOrNot == 0)
                {
                    gettingHit = true;
                    StartCoroutine(StopGettingHit());
                }
            }
        }
    }

    public void RestoreHealth(float healAmount)
    {
        currentHealth += healAmount;
    }

    IEnumerator StopGettingHit()
    {
        yield return new WaitForSeconds(2);
        gettingHit = false;
    }

    IEnumerator StopBlocking()
    {
        yield return new WaitForSeconds(2);
        anim.SetBool("PikeBlock", false);
        blockingPlayer = false;
    }

    void Die()
    {
        ExperienceManager.instance.AddExperience(expAmount);
    }
}
