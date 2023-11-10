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

    private void Start()
    {
        //currentHealth = maxHealth;
        spriteRend = playerSprite.GetComponent<SpriteRenderer>();
        originalColor = spriteRend.color;
    }

    private void Update()
    {
        blockingFetch = player.GetComponent<Player>().blocking;
        GameManager.manager.currentHealth = Mathf.Clamp(GameManager.manager.currentHealth, 0,
            GameManager.manager.maxHealth);
        UpdateHealthUI();

        /*if (Input.GetKeyDown(KeyCode.T))
        {
            TakeDamage(10);
        }*/
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

        }
        else
        {
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
        PlayerManager.instance.KillPlayer();
    }

    public void IncreaseHealth(int level)
    {
        GameManager.manager.maxHealth += (GameManager.manager.currentHealth * 0.01f) * ((50 - level) * 0.1f);
        GameManager.manager.currentHealth = GameManager.manager.maxHealth;
    }
}
