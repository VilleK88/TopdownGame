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
    }
}
