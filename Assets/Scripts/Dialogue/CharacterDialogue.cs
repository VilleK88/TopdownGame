using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterDialogue : MonoBehaviour
{
    //public DialogueAsset dialogueNPC;
    GameObject player;
    public GameObject npc;
    public Transform character;
    [SerializeField] GameObject speechIcon;

    public float radius = 3;
    float distance;
    bool dialogueBoxOpen = false;

    int dialogueIndex = 0;

    [SerializeField] bool firstInteraction = true;
    [SerializeField] int repeatStartPosition;
    public string npcName;
    public DialogueAsset dialogueAsset;

    bool inConversation;

    [HideInInspector]
    public int StartPosition
    {
        get
        {
            if(firstInteraction)
            {
                firstInteraction = false;
                return 0;
            }
            else
            {
                return repeatStartPosition;
            }
        }
    }


    private void Start()
    {
        player = PlayerManager.instance.player;
        speechIcon.SetActive(false);
    }

    private void Update()
    {
        distance = Vector3.Distance(player.transform.position, character.position);
        if (distance <= radius)
        {
            speechIcon.SetActive(true);
            if (Input.GetKeyDown(KeyCode.T))
            {
                Interact();
                /*if(!dialogueBoxOpen)
                {
                    StartDialogue();
                    dialogueBoxOpen = true;
                }
                else
                {
                    if (dialogueIndex <= dialogueNPC.dialogue.Length)
                    {
                        dialogueIndex++;
                        if (dialogueIndex < dialogueNPC.dialogue.Length)
                        {
                            DialogueBox.instance.ShowNextDialogue();
                        }
                        else
                        {
                            dialogueIndex = 0;
                            DialogueBox.instance.CloseDialogue();
                            dialogueBoxOpen = false;
                        }
                    }
                }*/
            }
        }
        else
        {
            speechIcon.SetActive(false);
        }
    }

    void Interact()
    {
        if(inConversation)
        {
            DialogueBox.instance.SkipLine();
        }
        else
        {
            DialogueBox.instance.StartDialogue(dialogueAsset.dialogue, StartPosition, npcName);
        }
    }

    void JoinConversation()
    {
        inConversation = true;
    }

    void LeaveConversation()
    {
        inConversation = false;
    }

    private void OnEnable()
    {
        DialogueBox.OnDialogueStarted += JoinConversation;
        DialogueBox.OnDialogueEnded += LeaveConversation;
    }

    private void OnDisable()
    {
        DialogueBox.OnDialogueStarted -= JoinConversation;
        DialogueBox.OnDialogueEnded -= LeaveConversation;
    }

    /*public void StartDialogue()
    {
        DialogueBox.instance.SetDialogue(dialogueNPC);
        DialogueBox.instance.ShowDialogue();
    }*/

    public void ContinueDialogue()
    {

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
