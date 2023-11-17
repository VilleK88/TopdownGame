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

        for (int i = 0; i < QuestManager.questManager.quests.Length; i++)
        {
            if (QuestManager.questManager.quests[i] == null)
            {
                QuestManager.questManager.quests[i] = QuestManager.questManager.currentQuest;
            }
            return;
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

        for (int i = 0; i < QuestManager.questManager.quests.Length; i++)
        {
            if (QuestManager.questManager.quests[i] == QuestManager.questManager.currentQuest)
            {
                QuestManager.questManager.quests[i] = null;

                AddToFinishedQuests(QuestManager.questManager.currentQuest);
                return;
            }
        }
    }

    public void MoveToFinishedQuests(QuestBase quest)
    {
        for(int i = 0; i < QuestManager.questManager.quests.Length; i++)
        {
            if (QuestManager.questManager.quests[i] == quest)
            {
                QuestManager.questManager.quests[i] = null;

                AddToFinishedQuests(quest);
                return;
            }
        }
    }

    void AddToFinishedQuests(QuestBase quest)
    {
        for(int i = 0; i < QuestManager.questManager.finishedQuests.Length; i++)
        {
            if (QuestManager.questManager.finishedQuests[i] == null)
            {
                QuestManager.questManager.finishedQuests[i] = quest;
                return;
            }
        }
    }
}
