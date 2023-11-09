using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterDialogue : MonoBehaviour
{
    public DialogueAsset dialogueNPC;
    GameObject player;
    public Transform character;

    public float radius = 3;
    float distance;
    bool dialogueBoxOpen = false;

    int dialogueIndex = 0;

    private void Start()
    {
        player = PlayerManager.instance.player;
    }

    private void Update()
    {
        distance = Vector3.Distance(player.transform.position, character.position);
        if (distance <= radius)
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                if(!dialogueBoxOpen)
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
                    //DialogueBox.instance.ShowNextDialogue();
                }
            }
        }
    }

    public void StartDialogue()
    {
        DialogueBox.instance.SetDialogue(dialogueNPC);
        DialogueBox.instance.ShowDialogue();
    }

    public void ContinueDialogue()
    {

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
