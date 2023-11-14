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

    public int currentLevel;
    public float currentExperience;
    public float maxExperience;

    public float health;
    public float currentHealth;
    public float maxHealth;

    public float currentStamina;
    public float maxStamina;

    public float x;
    public float y;
    public float z;

    public delegate void OnEnemyDeathCallBack(EnemyProfile enemyProfile);
    public OnEnemyDeathCallBack onEnemyDeathCallBack;


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

        BinaryFormatter bf = new BinaryFormatter(); // Tehd��n uusi olio tai instanssi luokasta BinaryFormatter
        FileStream file = File.Create(Application.persistentDataPath + "/gameInfo.dat");
        GameData data = new GameData();
        //data.currentLevel = currentLevel;
        data.currentLevel = currentLevel;
        data.currentExperience = currentExperience;
        data.maxExperience = maxExperience;
        data.health = health;
        data.currentHealth = currentHealth;
        data.maxHealth = maxHealth;
        data.currentStamina = currentStamina;
        data.maxStamina = maxStamina;
        data.x = x;
        data.y = y;
        data.z = z;
        //data.Level1 = Level1;
        //data.Level2 = Level2;
        //data.Level3 = Level3;
        // Serialisoidaan GameData objekti, joka tallennetaan samalla tiedostoon.
        bf.Serialize(file, data);
        file.Close(); // Suljetaan tiedosto, ettei kukaan hakkeri p��se siihen k�siksi.
    }

    public void Load()
    {
        // Muista aina kun lataat tiedostoa mist� tahansa, tarkista ett� se on edes olemassa.
        // Jos on, niin sitten vasta jatka prosessia.
        if (File.Exists(Application.persistentDataPath + "/gameInfo.dat"))
        {
            Debug.Log("Game Loaded!");
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gameInfo.dat", FileMode.Open);
            // Deserialisoidaan ja muunnetaan data GameData -muotoon. Me tied�mme, ett� data on GameData objectin informaatio
            GameData data = (GameData)bf.Deserialize(file);
            // T�rke�. Muista sulkea tiedosto, ettei hakkerit p��se k�siksi.
            file.Close();

            // Kun tieto on ladattu data objektiin, siirret��n muuttujien arvot Game Manager:in muuttujiin.

            //currentLevel = data.currentLevel;
            currentLevel = data.currentLevel;
            currentExperience = data.currentExperience;
            maxExperience = data.maxExperience;
            health = data.health;
            currentHealth = data.currentHealth;
            maxHealth = data.maxHealth;
            currentStamina = data.currentStamina;
            maxStamina = data.maxStamina;
            x = data.x;
            y = data.y;
            z = data.z;
            //Level1 = data.Level1;
            //Level2 = data.Level2;
            //Level3 = data.Level3;
        }
    }
}

// Toinen luokka, joka voidaan serialisoida. Pit�� sis�ll��n vaan sen datan mit� halutaan serialisoida ja tallentaa.

[Serializable]
class GameData
{
    //public string currentLevel;
    public int currentLevel;
    public float currentExperience;
    public float maxExperience;

    public float health;
    public float currentHealth;
    public float maxHealth;

    public float currentStamina;
    public float maxStamina;

    public float x;
    public float y;
    public float z;
    //public bool Level1;
    //public bool Level2;
    //public bool Level3;
}
