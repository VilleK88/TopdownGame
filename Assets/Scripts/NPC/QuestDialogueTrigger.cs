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
        }
    }
}
