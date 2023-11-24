using System.Collections;
using System.Collections.Generic;
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
            DialogueManager.instance.EnqueueDialogue(dialogueQuests[QuestIndex]);
            QuestManager.questManager.currentQuestDialogueTrigger = this;
        }
        else
        {
            base.Interact();
        }

        if (DialogueManager.instance.completedQuestReady)
        {
            SetItemRewards();
        }
    }

    void SetItemRewards()
    {
        if (DialogueManager.instance.completedQuestReady)
        {
            //RewardManager.instance.SetRewardUI(completedQuest);
            
            foreach(DialogueQuest quest in dialogueQuests)
            {
                if(quest.quest != null)
                {
                    RewardManager.instance.SetRewardUI(quest.quest);
                    DialogueManager.instance.completedQuestReady = false;
                }
            }

            //DialogueManager.instance.completedQuestReady = false;
        }
    }
}
