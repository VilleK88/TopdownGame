using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyClass
{
    Knight, Peasant, Priest, Dog, Longbowman, Bear, Bishop
}

public class EnemyHealth : MonoBehaviour
{
    Animator anim;
    [SerializeField] GameObject enemySprite;

    [Header("Enemy Class/Profile")]
    public EnemyClass enemyClass;
    public EnemyProfile enemyProfile;

    [Header("Health")]
    float maxHealth = 100;
    public float currentHealth;
    public bool dead = false;

    [Header("Blocking Or Getting Hit")]
    public bool playerBlockFetch;
    int blockOrNot;
    public bool blockingPlayer; // this is fetched from the Enemy -script
    int gettingHitOrNot;
    public bool gettingHit = false;
    float blockingTime;

    [Header("Player XP")]
    float expAmount;

    [Header("Audio")]
    [SerializeField] AudioClip hitSound;
    [SerializeField] AudioClip blockHitSound;
    [SerializeField] AudioClip dieSound;

    [Header("Enemy ID Info")]
    public int enemyID;
    public int questEnemyID;

    [Header("Loot Items")]
    [SerializeField] ItemPickup healthPotion;
    [SerializeField] ItemPickup staminaPotion;

    [SerializeField] Animator bloodSplatterAnim;

    private void Start()
    {
        //currentHealth = maxHealth;
        anim = enemySprite.GetComponent<Animator>();
        HowMuchXp();
    }

    private void Update()
    {
        if(enemyClass == EnemyClass.Bear)
        {
            currentHealth = Mathf.Clamp(currentHealth, 0, currentHealth);
        }
        else if(enemyClass == EnemyClass.Bishop)
        {
            currentHealth = Mathf.Clamp(currentHealth, 0, currentHealth);
        }
        else
        {
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        }
    }

    void HowMuchXp()
    {
        if (enemyClass == EnemyClass.Knight)
        {
            expAmount = 25;
        }
        if(enemyClass == EnemyClass.Longbowman)
        {
            expAmount = 25;
        }
        if (enemyClass == EnemyClass.Peasant)
        {
            expAmount = 15;
        }
        if (enemyClass == EnemyClass.Priest)
        {
            expAmount = 50;
        }
        if(enemyClass == EnemyClass.Bear)
        {
            expAmount = 100;
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        gettingHit = true;
        if (currentHealth <= 0)
        {
            Die();
            AudioManager.instance.PlaySound(dieSound);
            dead = true;
        }
        else
        {
            if(enemyClass == EnemyClass.Knight)
            {
                blockOrNot = Random.Range(0, 2);
                if (blockOrNot == 0)
                {
                    AudioManager.instance.PlaySound(blockHitSound);
                    blockingPlayer = true;
                    anim.SetBool("CrusaderBlock", true);
                    blockingTime = Random.Range(1, 2);
                    StartCoroutine(StopBlocking());
                }
                else
                {
                    AudioManager.instance.PlaySound(hitSound);
                    bloodSplatterAnim.SetTrigger("Hit");
                    gettingHit = false;
                }
            }
            else if(enemyClass == EnemyClass.Priest)
            {
                AudioManager.instance.PlaySound(hitSound);
                bloodSplatterAnim.SetTrigger("Hit");
            }
            else if(enemyClass == EnemyClass.Peasant)
            {
                AudioManager.instance.PlaySound(hitSound);
                bloodSplatterAnim.SetTrigger("Hit");
                gettingHitOrNot = Random.Range(0, 1);
                if (gettingHitOrNot == 0)
                {
                    gettingHit = true;
                    StartCoroutine(StopGettingHit());
                }
            }
            else if(enemyClass == EnemyClass.Bishop)
            {
                AudioManager.instance.PlaySound(hitSound);
                bloodSplatterAnim.SetTrigger("Hit");
                gettingHit = true;
                StartCoroutine(StopGettingHit());
            }
        }
    }

    public void RestoreHealth(float healAmount)
    {
        currentHealth += healAmount;
    }

    IEnumerator StopGettingHit()
    {
        yield return new WaitForSeconds(1);
        gettingHit = false;
    }

    IEnumerator StopBlocking()
    {
        yield return new WaitForSeconds(blockingTime);
        anim.SetBool("CrusaderBlock", false);
        blockingPlayer = false;
        gettingHit = false;
    }

    public void Die()
    {
        if(GameManager.manager.onEnemyDeathCallBack != null)
        {
            GameManager.manager.onEnemyDeathCallBack.Invoke(enemyProfile);
        }
        ExperienceManager.instance.AddExperience(expAmount);
        AddQuestEnemyIDToArray(this.questEnemyID);
        SpawnLoot();
    }

    void AddQuestEnemyIDToArray(int newQuestEnemyID)
    {
        int[] newQuestEnemyIDs = new int[GameManager.manager.questEnemyIDs.Length + 1];
        for(int i = 0; i < GameManager.manager.questEnemyIDs.Length; i++)
        {
            newQuestEnemyIDs[i] = GameManager.manager.questEnemyIDs[i];
        }
        newQuestEnemyIDs[GameManager.manager.questEnemyIDs.Length] = newQuestEnemyID;
        GameManager.manager.questEnemyIDs = newQuestEnemyIDs;
    }

    void SpawnLoot()
    {
        int itemToSpawn = Random.Range(0, 2);
        if(itemToSpawn == 0)
        {
            Instantiate(healthPotion, transform.position, Quaternion.identity);
        }
        else if(itemToSpawn == 1)
        {
            Instantiate(staminaPotion, transform.position, Quaternion.identity);
        }
    }
}
