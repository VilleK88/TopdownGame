using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Rendering;

public class DialogueBox : MonoBehaviour
{
    public static DialogueBox instance;
    public DialogueAsset dialogue;

    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] GameObject dialoguePanel;
    [SerializeField] GameObject answerBox;
    [SerializeField] Button[] answerObjects;
    public static event Action OnDialogueStarted;
    public static event Action OnDialogueEnded;
    bool skipLineTriggered;
    bool answerTriggered;
    int answerIndex;

    //int dialogueIndex = 0;

    DialogueTree currentDialogue;


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    float charactersPerSecond = 5;
    IEnumerator TypeText(string line)
    {
        foreach (var letter in line.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(1 / charactersPerSecond);
        }
    }

    private void Start()
    {
        //Debug.Log(dialogue.dialogue[0]);
    }

    public void StartDialogue(DialogueTree dialogueTree, int startSection, string name)
    {
        ResetBox();
        nameText.text = name + "";
        dialoguePanel.gameObject.SetActive(true);
        OnDialogueStarted?.Invoke();
        StartCoroutine(RunDialogue(dialogueTree, startSection));
    }

    IEnumerator RunDialogue(DialogueTree dialogueTree, int section)
    {
        for(int i = 0; i < dialogueTree.sections[section].dialogue.Length; i++)
        {
            dialogueText.text = dialogueTree.sections[section].dialogue[i];
            while(skipLineTriggered == false)
            {
                yield return null;
            }
            skipLineTriggered = false;
        }
        if (dialogueTree.sections[section].endAfterDialogue)
        {
            OnDialogueEnded?.Invoke();
            dialoguePanel.SetActive(false);
            yield break;
        }
        dialogueText.text = dialogueTree.sections[section].branchPoint.question;
        ShowAnswers(dialogueTree.sections[section].branchPoint);
        while(answerTriggered == false)
        {
            yield return null;
        }
        answerBox.SetActive(false);
        answerTriggered = false;
        StartCoroutine(RunDialogue(dialogueTree, dialogueTree.sections[section].branchPoint.answers[answerIndex].nextElement));
    }

    void ResetBox()
    {
        StopAllCoroutines();
        dialoguePanel.SetActive(false);
        skipLineTriggered = false;
        answerTriggered = false;
    }

    void ShowAnswers(BranchPoint branchPoint)
    {
        answerBox.SetActive(true);
        for(int i = 0; i < 3; i++)
        {
            if(i < branchPoint.answers.Length)
            {
                answerObjects[i].GetComponentInChildren<TextMeshProUGUI>().text = branchPoint.answers[i].answerlabel;
                answerObjects[i].gameObject.SetActive(true);
            }
            else
            {
                answerObjects[i].gameObject.SetActive(false);
            }
        }
    }

    public void SkipLine()
    {
        skipLineTriggered = true;
    }

    public void AnswerQuestion(int answer)
    {
        answerIndex = answer;
        answerTriggered = true;
    }

    void CheckIfDialogueQuest()
    {
        Debug.Log("Start Quest");
        if(currentDialogue is DialogueQuest)
        {
            DialogueQuest DQ = currentDialogue as DialogueQuest;
            QuestManager.questManager.SetQuestUI(DQ.quest);
        }
    }

    /*
    public void ShowDialogue()
    {
        dialoguePanel.SetActive(true);
        StartCoroutine(TypeText(dialogue.dialogue[0]));
        nameText.text = dialogue.name + "";
    }

    public void EndDialogue()
    {
        nameText.text = null;
        dialogueText.text = null; ;
        dialoguePanel.SetActive(false);
    }

    public void SetDialogue(DialogueAsset newDialogue)
    {
        if(newDialogue != null)
        {
            dialogue = newDialogue;
        }
    }

    public void ShowNextDialogue()
    {
        if(dialogueIndex <= dialogue.dialogue.Length)
        {
            dialogueIndex++;
            if(dialogueIndex < dialogue.dialogue.Length)
            {
                dialogueText.text = string.Empty;
                StartCoroutine(TypeText(dialogue.dialogue[dialogueIndex]));
            }
        }
    }

    public void CloseDialogue()
    {
        dialogueText.text = string.Empty;
        dialogueIndex = 0;
        dialoguePanel.SetActive(false);
    }
    */
}
