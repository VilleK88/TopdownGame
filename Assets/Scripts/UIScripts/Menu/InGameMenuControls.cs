using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameMenuControls : MonoBehaviour
{
    public Player player;
    public GameObject settingsUI;
    Scene currentScene;

    private void Start()
    {
        currentScene = SceneManager.GetActiveScene();
    }

    public void StartGame()
    {
        //PlayerPrefs.DeleteKey("ItemCollected");
        //PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("2 - LallisHouse");
    }

    public void Save()
    {
        if (currentScene.name != "1 - Menu")
        {
            SaveSceneID();
            player.GetComponent<Player>().SavePlayerTransformPosition();
            //GameManager.manager.loadPlayerPosition = true;
            GameManager.manager.Save();
            PlayerManager.instance.player.GetComponent<Player>().isPaused = false;
            PlayerManager.instance.player.GetComponent<Player>().menuButtons.gameObject.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void SaveSceneID()
    {
        if (currentScene.name == "2 - LallisHouse")
        {
            GameManager.manager.savedSceneID = 2;
        }
        if (currentScene.name == "3 - Village")
        {
            GameManager.manager.savedSceneID = 3;
        }
        if (currentScene.name == "4 - Fields")
        {
            GameManager.manager.savedSceneID = 4;
        }
        if (currentScene.name == "5 - DeepForest")
        {
            GameManager.manager.savedSceneID = 5;
        }
        if (currentScene.name == "6 - Fortress")
        {
            GameManager.manager.savedSceneID = 6;
        }
        if (currentScene.name == "7 - FrozenLake")
        {
            GameManager.manager.savedSceneID = 7;
        }
    }

    public void LoadSaveSceneID()
    {
        if (GameManager.manager.savedSceneID == 2)
        {
            SceneManager.LoadScene("2 - LallisHouse");
        }
        if (GameManager.manager.savedSceneID == 3)
        {
            SceneManager.LoadScene("3 - Village");
        }
        if (GameManager.manager.savedSceneID == 4)
        {
            SceneManager.LoadScene("4 - Fields");
        }
        if (GameManager.manager.savedSceneID == 5)
        {
            SceneManager.LoadScene("5 - DeepForest");
        }
        if (GameManager.manager.savedSceneID == 6)
        {
            SceneManager.LoadScene("6 - Fortress");
        }
        if (GameManager.manager.savedSceneID == 7)
        {
            SceneManager.LoadScene("7 - FrozenLake");
        }
    }

    public void Load()
    {
        GameObject questCanvas = GameObject.Find("QuestCanvas");
        if(questCanvas != null)
        {
            Destroy(questCanvas);
        }
        else
        {
            Debug.LogError("QuestCanvas object not found!");
        }

        GameManager.manager.Load();
        LoadSaveSceneID();

        PlayerManager.instance.player.GetComponent<Player>().isPaused = false;
        Time.timeScale = 1;
    }

    public void Settings()
    {
        settingsUI.SetActive(true);
    }

    public void QuitGame()
    {
        SceneManager.LoadScene("0 - Loading");
        //Application.Quit();
    }
}
