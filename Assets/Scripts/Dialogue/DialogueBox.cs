using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBox : MonoBehaviour
{
    public DialogueAsset dialogue;

    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] GameObject dialoguePanel;

    public static DialogueBox instance;

    private void Awake()
    {
        instance = this;
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
        Debug.Log(dialogue.dialogue[0]);
    }

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
}
