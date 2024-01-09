using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HenrikDeadManager : MonoBehaviour
{
    [SerializeField] GameObject bishopHenrik;
    bool deadFetch;

    private void Update()
    {
        deadFetch = bishopHenrik.GetComponent<EnemyHealth>().dead;
        if(deadFetch)
        {
            StartCoroutine(Count());
        }
    }

    IEnumerator Count()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("8 - GameCompleted");
        GameManager.manager.currentHealth = GameManager.manager.maxHealth;
    }
}
