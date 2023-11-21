using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    public TextMeshProUGUI dialogueName;
    public TextMeshProUGUI dialogueText;
    public Image dialoguePortrait;
    public float delay = 0.001f;

    public Queue<DialogueBase.Info> dialogueInfo;

    // options stuff
    bool isDialogueOption;
    public GameObject dialogueOptionUI;
    public bool inDialogue;
    public GameObject[] optionButtons;
    int optionsAmount;
    public TextMeshProUGUI questionText;

    private void Start()
    {
        dialogueInfo = new Queue<DialogueBase.Info>();
    }

    public void EnqueueDialogue(DialogueBase db)
    {
        if(inDialogue)
        {
            return;
        }
        inDialogue = true;
        dialogueBox.SetActive(true);
        dialogueInfo.Clear();

        if(db is DialogueOptions)
        {
            isDialogueOption = true;
            DialogueOptions dialogueOptions = db as DialogueOptions;
            optionsAmount = dialogueOptions.optionsInfo.Length;
            questionText.text = dialogueOptions.questionText;

            for(int i = 0; i < optionButtons.Length; i++)
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

        foreach(DialogueBase.Info info in db.dialogueInfo)
        {
            dialogueInfo.Enqueue(info);
        }

        DequeueDialogue();
    }

    public void DequeueDialogue()
    {
        if(dialogueInfo.Count == 0)
        {
            EndOfDialogue();
            inDialogue = false;
            return;
        }

        DialogueBase.Info info = dialogueInfo.Dequeue();

        dialogueName.text = info.myName;
        dialogueText.text = info.myText;
        dialoguePortrait.sprite = info.portrait;

        StartCoroutine(TypeText(info));
    }

    IEnumerator TypeText(DialogueBase.Info info)
    {
        dialogueText.text = "";
        foreach(char c in info.myText.ToCharArray())
        {
            yield return new WaitForSeconds(delay);
            dialogueText.text += c;
            yield return null;
        }
    }

    public void EndOfDialogue()
    {
        dialogueBox.SetActive(false);
        inDialogue = false;
        OptionsLogic();
    }

    void OptionsLogic()
    {
        if(isDialogueOption == true)
        {
            dialogueOptionUI.SetActive(true);
        }
    }

    public void CloseOptions()
    {
        dialogueOptionUI.SetActive(false);
    }
}
