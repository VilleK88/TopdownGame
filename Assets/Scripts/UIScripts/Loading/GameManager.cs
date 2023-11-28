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

    public DialogueTrigger[] allDialogueTriggers;

    public delegate void OnEnemyDeathCallBack(EnemyProfile enemyProfile);
    public OnEnemyDeathCallBack onEnemyDeathCallBack;

    public QuestBase[] triggeredQuests;
    public QuestBase[] completedQuestsReady;
    public QuestBase[] completedQuests;
    public Item[] items;
    public ItemPickup[] collectedItems;

    public bool isGameLoaded = false;


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

        allDialogueTriggers = FindObjectsOfType<DialogueTrigger>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            SceneManager.LoadScene("Menu");
        }
    }

    public void AddItemPickupToArray(ItemPickup itemPickup)
    {
        ItemPickup[] newItemPickupArray = new ItemPickup[GameManager.manager.collectedItems.Length + 1];
        for (int i = 0; i < GameManager.manager.collectedItems.Length; i++)
        {
            newItemPickupArray[i] = GameManager.manager.collectedItems[i];
        }
        newItemPickupArray[GameManager.manager.collectedItems.Length] = itemPickup;
        GameManager.manager.collectedItems = newItemPickupArray;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        allDialogueTriggers = FindObjectsOfType<DialogueTrigger>();
    }

    public void Save()
    {
        Debug.Log("Game Saved!");

        BinaryFormatter bf = new BinaryFormatter(); // Tehd‰‰n uusi olio tai instanssi luokasta BinaryFormatter
        FileStream file = File.Create(Application.persistentDataPath + "/gameInfo.dat");
        GameData data = new GameData();
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

        // Serialisoidaan GameData objekti, joka tallennetaan samalla tiedostoon.
        bf.Serialize(file, data);
        file.Close(); // Suljetaan tiedosto, ettei kukaan hakkeri p‰‰se siihen k‰siksi.

        string json = ToJson(triggeredQuests, true);
        File.WriteAllText(Application.persistentDataPath + "/quest.json", json);

        string jsonCompleted = ToJson(completedQuests, true);
        File.WriteAllText(Application.persistentDataPath + "/quest.jsonCompleted", jsonCompleted);

        string jsonItems = ToJson(items, true);
        File.WriteAllText(Application.persistentDataPath + "/item.jsonItems", jsonItems);

        string jsonCollectedItems = ToJson(collectedItems, true);
        File.WriteAllText(Application.persistentDataPath + "/item.jsonCollectedItems", jsonCollectedItems);
    }

    public void Load()
    {
        // Muista aina kun lataat tiedostoa mist‰ tahansa, tarkista ett‰ se on edes olemassa.
        // Jos on, niin sitten vasta jatka prosessia.
        if (File.Exists(Application.persistentDataPath + "/gameInfo.dat"))
        {
            Debug.Log("Game Loaded!");
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gameInfo.dat", FileMode.Open);
            // Deserialisoidaan ja muunnetaan data GameData -muotoon. Me tied‰mme, ett‰ data on GameData objectin informaatio
            GameData data = (GameData)bf.Deserialize(file);
            // T‰rke‰. Muista sulkea tiedosto, ettei hakkerit p‰‰se k‰siksi.
            file.Close();

            // Kun tieto on ladattu data objektiin, siirret‰‰n muuttujien arvot Game Manager:in muuttujiin.

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

            isGameLoaded = true;

            string json = File.ReadAllText(Application.persistentDataPath + "/quest.json");
            triggeredQuests = FromJson<QuestBase>(json);

            string jsonCompleted = File.ReadAllText(Application.persistentDataPath + "/quest.jsonCompleted");
            completedQuests = FromJson<QuestBase>(jsonCompleted);

            string jsonItems = File.ReadAllText(Application.persistentDataPath + "/item.jsonItems");
            items = FromJson<Item>(jsonItems);

            string jsonCollectedItems = File.ReadAllText(Application.persistentDataPath + "/item.jsonCollectedItems");
            collectedItems = FromJson<ItemPickup>(jsonCollectedItems);
        }
    }

    public static string ToJson<T>(T[] array, bool prettyPrint = false)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Quests = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Quests;
    }

    [System.Serializable]
    class Wrapper<T>
    {
        public T[] Quests;
    }
}

// Toinen luokka, joka voidaan serialisoida. Pit‰‰ sis‰ll‰‰n vaan sen datan mit‰ halutaan serialisoida ja tallentaa.

[Serializable]
class GameData
{
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

    public string[] dialogueTriggerNames;

    public QuestBase[] triggeredQuests;
    public QuestBase[] completedQuestsReady;
    public QuestBase[] completedQuests;

    public ItemPickup[] collectedItems;
}
