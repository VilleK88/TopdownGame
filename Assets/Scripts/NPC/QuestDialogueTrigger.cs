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
                            /*if(!hasActiveQuest)
                            {
                                base.Interact();
                            }*/
                            return;
                        }
                    }
                }
            }
            DialogueManager.instance.EnqueueDialogue(dialogueQuests[QuestIndex]);
            QuestManager.questManager.currentQuestDialogueTrigger = this;
        }
        else
        {
            base.Interact();
        }

        if(!CheckIfQuestAlreadyDone() || DialogueManager.instance.completedQuestReady)
        {
            SetItemRewards();
        }
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
                    AddQuestToCompletedArray(dialogueQuest.quest);
                    RemoveQuestFromTriggeredQuestsArray(dialogueQuest.quest);
                    return;
                }
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
