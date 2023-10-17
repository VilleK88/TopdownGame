using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerHitbox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            TestEnemyHealth health = other.gameObject.GetComponent<TestEnemyHealth>();
            if(health != null)
            {
                health.TakeDamage(10);
            }
        }
    }
}
