using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class QuestlogManager : MonoBehaviour
{
    #region Singleton
    public static QuestlogManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    #endregion

    public TextMeshProUGUI questDescription;
    public TextMeshProUGUI questName;
    public Transform questHolder;
    public GameObject questlogButtonPrefab;
    public GameObject questlogUI;
    public bool isQuestlogActive = false;
    public TextMeshProUGUI objectiveText;
    public TextMeshProUGUI npcTurnInText;
    public Image[] rewardIcons;
    QuestBase lastDisplayedQuest;
    Scene currentScene;
    public int[] triggeredQuestButtonIDs;


    public void UpdateQuestlogUI(QuestBase newQuest, string objectiveList)
    {
        lastDisplayedQuest = newQuest;
        questName.text = newQuest.questName;
        questDescription.text = newQuest.questDescription;
        npcTurnInText.text = "Turn into " + newQuest.NPCTurnIn.myName;
        objectiveText.text = objectiveList;
    }

    public void AddQuestLog(QuestBase newQuest)
    {
        if(!triggeredQuestButtonIDs.Contains(newQuest.questID))
        {
            var questButton = Instantiate(questlogButtonPrefab, questHolder);
            questButton.GetComponent<QuestlogButton>().SetQuest(newQuest);
            AddToTriggeredQuestButtonIDs(newQuest.questID);
        }
    }

    public void AddToTriggeredQuestButtonIDs(int newQuestButtonID)
    {
        int[] newQuestButtonIDs = new int[triggeredQuestButtonIDs.Length + 1];
        for(int i = 0; i < triggeredQuestButtonIDs.Length; i++)
        {
            newQuestButtonIDs[i] = triggeredQuestButtonIDs[i];
        }
        newQuestButtonIDs[triggeredQuestButtonIDs.Length] = newQuestButtonID;
        triggeredQuestButtonIDs = newQuestButtonIDs;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.J))
        {
            questlogUI.SetActive(!questlogUI.activeSelf);
            isQuestlogActive = questlogUI.activeSelf;

            if(questlogUI.activeSelf)
            {
                //if (lastDisplayedQuest == null)
                    //return;

                try
                {
                    var firstButton = questHolder.GetChild(0).GetComponent<Button>();
                    firstButton.Select();
                    if(GameManager.manager != null && GameManager.manager.completedQuests != null &&
                        GameManager.manager.completedQuests.Contains(lastDisplayedQuest))
                    {
                        QuestBase completedQuest = Array.Find(GameManager.manager.completedQuests, quest => quest == lastDisplayedQuest);
                        if(completedQuest != null)
                        {
                            Debug.Log("CompletedQuests debuggaa");
                            UpdateQuestlogUI(completedQuest, completedQuest.GetCompletedObjectiveList());
                        }
                    }
                    if(GameManager.manager != null && GameManager.manager.rewardReadyQuests != null &&
                        GameManager.manager.rewardReadyQuests.Contains(lastDisplayedQuest))
                    {
                        QuestBase rewardReadyQuest = Array.Find(GameManager.manager.rewardReadyQuests, quest => quest == lastDisplayedQuest);
                        if(rewardReadyQuest != null)
                        {
                            Debug.Log("RewardReadyQuests debuggaa");
                            UpdateQuestlogUI(rewardReadyQuest, rewardReadyQuest.GetCompletedObjectiveList());
                        }
                    }
                    if(GameManager.manager != null && GameManager.manager.triggeredQuests != null &&
                        GameManager.manager.triggeredQuests.Contains(lastDisplayedQuest))
                    {
                        Debug.Log("TriggeredQuests debuggaa");
                        UpdateQuestlogUI(lastDisplayedQuest, lastDisplayedQuest.GetObjectiveList());
                    }
                }
                catch
                {
                    return;
                }
            }
        }
    }
}
