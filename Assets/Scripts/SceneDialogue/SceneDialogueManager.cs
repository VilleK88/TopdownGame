using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SceneDialogueManager : MonoBehaviour
{
    #region Singleton
    public static SceneDialogueManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    #endregion

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;

    Animator anim;

    Queue<string> sentences;

    public bool inSceneDialogue = false;

    private void Start()
    {
        anim = GetComponent<Animator>();
        sentences = new Queue<string>();
    }

    public void StartSceneDialogue(SceneDialogue sceneDialogue)
    {
        inSceneDialogue = true;

        anim.SetBool("IsOpen", true);

        nameText.text = sceneDialogue.name;

        sentences.Clear();

        foreach(string sentence in sceneDialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if(sentences.Count == 0)
        {
            EndSceneDialogue();
            inSceneDialogue = false;
            return;
        }

        string sentence = sentences.Dequeue();
        //dialogueText.text = sentence;
        StopCoroutine(TypeSentence(sentence));
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.01f);
        }
    }

    public void EndSceneDialogue()
    {
        anim.SetBool("IsOpen", false);
    }
}
