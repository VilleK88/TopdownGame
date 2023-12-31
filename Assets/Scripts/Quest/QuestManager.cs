using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using Unity.VisualScripting;
using System;

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

    public QuestBase[] allQuests;

    public QuestBase currentQuest { get; set; }
    public QuestDialogueTrigger currentQuestDialogueTrigger { get; set; }
    public bool inQuestUI { get; set; }
    public int questIndex;

    [SerializeField] AudioClip questStartsSound;


    private void Start()
    {
        RemoveCompletedQuestIDs();
        RemoveAllCompletedQuestsOnStart();
        //RemoveEnemiesForRewardReadyQuests();
        //RemoveEnemiesForCompletedQuests();
    }

    public void RemoveAllCompletedQuestsOnStart()
    {
        if (GameManager.manager != null && GameManager.manager.completedQuestIDs != null)
        {
            foreach (int completedQuestID in GameManager.manager.completedQuestIDs)
            {
                for (int i = 0; i < allQuests.Length; i++)
                {
                    if (allQuests[i].questID == completedQuestID)
                    {
                        if (!GameManager.manager.completedQuests.Contains(allQuests[i]))
                        {
                            AddQuestToCompletedArray(allQuests[i]);
                        }
                    }
                }
            }
        }

        if (GameManager.manager != null && GameManager.manager.rewardReadyQuestIDs != null)
        {
            foreach (int rewardReadyQuestID in GameManager.manager.rewardReadyQuestIDs)
            {
                for (int i = 0; i < allQuests.Length; i++)
                {
                    if (allQuests[i].questID == rewardReadyQuestID)
                    {
                        if (!GameManager.manager.rewardReadyQuests.Contains(allQuests[i]))
                        {
                            if (!GameManager.manager.completedQuests.Contains(allQuests[i]))
                            {
                                allQuests[i].InitializeRewardReadyQuest();
                                AddQuestToRewardReadyArray(allQuests[i]);
                            }
                        }
                    }
                }
            }
        }


        if (GameManager.manager != null && GameManager.manager.triggeredQuestIDs != null)
        {
            foreach (int questID in GameManager.manager.triggeredQuestIDs)
            {
                for (int i = 0; i < allQuests.Length; i++)
                {
                    if (allQuests[i].questID == questID)
                    {
                        if (!GameManager.manager.rewardReadyQuests.Contains(allQuests[i]))
                        {
                            if (!GameManager.manager.completedQuests.Contains(allQuests[i]))
                            {
                                allQuests[i].initializeQuest();
                            }
                        }
                    }
                }
            }
        }


        if (GameManager.manager != null && GameManager.manager.completedQuestIDs != null)
        {
            foreach (int questID in GameManager.manager.completedQuestIDs)
            {
                for (int i = 0; i < allQuests.Length; i++)
                {
                    if (allQuests[i].questID == questID)
                    {
                        allQuests[i].InitializeCompletedQuest();

                        if (!GameManager.manager.completedQuests.Contains(allQuests[i]))
                        {
                            AddQuestToCompletedArray(allQuests[i]);
                        }
                    }
                }
            }
        }
    }

    public void RemoveCompletedQuestIDs()
    {
        if(GameManager.manager != null && GameManager.manager.triggeredQuestIDs != null &&
            GameManager.manager.rewardReadyQuestIDs != null && GameManager.manager.completedQuestIDs != null)
        {
            List<int> questsToRemove = new List<int>();

            foreach(int questID in GameManager.manager.triggeredQuestIDs)
            {
                if (Array.IndexOf(GameManager.manager.completedQuestIDs, questID) != -1)
                {
                    questsToRemove.Add(questID);
                }
            }

            foreach (int questIDToRemove in questsToRemove)
            {
                GameManager.manager.triggeredQuestIDs = Array.FindAll(GameManager.manager.triggeredQuestIDs, id => id != questIDToRemove);
                GameManager.manager.rewardReadyQuestIDs = Array.FindAll(GameManager.manager.rewardReadyQuestIDs, id => id != questIDToRemove);
            }
        }
    }

    public void AddQuestToRewardReadyArray(QuestBase quest)
    {
        if(!GameManager.manager.rewardReadyQuests.Contains(quest))
        {
            QuestBase[] newRewardReadyQuests = new QuestBase[GameManager.manager.rewardReadyQuests.Length + 1];
            for(int i = 0; i < GameManager.manager.rewardReadyQuests.Length; i++)
            {
                newRewardReadyQuests[i] = GameManager.manager.rewardReadyQuests[i];
            }
            newRewardReadyQuests[GameManager.manager.rewardReadyQuests.Length] = quest;
            GameManager.manager.rewardReadyQuests = newRewardReadyQuests;
        }
    }

    public void AddQuestToCompletedArray(QuestBase quest)
    {
        if (!GameManager.manager.completedQuests.Contains(quest))
        {
            QuestBase[] newQuestArray = new QuestBase[GameManager.manager.completedQuests.Length + 1];
            for (int i = 0; i < GameManager.manager.completedQuests.Length; i++)
            {
                newQuestArray[i] = GameManager.manager.completedQuests[i];
            }
            newQuestArray[GameManager.manager.completedQuests.Length] = quest;
            GameManager.manager.completedQuests = newQuestArray;
        }
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

    public void RemoveEnemiesForRewardReadyQuests()
    {
        int[] rewardReadyQuestIDs = GameManager.manager.rewardReadyQuestIDs;

        EnemyHealth[] enemies = FindObjectsOfType<EnemyHealth>();
        foreach(EnemyHealth enemy in enemies)
        {
            if(rewardReadyQuestIDs != null && rewardReadyQuestIDs.Contains(enemy.questEnemyID))
            {
                DestroyEnemy(enemy);
            }
        }
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
            if(!GameManager.manager.triggeredQuestIDs.Contains(quest.questID))
            {
                AddQuestIDToArray(quest.questID);
            }
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

    void AddRewardReadyQuestToArray(QuestBase newRewardReadyQuest)
    {
        QuestBase[] newRewardReadyQuests = new QuestBase[GameManager.manager.rewardReadyQuests.Length + 1];
        for(int i = 0; i < GameManager.manager.rewardReadyQuests.Length; i++)
        {
            newRewardReadyQuests[i] = GameManager.manager.rewardReadyQuests[i];
        }
        newRewardReadyQuests[GameManager.manager.rewardReadyQuests.Length] = newRewardReadyQuest;
        GameManager.manager.rewardReadyQuests = newRewardReadyQuests;
    }

    public void AddQuestIDToRewardReadyArray(int newRewardReadyQuestID)
    {
        int[] newRewardReadyQuestIDs = new int[GameManager.manager.rewardReadyQuestIDs.Length + 1];
        for (int i = 0; i < GameManager.manager.rewardReadyQuestIDs.Length; i++)
        {
            newRewardReadyQuestIDs[i] = GameManager.manager.rewardReadyQuestIDs[i];
        }
        newRewardReadyQuestIDs[GameManager.manager.rewardReadyQuestIDs.Length] = newRewardReadyQuestID;
        GameManager.manager.rewardReadyQuestIDs = newRewardReadyQuestIDs;
    }

    public void RemoveQuestFromTriggeredQuestsArray(QuestBase quest)
    {
        List<QuestBase> triggeredQuestsList = new List<QuestBase>(GameManager.manager.triggeredQuests);
        if (triggeredQuestsList.Contains(quest))
        {
            triggeredQuestsList.Remove(quest);
        }
        GameManager.manager.triggeredQuests = triggeredQuestsList.ToArray();
    }

    public void RemoveQuestFromTriggeredQuestIdsArray(int questID)
    {
        List<int> triggeredQuestIDsList = new List<int>(GameManager.manager.triggeredQuestIDs);
        if (triggeredQuestIDsList.Contains(questID))
        {
            triggeredQuestIDsList.Remove(questID);
        }
        GameManager.manager.triggeredQuestIDs = triggeredQuestIDsList.ToArray();
    }
}
