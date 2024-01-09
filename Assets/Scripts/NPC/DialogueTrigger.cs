using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : NPCInteraction
{
    [Header("THIS NPC")]
    public CharacterProfile targetNPC;

    [Header("Basic Dialogues Info")]
    public DialogueBase[] db;
    [HideInInspector] public int index;
    public bool nextDialogueOnInteract;

    public bool hasCompletedQuest { get; set; }
    public DialogueBase completedQuestDialogue { get; set; }

    public override void Interact()
    {
        if (!DialogueManager.instance.isDialogueOption)
        {
            if(hasCompletedQuest)
            {
                DialogueManager.instance.EnqueueDialogue(completedQuestDialogue);
                DialogueManager.instance.completedQuestReady = true;
                hasCompletedQuest = false;
                return;
            }
        }

        if (nextDialogueOnInteract && DialogueManager.instance.inDialogue)
        {
            DialogueManager.instance.EnqueueDialogue(db[index]);

            if (index < db.Length - 1)
            {
                index++;
            }
        }
        else if(!DialogueManager.instance.inDialogue)
        {
            DialogueManager.instance.EnqueueDialogue(db[index]);
        }
    }

    public void SetIndex(int i)
    {
        index = i;
    }
}
