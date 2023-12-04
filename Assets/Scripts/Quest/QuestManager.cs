using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class QuestManager : MonoBehaviour
{
    #region Singleton
    public static QuestManager questManager;

    private void Awake()
    {
        if(questManager == null)
        {
            questManager = this;
        }
    }
    #endregion

    GameManager manager;

    public GameObject questUI;
    public TextMeshProUGUI questName;
    public TextMeshProUGUI questDescription;
    public Button questAcceptButton;

    public QuestBase currentQuest { get; set; }
    public QuestDialogueTrigger currentQuestDialogueTrigger { get; set; }
    public bool inQuestUI { get; set; }
    public int questIndex;

    [SerializeField] AudioClip questStartsSound;


    private void Start()
    {
        if (GameManager.manager != null && GameManager.manager.triggeredQuests != null)
        {
            foreach(QuestBase quest in GameManager.manager.triggeredQuests)
            {
                quest.initializeQuest(); // a reminder to yourself: if an error occurs it's propably because there are no quests to initialize?
            }
        }

        if (GameManager.manager != null && GameManager.manager.completedQuests != null)
        {
            foreach (QuestBase quest in GameManager.manager.completedQuests)
            {
                quest.InitializeCompletedQuest();
            }
        }

        RemoveEnemiesForCompletedQuests();
    }



    public List<string> GetCompletedQuestNames()
    {
        List<string> completedQuestNames = new List<string>();

        if(GameManager.manager.completedQuests != null)
        {
            foreach (QuestBase quest in GameManager.manager.completedQuests)
            {
                completedQuestNames.Add(quest.name);
            }
        }

        return completedQuestNames;
    }

    public void RemoveEnemiesForCompletedQuests()
    {
        int[] completedQuestIDs = GameManager.manager.completedQuestIDs;

        EnemyHealth[] enemies = FindObjectsOfType<EnemyHealth>();
        foreach (EnemyHealth enemy in enemies)
        {
            if (completedQuestIDs != null && completedQuestIDs.Contains(enemy.questEnemyID))
            {
                Debug.Log("Removing enemy with questEnemyID: " + enemy.questEnemyID);
                DestroyEnemy(enemy);
            }
        }
    }

    void DestroyEnemy(EnemyHealth enemy)
    {
        enemy.Die();
        enemy.dead = true;
        enemy.gameObject.SetActive(false);
    }


    public void SetQuestUI(QuestBase newQuest)
    {
        inQuestUI = true;
        currentQuest = newQuest;
        questUI.SetActive(true);
        questName.text = newQuest.questName;
        questDescription.text = newQuest.questDescription;
        questAcceptButton.Select();
    }

    public void StartQuestBuffer()
    {
        StartCoroutine(QuestBuffer());
    }

    IEnumerator QuestBuffer()
    {
        yield return new WaitForSeconds(0.1f);
        inQuestUI = false;
    }

    public void QuestAcceptedSound()
    {
        AudioManager.instance.PlaySound(QuestManager.questManager.questStartsSound);
    }

    public void HasTriggeredQuest(QuestBase quest)
    {
        if(!GameManager.manager.triggeredQuests.Contains(quest))
        {
            AddQuestToArray(quest);
            AddQuestIDToArray(quest.questID);
        }
    }

    public void HasCompletedQuest(QuestBase quest)
    {
        GameManager.manager.completedQuests.Contains(quest);
    }

    void AddQuestToArray(QuestBase quest)
    {
        QuestBase[] newQuestArray = new QuestBase[GameManager.manager.triggeredQuests.Length + 1];
        for (int i = 0; i < GameManager.manager.triggeredQuests.Length; i++)
        {
            newQuestArray[i] = GameManager.manager.triggeredQuests[i];
        }
        newQuestArray[GameManager.manager.triggeredQuests.Length] = quest;
        GameManager.manager.triggeredQuests = newQuestArray;
    }

    void AddQuestIDToArray(int newTriggeredQuestID)
    {
        int[] newTriggeredQuestIDs = new int[GameManager.manager.triggeredQuestIDs.Length + 1];
        for(int i = 0; i < GameManager.manager.triggeredQuestIDs.Length; i++)
        {
            newTriggeredQuestIDs[i] = GameManager.manager.triggeredQuestIDs[i];
        }
        newTriggeredQuestIDs[GameManager.manager.triggeredQuestIDs.Length] = newTriggeredQuestID;
        GameManager.manager.triggeredQuestIDs = newTriggeredQuestIDs;
    }
}
