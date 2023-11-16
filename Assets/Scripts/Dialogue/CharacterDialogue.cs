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
    public DialogueTree secondDialogueTree;
    public DialogueTree questFinishedDialogueTree;

    bool inConversation;

    public QuestBase quest;
    public int launchQuestAnswerIndex = 0;
    bool inQuest;

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
            if(!QuestManager.questManager.inQuestUI)
            {
                /*if(quest != QuestManager.questManager.currentQuest)
                {
                    DialogueBox.instance.StartDialogue(dialogueTree, StartPosition, npcName);
                }*/

                if(!QuestsContainsQuest(QuestManager.questManager.quests, quest) && !QuestsContainsQuest(QuestManager.questManager.finishedQuests, quest))
                {
                    DialogueBox.instance.StartDialogue(dialogueTree, StartPosition, npcName);
                }
                else if(QuestsContainsQuest(QuestManager.questManager.finishedQuests, quest))
                {
                    DialogueBox.instance.StartDialogue(questFinishedDialogueTree, StartPosition, npcName);
                }
                else
                {
                    DialogueBox.instance.StartDialogue(secondDialogueTree, StartPosition, npcName);
                }
            }
        }
    }

    bool QuestsContainsQuest(QuestBase[] questArray, QuestBase questToCheck)
    {
        foreach(QuestBase q in questArray)
        {
            if(q == questToCheck)
            {
                return true;
            }
        }
        return false;
    }

    void JoinConversation()
    {
        inConversation = true;
    }

    void LeaveConversation()
    {
        inConversation = false;
        if(DialogueBox.instance.answerIndex == launchQuestAnswerIndex && !QuestsContainsQuest(QuestManager.questManager.quests, quest) &&
            !QuestsContainsQuest(QuestManager.questManager.finishedQuests, quest))
        {
            //quest.initializeQuest();
            QuestManager.questManager.SetQuestUI(quest);
        }
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
