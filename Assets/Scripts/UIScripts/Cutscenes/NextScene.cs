using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    public string sceneName;
    public float time;

    private void Start()
    {
        StartCoroutine(Count());
    }

    IEnumerator Count()
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(sceneName);
        GameManager.manager.currentHealth = GameManager.manager.maxHealth;
    }
}
