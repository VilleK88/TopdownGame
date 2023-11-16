using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterDialogue : MonoBehaviour
{
    GameObject player;
    public GameObject npc;
    public Transform character;
    [SerializeField] GameObject speechIcon;

    public float radius = 3;
    float distance;

    [SerializeField] bool firstInteraction = true;
    [SerializeField] int repeatStartPosition;
    public string npcName;
    //public DialogueAsset dialogueAsset;
    public DialogueTree dialogueTree;

    bool inConversation;

    public QuestBase quest;

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
        //quest.initializeQuest();
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
            DialogueBox.instance.StartDialogue(dialogueTree, StartPosition, npcName);
        }
    }

    void JoinConversation()
    {
        inConversation = true;
    }

    void LeaveConversation()
    {
        inConversation = false;
        if(DialogueBox.instance.answerIndex == 0)
        {
            quest.initializeQuest();
            QuestManager.questManager.SetQuestUI(quest);
        }
        //quest.initializeQuest();
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
