using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestDialogueTrigger : DialogueTrigger
{
    [Header("Quest Dialogue Info")]
    public bool hasActiveQuest;
    public DialogueQuest[] dialogueQuests;
    public int QuestIndex { get; set; }

    public override void Interact()
    {
        if(hasActiveQuest)
        {
            if (GameManager.manager != null && GameManager.manager.triggeredQuests != null)
            {
                foreach(QuestBase quest in GameManager.manager.triggeredQuests)
                {
                    foreach(DialogueQuest dialogueQuest in dialogueQuests)
                    {
                        if(dialogueQuest.quest != null && quest == dialogueQuest.quest)
                        {
                            QuestManager.questManager.currentQuestDialogueTrigger = this;
                            hasActiveQuest = false;
                            if(!hasActiveQuest)
                            {
                                base.Interact();
                            }
                            return;
                        }
                    }
                }
            }

            if(!CheckIfQuestRewardReady())
            {
                if(CheckIfQuestAlreadyDone())
                {
                    hasActiveQuest = false;
                }
                else
                {
                    DialogueManager.instance.EnqueueDialogue(dialogueQuests[QuestIndex]);
                    QuestManager.questManager.currentQuestDialogueTrigger = this;
                    Debug.Log("Debuggaa t��ll�");
                }
                //DialogueManager.instance.EnqueueDialogue(dialogueQuests[QuestIndex]);
                //QuestManager.questManager.currentQuestDialogueTrigger = this;
                //Debug.Log("Debuggaa t��ll�");
            }
            else
            {
                hasActiveQuest = false;
            }
        }
        else
        {
            if(CheckIfQuestRewardReady())
            {
                base.Interact();
                DialogueManager.instance.completedQuestReady = true;
                SetItemRewards();
            }
            else
            {
                base.Interact();
            }
            //base.Interact();
        }

        if(/*!CheckIfQuestAlreadyDone() &&*/ CheckIfQuestRewardReady() || DialogueManager.instance.completedQuestReady)
        {
            SetItemRewards();
        }
    }

    bool CheckIfQuestRewardReady()
    {
        foreach(QuestBase quest in GameManager.manager.rewardReadyQuests)
        {
            foreach(DialogueQuest dialogueQuest in dialogueQuests)
            {
                if(dialogueQuest.quest != null && quest == dialogueQuest.quest)
                {
                    Debug.Log("True");
                    return true;
                }
            }
        }

        Debug.Log("False");
        return false;
    }

    bool CheckIfQuestAlreadyDone()
    {
        foreach (QuestBase quest in GameManager.manager.completedQuests)
        {
            foreach (DialogueQuest dialogueQuest in dialogueQuests)
            {
                if (dialogueQuest.quest != null && quest == dialogueQuest.quest)
                {
                    return true;
                }
            }
        }

        return false;
    }

    void SetItemRewards()
    {
        /*if(DialogueManager.instance.completedQuestReady)
        {
            foreach(DialogueQuest quest in dialogueQuests)
            {
                if(quest.quest != null)
                {
                    RewardManager.instance.SetRewardUI(quest.quest);
                    DialogueManager.instance.completedQuestReady = false;
                }
            }
        }*/

        if (DialogueManager.instance.completedQuestReady)
        {   
            foreach(DialogueQuest quest in dialogueQuests)
            {
                if(quest.quest != null)
                {
                    RewardManager.instance.SetRewardUI(quest.quest);
                    DialogueManager.instance.completedQuestReady = false;
                }
            }

            foreach (DialogueQuest dialogueQuest in dialogueQuests)
            {
                if (dialogueQuest.quest != null)
                {
                    AddQuestIDToCompletedArray(dialogueQuest.quest.questID);
                    //RemoveQuestIDFromTriggeredArray();
                    RemoveRewardReadyQuestIDFromArray();
                    AddQuestToCompletedArray(dialogueQuest.quest);
                    RemoveRewardReadyQuestsArray(dialogueQuest.quest);
                    //RemoveQuestFromTriggeredQuestsArray(dialogueQuest.quest);
                    return;
                }
            }
        }
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

    void AddQuestIDToCompletedArray(int newCompletedQuestID)
    {
        int[] newCompletedQuestIDs = new int[GameManager.manager.completedQuestIDs.Length + 1];
        for(int i = 0; i < GameManager.manager.completedQuestIDs.Length; i++)
        {
            newCompletedQuestIDs[i] = GameManager.manager.completedQuestIDs[i];
        }
        newCompletedQuestIDs[GameManager.manager.completedQuestIDs.Length] = newCompletedQuestID;
        GameManager.manager.completedQuestIDs = newCompletedQuestIDs;
    }

    void RemoveRewardReadyQuestIDFromArray()
    {
        List<int> rewardReadyQuestIDs = GameManager.manager.rewardReadyQuestIDs.ToList();

        foreach(int questID in GameManager.manager.completedQuestIDs)
        {
            if(rewardReadyQuestIDs.Contains(questID))
            {
                rewardReadyQuestIDs.Remove(questID);
            }
        }
    }

    void RemoveQuestIDFromTriggeredArray()
    {
        List<int> triggeredQuestIDs = GameManager.manager.triggeredQuestIDs.ToList();

        foreach (int questID in GameManager.manager.completedQuestIDs)
        {
            if (triggeredQuestIDs.Contains(questID))
            {
                triggeredQuestIDs.Remove(questID);
            }
        }

        GameManager.manager.triggeredQuestIDs = triggeredQuestIDs.ToArray();
    }

    void RemoveRewardReadyQuestsArray(QuestBase quest)
    {
        for(int i = 0; i < GameManager.manager.rewardReadyQuests.Length; i++)
        {
            if (GameManager.manager.rewardReadyQuests[i] == quest)
            {
                GameManager.manager.rewardReadyQuests = GameManager.manager.rewardReadyQuests.Where((val, idx) => idx != i).ToArray();
                return;
            }
        }
    }

    void RemoveQuestFromTriggeredQuestsArray(QuestBase quest)
    {
        for(int i = 0; i < GameManager.manager.triggeredQuests.Length; i++)
        {
            if (GameManager.manager.triggeredQuests[i] == quest)
            {
                GameManager.manager.triggeredQuests = GameManager.manager.triggeredQuests.Where((val, idx) => idx != i).ToArray();
                return;
            }
        }
    }

    public void HasCompletedQuest(QuestBase quest)
    {
        foreach(DialogueQuest dialogueQuest in dialogueQuests)
        {
            if(dialogueQuest.quest != null)
            {
                AddQuestToCompletedArray(quest);
                return;
            }
        }
    }

    public void AddQuestToCompletedArray(QuestBase quest)
    {
        if(!GameManager.manager.completedQuests.Contains(quest))
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
}
