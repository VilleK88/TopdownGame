using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : NPCInteraction
{
    [Header("Dialogue")]
    public DialogueBase[] db;
    [HideInInspector] public int index;
    public bool nextDialogueOnInteract;


    public override void Interact()
    {
        if (!DialogueManager.instance.isDialogueOption)
        {
            if (nextDialogueOnInteract && !DialogueManager.instance.inDialogue)
            {
                DialogueManager.instance.EnqueueDialogue(db[index]);

                if (index < db.Length - 1)
                {
                    index++;
                }
            }
            else
            {
                DialogueManager.instance.EnqueueDialogue(db[index]);
            }
        }

        base.Interact();
    }

    public void SetIndex(int i)
    {
        index = i;
    }
}
