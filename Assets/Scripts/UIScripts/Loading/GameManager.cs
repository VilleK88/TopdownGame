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
    public int skillPoints;

    public bool healthRegen;
    public bool healthPotionPlus;
    public bool healthPlusFirst;
    public bool healthPlusSecond;

    public bool smashUpgrade;
    public bool spin;

    public bool staminaRegen;
    public bool staminaPotionPlus;
    public bool staminaPlusFirst;
    public bool staminaPlusSecond;

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

    public delegate void OnCharacterSaveCallBack(CharacterProfile characterProfile);
    public OnCharacterSaveCallBack onCharacterSaveCallBack;

    public QuestBase[] triggeredQuests;
    public QuestBase[] rewardReadyQuests;
    public QuestBase[] completedQuests;

    public int[] triggeredQuestIDs;
    public int[] rewardReadyQuestIDs;
    public int[] completedQuestIDs;

    public int[] pickUpItemIDs;
    public int healthPotions;
    public int staminaPotions;
    public int xpPotions;
    public bool woodenAxeCollected;
    public int weaponSlot;

    public int[] enemyIDs;
    public int[] questEnemyIDs;

    public bool isGameLoaded = false;

    public int savedSceneID;
    public bool loadPlayerPosition = false;


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
        data.skillPoints = skillPoints;

        data.healthRegen = healthRegen;
        data.healthPotionPlus = healthPotionPlus;
        data.healthPlusFirst = healthPlusFirst;
        data.healthPlusSecond = healthPlusSecond;

        data.smashUpgrade = smashUpgrade;
        data.spin = spin;

        data.staminaRegen = staminaRegen;
        data.staminaPotionPlus = staminaPotionPlus;
        data.staminaPlusFirst = staminaPlusFirst;
        data.staminaPlusSecond = staminaPlusSecond;

        data.health = health;
        data.currentHealth = currentHealth;
        data.maxHealth = maxHealth;

        data.currentStamina = currentStamina;
        data.maxStamina = maxStamina;

        data.x = x;
        data.y = y;
        data.z = z;

        data.pickUpItemIDs = pickUpItemIDs;
        data.healthPotions = healthPotions;
        data.staminaPotions = staminaPotions;
        data.xpPotions = xpPotions;

        data.woodenAxeCollected = woodenAxeCollected;
        data.weaponSlot = weaponSlot;

        data.enemyIDs = enemyIDs;
        data.questEnemyIDs = questEnemyIDs;

        data.triggeredQuestIDs = triggeredQuestIDs;
        data.rewardReadyQuestIDs = rewardReadyQuestIDs;
        data.completedQuestIDs = completedQuestIDs;

        data.savedSceneID = savedSceneID;
        data.loadPlayerPosition = false;

        // Serialisoidaan GameData objekti, joka tallennetaan samalla tiedostoon.
        bf.Serialize(file, data);
        file.Close(); // Suljetaan tiedosto, ettei kukaan hakkeri p‰‰se siihen k‰siksi.
    }

    public void Load()
    {
        // Muista aina kun lataat tiedostoa mist‰ tahansa, tarkista ett‰ se on edes olemassa.
        // Jos on, niin sitten vasta jatka prosessia.
        if (File.Exists(Application.persistentDataPath + "/gameInfo.dat"))
        {
            //Debug.Log("Game Loaded!");
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
            skillPoints = data.skillPoints;

            healthRegen = data.healthRegen;
            healthPotionPlus = data.healthPotionPlus;
            healthPlusFirst = data.healthPlusFirst;
            healthPlusSecond = data.healthPlusSecond;

            smashUpgrade = data.smashUpgrade;
            spin = data.spin;

            staminaRegen = data.staminaRegen;
            staminaPotionPlus = data.staminaPotionPlus;
            staminaPlusFirst = data.staminaPlusFirst;
            staminaPlusSecond = data.staminaPlusSecond;

            health = data.health;
            currentHealth = data.currentHealth;
            maxHealth = data.maxHealth;

            currentStamina = data.currentStamina;
            maxStamina = data.maxStamina;

            x = data.x;
            y = data.y;
            z = data.z;

            pickUpItemIDs = data.pickUpItemIDs;
            healthPotions = data.healthPotions;
            staminaPotions = data.staminaPotions;
            xpPotions = data.xpPotions;

            woodenAxeCollected = data.woodenAxeCollected;
            weaponSlot = data.weaponSlot;

            enemyIDs = data.enemyIDs;
            questEnemyIDs = data.questEnemyIDs;

            triggeredQuestIDs = data.triggeredQuestIDs;
            rewardReadyQuestIDs = data.rewardReadyQuestIDs;
            completedQuestIDs = data.completedQuestIDs;

            isGameLoaded = true;

            savedSceneID = data.savedSceneID;
            //loadPlayerPosition = data.loadPlayerPosition;
            loadPlayerPosition = true;
            //QuestManager.questManager.RemoveCompletedQuestIDs();
            //QuestManager.questManager.RemoveAllCompletedQuestsOnStart();
        }
    }
}

// Toinen luokka, joka voidaan serialisoida. Pit‰‰ sis‰ll‰‰n vaan sen datan mit‰ halutaan serialisoida ja tallentaa.

[Serializable]
class GameData
{
    public int currentLevel;
    public float currentExperience;
    public float maxExperience;
    public int skillPoints;

    public bool healthRegen;
    public bool healthPotionPlus;
    public bool healthPlusFirst;
    public bool healthPlusSecond;

    public bool smashUpgrade;
    public bool spin;

    public bool staminaRegen;
    public bool staminaPotionPlus;
    public bool staminaPlusFirst;
    public bool staminaPlusSecond;

    public float health;
    public float currentHealth;
    public float maxHealth;

    public float currentStamina;
    public float maxStamina;

    public float x;
    public float y;
    public float z;

    public string[] dialogueTriggerNames;

    public int[] triggeredQuestIDs;
    public int[] rewardReadyQuestIDs;
    public int[] completedQuestIDs;

    public int[] pickUpItemIDs;
    public int healthPotions;
    public int staminaPotions;
    public int xpPotions;
    public bool woodenAxeCollected;
    public int weaponSlot;

    public int[] enemyIDs;
    public int[] questEnemyIDs;

    public int savedSceneID;
    public bool loadPlayerPosition = false;
}
