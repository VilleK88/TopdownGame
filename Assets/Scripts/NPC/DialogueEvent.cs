using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(DialogueTrigger))]
public class DialogueEvent : NPCInteraction
{
    [Header("Target info")]
    public int targetIndex;
    public int targetLine;

    public UnityEvent unityEvent;

    public DialogueTrigger npcDialogueTrigger;

    bool hasAdded;

    private void Start()
    {
        npcDialogueTrigger = GetComponent<DialogueTrigger>();
    }

    public override void Interact()
    {
        if (hasAdded || DialogueManager.instance.inDialogue)
            return;

        if(npcDialogueTrigger.index == targetIndex)
        {
            DialogueManager.instance.onDialogueLineCallBack += GenericEvent;
            hasAdded = true;
        }

        base.Interact();
    }

    public void GenericEvent(int line)
    {
        if(line == targetLine)
        {
            unityEvent.Invoke();
            DialogueManager.instance.onDialogueLineCallBack -= GenericEvent;
        }
    }
}
