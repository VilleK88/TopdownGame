using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestPlayerHealth : MonoBehaviour
{
    float startingHealth = 100;
    float maxHealth = 100;
    public float currentHealth; //{ get; set; }
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
        currentHealth = maxHealth;
        spriteRend = playerSprite.GetComponent<SpriteRenderer>();
        originalColor = spriteRend.color;
    }

    private void Update()
    {
        blockingFetch = player.GetComponent<TestPlayer>().blocking;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();

        if (Input.GetKeyDown(KeyCode.T))
        {
            TakeDamage(10);
        }
    }

    public void UpdateHealthUI()
    {
        Debug.Log(currentHealth);
        float fillF = frontHealthbar.fillAmount;
        float fillB = backHealthbar.fillAmount;
        float hFraction = currentHealth / maxHealth;
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

    public void TakeDamage(float damage)
    {
        if(currentHealth <= 0)
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
                currentHealth -= damage;
            }
            else if(damage < armor.GetValue())
            {

            }
            //currentHealth -= damage;
            StartCoroutine(Invulnerability());
        }

        lerpTimer = 0;
    }

    public void RestoreHealth(float healAmount)
    {
        currentHealth += healAmount;
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
}
