using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
    public DialogueTree dialogueTree;
    public DialogueTree secondDialogueTree;
    public DialogueTree questFinishedDialogueTree;
    public CharacterProfile characterProfile;

    bool inConversation;

    [SerializeField] QuestBase npcQuest;
    //QuestBase npcQuest;
    public int launchQuestAnswerIndex = 0;
    //bool inQuest;

    bool reward = true;

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
            gameObject.GetComponent<NavMeshAgent>().isStopped = true;
            speechIcon.SetActive(true);
            if (Input.GetKeyDown(KeyCode.T))
            {
                Interact();
            }
        }
        else
        {
            gameObject.GetComponent<NavMeshAgent>().isStopped = false;
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
                if(!QuestsContainsQuest(QuestManager.questManager.quests, npcQuest) && !QuestsContainsQuest(QuestManager.questManager.finishedQuests, npcQuest))
                {
                    DialogueBox.instance.StartDialogue(dialogueTree, StartPosition, npcName);
                }
                else if(QuestsContainsQuest(QuestManager.questManager.finishedQuests, npcQuest))
                {
                    DialogueBox.instance.StartDialogue(questFinishedDialogueTree, StartPosition, npcName);
                    if(reward)
                    {
                        RewardManager.instance.SetRewardUI(npcQuest);
                        reward = false;
                    }
                }
                else
                {
                    DialogueBox.instance.StartDialogue(secondDialogueTree, StartPosition, npcName);
                }
            }
        }
    }

    bool AnotherNPCHasQuest(QuestBase questToCheck)
    {
        CharacterDialogue[] allNPCDialogues = FindObjectsOfType<CharacterDialogue>();

        foreach(CharacterDialogue npcDialogue in allNPCDialogues)
        {
            if(npcDialogue != this && npcDialogue.QuestsContainsQuest(QuestManager.questManager.quests, questToCheck))
            {
                return true;
            }
        }

        return false;
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
        if(DialogueBox.instance.answerIndex == launchQuestAnswerIndex && !QuestsContainsQuest(QuestManager.questManager.quests, npcQuest) &&
            !QuestsContainsQuest(QuestManager.questManager.finishedQuests, npcQuest))
        {
            if(!AnotherNPCHasQuest(npcQuest))
            {
                if(IsThisNPCQuest(npcQuest))
                {
                    QuestManager.questManager.SetQuestUI(npcQuest);
                }
                //QuestManager.questManager.SetQuestUI(npcQuest);
            }
            //QuestManager.questManager.SetQuestUI(npcQuest);
        }
    }

    bool IsThisNPCQuest(QuestBase questToCheck)
    {
        return questToCheck == npcQuest;
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
