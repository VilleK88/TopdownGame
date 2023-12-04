using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class QuestBase : ScriptableObject
{
    public string questName;
    public int questID = 0;
    [TextArea(5, 10)]
    public string questDescription;

    public int[] currentAmount { get; set; }
    public int[] requiredAmount { get; set; }

    public bool isCompleted { get; set; }

    public CharacterProfile NPCTurnIn;
    public DialogueBase completedQuestDialogue;

    [System.Serializable]
    public class Rewards
    {
        public Item[] itemRewards;
        public float experienceReward;
    }

    public Rewards rewards;

    public virtual void initializeQuest()
    {
        isCompleted = false;
        Debug.Log("Start Quest");
        currentAmount = new int[requiredAmount.Length];
        QuestlogManager.instance.AddQuestLog(this);
        QuestManager.questManager.HasTriggeredQuest(this);
    }

    public virtual void InitializeCompletedQuest()
    {
        isCompleted = true;
        currentAmount = new int[requiredAmount.Length];
        QuestlogManager.instance.AddQuestLog(this);
    }

    public void Evaluate()
    {
        for(int i = 0; i < requiredAmount.Length; i++)
        {
            if (currentAmount[i] < requiredAmount[i])
            {
                return;
            }
        }

        Debug.Log("Quest is Completed");

        for (int i = 0; i < GameManager.manager.allDialogueTriggers.Length; i++)
        {
            if (GameManager.manager.allDialogueTriggers[i].targetNPC == NPCTurnIn)
            {
                GameManager.manager.allDialogueTriggers[i].hasCompletedQuest = true;
                GameManager.manager.allDialogueTriggers[i].completedQuestDialogue = completedQuestDialogue;
                Debug.Log("We Found: " + NPCTurnIn);
                break;
            }
        }

        isCompleted = true;
        DialogueManager.instance.completedQuest = this;
    }

    public virtual string GetObjectiveList()
    {
        return null;
    }

    public virtual string GetCompletedObjectiveList()
    {
        return null;
    }
}
