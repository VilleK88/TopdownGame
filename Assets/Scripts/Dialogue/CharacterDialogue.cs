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
                //DialogueBox.instance.ShowDialogue();
                StartDialogue();
            }
        }
    }

    public void StartDialogue()
    {
        DialogueBox.instance.SetDialogue(dialogueNPC);
        DialogueBox.instance.ShowDialogue();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
