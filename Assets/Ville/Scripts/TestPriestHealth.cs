using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPriestHealth : MonoBehaviour
{
    Animator anim;
    [SerializeField] GameObject enemySprite;

    //float startingHealth = 100;
    float maxHealth = 30;
    public float currentHealth; //{ get; set; }

    public bool dead = false;

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

        if (currentHealth <= 0)
        {
            // die
            dead = true;
        }
        else
        {

        }
    }

    public void RestoreHealth(float healAmount)
    {
        currentHealth += healAmount;
    }
}
