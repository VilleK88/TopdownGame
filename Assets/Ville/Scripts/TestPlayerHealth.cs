using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestPlayerHealth : MonoBehaviour
{
    Animator anim;

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

    public bool playerBlockFetch;

    private void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
        spriteRend = playerSprite.GetComponent<SpriteRenderer>();
        originalColor = spriteRend.color;
    }

    private void Update()
    {
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();
        /*if(Input.GetKeyDown(KeyCode.A))
        {
            TakeDamage(Random.Range(5, 10));
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            RestoreHealth(Random.Range(5, 10));
        }*/
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
        currentHealth -= damage;
        lerpTimer = 0;
        StartCoroutine(Invulnerability());
    }

    public void RestoreHealth(float healAmount)
    {
        currentHealth += healAmount;
        lerpTimer = 0;
    }

    /*public void TakeDamage(float damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, startingHealth);

        if (currentHealth > 0 && playerBlockFetch)
        {

        }
        else if (currentHealth > 0)
        {
            StartCoroutine(Invulnerability());
        }
    }*/

    /*public void AddHealth(float value)
    {
        currentHealth = Mathf.Clamp(currentHealth + value, 0, startingHealth);
    }*/

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
}
