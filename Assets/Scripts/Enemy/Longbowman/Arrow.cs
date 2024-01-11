using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    float arrowSpeed = 12;
    float lifeTime = 4;

    private void Start()
    {
        Invoke("DestroyArrowPrefab", lifeTime);
    }

    private void FixedUpdate()
    {
        transform.Translate(Vector3.forward * arrowSpeed * Time.deltaTime);
    }

    void DestroyArrowPrefab()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth health = collision.gameObject.GetComponent<PlayerHealth>();
            Player player = collision.gameObject.GetComponent<Player>();
            if (health != null && !player.ukko)
            {
                health.TakeDamage(10);
                Destroy(gameObject);
            }
            if(player != null && !player.ukko)
            {
                if(player.blocking)
                {
                    GameManager.manager.currentStamina -= 10;
                    Destroy(gameObject);
                }
            }
        }
    }
}
