using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameManager : MonoBehaviour
{
    public static GameManager manager;
    public float health;
    public float previousHealth;
    public float maxHealth;


    private void Awake()
    {
        if(manager == null)
        {
            DontDestroyOnLoad(gameObject);
            manager = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            SceneManager.LoadScene("Menu");
        }
    }

    public void Save()
    {
        Debug.Log("Game Saved!");

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gameInfo.dat");
    }
}

[Serializable]

class GameData
{

}
