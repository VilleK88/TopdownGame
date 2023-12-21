using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    #region Singleton
    public static GameOver instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    #endregion

    Animator anim;
    float maxTime = 5;
    float counter = 0;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    void GameOverScreenOn()
    {
        anim.SetBool("GameOver", true);

        if(maxTime >= counter)
        {
            counter += Time.deltaTime;
        }
        else
        {
            SceneManager.LoadScene("0 - Loading");
            GameManager.manager.currentHealth = GameManager.manager.maxHealth;
        }
    }

    void GameOverScreenOff()
    {
        anim.SetBool("GameOver", false);
    }
}
