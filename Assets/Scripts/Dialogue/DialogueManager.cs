using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static DialogueBase;

public class DialogueManager : MonoBehaviour
{
    #region Singleton
    public static DialogueManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    #endregion

    public GameObject dialogueBox;
    public GameObject questionTextBubble;

    public TextMeshProUGUI dialogueName;
    public TextMeshProUGUI dialogueText;
    public Image dialoguePortrait;
    float delay = 0.05f; // 0.001f

    public Queue<DialogueBase.Info> dialogueInfo;

    // options stuff
    public bool isDialogueOption;
    public GameObject dialogueOptionUI;
    public bool inDialogue;
    public GameObject[] optionButtons;
    int optionsAmount;
    public TextMeshProUGUI questionText;

    DialogueBase currentDialogue;
    bool isCurrentlyTyping;
    string completeText;
    bool buffer;
    Coroutine typeCoroutine;

    public delegate void OnDialogueLineCallBack(int dialogueLine);
    public OnDialogueLineCallBack onDialogueLineCallBack;
    int totalLineCount;

    public QuestBase completedQuest { get; set; }
    public bool completedQuestReady { get; set; }


    private void Start()
    {
        dialogueInfo = new Queue<DialogueBase.Info>();
    }

    public void EnqueueDialogue(DialogueBase db)
    {
        if(inDialogue || QuestManager.questManager.inQuestUI || RewardManager.instance.inQuestReward)
            return;

        buffer = true;
        inDialogue = true;
        StartCoroutine(BufferTimer());

        dialogueBox.SetActive(true);
        dialogueInfo.Clear();
        currentDialogue = db;

        OptionsParser(db);

        foreach(DialogueBase.Info info in db.dialogueInfo)
        {
            dialogueInfo.Enqueue(info);
        }

        totalLineCount = dialogueInfo.Count;
        DequeueDialogue();
    }

    public void DequeueDialogue()
    {
        if (isCurrentlyTyping == true)
        {
            if (buffer == true)
                return;

            CompleteText();
            StopCoroutine(typeCoroutine);
            isCurrentlyTyping = false;
            return;
        }

        if (dialogueInfo.Count == 0)
        {
            EndOfDialogue();
            inDialogue = false;
            return;
        }

        DialogueBase.Info info = dialogueInfo.Dequeue();

        if(onDialogueLineCallBack != null)
        {
            onDialogueLineCallBack.Invoke(totalLineCount = dialogueInfo.Count);
        }

        completeText = info.myText;
        dialogueName.text = info.character.myName;
        dialogueText.text = info.myText;
        //dialoguePortrait.sprite = info.character.myPortrait;
        info.ChangeEmotion();
        dialoguePortrait.sprite = info.character.MyPortrait;

        dialogueText.text = ""; // prevent text repetition
        typeCoroutine = StartCoroutine(TypeText(info));
    }

    IEnumerator TypeText(DialogueBase.Info info)
    {
        isCurrentlyTyping = true;
        foreach(char c in info.myText.ToCharArray())
        {
            yield return new WaitForSeconds(delay);
            dialogueText.text += c;
            AudioManager.instance.PlaySound(info.character.myVoice);
        }
        isCurrentlyTyping = false;
    }

    IEnumerator BufferTimer()
    {
        yield return new WaitForSeconds(0.1f);
        buffer = false;
    }

    void CompleteText()
    {
        dialogueText.text = completeText;
    }

    public void EndOfDialogue()
    {
        dialogueBox.SetActive(false);
        OptionsLogic();
        CheckIfDialogueQuest();
        SetItemRewards();
    }

    void SetItemRewards()
    {
        if(completedQuestReady)
        {
            RewardManager.instance.SetRewardUI(completedQuest);

            completedQuestReady = false;
        }
    }

    void CheckIfDialogueQuest()
    {
        if(currentDialogue is DialogueQuest)
        {
            DialogueQuest dq = currentDialogue as DialogueQuest;
            QuestManager.questManager.SetQuestUI(dq.quest);
        }
    }

    void OptionsLogic()
    {
        if(isDialogueOption == true)
        {
            //questionTextBubble.SetActive(true);
            dialogueOptionUI.SetActive(true);
        }
        else
        {
            inDialogue = false;
        }
    }

    public void CloseOptions()
    {
        //questionTextBubble.SetActive(false);
        dialogueOptionUI.SetActive(false);
        isDialogueOption = false; // oma testi
    }

    void OptionsParser(DialogueBase db)
    {
        if (db is DialogueOptions)
        {
            isDialogueOption = true;
            DialogueOptions dialogueOptions = db as DialogueOptions;
            optionsAmount = dialogueOptions.optionsInfo.Length;
            questionText.text = dialogueOptions.questionText;

            optionButtons[0].GetComponent<Button>().Select();

            for (int i = 0; i < optionButtons.Length; i++)
            {
                optionButtons[i].SetActive(false);
            }

            for (int i = 0; i < optionsAmount; i++)
            {
                optionButtons[i].SetActive(true);
                optionButtons[i].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = dialogueOptions.optionsInfo[i].buttonName;
                UnityEventHandler myEventHandler = optionButtons[i].GetComponent<UnityEventHandler>();
                myEventHandler.eventHandler = dialogueOptions.optionsInfo[i].myEvent;
                if (dialogueOptions.optionsInfo[i].nextDialogue != null)
                {
                    myEventHandler.myDialogue = dialogueOptions.optionsInfo[i].nextDialogue;
                }
                else
                {
                    myEventHandler.myDialogue = null;
                }
            }
        }
        else
        {
            isDialogueOption = false;
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            if(inDialogue)
            {
                DialogueManager.instance.DequeueDialogue();
            }
        }
    }
}
