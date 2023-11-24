using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControl : MonoBehaviour
{
    Scene currentScene;
    string savedSceneName = "";

    private void Start()
    {
        currentScene = SceneManager.GetActiveScene();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("VilleScene");
    }

    public void Save()
    {
        if(currentScene.name != "1 - Menu")
        {
            savedSceneName = currentScene.name;
            GameManager.manager.Save();
            PlayerManager.instance.player.GetComponent<Player>().isPaused = false;
            PlayerManager.instance.player.GetComponent<Player>().menuButtons.gameObject.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void Load()
    {
        if(currentScene.name != "1 - Menu")
        {
            GameManager.manager.Load();
            if (!string.IsNullOrEmpty(savedSceneName))
            {
                SceneManager.LoadScene(savedSceneName);
            }
        }
        else
        {
            GameManager.manager.Load();
            StartGame();
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
