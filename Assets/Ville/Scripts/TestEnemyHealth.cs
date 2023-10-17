using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyHealth : MonoBehaviour
{
    Animator anim;

    float startingHealth = 100;
    float maxHealth = 100;
    public float currentHealth; //{ get; set; }
    float chipSpeed = 2;
    float lerpTimer;

    [Header("Components")]
    [SerializeField] Behaviour[] components;

    [Header("iFrames")]
    [SerializeField] float iFramesDuration = 2;
    [SerializeField] int numberOfFlashes = 2;
    SpriteRenderer spriteRend;

    public bool playerBlockFetch;

    private void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
    }

    public void RestoreHealth(float healAmount)
    {
        currentHealth += healAmount;
    }

    private IEnumerator Invulnerability()
    {
        Physics2D.IgnoreLayerCollision(10, 11, true);
        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRend.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 4));
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 4));
        }
        Physics2D.IgnoreLayerCollision(10, 11, false);
    }
}
