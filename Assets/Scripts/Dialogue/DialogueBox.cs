using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DialogueBox : MonoBehaviour
{
    public static DialogueBox instance;
    public DialogueAsset dialogue;

    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] GameObject dialoguePanel;
    public static event Action OnDialogueStarted;
    public static event Action OnDialogueEnded;
    bool skipLineTriggered;

    int dialogueIndex = 0;


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

    public void StartDialogue(string[] dialogue, int startPosition, string name)
    {
        nameText.text = name + "";
        dialoguePanel.gameObject.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(RunDialogue(dialogue, startPosition));
    }

    IEnumerator RunDialogue(string[] dialogue, int startPosition)
    {
        skipLineTriggered = false;
        OnDialogueStarted?.Invoke();
        for(int i = startPosition; i < dialogue.Length; i++)
        {
            dialogueText.text = dialogue[i];
            while(skipLineTriggered == false)
            {
                yield return null;
            }
            skipLineTriggered = false;
        }
        OnDialogueEnded?.Invoke();
        dialoguePanel.gameObject.SetActive(false);
    }

    public void SkipLine()
    {
        skipLineTriggered = true;
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
