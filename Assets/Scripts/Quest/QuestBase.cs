using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestBase : ScriptableObject
{
    public string questName;
    [TextArea(5, 10)]
    public string questDescription;

    public int[] currentAmount { get; set; }
    public int[] requiredAmount { get; set; }

    public bool isCompleted { get; set; }

    public CharacterProfile NPCTurnIn;

    [System.Serializable]
    public class Rewards
    {
        public Item[] itemRewards;
        public float experienceReward;
        public int goldReward;
    }

    public Rewards rewards;

    public virtual void initializeQuest()
    {
        Debug.Log("Start Quest");
        currentAmount = new int[requiredAmount.Length];

        if(!QuestAlreadyExists(QuestManager.questManager.currentQuest))
        {
            AddQuestToManager(QuestManager.questManager.currentQuest);
            QuestlogManager.instance.AddQuestLog(this);
        }
    }

    bool QuestAlreadyExists(QuestBase questToCheck)
    {
        foreach(QuestBase quest in QuestManager.questManager.quests)
        {
            if(quest == questToCheck)
            {
                return true;
            }
        }
        return false;
    }

    void AddQuestToManager(QuestBase questToAdd)
    {
        for(int i = 0; i < QuestManager.questManager.quests.Length; i++)
        {
            if (QuestManager.questManager.quests[i] == null)
            {
                QuestManager.questManager.quests[i] = questToAdd;
                break;
            }
        }
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

        QuestManager.questManager.MoveToFinishedQuests(this);
    }
}
