using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControl : MonoBehaviour
{
    Scene currentScene;

    private void Start()
    {
        currentScene = SceneManager.GetActiveScene();
    }

    public void StartGame()
    {
        //PlayerPrefs.DeleteKey("ItemCollected");
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("2 - LallisHouse");
        //SceneManager.LoadScene("VilleScene");
    }

    public void Save()
    {
        if(currentScene.name != "1 - Menu")
        {
            SaveSceneID();
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
    }

    public void LoadSaveSceneID()
    {
        if(GameManager.manager.savedSceneID == 2)
        {
            SceneManager.LoadScene("2 - LallisHouse");
        }
        if(GameManager.manager.savedSceneID == 3)
        {
            SceneManager.LoadScene("3 - Village");
        }
    }

    public void Load()
    {
        GameManager.manager.Load();
        LoadSaveSceneID();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
