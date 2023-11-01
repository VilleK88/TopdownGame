using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControl : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("VilleScene");
    }

    public void Save()
    {
        GameManager.manager.Save();
    }

    public void Load()
    {
        //GameManager.manager.Load();
    }
}
