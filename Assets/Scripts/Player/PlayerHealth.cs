using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    //public float maxHealth = 100;
    //public float currentHealth; //{ get; set; }
    float chipSpeed = 2;
    float lerpTimer;
    public Image frontHealthbar;
    public Image backHealthbar;

    [Header("Components")]
    [SerializeField] Behaviour[] components;

    [Header("iFrames")]
    [SerializeField] float iFramesDuration = 2;
    [SerializeField] int numberOfFlashes = 2;
    [SerializeField] GameObject playerSprite;
    SpriteRenderer spriteRend;
    Color originalColor;

    [SerializeField] GameObject player;
    public bool blockingFetch; // from Player -script

    public Stat damage;
    public Stat armor;

    [Header("Audio")]
    [SerializeField] AudioClip hitSound;
    [SerializeField] AudioClip blockHitSound;
    [SerializeField] AudioClip dieSound;

    public bool dead;

    [SerializeField] Animator splatterAnim;

    [Header("Upgrades")]
    float hrMaxTime = 20;
    public float hrCounter = 20;
    public bool regenHealth = false;
    float chargeMaxTime = 5;
    public float chargeCounter = 0;

    private void Start()
    {
        //currentHealth = maxHealth;
        dead = false;
        spriteRend = playerSprite.GetComponent<SpriteRenderer>();
        originalColor = spriteRend.color;
    }

    private void Update()
    {
        blockingFetch = player.GetComponent<Player>().blocking;
        GameManager.manager.currentHealth = Mathf.Clamp(GameManager.manager.currentHealth, 0,
            GameManager.manager.maxHealth);

        if (GameManager.manager.healthRegen)
        {
            if (hrCounter >= hrMaxTime)
            {
                if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    regenHealth = true;
                    hrCounter = 0;
                }
            }
            else
            {
                hrCounter += Time.deltaTime;
            }
        }

        if (regenHealth)
        {
            if(GameManager.manager.currentHealth < GameManager.manager.maxHealth)
            {
                GameManager.manager.currentHealth += 20 * Time.deltaTime;
                if (GameManager.manager.currentHealth > GameManager.manager.maxHealth)
                {
                    GameManager.manager.currentHealth = GameManager.manager.maxHealth;
                }
            }

            if(chargeMaxTime >= chargeCounter)
            {
                chargeCounter += Time.deltaTime;
            }
            else
            {
                regenHealth = false;
                hrCounter = 0;
                chargeCounter = 0;
            }
        }

        UpdateHealthUI();
    }

    public void UpdateHealthUI()
    {
        float fillF = frontHealthbar.fillAmount;
        float fillB = backHealthbar.fillAmount;
        float hFraction = GameManager.manager.currentHealth / GameManager.manager.maxHealth;
        if(fillB > hFraction)
        {
            frontHealthbar.fillAmount = hFraction;
            backHealthbar.color = Color.red;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            backHealthbar.fillAmount = Mathf.Lerp(fillB, hFraction, percentComplete);
        }
        if(fillF < hFraction)
        {
            backHealthbar.fillAmount = hFraction;
            backHealthbar.color = Color.green;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            frontHealthbar.fillAmount = Mathf.Lerp(fillF, backHealthbar.fillAmount, percentComplete);
        }
    }

    public void TakeDamage(int damage)
    {
        if(GameManager.manager.currentHealth <= 0)
        {
            // die
            Die();
        }
        else if(blockingFetch)
        {
            AudioManager.instance.PlaySound(blockHitSound);
        }
        else
        {
            AudioManager.instance.PlaySound(hitSound);
            splatterAnim.SetTrigger("Hit");

            if(damage > armor.GetValue())
            {
                damage -= armor.GetValue();
                GameManager.manager.currentHealth -= damage;
            }
            else if(damage < armor.GetValue())
            {

            }
            //currentHealth -= damage;
            StartCoroutine(Invulnerability());
        }

        lerpTimer = 0;
    }

    public void RestoreHealth(int healAmount)
    {
        GameManager.manager.currentHealth += healAmount;
        lerpTimer = 0;
    }

    private IEnumerator Invulnerability()
    {
        Physics2D.IgnoreLayerCollision(6, 10, true);
        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRend.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
            spriteRend.color = originalColor;
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
        }
        Physics2D.IgnoreLayerCollision(6, 10, false);
    }

    public virtual void Die()
    {
        //base.Die();
        //PlayerManager.instance.KillPlayer();
        dead = true;
        AudioManager.instance.PlaySound(dieSound);
        GameOver.instance.GameOverScreenOn();
    }

    public void IncreaseHealth(int level)
    {
        GameManager.manager.maxHealth += (GameManager.manager.currentHealth * 0.01f) * ((50 - level) * 0.1f);
        GameManager.manager.currentHealth = GameManager.manager.maxHealth;
    }
}
